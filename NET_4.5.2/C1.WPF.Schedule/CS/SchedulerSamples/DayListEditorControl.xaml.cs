using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SchedulerSamples
{
	/// <summary>
	/// Interaction logic for DayListControl.xaml
	/// </summary>
	public partial class DayListEditorControl : UserControl
	{
		public DayListEditorControl()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty DaysProperty =
			DependencyProperty.Register("Days", typeof(ObservableCollection<DateTime>),
			typeof(DayListEditorControl),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnDaysChanged)));

		public ObservableCollection<DateTime> Days
		{
			get { return (ObservableCollection<DateTime>)base.GetValue(DaysProperty); }
			set { base.SetValue(DaysProperty, value); }
		}

		private static void OnDaysChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DayListEditorControl editor = (DayListEditorControl)d;
			if (e.NewValue != null)
			{
				editor.list.ItemsSource = (ObservableCollection<DateTime>)e.NewValue;
			}
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
            if (calendar.DateTime.HasValue)
            {
                DateTime date = calendar.DateTime.Value.Date;
                if (!Days.Contains(date))
                {
                    Days.Add(date);
                }
                Days = Days;
            }
		}

		private void btnRemove_Click(object sender, RoutedEventArgs e)
		{
			if (list.SelectedItems.Count > 0)
			{
				List<DateTime> l = new List<DateTime>();
				foreach (DateTime d in list.SelectedItems)
				{
					l.Add(d);
				}
				while (l.Count > 0)
				{
					DateTime date = l[0].Date;
					if (Days.Contains(date))
					{
						Days.Remove(date);
					}
					l.RemoveAt(0);
					Days = Days;
				}
			}
		}
	}
}
