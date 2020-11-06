using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    public partial class DataGridDateRangeFilter : UserControl, IDataGridFilterUnity
    {
        private DataGridFilterState _filter = null;
        private DateTime? _lowerValue = null;
        private DateTime? _upperValue = null;
        private string _format = "d";

        public DataGridDateRangeFilter()
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

        public DateTime? LowerValue
        {
            get
            {
                return _lowerValue ?? Minimum;
            }
            set
            {
                _lowerValue = value.HasValue ? value.Value.Date : (DateTime?)null;
                RaisePropertyChanged("LowerValue");
                RaisePropertyChanged("LowerValueText");
                _filter = GetFilter();
                RaisePropertyChanged("Filter");
            }
        }

        public DateTime? UpperValue
        {
            get
            {
                return _upperValue ?? Maximum;
            }
            set
            {
                _upperValue = value.HasValue ? value.Value.Date : (DateTime?)null;
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
                return _lowerValue == null ? "" : string.Format("{0:" + Format + "}", _lowerValue);
            }
        }

        public string UpperValueText
        {
            get
            {
                return _upperValue == null ? "" : string.Format("{0:" + Format + "}", _upperValue);
            }
        }

        #region Minimum

        /// <summary>
        /// Gets or sets the minimum value that can be selected in the range slider.
        /// </summary>
        public DateTime Minimum
        {
            get { return (DateTime)GetValue(MinimumProperty); }
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
                typeof(DateTime),
                typeof(DataGridDateRangeFilter),
                new PropertyMetadata(DateTime.MinValue, (d, a) =>
                    {
                        (d as DataGridDateRangeFilter).RaisePropertyChanged("LowerValue");
                        (d as DataGridDateRangeFilter).RaisePropertyChanged("LowerValueText");
                    })
                );



        #endregion Minimum

        #region Maximum


        /// <summary>
        /// Gets or sets the maximum value that can be selected in the range slider.
        /// </summary>
        public DateTime Maximum
        {
            get { return (DateTime)GetValue(MaximumProperty); }
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
                typeof(DateTime),
                typeof(DataGridDateRangeFilter),
                new PropertyMetadata(DateTime.MaxValue, (d, a) =>
                {
                    (d as DataGridDateRangeFilter).RaisePropertyChanged("UpperValue");
                    (d as DataGridDateRangeFilter).RaisePropertyChanged("UpperValueText");
                })
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
            if (_lowerValue.HasValue)
                result.Add(new DataGridFilterInfo { FilterCombination = DataGridFilterCombination.None, FilterOperation = DataGridFilterOperation.GreaterThanOrEqual, FilterType = DataGridFilterType.DateTime, Value = _lowerValue.Value });
            if (_upperValue.HasValue)
                result.Add(new DataGridFilterInfo { FilterCombination = DataGridFilterCombination.And, FilterOperation = DataGridFilterOperation.LessThanOrEqual, FilterType = DataGridFilterType.DateTime, Value = _upperValue.Value });
            return new DataGridFilterState { FilterInfo = result };
        }

        private void SetFilter(DataGridFilterState filter)
        {
            _lowerValue = null;
            _upperValue = null;
            if (filter != null)
            {
                var list = filter.FilterInfo;
                var lower = list.FirstOrDefault(fi => fi.FilterOperation == DataGridFilterOperation.GreaterThan || fi.FilterOperation == DataGridFilterOperation.GreaterThanOrEqual);
                if (lower != null)
                {
                    _lowerValue = (DateTime)lower.Value;
                }
                var upper = list.FirstOrDefault(fi => fi.FilterOperation == DataGridFilterOperation.LessThan || fi.FilterOperation == DataGridFilterOperation.LessThanOrEqual);
                if (upper != null)
                {
                    _upperValue = (DateTime)upper.Value;
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
