using System.Threading;
using System.Windows.Controls;

namespace BasicControls
{
    /// <summary>
    /// Interaction logic for DateTimePickerAdvanced.xaml
    /// </summary>
    public partial class DateTimePickerAdvanced : UserControl
    {
        public DateTimePickerAdvanced()
        {
            InitializeComponent();
            // add some custom formats to dateFormatCombo
            dateFormatCombo.Items.Add("MM/dd/yyyy");
            dateFormatCombo.Items.Add("dd-MM-yyyy");
            int monthNameLength = Thread.CurrentThread.CurrentUICulture.DateTimeFormat.AbbreviatedMonthNames[1].Length;
            if (monthNameLength != 3)
            {
                // (most cultures use 3 symbols for abbreviated month name, but not all. For example, Japanese culture uses single digit)
                // in this case it's not safe to use abbreviated month names, as we can't guarantee the correct mask and correct parsing for the text input
                // use format with numbers instead
                dateFormatCombo.Items.Add("yyyy MM dd");
            }
            else
            {
                dateFormatCombo.Items.Add("yyyy MMM dd");
            }
            dateFormatCombo.SelectedIndex = 0;

            // add some custom formats to timeFormatCombo
            timeFormatCombo.Items.Add("HH:mm");
            timeFormatCombo.Items.Add("HH-mm-ss");
            timeFormatCombo.Items.Add("h:mm:ss tt");
            timeFormatCombo.Items.Add("h 'hours' mm 'minutes'");
            timeFormatCombo.SelectedIndex = 0;
        }

        private void dateFormatCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // use custom date format from dateFormatCombo.SelectedItem
            dateTimePicker.CustomDateFormat = (string)dateFormatCombo.SelectedItem;
            // use text mask acceptable for the selected date format
            if (dateFormatCombo.SelectedIndex == 0)
            {
                // format is "MM/dd/yyyy"
                dateTimePicker.DateMask = "00/00/0000";
            }
            else if (dateFormatCombo.SelectedIndex == 1)
            {
                // format is "dd-MM-yyyy"
                dateTimePicker.DateMask = "00-00-0000";
            }
            else
            {
                if (dateFormatCombo.SelectedItem.ToString() == "yyyy MMM dd")
                {
                    dateTimePicker.DateMask = "0000 LLL 00";
                }
                else
                {
                    // format is "yyy MM dd"
                    dateTimePicker.DateMask = "0000 00 00";
                }
            }
        }

        private void timeFormatCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // use custom time format from timeFormatCombo.SelectedItem
            dateTimePicker.CustomTimeFormat = (string)timeFormatCombo.SelectedItem;
            // use text mask acceptable for the selected time format
            if (timeFormatCombo.SelectedIndex == 0)
            {
                // format is "HH:mm"
                dateTimePicker.TimeMask = "00:00";
            }
            else if (timeFormatCombo.SelectedIndex == 1)
            {
                // format is "HH-mm-ss"
                dateTimePicker.TimeMask = "00-00-00";
            }
            else if (timeFormatCombo.SelectedIndex == 2)
            {
                if (!string.IsNullOrEmpty(Thread.CurrentThread.CurrentUICulture.DateTimeFormat.AMDesignator))
                {
                    // format is "h:mm:ss tt"
                    dateTimePicker.TimeMask = "00:00:00 aa";
                }
                else
                {
                    // format is "h:mm:ss tt", but current culture has no am/pm designators
                    dateTimePicker.TimeMask = "00:00:00";
                }
            }
            else
            {
                // format is "h 'hours' mm 'minute's"
                dateTimePicker.TimeMask = "00 hours 00 minutes";
            }
        }
    }
}
