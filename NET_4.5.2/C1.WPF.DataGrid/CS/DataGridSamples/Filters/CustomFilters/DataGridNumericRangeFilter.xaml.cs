using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    public partial class DataGridNumericRangeFilter : UserControl, IDataGridFilterUnity
    {
        private DataGridFilterState _filter = null;
        private double _lowerValue = double.NaN;
        private double _upperValue = double.NaN;
        private string _format = "F2";

        public DataGridNumericRangeFilter()
        {
            DataContext = this;

            InitializeComponent();
        }

        public string Format
        {
            get
            {
                return _format;
            }
            set
            {
                _format = value;
                RaisePropertyChanged("LowerValueText");
                RaisePropertyChanged("UpperValueText");
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
                RaisePropertyChanged("LowerValue");
                RaisePropertyChanged("LowerValueText");
                _filter = GetFilter();
                RaisePropertyChanged("Filter");
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
                RaisePropertyChanged("UpperValue");
                RaisePropertyChanged("UpperValueText");
                _filter = GetFilter();
                RaisePropertyChanged("Filter");
            }
        }

        public string LowerValueText
        {
            get
            {
                return double.IsNaN(_lowerValue) ? "" : string.Format("{0:" + Format + "}", _lowerValue);
            }
        }

        public string UpperValueText
        {
            get
            {
                return double.IsNaN(_upperValue) ? "" : string.Format("{0:" + Format + "}", _upperValue);
            }
        }

        #region Minimum

        /// <summary>
        /// Gets or sets the minimum value that can be selected in the range slider.
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set
            {
                SetValue(MinimumProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Minimum"/> dependency property. 
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                "Minimum",
                typeof(double),
                typeof(DataGridNumericRangeFilter),
                                null
                );



        #endregion Minimum

        #region Maximum


        /// <summary>
        /// Gets or sets the maximum value that can be selected in the range slider.
        /// </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set
            {
                SetValue(MaximumProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Maximum"/> dependency property. 
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                "Maximum",
                typeof(double),
                typeof(DataGridNumericRangeFilter),
                                null
                );



        #endregion Maximum

        public DataGridFilterState Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                RaisePropertyChanged("Filter");
                SetFilter(value);
            }
        }

        private DataGridFilterState GetFilter()
        {
            List<DataGridFilterInfo> result = new List<DataGridFilterInfo>();
            if (!double.IsNaN(_lowerValue))
                result.Add(new DataGridFilterInfo { FilterCombination = DataGridFilterCombination.None, FilterOperation = DataGridFilterOperation.GreaterThanOrEqual, FilterType = DataGridFilterType.Numeric, Value = _lowerValue });
            if (!double.IsNaN(_upperValue))
                result.Add(new DataGridFilterInfo { FilterCombination = DataGridFilterCombination.And, FilterOperation = DataGridFilterOperation.LessThanOrEqual, FilterType = DataGridFilterType.Numeric, Value = _upperValue });
            return new DataGridFilterState { FilterInfo = result };
        }

        private void SetFilter(DataGridFilterState filter)
        {
            _lowerValue = double.NaN;
            _upperValue = double.NaN;
            if (filter != null)
            {
                var list = filter.FilterInfo;
                var lower = list.FirstOrDefault(fi => fi.FilterOperation == DataGridFilterOperation.GreaterThan || fi.FilterOperation == DataGridFilterOperation.GreaterThanOrEqual);
                if (lower != null)
                {
                    _lowerValue = (double)lower.Value;
                }
                var upper = list.FirstOrDefault(fi => fi.FilterOperation == DataGridFilterOperation.LessThan || fi.FilterOperation == DataGridFilterOperation.LessThanOrEqual);
                if (upper != null)
                {
                    _upperValue = (double)upper.Value;
                }
            }
            RaisePropertyChanged("LowerValue");
            RaisePropertyChanged("UpperValue");
            RaisePropertyChanged("LowerValueText");
            RaisePropertyChanged("UpperValueText");
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
