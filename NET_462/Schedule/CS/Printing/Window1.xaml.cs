using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace Printing
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		private PrintInfo _printInfo;

		public Window1()
		{
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name);
            InitializeComponent();

			Scheduler.VisualIntervalScale = TimeSpan.FromMinutes(30);            

			minutesGroup.Visibility = Visibility.Visible;
			
			Scheduler.CalendarHelper.StartDayTime = TimeSpan.Parse("09:00:00");
			Scheduler.Settings.FirstVisibleTime = TimeSpan.Parse("09:00:00");

			Scheduler.CalendarHelper.EndDayTime = TimeSpan.Parse("18:00:00");
			_printInfo = new PrintInfo(this.Scheduler);
		}

		private void rbChecked(object sender, RoutedEventArgs e)
		{
			ChangeStyle();
		}

		private void ChangeStyle()
		{
			if (!IsLoaded)
				return;
			Cursor curCursor = Cursor;
			Cursor = Cursors.Wait;
			Scheduler.BeginUpdate();
			try
			{
				if (oneDayRb.IsChecked.Value)
				{
					Scheduler.ChangeStyle(Scheduler.OneDayStyle);
					minutesGroup.Visibility = Visibility.Visible;
					minutesChecked(null, null);
				}
				else if (workingWeekRb.IsChecked.Value)
				{
					Scheduler.ChangeStyle(Scheduler.WorkingWeekStyle);
					minutesGroup.Visibility = Visibility.Visible;
					minutesChecked(null, null);
				}
				else if (weekRb.IsChecked.Value)
				{
					minutesGroup.Visibility = Visibility.Visible;
					Scheduler.ChangeStyle(Scheduler.WeekStyle);
					minutesChecked(null, null);
				}
				else
				{
					minutesGroup.Visibility = Visibility.Collapsed;
					Scheduler.ChangeStyle(Scheduler.MonthStyle);
				}                
			}
			finally
			{
				Scheduler.EndUpdate();
				Cursor = curCursor;
			}
		}

		void minutesChecked(object sender, RoutedEventArgs e)
		{
			if (!IsLoaded)
				return;
			Cursor curCursor = Cursor;
			Cursor = Cursors.Wait;
			try
			{
				if (m60.IsChecked.Value)
					Scheduler.VisualIntervalScale = TimeSpan.FromMinutes(60);
				else if (m30.IsChecked.Value)
					Scheduler.VisualIntervalScale = TimeSpan.FromMinutes(30);
				else if (m15.IsChecked.Value)
					Scheduler.VisualIntervalScale = TimeSpan.FromMinutes(15);
			}
			finally
			{
				Cursor = curCursor;
			}
		}

		private void btnToday_Click(object sender, RoutedEventArgs e)
		{
			DateTime targetDate = DateTime.Today;
			if (Scheduler.Style != Scheduler.OneDayStyle)
			{
				// find the first week day
				while (targetDate.DayOfWeek != Scheduler.CalendarHelper.WeekStart)
				{
					targetDate = targetDate.AddDays(-1);
				}
			}

			// navigate to the current week 
			Scheduler.VisualStartTime = targetDate;
		}
		
		private void btnPrev_Click(object sender, RoutedEventArgs e)
		{
			if (Scheduler.Style == Scheduler.WorkingWeekStyle || Scheduler.Style == Scheduler.WeekStyle)
			{
				Scheduler.VisualStartTime = Scheduler.VisualStartTime.AddDays(-7);
			}
			else
			{
				Scheduler.VisualStartTime = Scheduler.VisualStartTime.Add(Scheduler.VisualTimeSpan.Negate());
			}

		}
		private void btnNext_Click(object sender, RoutedEventArgs e)
		{
			if (Scheduler.Style == Scheduler.WorkingWeekStyle || Scheduler.Style == Scheduler.WeekStyle)
			{
				Scheduler.VisualStartTime = Scheduler.VisualStartTime.AddDays(7);
			}
			else
			{
				Scheduler.VisualStartTime = Scheduler.VisualStartTime.Add(Scheduler.VisualTimeSpan);
			}
		}

		private void Scheduler_LayoutUpdated(object sender, EventArgs e)
		{
			// refresh buttons' state if Scheduler's layout has been changed
			// by Scheduler's commands defined in default templates.
			if (Scheduler.Style == Scheduler.OneDayStyle)
			{
				oneDayRb.IsChecked = true;
				minutesGroup.Visibility = Visibility.Visible;
			}
			else if (Scheduler.Style == Scheduler.WorkingWeekStyle)
			{
				workingWeekRb.IsChecked = true;
				minutesGroup.Visibility = Visibility.Visible;
			}
			else if (Scheduler.Style == Scheduler.WeekStyle)
			{
				weekRb.IsChecked = true;
				minutesGroup.Visibility = Visibility.Visible;
			}
			else
			{
				minutesGroup.Visibility = Visibility.Collapsed;
				monthRb.IsChecked = true;
			}
		}
		
		private void Print_Click(object sender, RoutedEventArgs e)
		{
			_printInfo.Print();
		}
		private void PrintPreview_Click(object sender, RoutedEventArgs e)
		{
			_printInfo.Preview();
		}
	}
}
