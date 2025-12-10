using FlexChartExplorer.Resources;
using System;
using System.Collections.ObjectModel;

namespace FlexChartExplorer
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem(AppResources.IntroductionTitle,
                AppResources.IntroductionDesc,
                new Introduction()));

            _allItems.Add(new SampleItem(AppResources.BindingTitle,
                AppResources.BindingDesc,
                new Binding()));
            _allItems.Add(new SampleItem(AppResources.SeriesBindingTitle,
                AppResources.SeriesBindingDesc,
                new SeriesBinding()));
            _allItems.Add(new SampleItem(AppResources.HeaderFooterTitle,
                AppResources.HeaderFooterDesc,
                new HeaderAndFooter()));
            _allItems.Add(new SampleItem(AppResources.SelectionTitle,
                AppResources.SelectionDesc,
                new Selection()));
            _allItems.Add(new SampleItem(AppResources.LabelsTitle,
                AppResources.LabelsDesc,
                new Labels()));
            _allItems.Add(new SampleItem(AppResources.AutoLabelsTitle,
                AppResources.AutoLabelDesc,
                new AutoLabels()));
            _allItems.Add(new SampleItem(AppResources.HitTestTitle,
                AppResources.HitTestDesc,
                new HitTest()));
            _allItems.Add(new SampleItem(AppResources.ZoomTitle,
               AppResources.ZoomDesc,
               new Zoom()));
            _allItems.Add(new SampleItem(AppResources.BubbleTitle,
                AppResources.BubbleDesc,
                new Bubble()));
            _allItems.Add(new SampleItem(AppResources.FinancialChartTitle,
                AppResources.FinancialCharDesc,
                new Financial()));
            _allItems.Add(new SampleItem(AppResources.AxesTitle,
                AppResources.AxesDesc,
                new Axes()));
            _allItems.Add(new SampleItem(AppResources.AxisLabelsTitle,
                AppResources.AxisLabelsDesc,
                new AxisLabels()));
            _allItems.Add(new SampleItem(AppResources.AxisGroupingTitle,
                AppResources.AxisGroupingDesc,
                new AxisGrouping()));
            _allItems.Add(new SampleItem(AppResources.NumericAxisGroupingTitle,
                AppResources.NumericAxisGroupingDesc,
                new NumericAxisGrouping()));
            _allItems.Add(new SampleItem(AppResources.DateTimeAxisGroupingTitle,
                AppResources.DateTimeAxisGroupingDesc,
                new DateTimeAxisGrouping()));
            _allItems.Add(new SampleItem(AppResources.PlotAreasTitle,
                AppResources.PlotAreasDesc,
                new PlotAreas()));
            _allItems.Add(new SampleItem(AppResources.AxisBindingTitle,
                AppResources.AxisBindingDesc,
                new AxisBinding()));
            _allItems.Add(new SampleItem(AppResources.ImageExportTitle,
                AppResources.ImageExportDesc,
                new ImageExport()));
            _allItems.Add(new SampleItem(AppResources.ZonesTitle,
                AppResources.ZonesDesc,
                new Zones()));
            _allItems.Add(new SampleItem(AppResources.TrendLineTitle,
                AppResources.TrendLineDesc,
                new TrendLine()));
            _allItems.Add(new SampleItem(AppResources.WaterfallTitle,
                AppResources.WaterfallDesc,
                new Waterfall()));
            _allItems.Add(new SampleItem(AppResources.PieChartTitle,
                AppResources.PieChartDesc,
                new PieIntroduction()));
            _allItems.Add(new SampleItem(AppResources.PieChartSelectionTitle,
                AppResources.PieCharSelectionDesc,
                new PieSelection()));
            _allItems.Add(new SampleItem(AppResources.PieChartSliceTitle,
                AppResources.PieChartSliceDesc,
                new PieSliceColor()));
            _allItems.Add(new SampleItem(AppResources.BoxWhiskerTitle,
                AppResources.BoxWhiskerDesc,
                new BoxWhisker()));
            _allItems.Add(new SampleItem(AppResources.ErrorBarTitle,
               AppResources.ErrorBarDesc,
               new ErrorBar()));
            _allItems.Add(new SampleItem(AppResources.LegendTitle,
                AppResources.LegendDesc,
                new Legend()));
            _allItems.Add(new SampleItem(AppResources.TreeMapTitle,
               AppResources.TreeMapDesc,
               new TreeMap()));
            _allItems.Add(new SampleItem(AppResources.TreeMapNodeColorTitle,
               AppResources.TreeMapNodeColorDesc,
               new TreeMapNodeColor()));
            _allItems.Add(new SampleItem(AppResources.HistogramTitle,
                AppResources.HistogramDesc,
               new Histogram()));
            _allItems.Add(new SampleItem(AppResources.RangedHistogramTitle,
               AppResources.RangedHistogramDesc,
               new RangedHistogram()));
            _allItems.Add(new SampleItem(AppResources.ParetoTitle,
               AppResources.ParetoDesc,
               new Pareto()));
            _allItems.Add(new SampleItem(AppResources.BreakEvenTitle,
                AppResources.BreakEvenDesc,
                new BreakEven()));
            _allItems.Add(new SampleItem(AppResources.HeatmapTitle,
                AppResources.HeatmapDesc,
                new Heatmap()));
            _allItems.Add(new SampleItem(AppResources.ContourTitle,
               AppResources.ContourDesc,
               new ContourChart()));
            _allItems.Add(new SampleItem(AppResources.ColumnHeatmapTitle,
                AppResources.ColumnHeatmapDesc,
                new ColumnHeatmap()));
            _allItems.Add(new SampleItem(AppResources.AxisMarkersTitle,
                AppResources.AxisMarkersDesc,
                new AxisMarkers()));
            _allItems.Add(new SampleItem(AppResources.AxisBreakTitle,
                AppResources.AxisBreakDesc,
                new AxisBreak()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
