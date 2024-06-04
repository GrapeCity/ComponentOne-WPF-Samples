using System;
using System.Windows;
using System.Windows.Controls;
using C1.Schedule;

namespace SchedulerSamples
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class WorkDaysEditorControl : UserControl
	{
		public WorkDaysEditorControl()
		{
			InitializeComponent();
		}
		
		public static readonly DependencyProperty WorkDaysProperty =
			DependencyProperty.Register("WorkDays", typeof(WorkDays), 
			typeof(WorkDaysEditorControl), 
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnWorkDaysChanged)));

		public WorkDays WorkDays
		{
			get { return (WorkDays)base.GetValue(WorkDaysProperty); }
			set { base.SetValue(WorkDaysProperty, value); }
		}

		private static void OnWorkDaysChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			WorkDaysEditorControl editor = (WorkDaysEditorControl)d;
			if (e.NewValue != null)
			{
				WorkDays days = e.NewValue as WorkDays;
				editor.chFriday.IsChecked = days.Contains(DayOfWeek.Friday);
				editor.chMonday.IsChecked = days.Contains(DayOfWeek.Monday);
				editor.chSaturday.IsChecked = days.Contains(DayOfWeek.Saturday);
				editor.chSunday.IsChecked = days.Contains(DayOfWeek.Sunday);
				editor.chThursday.IsChecked = days.Contains(DayOfWeek.Thursday);
				editor.chTuesday.IsChecked = days.Contains(DayOfWeek.Tuesday);
				editor.chWednesday.IsChecked = days.Contains(DayOfWeek.Wednesday);
			}
		}

		private void ch_Changed(object sender, RoutedEventArgs e)
		{
			WorkDays days = new WorkDays();
			days.AddRange(WorkDays);
			CheckBox box = e.Source as CheckBox;
			DayOfWeek day = (DayOfWeek)box.Content;
			bool changed = false;
			if (box.IsChecked.Value && !days.Contains(day))
			{
				days.Add(day);
				changed = true;
			}
			if (!box.IsChecked.Value && days.Contains(day))
			{
				days.Remove(day);
				changed = true;
			}
			if (changed)
			{
				SetValue(WorkDaysProperty, days);
			}
		}
	}
}
