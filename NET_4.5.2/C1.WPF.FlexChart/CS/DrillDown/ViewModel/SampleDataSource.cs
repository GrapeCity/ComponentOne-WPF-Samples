using System.Collections.ObjectModel;

namespace DrillDown
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem("BASIC DRILLDOWN",
                "BASIC DRILLDOWN",
                "This sample shows how to perform drilldown in FlexChart. Click a data point to drill down into the details, or click on the header to return to a higher level.",
                new BasicDrillDownDemo()));

            _allItems.Add(new SampleItem("ASYNC DRILLDOWN",
                    "ASYNCHRONOUS DRILLDOWN",
                    "This sample demonstrates performing an asynchronous drilldown in FlexChart. Click a data point to drill down into the details, or click on the header to return to a higher level.",
                    new AsyncDrillDownDemo()));

            _allItems.Add(new SampleItem("SUNBURST",
                "SUNBURST",
                "This sample shows how to perform drilldown in SunBurst.",
                new SunburstDemo()));
            _allItems.Add(new SampleItem("TREEMAP",
                "TREEMAP",
                "This sample shows how to perform drilldown in TreeMap. You can click on a cell with left button to open the next level and click with right button to return to a higher level.",
                new TreemapDemo()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
