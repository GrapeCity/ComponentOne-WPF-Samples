using C1.Schedule;
using SchedulerExplorer.Resources;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SchedulerExplorer
{
    public partial class Grouping : UserControl
    {
        public Grouping()
        {
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name);
            InitializeComponent();
            Tag = AppResources.GroupingDescription;
            cmbGroup.Items.Add("None");
            cmbGroup.Items.Add("Category");
            cmbGroup.Items.Add("Resource");
            cmbGroup.Items.Add("Contact");
            cmbGroup.SelectedIndex = 3;

            // add some resources
            Resource res = new Resource();
            res.Text = "Meeting room";
            res.Color = System.Drawing.Color.FromArgb(255, 218, 186, 198);
            sched1.DataStorage.ResourceStorage.Resources.Add(res);
            Resource res1 = new Resource();
            res1.Text = "Conference hall";
            res1.Color = System.Drawing.Color.FromArgb(255, 220, 236, 201);
            sched1.DataStorage.ResourceStorage.Resources.Add(res1);

            // add some contacts
            Contact cnt = new Contact();
            cnt.Text = "Andy Garcia";
            sched1.DataStorage.ContactStorage.Contacts.Add(cnt);
            Contact cnt1 = new Contact();
            cnt1.Text = "Nancy Drew";
            sched1.DataStorage.ContactStorage.Contacts.Add(cnt1);
            Contact cnt2 = new Contact();
            cnt2.Text = "Robert Clark";
            sched1.DataStorage.ContactStorage.Contacts.Add(cnt2);

            // add sample appointments
            Appointment app = new Appointment(DateTime.Today.AddHours(12), TimeSpan.FromHours(1));
            app.Subject = "Sales meeting";
            sched1.DataStorage.AppointmentStorage.Appointments.Add(app);
            app.Resources.Add(res);
            app.Links.Add(cnt1);
            app.Links.Add(cnt2);

            app = new Appointment(DateTime.Today.AddHours(14), TimeSpan.FromHours(3));
            app.Subject = "Retirement Planning Session";
            app.Body = "A retirement planning education session. Please attend if possible.";
            sched1.DataStorage.AppointmentStorage.Appointments.Add(app);
            app.Resources.Add(res1);
            app.Links.Add(cnt1);
            app.Links.Add(cnt2);
            app.Links.Add(cnt);

            app = new Appointment(DateTime.Today.AddHours(10), TimeSpan.FromMinutes(15));
            app.Subject = "Conference call";
            sched1.DataStorage.AppointmentStorage.Appointments.Add(app);
            app.Links.Add(cnt);
            sched1.LayoutUpdated += new EventHandler(sched1_LayoutUpdated);

            sched1.GroupBy = "Contact";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbGroup.SelectedIndex)
            {
                case 0:
                    sched1.GroupBy = string.Empty;
                    break;
                case 1:
                    sched1.GroupBy = "Category";
                    break;
                case 2:
                    sched1.GroupBy = "Resource";
                    break;
                case 3:
                    sched1.GroupBy = "Contact";
                    break;
            }
        }

        void sched1_LayoutUpdated(object sender, EventArgs e)
        {
            if (!IsLoaded)
            {
                return;
            }
            if (sched1.Style == sched1.MonthStyle)
            {
                views.SelectedIndex = 3;
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
            }
        }

        void views_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
            {
                return;
            }
            switch (views.SelectedValue)
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
                sched1_LayoutUpdated(null, null);
                // Always call EndUpdate to apply all changes.
                sched1.EndUpdate();
            }
        }
    }
}