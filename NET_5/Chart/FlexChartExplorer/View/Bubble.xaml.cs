using FlexChartExplorer.Resources;
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
            Tag = AppResources.BubbleTag;
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
