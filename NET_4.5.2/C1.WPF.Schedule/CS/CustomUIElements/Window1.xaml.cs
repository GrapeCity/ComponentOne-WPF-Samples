using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using C1.C1Schedule;

namespace CustomUIElements
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		public Window1()
		{
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name);
            InitializeComponent();

			Scheduler.NextAppointmentText = "NEXT";
			Scheduler.PreviousAppointmentText = "PREVIOUS";
			
			Scheduler.VisualIntervalScale = TimeSpan.FromMinutes(30);            

			Scheduler.CalendarHelper.StartDayTime = TimeSpan.Parse("09:00:00");
			Scheduler.Settings.FirstVisibleTime = TimeSpan.Parse("09:00:00");

			Scheduler.CalendarHelper.EndDayTime = TimeSpan.Parse("18:00:00");

            Appointment app = new Appointment(DateTime.Now, DateTime.Now.AddMinutes(45), "Test Appointment");
            app.Location = "Test location";
            Scheduler.DataStorage.AppointmentStorage.Appointments.Add(app);

            Scheduler.StyleChanged += new System.EventHandler<RoutedEventArgs>(Scheduler_StyleChanged);
            Scheduler_StyleChanged(null, null);
        }

        private void Scheduler_StyleChanged(object sender, RoutedEventArgs e)
        {
            // update toolbar buttons state according to the current C1Scheduler view
            if (Scheduler.Style == Scheduler.WorkingWeekStyle)
            {
                btnWorkWeek.IsChecked = true;
            }
            else if (Scheduler.Style == Scheduler.WeekStyle)
            {
                btnWeek.IsChecked = true;
            }
            else if (Scheduler.Style == Scheduler.MonthStyle)
            {
                btnMonth.IsChecked = true;
            }
            else if (Scheduler.Style == Scheduler.OneDayStyle)
            {
                btnDay.IsChecked = true;
            }
            else
            {
                btnTimeLine.IsChecked = true;
            }

        }
    }
}
