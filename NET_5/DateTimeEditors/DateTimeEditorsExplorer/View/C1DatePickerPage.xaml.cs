using System.Windows.Controls;
using System.Windows.Media;
using System;
using C1.WPF.DateTimeEditors;
using System.Windows;

namespace DateTimeEditorsExplorer
{
    /// <summary>
    /// Interaction logic for DemoDatePicker.xaml
    /// </summary>
    public partial class DemoDatePicker : UserControl
    {
        public DemoDatePicker()
        {
            InitializeComponent();
            Tag = "The C1DatePicker control provides a simple and intuitive UI for selecting date values. Click the C1DatePickers drop-down arrow and select a date in the calendar.";
        }

        private void displaymode_SelectedIndexChanged(object sender, C1.WPF.Core.PropertyChangedEventArgs<int> e)
        {
            if (displaymode.SelectedIndex == 0)
                datePicker.DisplayMode = CalendarMode.Month;
            else if (displaymode.SelectedIndex == 1)
                datePicker.DisplayMode = CalendarMode.Year;
            else
                datePicker.DisplayMode = CalendarMode.Decade;
        }
    }
}
