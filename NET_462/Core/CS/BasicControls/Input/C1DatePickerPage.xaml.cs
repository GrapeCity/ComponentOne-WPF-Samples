using System.Windows.Controls;
using System.Windows.Media;
using System;
using C1.WPF.DateTimeEditors;
using System.Windows;

namespace BasicControls
{
    /// <summary>
    /// Interaction logic for DemoDatePicker.xaml
    /// </summary>
    public partial class DemoDatePicker : UserControl
    {
        public DemoDatePicker()
        {
            InitializeComponent();
        }

        #region Object model

        public bool AllowNull
        {
            get
            {
                return datePicker.AllowNull;
            }
            set
            {
                datePicker.AllowNull = value;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return datePicker.IsEnabled;
            }
            set
            {
                datePicker.IsEnabled = value;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return datePicker.IsReadOnly;
            }
            set
            {
                datePicker.IsReadOnly = value;
            }
        }

        public DayOfWeek FirstDayOfWeek
        {
            get
            {
                return datePicker.FirstDayOfWeek;
            }
            set
            {
                datePicker.FirstDayOfWeek = value;
            }
        }

        public C1DatePickerFormat SelectedDateFormat
        {
            get
            {
                return datePicker.SelectedDateFormat;
            }
            set
            {
                datePicker.SelectedDateFormat = value;
            }
        }

        public string CustomFormat
        {
            get
            {
                return datePicker.CustomFormat;
            }
            set
            {
                datePicker.CustomFormat = value;
            }
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return datePicker.CornerRadius;
            }
            set
            {
                datePicker.CornerRadius = value;
            }
        }

        public object Watermark
        {
            get
            {
                return datePicker.Watermark;
            }
            set
            {
                datePicker.Watermark = value;
            }
        }

        #endregion

        private void displaymode_SelectedIndexChanged(object sender, C1.WPF.PropertyChangedEventArgs<int> e)
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
