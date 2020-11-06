using System;
using System.Collections.Generic;
using System.Windows.Controls;
using C1.Chart;
using C1.WPF.Chart;

namespace FlexChartExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Financial : UserControl
    {
        int npts = 30;
        List<Quote> _data;
        List<string> _chartTypes;
        Random rnd = new Random();

        public Financial()
        {
            this.InitializeComponent();
            Tag = "This view shows how to create financial charts with the FlexChart control.\r" +
                "The FlexChart supports two types of financial chart: Candlestick and HiLowOpenClose.To use them, set the chartType property to the type you want, and set the series binding property to a string that specifies the fields that contain the high, low, open, and close values in the data source.";
        }

        public List<string> ChartTypes
        {
            get
            {
                if (_chartTypes == null)
                {
                    _chartTypes = DataCreator.CreateFinancialChartTypes();
                }

                return _chartTypes;
            }
        }

        public List<Quote> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = CreateData();
                }

                return _data;
            }
        }

        List<Quote> CreateData()
        {
            List<Quote> list = new List<Quote>();
            var dt = DateTime.Today;
            for (var i = 0; i < npts; i++)
            {
                var q = new Quote();
                q.Time = dt.AddDays(i);
                if (i > 0)
                    q.Open = list[i - 1].Close;
                else
                    q.Open = 1000;
                q.High = q.Open + rnd.Next(50);
                q.Low = q.Open - rnd.Next(50);
                q.Close = rnd.Next((int)q.Low, (int)q.High);
                q.Volume = rnd.Next(0, 100);
                list.Add(q);
            }

            return list;
        }
    }

    public class Quote
    {
        public DateTime Time { get; set; }

        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Volume { get; set; }
    }
}
