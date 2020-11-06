using System;
using System.Windows;
using System.Windows.Controls;

using C1.WPF.C1Chart;

namespace ChartSamples
{
    /// <summary>
    /// Interaction logic for Gallery.xaml
    /// </summary>
    [System.ComponentModel.Description("This sample displays all 2D and 3D chart types supported by C1Chart.")]
    public partial class Gallery : UserControl
    {
        SampleChartData data = new SampleChartData();

        public Gallery()
        {
            InitializeComponent();
            UpdateData(chart.ChartType);
        }

        private void UpdateData(ChartType ct)
        {
            string stylename = "sstyle";

            if (ct == ChartType.Ribbon || ct.ToString().Contains("3D"))
            {
                stylename = "sstyle3d";
                chart.Actions.Add(new Rotate3DAction());
            }

            chart.BeginUpdate();

            // use appropriate sample data
            if (ct == ChartType.HighLowOpenClose || ct == ChartType.Candle)
                chart.Data = data.FinancialData;
            else if (ct == ChartType.Bubble)
                chart.Data = data.BubbleData;
            else if (ct == ChartType.Gantt)
                chart.Data = data.GanttData;
            else
                chart.Data = data.DefaultData;

            // set style of plot elements
            foreach (DataSeries ds in chart.Data.Children)
            {
                ds.SymbolStyle = FindResource(stylename) as Style;
                ds.ConnectionStyle = FindResource(stylename) as Style;
                ds.PointTooltipTemplate = FindResource("lbl") as DataTemplate;
            }

            chart.EndUpdate();
        }

        void ds_PlotElementLoaded(object sender, EventArgs e)
        {
            PlotElement pe = sender as PlotElement;
            if (pe != null)
            {
                //pe.MouseEn
            }
        }

        private void gallery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (chart == null || !chart.IsLoaded)
                return;

            ChartType ct = (ChartType)gallery.SelectedValue;
            UpdateData(ct);
        }

        private void palettes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (chart != null && chart.IsLoaded)
            {
                chart.Palette = (ColorGeneration)palettes.SelectedItem;
            }
        }
    }

    /// <summary>
    /// Sample sata for different chart types
    /// </summary>
    class SampleChartData
    {
        ChartData _defaultData, _finData, _bubbleData, _ganttData;

        public SampleChartData()
        {
        }

        /// <summary>
        /// Default data is appropriate for the most chart types.
        /// </summary>
        public ChartData DefaultData
        {
            get
            {
                if (_defaultData == null)
                {
                    _defaultData = new ChartData();

                    DataSeries ds1 = new DataSeries() { Label = "s1" };
                    ds1.ValuesSource = new double[] { 3, 2, 7, 4, 8 };
                    _defaultData.Children.Add(ds1);

                    DataSeries ds2 = new DataSeries() { Label = "s2" };
                    ds2.ValuesSource = new double[] { 1, 2, 3, 4, 6 };
                    _defaultData.Children.Add(ds2);

                    DataSeries ds3 = new DataSeries() { Label = "s3" };
                    ds3.ValuesSource = new double[] { 0, 1, 6, 2, 3 };
                    _defaultData.Children.Add(ds3);
                }

                return _defaultData;
            }
        }

        /// <summary>
        /// Data for financial charts(HighLowOpenClose and Candle).
        /// </summary>
        public ChartData FinancialData
        {
            get
            {
                if (_finData == null)
                {
                    _finData = new ChartData();

                    HighLowOpenCloseSeries ds = new HighLowOpenCloseSeries() { Label = "s1" };
                    ds.SymbolStrokeThickness = 3; ds.SymbolSize = new Size(20, 20);
                    ds.XValuesSource = new DateTime[] { 
            new DateTime(2008,10,1), new DateTime(2008,10,2), new DateTime(2008,10,3),
            new DateTime(2008,10,6), new DateTime(2008,10,7), new DateTime(2008,10,8) 
          };
                    ds.OpenValuesSource = new double[] { 100, 102, 104, 100, 107, 102 };
                    ds.CloseValuesSource = new double[] { 102, 104, 100, 107, 102, 100 };
                    ds.HighValuesSource = new double[] { 102, 105, 105, 108, 109, 105 };
                    ds.LowValuesSource = new double[] { 99, 95, 95, 100, 96, 99, 98 };
                    _finData.Children.Add(ds);
                }
                return _finData;
            }
        }

        /// <summary>
        /// Data for Bubble chart.
        /// </summary>
        public ChartData BubbleData
        {
            get
            {
                if (_bubbleData == null)
                {
                    _bubbleData = new ChartData();

                    BubbleSeries ds1 = new BubbleSeries() { Label = "s1" };
                    ds1.ValuesSource = new double[] { 3, 4, 7, 5, 8 };
                    ds1.SizeValuesSource = new double[] { 1, 5, 7, 4, 9 };
                    _bubbleData.Children.Add(ds1);

                    BubbleSeries ds2 = new BubbleSeries() { Label = "s2" };
                    ds2.ValuesSource = new double[] { 1, 2, 3, 4, 6 };
                    ds2.SizeValuesSource = new double[] { 10, 3, 6, 8, 9 };
                    _bubbleData.Children.Add(ds2);
                }

                return _bubbleData;
            }
        }

        /// <summary>
        /// Data for Gantt chart.
        /// </summary>
        public ChartData GanttData
        {
            get
            {
                if (_ganttData == null)
                {
                    _ganttData = new ChartData();
                    _ganttData.ItemNames = new string[] { "Task1", "Task2", "Task3", "Task4" };

                    HighLowSeries ds1 = new HighLowSeries() { Label = "Task1" };
                    ds1.XValuesSource = new double[] { 0 };
                    ds1.LowValuesSource = new DateTime[] { new DateTime(2008, 10, 1) };
                    ds1.HighValuesSource = new DateTime[] { new DateTime(2008, 10, 5) };
                    _ganttData.Children.Add(ds1);

                    HighLowSeries ds2 = new HighLowSeries() { Label = "Task2" };
                    ds2.XValuesSource = new double[] { 1 };
                    ds2.LowValuesSource = new DateTime[] { new DateTime(2008, 10, 3) };
                    ds2.HighValuesSource = new DateTime[] { new DateTime(2008, 10, 4) };
                    _ganttData.Children.Add(ds2);

                    HighLowSeries ds3 = new HighLowSeries() { Label = "Task3" };
                    ds3.XValuesSource = new double[] { 2 };
                    ds3.LowValuesSource = new DateTime[] { new DateTime(2008, 10, 2) };
                    ds3.HighValuesSource = new DateTime[] { new DateTime(2008, 10, 8) };
                    _ganttData.Children.Add(ds3);

                    HighLowSeries ds4 = new HighLowSeries() { Label = "Task4" };
                    ds4.XValuesSource = new double[] { 3 };
                    ds4.LowValuesSource = new DateTime[] { new DateTime(2008, 10, 4) };
                    ds4.HighValuesSource = new DateTime[] { new DateTime(2008, 10, 7) };
                    _ganttData.Children.Add(ds4);

                    foreach (DataSeries ds in _ganttData.Children)
                    {
                        ds.SymbolSize = new Size(30, 30);
                    }
                }
                return _ganttData;
            }
        }
    }
}
