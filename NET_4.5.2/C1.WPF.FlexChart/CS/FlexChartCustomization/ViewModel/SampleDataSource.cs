using System;
using System.Collections.ObjectModel;

namespace FlexChartCustomization
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem("Line Styles",
                "Line Styles",
                "The sample below shows how to customize a single line series with different styles and colors.",
                new LineStyles()));
            _allItems.Add(new SampleItem("Legend Items",
                "Legend Items",
                "The sample below shows each point of a series in a different color and displays its corresponding customized entry in the Legend.",
                new LegendItems()));
            _allItems.Add(new SampleItem("Legend Ranges",
                "Legend Ranges",
                "The sample below shows a series distributed into different range of values in different colors and display their corresponding entries in the legend.",
                new LegendRanges()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
