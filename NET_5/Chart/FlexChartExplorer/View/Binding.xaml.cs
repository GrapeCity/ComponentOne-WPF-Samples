using System;
using System.Collections.Generic;
using System.Windows.Controls;
using C1.Chart;
using C1.WPF.Chart;

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
            Tag = "This view shows how you can use the FlexChart to show two sets of values from a single array of data. This is the most common usage scenario for the FlexChart.\r" +
                "The sample does the following:\r" + Environment.NewLine +
                "1.Set the chart's DataSource property to an array of data objects. Each data object has values for 'date', 'sales', and 'downloads'.\r" +
                "2.Set the chart's bindingX property to 'date'.\r" +
                "3.Add a Series object to the chart's series array and set its binding property to 'sales'.\r" +
                "4.Add a second Series object to the chart's series array and set its binding property to 'downloads'.\r" + Environment.NewLine +
                "In addition to binding, this sample shows the effect of the interpolateNulls and legendToggle properties.When you set interpolateNulls to true, the chart fills in gaps created by null values in the data.When you set legendToggle to true, the chart toggles the visibility of the series when you click its name in the legend.";
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
