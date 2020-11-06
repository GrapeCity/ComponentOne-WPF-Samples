using DataManipulation.Business.FunctionSeries;
using System;
using System.Windows.Controls;

namespace DataManipulation
{
    /// <summary>
    /// Interaction logic for TopNView.xaml
    /// </summary>
    public partial class YFunctionSeriesView : UserControl
    {
        YFunctionSeries series;
        public YFunctionSeriesView()
        {
            InitializeComponent();
            series = new YFunctionSeries();
            series.SampleCount = 300;
            series.Min = -10;
            series.Max = 10;
            series.SeriesName = "Y-Function Series";
            series.Function = Calculate;
            this.flexChart1.Series.Clear();
            this.flexChart1.Series.Add(series);
        }
        private double Calculate(double v)
        {
            return Math.Sin(4 * v) * Math.Cos(3 * v);
        }
    }
}
