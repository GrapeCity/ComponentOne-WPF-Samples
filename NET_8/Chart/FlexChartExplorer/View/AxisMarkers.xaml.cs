using System;
using System.Collections.Generic;
using System.Windows.Controls;
using C1.Chart;
using C1.WPF.Chart;
using FlexChartExplorer.Resources;

namespace FlexChartExplorer
{
    public sealed partial class AxisMarkers : UserControl
    {
        int npts = 150;
        Random rnd = new Random();
        List<CpuDataItem> _data;

        public AxisMarkers()
        {
            this.InitializeComponent();
            Tag = AppResources.AxisMarkersTag;
        }

        public List<CpuDataItem> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new List<CpuDataItem>();
                    var dateStep = 0;
                    for (var i = 0; i < npts; i++)
                    {
                        var date = DateTime.Today.AddSeconds(dateStep += 9);
                        _data.Add(new CpuDataItem()
                        {
                            Cpu1 = rnd.Next(10, 20),
                            Cpu2 = rnd.Next(40, 80),
                            Time = date
                        });
                    }
                }

                return _data;
            }
        }
    }

    public class CpuDataItem
    {
        public int? Cpu1 { get; set; }
        public int? Cpu2 { get; set; }
        public DateTime Time { get; set; }
    }
}
