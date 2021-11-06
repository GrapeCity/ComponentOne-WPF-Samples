using System.Windows.Controls;
using System.Windows.Media;
using System;
using C1.WPF.DateTimeEditors;
using System.Windows;
using DateTimeEditorsExplorer.Resources;

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
            Tag = AppResources.DatePickerTag;
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
