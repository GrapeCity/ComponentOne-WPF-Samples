using System.Windows.Controls;
using System.Windows.Media;

namespace BasicControls
{
    /// <summary>
    /// Interaction logic for DemoRangeSlider.xaml
    /// </summary>
    public partial class DemoRangeSlider : UserControl
    {
        public DemoRangeSlider()
        {
            InitializeComponent();
        }

        #region Object model

        public bool IsEnabled
        {
            get
            {
                return rangeSlider.IsEnabled;
            }
            set
            {
                rangeSlider.IsEnabled = value;
            }
        }

        public double LowerValue
        {
            get
            {
                return rangeSlider.LowerValue;
            }
            set
            {
                rangeSlider.LowerValue = value;
            }
        }

        public double UpperValue
        {
            get
            {
                return rangeSlider.UpperValue;
            }
            set
            {
                rangeSlider.UpperValue = value;
            }
        }

        public double Minimum
        {
            get
            {
                return rangeSlider.Minimum;
            }
            set
            {
                rangeSlider.Minimum = value;
            }
        }

        public double Maximum
        {
            get
            {
                return rangeSlider.Maximum;
            }
            set
            {
                rangeSlider.Maximum = value;
            }
        }

        public Orientation Orientation
        {
            get
            {
                return rangeSlider.Orientation;
            }
            set
            {
                rangeSlider.Orientation = value;
            }
        }

        #region ** ClearStyle properties

        public Brush DEMO_Background
        {
            get
            {
                return rangeSlider.Background;
            }
            set
            {
                rangeSlider.Background = value;
            }
        }

        public Brush DEMO_BorderBrush
        {
            get
            {
                return rangeSlider.BorderBrush;
            }
            set
            {
                rangeSlider.BorderBrush = value;
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return rangeSlider.MouseOverBrush;
            }
            set
            {
                rangeSlider.MouseOverBrush = value;
            }
        }

        public Brush FocusBrush
        {
            get
            {
                return rangeSlider.FocusBrush;
            }
            set
            {
                rangeSlider.FocusBrush = value;
            }
        }

        #endregion

        #endregion

    }
}
