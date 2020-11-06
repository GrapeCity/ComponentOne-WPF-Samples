using DataManipulation.Business.FunctionSeries;
using System;
using System.Windows.Controls;

namespace DataManipulation
{
    /// <summary>
    /// Interaction logic for TopNView.xaml
    /// </summary>
    public partial class ParametricFunctionSeriesView : UserControl
    {
        ParametricFunctionSeries series;
        public ParametricFunctionSeriesView()
        {
            InitializeComponent();
            series = new ParametricFunctionSeries();
            series.SampleCount = 1000;
            series.Max = 2 * Math.PI;
            series.SeriesName = "Parametric Function Series";
            series.XFunction = CalculateX;
            series.YFunction = CalculateY;

            flexChart1.AxisX.Min = -1.5;
            flexChart1.AxisX.Max = 1.5;
            flexChart1.AxisX.MajorUnit = 0.2;

            this.flexChart1.Series.Clear();
            this.flexChart1.Series.Add(series);
        }
        private double CalculateX(double v)
        {
            return Math.Cos(5 * v);
        }

        private double CalculateY(double v)
        {
            return Math.Sin(7 * v);
        }
    }
}
