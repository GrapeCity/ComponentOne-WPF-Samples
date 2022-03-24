using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using C1.WPF;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    public partial class DataGridHistogramFilter : UserControl, IDataGridFilterUnity
    {
        //------------------------------------------------------------------
        #region ** fields

        private double _minimum = double.NaN;
        private double _maximum = double.NaN;
        private double _calculatedMinimum = 0;
        private double _calculatedMaximum = 1;
        private double _lowerValue = double.NaN;
        private double _upperValue = double.NaN;
        private List<double> _values;
        private DataGridFilterState _filter = null;
        private bool? _inverse = false;

        #endregion

        //------------------------------------------------------------------
        #region ** initialization

        public DataGridHistogramFilter()
        {
            InitializeComponent();

            _slider.SetBinding(C1RangeSlider.MinimumProperty, new Binding("Minimum") { Source = this });
            _slider.SetBinding(C1RangeSlider.MaximumProperty, new Binding("Maximum") { Source = this });
            _slider.SetBinding(C1RangeSlider.LowerValueProperty, new Binding("LowerValue") { Source = this, Mode = BindingMode.TwoWay });
            _slider.SetBinding(C1RangeSlider.UpperValueProperty, new Binding("UpperValue") { Source = this, Mode = BindingMode.TwoWay });
            _slider.SetBinding(C1RangeSlider.StyleProperty, new Binding("SliderStyle") { Source = this });
           // _histogram.SetBinding(Polygon.FillProperty, new Binding("ChartBrush") { Source = this });
            _histogram.SetBinding(Polygon.PointsProperty, new Binding("Points") { Source = this });
            _chkInverse.IsThreeState = false;
            _chkInverse.SetBinding(CheckBox.IsCheckedProperty, new Binding("Inverse") { Source = this, Mode = BindingMode.TwoWay });
        }

        #endregion

        //------------------------------------------------------------------
        #region ** object model

        public Style SliderStyle
        {
            get
            {
                return Inverse.HasValue && Inverse.Value ? Resources["InverseRangeSlider"] as Style : Resources["NormalRangeSlider"] as Style;
            }
        }

        public bool? Inverse
        {
            get
            {
                return _inverse;
            }
            set
            {
                _inverse = value;
                UpdateFilter();
                RaisePropertyChanged("Inverse");
                RaisePropertyChanged("SliderStyle");
            }
        }

        public double Minimum
        {
            get
            {
                return double.IsNaN(_minimum) ? _calculatedMinimum : _minimum;
            }
            set
            {
                _minimum = value;
                RaisePropertyChanged("Minimum");
            }
        }

        public double Maximum
        {
            get
            {
                return double.IsNaN(_maximum) ? _calculatedMaximum : _maximum;
            }
            set
            {
                _maximum = value;
                RaisePropertyChanged("Maximum");
            }
        }

        public double LowerValue
        {
            get
            {
                return double.IsNaN(_lowerValue) ? Minimum : _lowerValue;
            }
            set
            {
                _lowerValue = value;
                UpdateFilter();
                RaisePropertyChanged("LowerValue");
            }
        }

        public double UpperValue
        {
            get
            {
                return double.IsNaN(_upperValue) ? Maximum : _upperValue;
            }
            set
            {
                _upperValue = value;
                UpdateFilter();
                RaisePropertyChanged("UpperValue");
            }
        }

        public Brush ChartBrush
        {
            get { return (Brush)GetValue(ChartBrushProperty); }
            set { SetValue(ChartBrushProperty, value); }
        }

        public static readonly DependencyProperty ChartBrushProperty =
            DependencyProperty.Register("ChartBrush", typeof(Brush), typeof(DataGridHistogramFilter), new PropertyMetadata(new SolidColorBrush(Colors.Orange)));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(DataGridHistogramFilter), new PropertyMetadata(null, (d, a) => (d as DataGridHistogramFilter).UpdateHistogram()));

        public string ValueMemberPath
        {
            get { return (string)GetValue(ValueMemberPathProperty); }
            set { SetValue(ValueMemberPathProperty, value); }
        }

        public static readonly DependencyProperty ValueMemberPathProperty =
            DependencyProperty.Register("ValueMemberPath", typeof(string), typeof(DataGridHistogramFilter), new PropertyMetadata("", (d, a) => (d as DataGridHistogramFilter).UpdateHistogram()));

        public int BinCount
        {
            get { return Math.Max(1, (int)GetValue(BinCountProperty)); }
            set { SetValue(BinCountProperty, value); }
        }
        public static readonly DependencyProperty BinCountProperty =
            DependencyProperty.Register(
                "BinCount",
                typeof(int),
                typeof(DataGridHistogramFilter),
                new PropertyMetadata(50, (d, a) => (d as DataGridHistogramFilter).UpdateHistogram()));

        public PointCollection Points
        {
            get { return (PointCollection)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(PointCollection), typeof(DataGridHistogramFilter), new PropertyMetadata(null));


        #endregion

        //------------------------------------------------------------------
        #region ** implementation

        void UpdateHistogram()
        {
            // get items source
            var items = ItemsSource == null ? new List<object>() : ItemsSource.Cast<object>();
            try
            {
                _values = items.Select(o => C1DataGridFilterHelper.GetPropertyValue<double>(o, ValueMemberPath)).ToList();
            }
            catch
            {
                _values = new List<double>();
            }

            if (_values == null || _values.Count == 0)
            {
                Points = null;
                return;
            }

            // get range, bin count
            _calculatedMinimum = _values.Min();
            _calculatedMaximum = _values.Max();
            var min = Minimum;
            var max = Maximum;
            var range = max - min;
            var binSize = range / BinCount;

            // group values in bins
            var bins = new int[BinCount];
            foreach (var d in _values)
            {
                if (d >= min && d <= max)
                {
                    var bin = (int)((d - min) / range * BinCount);
                    if (bin < 0 || bin >= bins.Length)
                    {
                        bin = 0;
                    }
                    bins[bin]++;
                }
            }

            // plot values
            var pc = new PointCollection();
            pc.Add(new Point(0, 0));
            for (int i = 0; i < BinCount; i++)
            {
                pc.Add(new Point(i, bins[i]));
                pc.Add(new Point(i + 1, bins[i]));
            }
            pc.Add(new Point(BinCount, 0));

            // update control
            Points = pc;
            RaisePropertyChanged("Maximum");
            RaisePropertyChanged("Minimum");
            RaisePropertyChanged("LowerValue");
            RaisePropertyChanged("UpperValue");
        }

        #endregion

        //------------------------------------------------------------------
        #region ** filter interface implementation

        public DataGridFilterState Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                UpdateValues();
                RaisePropertyChanged("Filter");
            }
        }

        private void UpdateFilter()
        {
            if (!_inverse.HasValue)
            {
                _inverse = false;
                RaisePropertyChanged("Inverse");
            }
            _filter = GetFilter();
            RaisePropertyChanged("Filter");
        }

        private void UpdateValues()
        {
            _lowerValue = double.NaN;
            _upperValue = double.NaN;
            _inverse = null;
            if (_filter != null &&
               _filter.FilterInfo != null)
            {
                if (_filter.FilterInfo.Count == 1)
                {
                    var firstPart = _filter.FilterInfo[0];
                    try
                    {
                        double firstValue = Convert.ToDouble(firstPart.Value);
                        if (firstPart.FilterOperation == DataGridFilterOperation.GreaterThan ||
                            firstPart.FilterOperation == DataGridFilterOperation.GreaterThanOrEqual)
                        {
                            _inverse = false;
                            _lowerValue = firstValue;
                        }
                        else if (firstPart.FilterOperation == DataGridFilterOperation.LessThan ||
                            firstPart.FilterOperation == DataGridFilterOperation.LessThanOrEqual)
                        {
                            _inverse = false;
                            _upperValue = firstValue;
                        }
                    }
                    catch { }
                }
                else if (_filter.FilterInfo.Count >= 2)
                {
                    var firstPart = _filter.FilterInfo[0];
                    var secondPart = _filter.FilterInfo[1];
                    try
                    {
                        double firstValue = Convert.ToDouble(firstPart.Value);
                        double secondValue = Convert.ToDouble(secondPart.Value);
                        if ((firstPart.FilterOperation == DataGridFilterOperation.GreaterThan || firstPart.FilterOperation == DataGridFilterOperation.GreaterThanOrEqual) &&
                            secondPart.FilterCombination == DataGridFilterCombination.And &&
                            (secondPart.FilterOperation == DataGridFilterOperation.LessThan || secondPart.FilterOperation == DataGridFilterOperation.LessThanOrEqual) &&
                            firstValue <= secondValue)
                        {
                            _inverse = false;
                            _lowerValue = firstValue;
                            _upperValue = secondValue;
                        }
                        else if ((firstPart.FilterOperation == DataGridFilterOperation.LessThan || firstPart.FilterOperation == DataGridFilterOperation.LessThanOrEqual) &&
                                secondPart.FilterCombination == DataGridFilterCombination.Or &&
                                (secondPart.FilterOperation == DataGridFilterOperation.GreaterThan || secondPart.FilterOperation == DataGridFilterOperation.GreaterThanOrEqual) &&
                                firstValue <= secondValue)
                        {
                            _inverse = true;
                            _lowerValue = firstValue;
                            _upperValue = secondValue;
                        }
                    }
                    catch { }
                }
            }
            else
            {
                _inverse = false;
            }
            RaisePropertyChanged("Inverse");
            RaisePropertyChanged("SliderStyle");
            RaisePropertyChanged("LowerValue");
            RaisePropertyChanged("UpperValue");
        }

        /// <summary>
        /// Gets the filter state from the data entered in the control.
        /// </summary>
        private DataGridFilterState GetFilter()
        {
            if (!double.IsNaN(_lowerValue) || !double.IsNaN(_upperValue))
            {
                List<DataGridFilterInfo> result = new List<DataGridFilterInfo>();
                if (Inverse.HasValue && Inverse.Value)
                {
                    if (!double.IsNaN(_lowerValue))
                        result.Add(new DataGridFilterInfo
                        {
                            FilterCombination = DataGridFilterCombination.None,
                            FilterOperation = DataGridFilterOperation.LessThanOrEqual,
                            FilterType = DataGridFilterType.Numeric,
                            Value = Math.Ceiling((double)LowerValue)
                        });
                    if (!double.IsNaN(_upperValue))
                        result.Add(new DataGridFilterInfo
                        {
                            FilterCombination = DataGridFilterCombination.Or,
                            FilterOperation = DataGridFilterOperation.GreaterThanOrEqual,
                            FilterType = DataGridFilterType.Numeric,
                            Value = Math.Floor((double)UpperValue)
                        });
                }
                else
                {
                    if (!double.IsNaN(_lowerValue))
                        result.Add(new DataGridFilterInfo
                        {
                            FilterCombination = DataGridFilterCombination.None,
                            FilterOperation = DataGridFilterOperation.GreaterThanOrEqual,
                            FilterType = DataGridFilterType.Numeric,
                            Value = Math.Floor((double)LowerValue)
                        });
                    if (!double.IsNaN(_upperValue))
                        result.Add(new DataGridFilterInfo
                        {
                            FilterCombination = DataGridFilterCombination.And,
                            FilterOperation = DataGridFilterOperation.LessThanOrEqual,
                            FilterType = DataGridFilterType.Numeric,
                            Value = Math.Ceiling((double)UpperValue)
                        });
                }
                return new DataGridFilterState { FilterInfo = result };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Raises the <see cref="e:PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property that was changed.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
