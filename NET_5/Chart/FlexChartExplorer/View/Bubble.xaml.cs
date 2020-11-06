using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace FlexChartExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Bubble : UserControl
    {
        List<DataItem> _data;
        int npts = 30;
        Random rnd = new Random();

        public Bubble()
        {
            this.InitializeComponent();
            Tag = "This view shows how to create bubble charts using the FlexChart control.\r" +
                "Bubble charts are similar to other chart types, except in addition to X and Y you must specify a binding for the bubble size.This is done by setting the binding property to a comma - delimited string that specifies the name of the properties to be used for the Y and size values for each bubble.\r" +
                "In this example, the chart is bound to a list containing objects with 'x', 'y', and 'size' properties.The chart contains a single series and its binding property is set to the string 'y,size'.";
        }

        public List<DataItem> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new List<DataItem>();
                    for (int i = 0; i < npts; i++)
                    {
                        _data.Add(new DataItem()
                        {
                            X = rnd.NextDouble(),
                            Y = rnd.NextDouble(),
                            Size = rnd.Next(100)
                        });
                    }
                }

                return _data;
            }
        }

        public class DataItem
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Size { get; set; }
        }
    }
}
