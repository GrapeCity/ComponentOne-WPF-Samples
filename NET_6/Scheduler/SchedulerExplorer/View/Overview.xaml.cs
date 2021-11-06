using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using C1.C1Schedule;
using C1.WPF.Calendar;
using C1.WPF.Core;
using C1.WPF.DateTimeEditors;
using C1.WPF.Input;
using C1.WPF.Schedule;
using SchedulerExplorer.Resources;
using DateList = C1.WPF.Schedule.DateList;
using System.Globalization;
using C1.WPF.Input;

namespace SchedulerExplorer
{
    /// <summary>
    ///     Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : UserControl
    {
        bool _updatingSelection = false;
        public Overview()
        {
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
            InitializeComponent();
            Tag = AppResources.OverviewDescription;
            // initialize time scale combo
            scale.Items.Add(TimeSpan.FromMinutes(10));
            scale.Items.Add(TimeSpan.FromMinutes(15));
            scale.Items.Add(TimeSpan.FromMinutes(20));
            scale.Items.Add(TimeSpan.FromMinutes(30));
            scale.Items.Add(TimeSpan.FromMinutes(60));
            scale.Items.Add(TimeSpan.FromHours(2));
            scale.SelectedIndex = 3;

            scheduler1.DataStorage.ContactStorage.ListChanged += new System.ComponentModel.ListChangedEventHandler(ContactStorage_ListChanged);
            scheduler1.LayoutUpdated += new EventHandler(scheduler1_LayoutUpdated);
            scheduler1.VisibleDates.CollectionChanged += VisibleDates_CollectionChanged;

        }

        private void VisibleDates_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateCalendarSelection();
        }

        // Fill contact information after adding new contact from the Appointment form.
        void ContactStorage_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemAdded)
            {
                Contact c = scheduler1.DataStorage.ContactStorage.Contacts[e.NewIndex];
                // you have to fill at least MenuCaption as 
                // it's mapped to the Employees.LastName field which doesn't allow empty strings.
                c.MenuCaption = "test contact";
            }
        }

        // refresh buttons' state if Scheduler's layout has been changed
        // by Scheduler's commands defined in default templates.
        void scheduler1_LayoutUpdated(object sender, EventArgs e)
        {
            if (scheduler1.Style == scheduler1.MonthStyle)
            {
                views.SelectedIndex = 3;
                scale.IsEnabled = false;
            }
            else
            {
                if (scheduler1.Style == scheduler1.OneDayStyle)
                {
                    views.SelectedIndex = 0;
                }
                else if (scheduler1.Style == scheduler1.WorkingWeekStyle)
                {
                    views.SelectedIndex = 1;
                }
                else if (scheduler1.Style == scheduler1.WeekStyle)
                {
                    views.SelectedIndex = 2;
                }
                else
                {
                    views.SelectedIndex = 4;
                }
                scale.IsEnabled = true;
            }
            UpdateCalendarSelection();
        }

        void views_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (scheduler1 == null)
            {
                return;
            }
            switch (views.SelectedItems.FirstOrDefault())
            {
                case "Day":
                    SetStyle(scheduler1.OneDayStyle);
                    break;
                case "Work Week":
                    SetStyle(scheduler1.WorkingWeekStyle);
                    break;
                case "Week":
                    SetStyle(scheduler1.WeekStyle);
                    break;
                case "Month":
                    SetStyle(scheduler1.MonthStyle);
                    break;
                case "Time Line":
                    SetStyle(scheduler1.TimeLineStyle);
                    break;
            }
        }
        private void SetStyle(Style style)
        {
            if (!IsLoaded || scheduler1.Style == style)
            {
                return;
            }
            scheduler1.BeginUpdate();
            try
            {
                scheduler1.ChangeStyle(style);
            }
            finally
            {
                scheduler1_LayoutUpdated(null, null);
                // Always call EndUpdate to apply all changes.
                scheduler1.EndUpdate();
            }
            UpdateCalendarSelection();
        }

        // Detect whether or not this application has the requested permission
        private bool IsPermissionGranted(CodeAccessPermission requestedPermission)
        {
            try
            {
                // Try and get this permission
                requestedPermission.Demand();
                return true;
            }
            catch
            {
                return false;
            }
        }

        void Import_Click(Object sender, RoutedEventArgs e)
        {
            if (IsPermissionGranted(new FileIOPermission(PermissionState.Unrestricted)))
            {
                // Import previously save data from .xml, .ics or .dat file.
                C1Scheduler.ImportCommand.Execute(null, scheduler1);
            }
            else
            {
                try
                {
                    IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                    using (IsolatedStorageFileStream stream =
                        new IsolatedStorageFileStream("C1ScheduleData.xml", FileMode.Open, storage))
                    {
                        scheduler1.DataStorage.Import(stream, FileFormatEnum.XML);
                    }
                }
                catch
                {
                    MessageBox.Show("There is no permission to load data.",
                            "ComponentOne WPF Sample Explorer", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                MessageBox.Show("There is no permission to load data. Data have been loaded from the isolated storage.",
                    "ComponentOne WPF Sample Explorer", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        void Export_Click(Object sender, RoutedEventArgs e)
        {
            if (IsPermissionGranted(new FileIOPermission(PermissionState.Unrestricted)))
            {
                // Export all data to file.
                C1Scheduler.ExportCommand.Execute(null, scheduler1);
            }
            else
            {
                try
                {
                    IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                    using (IsolatedStorageFileStream stream =
                        new IsolatedStorageFileStream("C1ScheduleData.xml", FileMode.Create, storage))
                    {
                        scheduler1.DataStorage.Export(stream, FileFormatEnum.XML);
                    }
                }
                catch
                {
                    MessageBox.Show("There is no permission to save data.",
                        Application.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                MessageBox.Show("There is no permission to save file. Data have been saved to the isolated storage.",
                    Application.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        void selected_date_changed(object sender, CalendarSelectionChangedEventArgs e)
        {
            if (!_updatingSelection)
            {
                var calendar = sender as C1Calendar;
                if (calendar?.SelectedDate == default(DateTime))
                    return;

                scheduler1.VisibleDates.BeginUpdate();
                scheduler1.VisibleDates.Clear();
                foreach (var d in calendar.SelectedDates)
                {
                    scheduler1.VisibleDates.Add(d);
                }
                scheduler1.VisibleDates.EndUpdate();
            }
        }

        void UpdateCalendarSelection()
        {
            if ( !_updatingSelection && (calendar1.SelectedDates == null || 
                ( scheduler1.VisibleDates.Count != calendar1.SelectedDates.Count
                || (calendar1.SelectedDates.Count > 0 && scheduler1.VisibleDates[0] != calendar1.SelectedDates[0]))))
            {
                _updatingSelection = true;
                calendar1.SelectedDates = scheduler1.VisibleDates.ToList();
                _updatingSelection = false;
            }
        }
    }
}