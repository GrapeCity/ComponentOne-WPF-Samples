using C1.C1Schedule;
using C1.WPF.Calendar;
using C1.WPF.Schedule;
using SchedulerExplorer.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace SchedulerExplorer
{
    public partial class CustomDialogs : UserControl
    {
        bool _updatingSelection = false;
        public CustomDialogs()
        {
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
            InitializeComponent();
            Tag = AppResources.CustomDialogsDescription;
            // initialize time scale combo
            scale.Items.Add(TimeSpan.FromMinutes(10));
            scale.Items.Add(TimeSpan.FromMinutes(15));
            scale.Items.Add(TimeSpan.FromMinutes(20));
            scale.Items.Add(TimeSpan.FromMinutes(30));
            scale.Items.Add(TimeSpan.FromMinutes(60));
            scale.Items.Add(TimeSpan.FromHours(2));
            scale.SelectedIndex = 3;

            sched1.DataStorage.ContactStorage.ListChanged += new System.ComponentModel.ListChangedEventHandler(ContactStorage_ListChanged);
            sched1.LayoutUpdated += new EventHandler(scheduler1_LayoutUpdated);
            sched1.VisibleDates.CollectionChanged += VisibleDates_CollectionChanged;
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
                Contact c = sched1.DataStorage.ContactStorage.Contacts[e.NewIndex];
                // you have to fill at least MenuCaption as 
                // it's mapped to the Employees.LastName field which doesn't allow empty strings.
                c.MenuCaption = "test contact";
            }
        }

        // refresh buttons' state if Scheduler's layout has been changed
        // by Scheduler's commands defined in default templates.
        void scheduler1_LayoutUpdated(object sender, EventArgs e)
        {
            if (sched1.Style == sched1.MonthStyle)
            {
                views.SelectedIndex = 3;
                scale.IsEnabled = false;
            }
            else
            {
                if (sched1.Style == sched1.OneDayStyle)
                {
                    views.SelectedIndex = 0;
                }
                else if (sched1.Style == sched1.WorkingWeekStyle)
                {
                    views.SelectedIndex = 1;
                }
                else if (sched1.Style == sched1.WeekStyle)
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
            if (sched1 == null)
            {
                return;
            }
            switch (views.SelectedItems.FirstOrDefault())
            {
                case "Day":
                    SetStyle(sched1.OneDayStyle);
                    break;
                case "Work Week":
                    SetStyle(sched1.WorkingWeekStyle);
                    break;
                case "Week":
                    SetStyle(sched1.WeekStyle);
                    break;
                case "Month":
                    SetStyle(sched1.MonthStyle);
                    break;
                case "Time Line":
                    SetStyle(sched1.TimeLineStyle);
                    break;
            }
        }
        private void SetStyle(Style style)
        {
            if (!IsLoaded || sched1.Style == style)
            {
                return;
            }
            sched1.BeginUpdate();
            try
            {
                sched1.ChangeStyle(style);
            }
            finally
            {
                scheduler1_LayoutUpdated(null, null);
                // Always call EndUpdate to apply all changes.
                sched1.EndUpdate();
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
                C1Scheduler.ImportCommand.Execute(null, sched1);
            }
            else
            {
                try
                {
                    IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                    using (IsolatedStorageFileStream stream =
                        new IsolatedStorageFileStream("C1ScheduleData.xml", FileMode.Open, storage))
                    {
                        sched1.DataStorage.Import(stream, FileFormatEnum.XML);
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
                C1Scheduler.ExportCommand.Execute(null, sched1);
            }
            else
            {
                try
                {
                    IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                    using (IsolatedStorageFileStream stream =
                        new IsolatedStorageFileStream("C1ScheduleData.xml", FileMode.Create, storage))
                    {
                        sched1.DataStorage.Export(stream, FileFormatEnum.XML);
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
                sched1.VisibleDates.BeginUpdate();
                sched1.VisibleDates.Clear();

                foreach (var d in calendar.SelectedDates)
                {
                    sched1.VisibleDates.Add(d);
                }

                sched1.VisibleDates.EndUpdate();

            }
        }

        void UpdateCalendarSelection()
        {
            if (!_updatingSelection && (calendar1.SelectedDates == null ||
                (sched1.VisibleDates.Count != calendar1.SelectedDates.Count
                || (calendar1.SelectedDates.Count > 0 && sched1.VisibleDates[0] != calendar1.SelectedDates[0]))))
            {
                _updatingSelection = true;
                calendar1.SelectedDates = sched1.VisibleDates.ToList();
                _updatingSelection = false;
            }
        }
    }
}