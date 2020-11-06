using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using C1.C1Preview;
using C1.C1Schedule;


namespace PrintDocTemplates
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		#region ** fields
		// C1PrintDocument controls
		C1PrintDocument _printDoc = new C1PrintDocument();
		// preview window
		PrintPreviewWindow _preview;
		#endregion

		#region ** ctor
		/// <summary>
		/// Initializes a ne instance of the <see cref="Window1"/> class.
		/// </summary>
		public Window1()
		{
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name);
            InitializeComponent();

			Scheduler.VisualIntervalScale = TimeSpan.FromMinutes(30);            

			minutesGroup.Visibility = Visibility.Visible;
			
			Scheduler.CalendarHelper.StartDayTime = TimeSpan.Parse("09:00:00");
			Scheduler.Settings.FirstVisibleTime = TimeSpan.Parse("09:00:00");

			Scheduler.CalendarHelper.EndDayTime = TimeSpan.Parse("18:00:00");

			// subscribe to the DocumentStarting event
			_printDoc.DocumentStarting += new EventHandler(_printDoc_DocumentStarting);
		}
		#endregion

		#region ** object model
		/// <summary>
		/// Gets or sets the <see cref="PrintPreviewWindow"/> object, used for previewing. 
		/// </summary>
		public PrintPreviewWindow PrintPreviewDialog
		{
			get
			{
				if (_preview == null)
				{
					_preview = new PrintPreviewWindow();
				}
				return _preview;
			}
			set
			{
				_preview = value;
			}
		}
		#endregion

		#region ** printing
		// adds assembly references and initializes document tags
		private void _printDoc_DocumentStarting(object sender, EventArgs e)
		{
			// add references needed for document scripts execution 
            _printDoc.ScriptingOptions.ExternalAssemblies.Add(Assembly.GetAssembly(typeof(C1.WPF.Schedule.C1Scheduler)).Location);
            _printDoc.ScriptingOptions.ExternalAssemblies.Add("WindowsBase.dll");
            string path = Assembly.GetAssembly(typeof(System.Windows.Media.Color)).Location;
			_printDoc.ScriptingOptions.ExternalAssemblies.Add(path);

            path = Assembly.GetAssembly(typeof(System.Collections.Specialized.INotifyCollectionChanged)).Location;
            _printDoc.ScriptingOptions.ExternalAssemblies.Add(path);

			// initialize document tags
			DateTime start = Scheduler.VisibleDates[0];
			DateTime end = Scheduler.VisibleDates[Scheduler.VisibleDates.Count - 1];

			Tag tag = _printDoc.Tags["StartDate"];
			if (tag != null && tag.Type == typeof(DateTime))
			{
				tag.Value = start;
			}
			tag = _printDoc.Tags["EndDate"];
			if (tag != null && tag.Type == typeof(DateTime))
			{
				tag.Value = end;
			}

			// show tag input form to user
			_printDoc.EditTags();

			tag = _printDoc.Tags["StartDate"];
			if (tag != null && tag.Type == typeof(DateTime))
			{
				start = (DateTime)tag.Value;
			}
			tag = _printDoc.Tags["EndDate"];
			if (tag != null && tag.Type == typeof(DateTime))
			{
				end = (DateTime)tag.Value;
			}

			tag = _printDoc.Tags["Appointment"];
			if (tag != null && tag.Type == typeof(Appointment) && Scheduler.SelectedAppointment != null)
			{
				tag.Value = Scheduler.SelectedAppointment;
			}

			tag = _printDoc.Tags["CalendarInfo"];
			if (tag != null && tag.Type == typeof(CalendarInfo))
			{
				tag.Value = Scheduler.CalendarHelper.Info;
			}

			tag = _printDoc.Tags["Appointments"];
			if (tag != null)
			{
				if (tag.Type == typeof(AppointmentCollection))
				{
					tag.Value = Scheduler.DataStorage.AppointmentStorage.Appointments;
				}
				else if (tag.Type == typeof(IList<Appointment>))
				{
					bool isAppointmentContext = _printDoc.DocumentFileName.Contains("Memo") || _printDoc.DocumentFileName.Contains("Details");
					if (isAppointmentContext && Scheduler.SelectedAppointment != null)
					{
						List<Appointment> list = new List<Appointment>();
						list.Add(Scheduler.SelectedAppointment);
						tag.Value = list;
					}
					else
					{
						// get appointments for the currently selected SchedulerGroupItem if any, 
						// or all appointments otherwise.
						AppointmentList list = Scheduler.DataStorage.AppointmentStorage.Appointments.GetOccurrences(
							Scheduler.SelectedGroupItem == null ? null : Scheduler.SelectedGroupItem.Owner,
							Scheduler.GroupBy, start, end.AddDays(1), true);
						list.Sort();
                        tag.Value = list;
                    }
				}
			}
		}

		// create and preview daily style
		private void day_Click(object sender, RoutedEventArgs e)
		{
			Utils.MakeDay(_printDoc);
			_printDoc.Save("day.c1d");
			// clear and re-load to be sure that all works when document is loaded from file
			_printDoc.Clear();
			_printDoc.Load("day.c1d");
			Preview();
		}

		// create and preview weekly style
		private void week_Click(object sender, RoutedEventArgs e)
		{
			Utils.MakeWeek(_printDoc);
			_printDoc.Save("week.c1d");
			// clear and re-load to be sure that all works when document is loaded from file
			_printDoc.Clear();
			_printDoc.Load("week.c1d");
			Preview();
		}

		// create and preview monthly style
		private void month_Click(object sender, RoutedEventArgs e)
		{
			Utils.MakeMonth(_printDoc);
			_printDoc.Save("month.c1d");
			// clear and re-load to be sure that all works when document is loaded from file
			_printDoc.Clear();
			_printDoc.Load("month.c1d");
			Preview();
		}

		// create and preview details style
		private void detail_Click(object sender, RoutedEventArgs e)
		{
			Utils.MakeDetails(_printDoc);
			_printDoc.Save("details.c1d");
			// clear and re-load to be sure that all works when document is loaded from file
			_printDoc.Clear();
			_printDoc.Load("details.c1d");
			Preview();
		}

		// create and preview memo style
		private void memo_Click(object sender, RoutedEventArgs e)
		{
			Utils.MakeMemo(_printDoc);
			_printDoc.Save("memo.c1d");
			// clear and re-load to be sure that all works when document is loaded from file
			_printDoc.Clear();
			_printDoc.Load("memo.c1d");
			Preview();
		}

		/// <summary>
		/// Shows C1PrintDocument content in a print preview dialog.
		/// </summary>
		private void Preview()
		{
			// generate the document
			if (PrintPreviewDialog != null)
			{
				_preview.Document = _printDoc.FixedDocumentSequence;
				_preview.ShowDialog();
				_preview.Activate();
				_preview = null;
			}
		}
		#endregion

		#region ** navigation
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
		#endregion
	}
}
