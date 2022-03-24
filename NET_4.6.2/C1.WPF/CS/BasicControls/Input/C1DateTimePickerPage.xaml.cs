using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System;
using C1.WPF.DateTimeEditors;

namespace BasicControls
{
    /// <summary>
    /// Interaction logic for DemoDateTimePicker.xaml
    /// </summary>
    public partial class DemoDateTimePicker : UserControl
    {
        public DemoDateTimePicker()
        {
            InitializeComponent();
        }

        #region Object model

        public bool AllowNull
        {
            get
            {
                return picker.AllowNull;
            }
            set
            {
                picker.AllowNull = value;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return picker.IsEnabled;
            }
            set
            {
                picker.IsEnabled = value;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return picker.IsReadOnly;
            }
            set
            {
                picker.IsReadOnly = value;
            }
        }

        public DayOfWeek FirstDayOfWeek
        {
            get
            {
                return picker.FirstDayOfWeek;
            }
            set
            {
                picker.FirstDayOfWeek = value;
            }
        }

        public DateTime MinDate
        {
            get
            {
                return picker.MinDate;
            }
            set
            {
                picker.MinDate = value;
            }
        }

        public DateTime MaxDate
        {
            get
            {
                return picker.MaxDate;
            }
            set
            {
                picker.MaxDate = value;
            }
        }

        public C1DateTimePickerEditMode EditMode
        {
            get
            {
                return picker.EditMode;
            }
            set
            {
                picker.EditMode = value;
            }
        }

        public C1DatePickerFormat DateFormat
        {
            get
            {
                return picker.DateFormat;
            }
            set
            {
                picker.DateFormat = value;
            }
        }

        public C1TimeEditorFormat TimeFormat
        {
            get
            {
                return picker.TimeFormat;
            }
            set
            {
                picker.TimeFormat = value;
            }
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return picker.CornerRadius;
            }
            set
            {
                picker.CornerRadius = value;
            }
        }

        public TimeSpan TimeIncrement
        {
            get
            {
                return picker.TimeIncrement;
            }
            set
            {
                picker.TimeIncrement = value;
            }
        }

        #region ** ClearStyle properties

        public Brush DEMO_Background
        {
            get
            {
                return picker.Background;
            }
            set
            {
                picker.Background = value;
            }
        }

        public Brush DEMO_Foreground
        {
            get
            {
                return picker.Foreground;
            }
            set
            {
                picker.Foreground = value;
            }
        }

        public Brush DEMO_BorderBrush
        {
            get
            {
                return picker.BorderBrush;
            }
            set
            {
                picker.BorderBrush = value;
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return picker.MouseOverBrush;
            }
            set
            {
                picker.MouseOverBrush = value;
            }
        }

        public Brush PressedBrush
        {
            get
            {
                return picker.PressedBrush;
            }
            set
            {
                picker.PressedBrush = value;
            }
        }

        public Brush SelectionBackground
        {
            get
            {
                return picker.SelectionBackground;
            }
            set
            {
                picker.SelectionBackground = value;
            }
        }

        public Brush SelectionForeground
        {
            get
            {
                return picker.SelectionForeground;
            }
            set
            {
                picker.SelectionForeground = value;
            }
        }

        public Brush FocusBrush
        {
            get
            {
                return picker.FocusBrush;
            }
            set
            {
                picker.FocusBrush = value;
            }
        }

        public Brush CaretBrush
        {
            get
            {
                return picker.CaretBrush;
            }
            set
            {
                picker.CaretBrush = value;
            }
        }

        public Brush ButtonBackground
        {
            get
            {
                return picker.ButtonBackground;
            }
            set
            {
                picker.ButtonBackground = value;
            }
        }

        public Brush ButtonForeground
        {
            get
            {
                return picker.ButtonForeground;
            }
            set
            {
                picker.ButtonForeground = value;
            }
        }

        #endregion

        #endregion
    }
}
