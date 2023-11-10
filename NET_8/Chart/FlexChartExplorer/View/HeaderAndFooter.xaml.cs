using FlexChartExplorer.Resources;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace FlexChartExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HeaderAndFooter : UserControl
    {
        List<DataItem> _data;
        Random rnd = new Random();
        string[] year = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public HeaderAndFooter()
        {
            this.InitializeComponent();
            Tag = AppResources.HeaderFooterTag;
        }

        public List<DataItem> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new List<DataItem>();
                    var count = year.Length;
                    for (var i = 0; i < count - 1; i++)
                    {
                        _data.Add(new DataItem()
                        {
                            Amount = rnd.Next(0, 1000), Month = year[i]
                        });
                    }
                }

                return _data;
            }
        }

        public class DataItem
        {
            public int Amount { get; set; }
            public string Month { get; set; }
        }
    }
}
