using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.Chart;
using C1.WPF.Chart.Finance;

namespace StockAnalysis.Partial.CustomControls.IndicatorSeries
{
    public abstract class IndicatorSeriesBase : System.Windows.DependencyObject, IIndicator
    {
        public C1FlexChart Chart
        {
            get;
            protected set;
        }

        public IEnumerable<Series> Series
        {
            get;
            protected set;
        }

        private Axis _axisY;
        public Axis AxisY
        {
            get
            {
                if (_axisY == null)
                {
                    _axisY = new Axis();
                }
                return _axisY;
            }
        }

        private string _plotAreaName;

        public event EventHandler SettingParamsChanged;

        protected void OnSettingParamsChanged()
        {
            if (this.SettingParamsChanged != null)
            {
                this.SettingParamsChanged(this, new EventArgs());
            }
        }

        public string PlotAreaName
        {
            get
            {
                return _plotAreaName;
            }
            set
            {
                if (_plotAreaName != value)
                {
                    _plotAreaName = value;
                    foreach (var series in this.Series)
                    {
                        series.AxisY.PlotAreaName = _plotAreaName;
                    }
                }
            }
        }

        public void RemoveAllSeriesFromChart()
        {
            foreach (var series in this.Series)
            {
                Chart.Series.Remove(series);
            }
        }
        
        public IEnumerable<Object.QuoteRange> GetYRange(double low, double high)
        {
            Queue<Object.QuoteRange> ranges = new Queue<Object.QuoteRange>();

            foreach (var series in this.Series)
            {
                ranges.Enqueue(GetSeriesYRange(series, low, high));
            }
            return ranges;
        }

        private Object.QuoteRange GetSeriesYRange(Series series, double s, double e)
        {
            if (series is ThresholdSeries && series.Visibility == C1.Chart.SeriesVisibility.Visible)
            {
                var value = (series as ThresholdSeries).Threshold.Value;
                return new Object.QuoteRange() { Min = value, Max = value };
            }

            Object.QuoteRange qr = null;
            IEnumerable<double> quoteData = series.GetValues(0);

            var period = GetSeriesPeriod(series);
            
            var start = period == 0 ? Convert.ToInt32(s): (Convert.ToInt32(s) < (period - 1) ? 0 : (Convert.ToInt32(s) - (period - 1)));
            var len = Convert.ToInt32(e) - Convert.ToInt32(s) + 1;
            
            IEnumerable<double> quoteCache = quoteData.Skip(start).Take(len);
            if (quoteCache.Any())
            {
                qr = new Object.QuoteRange();
                var min = quoteCache.Min();
                var max = quoteCache.Max();
                qr.Min = min;
                qr.Max = max;
            }
            return qr;
        }

        private int GetSeriesPeriod(Series series)
        {
            Type type = series.GetType();

            System.Reflection.PropertyInfo property = type.GetProperty("Period");

            if (property != null)
            {
                return Convert.ToInt32(property.GetValue(series, null));
            }
            return 0;
        }
    }
}
