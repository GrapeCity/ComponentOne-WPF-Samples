using System;
using System.Collections.Generic;
using System.Windows.Controls;
using C1.Chart;
using C1.WPF.Chart;
using FlexChartExplorer.Resources;

namespace FlexChartExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AxisBinding : UserControl
    {
        int npts = 30;
        List<Quote> _data;
        List<AxisBindingItem> _axisBindingItems;
        Random rnd = new Random();

        public AxisBinding()
        {
            this.InitializeComponent();
            Tag = AppResources.AxisBindingTag;
            this.flexChart.AxisY.ItemsSource = AxisData;
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

        public List<AxisBindingItem> AxisData
        {
            get
            {
                if (_axisBindingItems == null)
                {
                    _axisBindingItems = CreateAxisData();
                }

                return _axisBindingItems;
            }
        }

        List<AxisBindingItem> CreateAxisData()
        {
            List<AxisBindingItem> list = new List<AxisBindingItem>();
            for (int i = 0; i < 20; i++)
            {
                list.Add(new AxisBindingItem()
                {
                    Value = 500 + i * 50,
                    Text = string.Format("$ {0:n0}", 500 + i * 50)
                });
            }

            return list;
        }


        List<Quote> CreateData()
        {
            List<Quote> list = new List<Quote>();
            var dt = DateTime.Today;
            var price = 1000;
            for (var i = 0; i < npts; i++)
            {
                price += rnd.Next(10) % 2 == 0 ? rnd.Next(50) : -rnd.Next(50);
                list.Add(new Quote() { Time = dt.AddDays(i), Price = price });
            }

            return list;
        }

        public class Quote
        {
            public DateTime Time { get; set; }
            public double Price { get; set; }
        }

        public class AxisBindingItem
        {
            public double Value { get; set; }
            public string Text { get; set; }
        }
    }
}
