using C1.Schedule;
using C1.WPF.Schedule;
using SchedulerExplorer.Resources;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace SchedulerExplorer
{
    public partial class CustomDialogs : UserControl
    {
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
        }

        void Import_Click(Object sender, RoutedEventArgs e)
        {
            // Import previously save data from .xml, .ics or .dat file.
            C1Scheduler.ImportCommand.Execute(null, sched1);
        }

        void Export_Click(Object sender, RoutedEventArgs e)
        {
            // Export all data to file.
            C1Scheduler.ExportCommand.Execute(null, sched1);
        }
    }
}