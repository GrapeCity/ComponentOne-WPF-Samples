using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SchedulerSamples
{
	/// <summary>
	/// Interaction logic for DateTimePicker.xaml
	/// </summary>
    public partial class CalendarSettings : UserControl
	{
		public CalendarSettings()
		{
			InitializeComponent();
            weekDays.SelectedItem = calendar1.CalendarHelper.WeekStart;
		}

		private void weekDays_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!IsLoaded)
			{
				return;
			}
			if ((DayOfWeek)weekDays.SelectedItem != calendar1.CalendarHelper.WeekStart)
			{
				calendar1.CalendarHelper.WeekStart = (DayOfWeek)weekDays.SelectedItem;
			}
		}

	}
}
