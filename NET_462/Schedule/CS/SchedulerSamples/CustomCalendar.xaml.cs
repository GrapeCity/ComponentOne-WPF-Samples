using System;
using System.Windows.Controls;

namespace SchedulerSamples
{
	/// <summary>
	/// Interaction logic for CustomCalendar.xaml
	/// </summary>
    public partial class CustomCalendar : UserControl
	{
		public CustomCalendar()
		{
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name);
            InitializeComponent();
            int year = DateTime.Today.Year;
            cal1.BoldedDates.Add(new DateTime(year, 1, 1));
            cal1.BoldedDates.Add(new DateTime(year, 1, 7));
            cal1.BoldedDates.Add(new DateTime(year, 3, 8));
            cal1.BoldedDates.Add(new DateTime(year, 2, 23));
            cal1.BoldedDates.Add(new DateTime(year, 6, 12));
            cal1.BoldedDates.Add(new DateTime(year, 11, 4));
            cal1.BoldedDates.Add(new DateTime(year, 8, 1));
            cal1.BoldedDates.Add(new DateTime(year, 5, 1));
            cal1.BoldedDates.Add(new DateTime(year, 5, 9));
        }
	}
}
