using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace BasicControls
{
    /// <summary>
    /// Interaction logic for DemoNumericBox.xaml
    /// </summary>
    public partial class DemoNumericBox : UserControl
    {
        public DemoNumericBox()
        {
            InitializeComponent();
        }

        public FrameworkElement C1ControlExplorerSampleEditingControl
        {
            get { return numericBox; }
        }

        #region Object model

        public bool IsEnabled
        {
            get
            {
                return numericBox.IsEnabled;
            }
            set
            {
                numericBox.IsEnabled = value;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return numericBox.IsReadOnly;
            }
            set
            {
                numericBox.IsReadOnly = value;
            }
        }

        public bool AllowNull
        {
            get
            {
                return numericBox.AllowNull;
            }
            set
            {
                numericBox.AllowNull = value;
            }
        }

        public double Value
        {
            get
            {
                return numericBox.Value;
            }
            set
            {
                numericBox.Value = value;
            }
        }

        public string Format
        {
            get
            {
                return numericBox.Format;
            }
            set
            {
                numericBox.Format = value;
            }
        }

        public double Increment
        {
            get
            {
                return numericBox.Increment;
            }
            set
            {
                numericBox.Increment = value;
            }
        }

        public double Minimum
        {
            get
            {
                return numericBox.Minimum;
            }
            set
            {
                numericBox.Minimum = value;
            }
        }

        public double Maximum
        {
            get
            {
                return numericBox.Maximum;
            }
            set
            {
                numericBox.Maximum = value;
            }
        }

        public int Delay
        {
            get
            {
                return numericBox.Delay;
            }
            set
            {
                numericBox.Delay = value;
            }
        }

        public int Interval
        {
            get
            {
                return numericBox.Interval;
            }
            set
            {
                numericBox.Interval = value;
            }
        }
        
        public bool ShowButtons
        {
            get
            {
                return numericBox.ShowButtons;
            }
            set
            {
                numericBox.ShowButtons = value;
            }
        }

        public TextAlignment TextAlignment
        {
            get
            {
                return numericBox.TextAlignment;
            }
            set
            {
                numericBox.TextAlignment = value;
            }
        }

        public object Watermark
        {
            get
            {
                return numericBox.Watermark;
            }
            set
            {
                numericBox.Watermark = value;
            }
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return numericBox.CornerRadius;
            }
            set
            {
                numericBox.CornerRadius = value;
            }
        }

        #region ** ClearStyle properties

        public Brush DEMO_Background
        {
            get
            {
                return numericBox.Background;
            }
            set
            {
                numericBox.Background = value;
            }
        }

        public Brush DEMO_Foreground
        {
            get
            {
                return numericBox.Foreground;
            }
            set
            {
                numericBox.Foreground = value;
            }
        }

        public Brush DEMO_BorderBrush
        {
            get
            {
                return numericBox.BorderBrush;
            }
            set
            {
                numericBox.BorderBrush = value;
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return numericBox.MouseOverBrush;
            }
            set
            {
                numericBox.MouseOverBrush = value;
            }
        }

        public Brush FocusBrush
        {
            get
            {
                return numericBox.FocusBrush;
            }
            set
            {
                numericBox.FocusBrush = value;
            }
        }

        public Brush CaretBrush
        {
            get
            {
                return numericBox.CaretBrush;
            }
            set
            {
                numericBox.CaretBrush = value;
            }
        }

        public Brush SelectionBackground
        {
            get
            {
                return numericBox.SelectionBackground;
            }
            set
            {
                numericBox.SelectionBackground = value;
            }
        }

        public Brush SelectionForeground
        {
            get
            {
                return numericBox.SelectionForeground;
            }
            set
            {
                numericBox.SelectionForeground = value;
            }
        }

        #endregion

        #endregion
    }
}
