using System;
using System.Collections.ObjectModel;

namespace FlexChartExplorer
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem("Introduction",
                "Introduction",
                new Introduction()));

            _allItems.Add(new SampleItem("Binding",
                "Binding",
                new Binding()));
            _allItems.Add(new SampleItem("Series Binding",
                "Series Binding",
                new SeriesBinding()));
            _allItems.Add(new SampleItem("Header And Footer",
                "Chart header and footer",
                new HeaderAndFooter()));
            _allItems.Add(new SampleItem("Selection",
                "Selection",
                new Selection()));
            _allItems.Add(new SampleItem("Labels",
                "Labels",
                new Labels()));
            _allItems.Add(new SampleItem("AutoLabels",
                "AutoLabels",
                new AutoLabels()));
            _allItems.Add(new SampleItem("Hit Test",
                "Hit Test",
                new HitTest()));
            _allItems.Add(new SampleItem("Zoom",
               "Zoom",
               new Zoom()));
            _allItems.Add(new SampleItem("Bubble",
                "Bubble",
                new Bubble()));
            _allItems.Add(new SampleItem("Financial Chart",
                "Financial Chart",
                new Financial()));
            _allItems.Add(new SampleItem("Axes",
                "Multiple axes",
                new Axes()));
            _allItems.Add(new SampleItem("Axis Labels",
                "Axis Labels",
                new AxisLabels()));
            _allItems.Add(new SampleItem("Axis Grouping",
                "Axis Grouping",
                new AxisGrouping()));
            _allItems.Add(new SampleItem("Numeric Axis Grouping",
                "Numeric Axis Grouping",
                new NumericAxisGrouping()));
            _allItems.Add(new SampleItem("DateTime Axis Grouping",
                "DateTime Axis Grouping",
                new DateTimeAxisGrouping()));
            _allItems.Add(new SampleItem("Plot Areas",
                "Multiple plot areas",
                new PlotAreas()));
            _allItems.Add(new SampleItem("Axis Binding",
                "Axis binding",
                new AxisBinding()));
            _allItems.Add(new SampleItem("Image Export",
                "Image Export",
                new ImageExport()));
            _allItems.Add(new SampleItem("Zones",
                "Zones",
                new Zones()));
            _allItems.Add(new SampleItem("Trend Line",
                "Trend line",
                new TrendLine()));
            _allItems.Add(new SampleItem("Waterfall",
                "FlexChart Waterfall",
                new Waterfall()));
            _allItems.Add(new SampleItem("PieChart",
                "Introduction",
                new PieIntroduction()));
            _allItems.Add(new SampleItem("PieChart Selection",
                "Selection",
                new PieSelection()));
            _allItems.Add(new SampleItem("PieChart Slice Color",
                "Slice Color",
                new PieSliceColor()));
            _allItems.Add(new SampleItem("Box & Whisker",
                "Box & Whisker",
                new BoxWhisker()));
            _allItems.Add(new SampleItem("Error Bar",
               "Error Bar",
               new ErrorBar()));
            _allItems.Add(new SampleItem("Legend",
                "Legend",
                new Legend()));
            _allItems.Add(new SampleItem("TreeMap",
               "TreeMap",
               new TreeMap()));
            _allItems.Add(new SampleItem("TreeMap Node Color",
               "Node Color",
               new TreeMapNodeColor()));
            _allItems.Add(new SampleItem("Histogram",
                "Histogram",
               new Histogram()));
            _allItems.Add(new SampleItem("Ranged Histogram",
               "Ranged Histogram",
               new RangedHistogram()));
            _allItems.Add(new SampleItem("Pareto",
               "Pareto",
               new Pareto()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
