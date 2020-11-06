using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DynamicConditionalFormatting
{
    /// <summary>
    /// Interaction logic for VisualRangeSlider.xaml
    /// </summary>
    public partial class VisualRangeSlider : UserControl
    {
        //------------------------------------------------------------------
        #region ** fields

        List<double> _values;
        static List<double> _sampleValues;

        #endregion

        //------------------------------------------------------------------
        #region ** ctor

        static VisualRangeSlider()
        {
            var rnd = new Random(0);
            _sampleValues = new List<double>();
            for (int i = 0; i < 500; i++)
            {
                var val = (rnd.NextDouble() + rnd.NextDouble() + rnd.NextDouble()) / 3;
                val = val * val;
                _sampleValues.Add(val);
            }
        }
        public VisualRangeSlider()
        {
            InitializeComponent();
            _values = _sampleValues;

            BindToSliderProperty("Minimum", MinimumProperty);
            BindToSliderProperty("Maximum", MaximumProperty);
            BindToSliderProperty("LowerValue", LowerValueProperty);
            BindToSliderProperty("UpperValue", UpperValueProperty);

            _slider.ValueChanged += (s, e) =>
            {
                if (_txtRange != null)
                {
                    _txtRange.Text = string.Format("{0:n0} < x < {1:n0}", LowerValue, UpperValue);
                }

                if (ValueChanged != null)
                {
                    ValueChanged(this, e);
                }
            };
        }
        #endregion

        //------------------------------------------------------------------
        #region ** object model

        /// <summary>
        /// Gets or sets the minimum possible value for this range's LowerValue.
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double),
                typeof(VisualRangeSlider), new PropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the maximum possible value for this range's UpperValue.
        /// </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double),
                typeof(VisualRangeSlider), new PropertyMetadata(100.0));

        /// <summary>
        /// Gets or sets the lower value in this range.
        /// </summary>
        public double LowerValue
        {
            get { return (double)GetValue(LowerValueProperty); }
            set { SetValue(LowerValueProperty, value); }
        }
        public static readonly DependencyProperty LowerValueProperty =
            DependencyProperty.Register("LowerValue", typeof(double),
                typeof(VisualRangeSlider), new PropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the upper value in this range.
        /// </summary>
        public double UpperValue
        {
            get { return (double)GetValue(UpperValueProperty); }
            set { SetValue(UpperValueProperty, value); }
        }
        public static readonly DependencyProperty UpperValueProperty =
            DependencyProperty.Register("UpperValue", typeof(double),
                typeof(VisualRangeSlider), new PropertyMetadata(100.0));

        /// <summary>
        /// Gets or sets the brush used to fill the value histogram.
        /// </summary>
        public Brush Fill
        {
            get { return _histogram.Fill; }
            set { _histogram.Fill = value; }
        }

        /// <summary>
        /// Gets or sets the number of bins in the value histogram.
        /// </summary>
        public int BinCount
        {
            get { return Math.Max(1, (int)GetValue(BinCountProperty)); }
            set { SetValue(BinCountProperty, value); }
        }
        public static readonly DependencyProperty BinCountProperty =
            DependencyProperty.Register("BinCount", typeof(int),
                typeof(VisualRangeSlider), new PropertyMetadata(50, VisualRangeSlider.OnBinCountPropertyChanged));
        static void OnBinCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (VisualRangeSlider)d;
            ctl.Update();
        }

        /// <summary>
        /// Gets or sets the values used to build the histogram.
        /// </summary>
        public IEnumerable<double> HistogramValues
        {
            get { return _values; }
            set
            {
                _values.Clear();
                if (value != null)
                {
                    foreach (var d in value)
                    {
                        _values.Add(d);
                    }
                }
                if (_values.Count > 0)
                {
                    Minimum = _values.Min();
                    Maximum = _values.Max();
                }
                Update();
            }
        }

        /// <summary>
        /// Occurs when the user modifies the range.
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion

        private void _clear_Click(object sender, RoutedEventArgs e)
        {
            LowerValue = Minimum;
            UpperValue = Maximum;
        }

        //------------------------------------------------------------------
        #region ** implementation

        void BindToSliderProperty(string propName, DependencyProperty dp)
        {
            var b = new Binding(propName);
            b.Source = _slider;
            b.Mode = BindingMode.TwoWay;
            this.SetBinding(dp, b);
        }
        void Update()
        {
            // sanity
            if (_values == null || _values.Count == 0)
            {
                _histogram.Points = null;
                return;
            }

            // get range, bin size
            var min = _values.Min();
            var max = _values.Max();
            var range = max - min;
            var binSize = range / BinCount;

            // group values in bins
            var bins = new int[BinCount];
            foreach (var d in _values)
            {
                var bin = (int)((d - min) / range * BinCount);
                if (bin < 0 || bin >= bins.Length)
                {
                    bin = 0;
                }
                bins[bin]++;
            }

            // build polygon points
            var pc = new PointCollection();
            pc.Add(new Point(0, 0));
            for (int i = 0; i < BinCount; i++)
            {
                pc.Add(new Point(i, bins[i]));
                pc.Add(new Point(i + 1, bins[i]));
            }
            pc.Add(new Point(BinCount, 0));

            // update RangeSlider control
            _histogram.Points = pc;
            Minimum = min;
            Maximum = max;
            LowerValue = min;
            UpperValue = max;
        }

        #endregion
    }
}
