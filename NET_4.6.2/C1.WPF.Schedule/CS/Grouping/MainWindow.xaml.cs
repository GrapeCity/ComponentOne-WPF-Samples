using System;
using System.Collections.Generic;
using System.Linq;
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
using C1.C1Schedule;

namespace Grouping
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name);
            InitializeComponent();
			cmbGroup.Items.Add("None");
			cmbGroup.Items.Add("Category");
			cmbGroup.Items.Add("Resource");
			cmbGroup.Items.Add("Contact");
			cmbGroup.SelectedIndex = 3;

            // add some resources
            Resource res = new Resource();
            res.Text = "Meeting room";
            res.Color = Color.FromArgb(255, 218, 186, 198);
            sched1.DataStorage.ResourceStorage.Resources.Add(res);
            Resource res1 = new Resource();
            res1.Text = "Conference hall";
            res1.Color = Color.FromArgb(255, 220, 236, 201);
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

            sched1.StyleChanged += new EventHandler<RoutedEventArgs>(sched1_StyleChanged);
            sched1_StyleChanged(null, null);
        }

        void sched1_StyleChanged(object sender, RoutedEventArgs e)
        {
            // update toolbar buttons state according to the current C1Scheduler view
            switch (sched1.ViewType)
            { 
                case C1.WPF.Schedule.ViewType.Day:
                    btnDay.IsChecked = true;
                    break;
                case C1.WPF.Schedule.ViewType.Month:
                    btnMonth.IsChecked = true;
                    break;
                case C1.WPF.Schedule.ViewType.TimeLine:
                    btnTimeLine.IsChecked = true;
                    break;
                case C1.WPF.Schedule.ViewType.Week:
                    btnWeek.IsChecked = true;
                    break;
                case C1.WPF.Schedule.ViewType.WorkingWeek:
                    btnWorkWeek.IsChecked = true;
                    break;
            }
        }

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			string str = (string)cmbGroup.SelectedItem;
			if (str == "None")
			{
				sched1.GroupBy = string.Empty;
			}
			else
			{
				sched1.GroupBy = str;
			}
		}

  	}
}
