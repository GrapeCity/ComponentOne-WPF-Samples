using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using C1.C1Preview;
using C1.C1Schedule;
using C1.WPF.Schedule;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Printing
{
	/// <summary>
	/// The <see cref="PrintStyle"/> class represents the single printing style for a schedule control.
	/// </summary>
	public class PrintStyle 
	{
		#region ** fields
		private PrintStyleCollection _owner = null;
		
		private string _description = String.Empty;
		private string _styleSource = String.Empty;
		#endregion

		#region ** ctor
		/// <summary>
		/// Initializes the new <see cref="PrintStyle"/> object.
		/// </summary>
		public PrintStyle()
		{
			StyleName = String.Empty;
			DocumentFormat = C1DocumentFormatEnum.C1d;
			Context = PrintContextType.DateRange;
		}
		#endregion

		#region ** public properties
		/// <summary>
		/// Gets or sets the <see cref="String"/> value, determining the unique style name for using in
		/// <see cref="PrintStyleCollection"/>. 
		/// </summary>
		[DefaultValue("")]
		public string StyleName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the <see cref="String"/> value, determining style description.
		/// </summary>
		[DefaultValue("")]
		public string Description
		{
			get
			{
				if (String.IsNullOrEmpty(_description))
				{
					_description = StyleName;
				}
				return _description;
			}
			set
			{
				_description = value;
			}
		}
		
		/// <summary>
		/// Gets an <see cref="int"/> value determining the format of source document.
		/// Returns 0 for .c1d and 1 for .c1dx documents.
		/// </summary>
		public C1DocumentFormatEnum DocumentFormat
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets a <see cref="PrintContextType"/> value, specifying whether the current
		/// <see cref="PrintStyle"/> objects displays the single appointment or appointments
		/// of the specified date range.
		/// </summary>
		[DefaultValue(PrintContextType.DateRange)]
		public PrintContextType Context
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the <see cref="String"/> value determining the source of
		/// C1PrintDocument template. It might be the name of .c1d or .c1dx file or the name
		/// of resource.
		/// </summary>
		[DefaultValue("")]
		public string StyleSource
		{
			get
			{
				return _styleSource;
			}
			set
			{
				_styleSource = value;
				if (_styleSource.EndsWith(".c1dx", StringComparison.OrdinalIgnoreCase))
				{
					DocumentFormat = C1DocumentFormatEnum.C1dx;
				}
				else
				{
					DocumentFormat = C1DocumentFormatEnum.C1d;
				}
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="Uri"/> of the current <see cref="PrintStyle"/> preview image.
		/// </summary>
		public BitmapImage PreviewImage
		{
			get;
			set;
		}
		#endregion

		#region ** public methods
		/// <summary>
		/// Loads style definition to the specified C1PrintDocument control.
		/// </summary>
		/// <param name="printDoc">The <see cref="PrintDocumentWrapper"/> object.</param>
		/// <returns>Returns true at successful loading; false - otherwise.</returns>
		public bool Load(C1PrintDocument printDoc)
		{
			if (String.IsNullOrEmpty(StyleSource))
			{
				return false;
			}
			printDoc.Clear();
			
			// load document
			Stream sr = ResourceLoader.GetStream(StyleSource);
			if (sr != null)
			{
				printDoc.Load(sr, DocumentFormat);
				sr.Dispose();
			}
			else
			{
				printDoc.Load(StyleSource, DocumentFormat);
			}

			if (String.IsNullOrEmpty(StyleName))
			{
				// fill name and description from document
				StyleName = (string)printDoc.DocumentInfo.Title;
				if (String.IsNullOrEmpty(StyleName))
				{
					StyleName = _owner.GetUniqueStyleName();
				}
			}
			if (String.IsNullOrEmpty(_description))
			{
				_description = (string)printDoc.DocumentInfo.Subject;
			}
			return true;
		}
		#endregion

		#region private stuff
		internal PrintStyleCollection Owner
		{
			get
			{
				return _owner;
			}
			set
			{
				_owner = value;
			}
		}

		// Set date range tags in a separate method, as it might
		// be useful to set them before editing style in print setup forms.
		internal void SetDateRangeTags(C1PrintDocument printDoc, DateTime start, DateTime end)
		{
			Tag tag = null;
			TagCollection Tags = printDoc.Tags;
			if ((Context & PrintContextType.DateRange) != 0)
			{
				if (Tags.IndexOfName("StartDate") >= 0)
				{
					tag = Tags["StartDate"];
					if (tag != null && tag.Type == typeof(DateTime))
					{
						tag.Value = start;
					}
				}
				if (Tags.IndexOfName("EndDate") >= 0)
				{
					tag = Tags["EndDate"];
					if (tag != null && tag.Type == typeof(DateTime))
					{
						tag.Value = end;
					}
				}
			}
		}

		// Use this method to set all tags at document starting.
		// Note: start and end parameters are just default values. 
		// If document contains StartDate and EndDate tags, 
		// actual start and end values go from the tags.
		internal void SetupTags(C1PrintDocument printDoc, AppointmentCollection appointmentCollection, 
			List<Appointment> appointmentList, DateTime start, DateTime end, bool hidePrivateAppointments, CalendarInfo calendarInfo)
		{
			Tag tag = null;
			TagCollection Tags = printDoc.Tags;
			if (Tags.IndexOfName("Appointment") >= 0)
			{
				tag = Tags["Appointment"];
				if (tag != null && tag.Type == typeof(Appointment)
					&& appointmentList != null && appointmentList.Count > 0)
				{
					tag.Value = appointmentList[0];
				}
			}
			if (Tags.IndexOfName("Appointments") >= 0)
			{
				tag = Tags["Appointments"];
				if (tag != null)
				{
					if (tag.Type == typeof(AppointmentCollection))
					{
						tag.Value = appointmentCollection;
					}
					else if (tag.Type == typeof(IList<Appointment>))
					{
						if ((Context & PrintContextType.Appointment) != 0
							&& appointmentList != null)
						{
							appointmentList.Sort(AppointmentComparer.Default);
							tag.Value = appointmentList;
						}
						else
						{
							Tag tag1 = null;
							if (Tags.IndexOfName("StartDate") >= 0)
							{
								tag1 = Tags["StartDate"];
								if (tag1 != null && tag1.Type == typeof(DateTime))
								{
									start = (DateTime)tag1.Value;
								}
							}
							if (Tags.IndexOfName("EndDate") >= 0)
							{
								tag1 = Tags["EndDate"];
								if (tag1 != null && tag1.Type == typeof(DateTime))
								{
									end = (DateTime)tag1.Value;
								}
							}
							C1Scheduler scheduler = ((C1ScheduleStorage)appointmentCollection.ParentStorage.ScheduleStorage).Scheduler;
							// get appointments for the currently selected SchedulerGroupItem if any, 
							// or all appointments otherwise.
							AppointmentList list = appointmentCollection.GetOccurrences(
								scheduler.SelectedGroupItem == null ? null : scheduler.SelectedGroupItem.Owner, 
								scheduler.GroupBy, start, end.AddDays(1), !hidePrivateAppointments);
							list.Sort();
							tag.Value = list;
						}
					}
				}
			}
			if (Tags.IndexOfName("CalendarInfo") >= 0)
			{
				tag = Tags["CalendarInfo"];
				if (tag != null && tag.Type == typeof(CalendarInfo))
				{
					tag.Value = calendarInfo;
				}
			}
			if (Tags.IndexOfName("HidePrivateAppointments") >= 0)
			{
				tag = Tags["HidePrivateAppointments"];
				if (tag != null && tag.Type == typeof(bool))
				{
					tag.Value = hidePrivateAppointments;
				}
			}
            // update string tags
            foreach (Tag t in Tags)
            {
                if (t.Type == typeof(string))
                {
                    string key = t.Name.ToLower();
                    switch (key)
                    {
                        case "start":
                            key = "startTime";
                            break;
                        case "end":
                            key = "endTime";
                            break;
                        case "showtimeas":
                            key = "showTimeAs";
                            break;
                        case "contacts":
                            key = "contactsButton";
                            break;
                        case "resources":
                            key = "resourcesButton";
                            break;
                        case "categories":
                            key = "categoriesButton";
                            break;
                    }
                    if (!string.IsNullOrEmpty(key))
                    {
                        // try find localized value from the Scheduler resources
                        string str = C1.WPF.Localization.C1Localizer.GetString("EditAppointment", key, (string)t.Value).Trim();
                        if (!string.IsNullOrEmpty(str))
                        {
                            while (str.EndsWith("."))
                            {
                                str = str.Substring(0, str.Length - 1);
                            }
                            if ( !str.EndsWith(":"))
                            {
                                str += ":";
                            }
                            t.Value = str;
                        }
                    }
                }
            }
		}
 		#endregion
	}

	/// <summary>
	/// The <see cref="PrintStyleCollection"/> class represents the keyed collection of scheduler printing styles.
	/// The <see cref="PrintStyle.StyleName"/> property is used as a collection key.
	/// </summary>
	public class PrintStyleCollection : KeyedCollection<string, PrintStyle>
	{
		#region ** fields
		private static string _defaultStyleName = "Unknown Style";
		#endregion

		#region ** public interface
		/// <summary>
		/// Fills collection with default printing styles.
		/// </summary>
		public void LoadDefaults()
		{
			Clear();

			PrintStyle st = new PrintStyle();
			st.StyleName = "Daily";
			st.Description = "Daily Style";
			st.StyleSource = "day.c1d"; // the name of C1Schedule resource containing the style
			st.Owner = this;
			BitmapImage bi = new BitmapImage();
			bi.BeginInit();
			bi.UriSource = new Uri(@"/Images/daily.png", UriKind.RelativeOrAbsolute);
			bi.EndInit();
			st.PreviewImage = bi;
			Add(st);

			st = new PrintStyle();
			st.StyleName = "Week";
			st.Description = "Weekly Style";
			st.StyleSource = "week.c1d";
			st.Owner = this;
			bi = new BitmapImage();
			bi.BeginInit();
			bi.UriSource = new Uri(@"/Images/week.png", UriKind.RelativeOrAbsolute);
			bi.EndInit();
			st.PreviewImage = bi;
			Add(st);

			st = new PrintStyle();
			st.StyleName = "Month";
			st.Description = "Monthly Style";
			st.StyleSource = "month.c1d";
			st.Owner = this;
			bi = new BitmapImage();
			bi.BeginInit();
			bi.UriSource = new Uri(@"/Images/month.png", UriKind.RelativeOrAbsolute);
			bi.EndInit();
			st.PreviewImage = bi;
			Add(st);

			st = new PrintStyle();
			st.StyleName = "Details";
			st.Description = "Details Style";
			st.StyleSource = "details.c1d";
			st.Owner = this;
			bi = new BitmapImage();
			bi.BeginInit();
			bi.UriSource = new Uri(@"/Images/details.png", UriKind.RelativeOrAbsolute);
			bi.EndInit();
			st.PreviewImage = bi;
			Add(st);

			st = new PrintStyle();
			st.StyleName = "Memo";
			st.Context = PrintContextType.Appointment;
			st.Description = "Memo Style";
			st.StyleSource = "memo.c1d";
			st.Owner = this;
			bi = new BitmapImage();
			bi.BeginInit();
			bi.UriSource = new Uri(@"/Images/memo.png", UriKind.RelativeOrAbsolute);
			bi.EndInit();
			st.PreviewImage = bi;
			Add(st);
		}

		/// <summary>
		/// Adds a set of <see cref="PrintStyle"/> objects to the collection.
		/// </summary>
		/// <param name="items">An array of type <see cref="PrintStyle"/>
		/// that contains the print styles to add. </param>
		public void AddRange(PrintStyle[] items)
		{
			Clear();
			foreach (PrintStyle item in items)
			{
				Add(item);
			}
		}

		/// <summary>
		/// Returns the <see cref="String"/> value which can be used
		/// as unique style name.
		/// </summary>
		public string GetUniqueStyleName()
		{
			string name = _defaultStyleName;
			int index = 0;
			while (Contains(name))
			{
				index++;
				name = _defaultStyleName + " " + index.ToString();
			}
			return name;
		}
		#endregion

		#region ** private stuff
		/// <summary>
		/// Extracts the key from the specified item.
		/// </summary>
		/// <param name="item">The <see cref="PrintStyle"/> object from which to extract the key.</param>
		/// <returns>The key for the specified item.</returns>
		protected override string GetKeyForItem(PrintStyle item)
		{
			return item.StyleName;
		}

		/// <summary>
		/// Inserts the specified item in the collection at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index where the item is to be inserted.</param>
		/// <param name="item">The item to insert in the collection.</param>
		protected override void InsertItem(int index, PrintStyle item)
		{
			item.Owner = this;
			base.InsertItem(index, item);
		}
		#endregion
	}
}