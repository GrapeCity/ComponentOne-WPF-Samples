using C1.WPF.Chart;
using C1.WPF.Chart.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockAnalysis.Partial.CustomControls.IndicatorSeries
{
    public interface IIndicator
    {
        event EventHandler SettingParamsChanged;
        C1FlexChart Chart
        {
            get;
        }


        IEnumerable<Series> Series
        {
            get;
        }

        Axis AxisY
        {
            get;
        }

        string PlotAreaName
        {
            get;
            set;
        }

        void RemoveAllSeriesFromChart();


        IEnumerable<Object.QuoteRange> GetYRange(double low, double high);
    }
}
