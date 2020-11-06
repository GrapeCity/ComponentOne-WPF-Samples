using System.Collections.ObjectModel;

namespace FinancialChartExplorer
{
    public class SampleDataSource
    {
        ObservableCollection<SampleGroup> _groups = new ObservableCollection<SampleGroup>();

        public SampleDataSource()
        {
            var chartTypeGroup = new SampleGroup("Chart Types");
            chartTypeGroup.Samples.Add(new Sample("Heikin-Ashi",
                new HeikinAshi())
            { IsSelected = true });
            chartTypeGroup.Samples.Add(new Sample("Line Break",
                new LineBreak()));
            chartTypeGroup.Samples.Add(new Sample("Renko",
                new Renko()));
            chartTypeGroup.Samples.Add(new Sample("Kagi",
                new Kagi()));
            chartTypeGroup.Samples.Add(new Sample("ColumnVolume",
                new ColumnVolume()));
            chartTypeGroup.Samples.Add(new Sample("EquiVolume",
                new EquiVolume()));
            chartTypeGroup.Samples.Add(new Sample("CandleVolume",
                new CandleVolume()));
            chartTypeGroup.Samples.Add(new Sample("Arms CandleVolume",
                new ArmsCandleVolume()));
            chartTypeGroup.Samples.Add(new Sample("Point&Figure",
                new PointAndFigure()));
            Groups.Add(chartTypeGroup);

            var interactionGroup = new SampleGroup("Interaction");
            interactionGroup.Samples.Add(new Sample("Markers",
                new Markers()));
            interactionGroup.Samples.Add(new Sample("Range Selector",
                new RangeSelector()));
            Groups.Add(interactionGroup);

            var analyticsGroup = new SampleGroup("Analytics");
            analyticsGroup.Samples.Add(new Sample("Trend Lines",
                new TrendLines()));
            analyticsGroup.Samples.Add(new Sample("Moving Average",
               new MovingAverages()));
            analyticsGroup.Samples.Add(new Sample("Overlays",
               new Overlays()));
            analyticsGroup.Samples.Add(new Sample("Indicators",
               new Indicators()));
            analyticsGroup.Samples.Add(new Sample("Event Annotations",
             new EventAnnotations()));
            analyticsGroup.Samples.Add(new Sample("Fibonacci Tool",
              new FibonacciTool()));
            Groups.Add(analyticsGroup);
        }

        public ObservableCollection<SampleGroup> Groups
        {
            get
            {
                return _groups;
            }
        }
    }
}
