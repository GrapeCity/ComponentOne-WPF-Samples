using System;
using System.Collections.Generic;
using System.Windows.Controls;
using C1.Chart;
using C1.WPF.Chart;
using FlexChartExplorer.Resources;

namespace FlexChartExplorer
{
    public sealed partial class Binding : UserControl
    {
        int npts = 50;
        Random rnd = new Random();
        List<DataItem> _data;

        public Binding()
        {
            this.InitializeComponent();
            Tag = AppResources.BindingTag;
        }

        public List<string> ChartTypes
        {
            get
            {
                return new List<string> { "Line", "LineSymbols", "Area" };
            }
        }

        public List<DataItem> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new List<DataItem>();
                    var dateStep = 0;
                    for (var i = 0; i < npts; i++)
                    {
                        var date = DateTime.Today.AddDays(dateStep += 9);
                        _data.Add(new DataItem()
                        {
                            Downloads = date.Month == 4 || date.Month == 8 ? (int?)null : rnd.Next(10, 20),
                            Sales = date.Month == 4 || date.Month == 8 ? (int?)null : rnd.Next(0, 10),
                            Date = date
                        });
                    }
                }

                return _data;
            }
        }
    }

    public class DataItem
    {
        public int? Downloads { get; set; }
        public int? Sales { get; set; }
        public DateTime Date { get; set; }
    }
}
