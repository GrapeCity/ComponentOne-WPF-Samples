using C1.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace DataManipulation
{
    public class AggregateViewModel: ViewModelBase<AggregateFilter>
    {
        static Random r = new Random();
        public AggregateViewModel()
        {
            Filter.AggregateType = AggregateType.Sum;
            Queue<AggregateItem> sis = new Queue<AggregateItem>();
            
            for (int i = 0; i < 240; i++)
            {
                int month = ((i % 12) + 1);
                int q = (month / 3) + ((month % 3) == 0 ? 0 : 1);
                int year = (1997 + (i - 1) / 12);

                sis.Enqueue(new AggregateItem()
                {
                    Year = year,
                    M = month.ToString(),
                    Q = q,
                    Value = r.Next(500),
                });
            }

            this.Source = sis;
        }

        public override AggregateFilter Filter
        {
            get
            {
                return base.Filter;
            }
            set
            {
                base.Filter = value;
            }
        }

        #region Selection Data



        private string _aggregateProperty = null;
        public string AggregateProperty
        {
            get
            {
                return _aggregateProperty;
            }
            set
            {
                string key = value;
                if (string.IsNullOrEmpty(key))
                {
                    Filter.Bindings = null;
                }

                switch (key)
                {
                    case "Year":
                        Filter.Bindings = new string[] { "Year" };
                        break;
                    case "Quarter":
                        Filter.Bindings = new string[] { "Year", "Q" };
                        break;
                    default:
                        Filter.Bindings = null;
                        break;
                }
            }
        }




        private Dictionary<string, AggregateType> _aggregateTypes;
        public Dictionary<string, AggregateType> AggregateTypes
        {
            get
            {
                if (_aggregateTypes == null)
                {
                    _aggregateTypes = new Dictionary<string, AggregateType>();
                    foreach (var item in Enum.GetValues(typeof(AggregateType)))
                    {
                        _aggregateTypes.Add("Aggregate Type: " + ((AggregateType)item).ToString(), (AggregateType)item);
                    }
                }
                return _aggregateTypes;
            }
        }

        private Dictionary<string, string> _aggregateProperties;
        public Dictionary<string, string> AggregateProperties
        {
            get
            {
                if (_aggregateProperties == null)
                {
                    _aggregateProperties = new Dictionary<string, string>();
                    _aggregateProperties.Add("Aggregate Property: Null", "");
                    _aggregateProperties.Add("Aggregate Property: Quarter", "Quarter");
                    _aggregateProperties.Add("Aggregate Property: Year", "Year");
                      
                }
                return _aggregateProperties;
            }
        }

        private Dictionary<string, ChartType> _chartTypes;
        public Dictionary<string, ChartType> ChartTypes
        {
            get
            {
                if (_chartTypes == null)
                {
                    _chartTypes = new Dictionary<string, ChartType>();
                    foreach (var item in Enum.GetValues(typeof(ChartType)))
                    {
                        if (item.ToString() != "Bubble" && item.ToString() != "Candlestick" && item.ToString() != "HighLowOpenClose" && item.ToString() != "Funnel")
                        {
                            _chartTypes.Add("Chart Type: " + ((ChartType)item).ToString(), (ChartType)item);
                        }
                    }
                }
                return _chartTypes;
            }
        }

        #endregion Selection Data
        
    }

    public class Boolean2StackingConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = System.Convert.ToBoolean(value);
            if (b)
            {
                return Stacking.Stacked;
            }
            return Stacking.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Stacking s = (Stacking)value;
            if (s == Stacking.Stacked)
            {
                return true;
            }
            return false;
        }
    }
}
