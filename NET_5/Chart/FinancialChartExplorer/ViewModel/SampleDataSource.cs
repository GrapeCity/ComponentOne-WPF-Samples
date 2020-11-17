using FinancialChartExplorer.Resources;
using System.Collections.ObjectModel;

namespace FinancialChartExplorer
{
    public class SampleDataSource
    {
        ObservableCollection<SampleGroup> _groups = new ObservableCollection<SampleGroup>();

        public SampleDataSource()
        {
            var chartTypeGroup = new SampleGroup(AppResources.ChartTypesTitle);
            chartTypeGroup.Samples.Add(new Sample(AppResources.HeikinAshiTitle,
                new HeikinAshi())
            { IsSelected = true });
            chartTypeGroup.Samples.Add(new Sample(AppResources.LineBreak,
                new LineBreak()));
            chartTypeGroup.Samples.Add(new Sample(AppResources.Renko,
                new Renko()));
            chartTypeGroup.Samples.Add(new Sample(AppResources.Kagi,
                new Kagi()));
            chartTypeGroup.Samples.Add(new Sample(AppResources.ColumnVolume,
                new ColumnVolume()));
            chartTypeGroup.Samples.Add(new Sample(AppResources.EquiVolume,
                new EquiVolume()));
            chartTypeGroup.Samples.Add(new Sample(AppResources.CandleVolume,
                new CandleVolume()));
            chartTypeGroup.Samples.Add(new Sample(AppResources.ArmsCandleVolume,
                new ArmsCandleVolume()));
            chartTypeGroup.Samples.Add(new Sample(AppResources.PointFigure,
                new PointAndFigure()));
            Groups.Add(chartTypeGroup);

            var interactionGroup = new SampleGroup(AppResources.Interaction);
            interactionGroup.Samples.Add(new Sample(AppResources.Markers,
                new Markers()));
            interactionGroup.Samples.Add(new Sample(AppResources.RangeSelector,
                new RangeSelector()));
            Groups.Add(interactionGroup);

            var analyticsGroup = new SampleGroup(AppResources.Analytics);
            analyticsGroup.Samples.Add(new Sample(AppResources.TrendLines,
                new TrendLines()));
            analyticsGroup.Samples.Add(new Sample(AppResources.MovingAverage,
               new MovingAverages()));
            analyticsGroup.Samples.Add(new Sample(AppResources.Overlays,
               new Overlays()));
            analyticsGroup.Samples.Add(new Sample(AppResources.Indicators,
               new Indicators()));
            analyticsGroup.Samples.Add(new Sample(AppResources.EventAnnotations,
             new EventAnnotations()));
            analyticsGroup.Samples.Add(new Sample(AppResources.FibonacciTool,
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
