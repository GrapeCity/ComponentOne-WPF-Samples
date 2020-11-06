using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.WPF.C1Chart;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace ChartSamples
{
    /// <summary>
    /// Interaction logic for RangeBarChart.xaml
    /// </summary>
    [System.ComponentModel.Description("This sample displays a Gantt chart to visualize high and low temperatures.")]
    public partial class RangeBarChart : UserControl
    {
        public RangeBarChart()
        {
            InitializeComponent();
            SetTemperatureChart();
        }
        private void SetTemperatureChart()
        {
            // read data
            CSVData data = new CSVData();
            using (Stream stream = Assembly.GetExecutingAssembly().
                GetManifestResourceStream("ChartSamples.Resources.weatherYear.csv"))
            {
                data.Read(stream, true, true);
            }

            int len = data.Length;
            WeatherData[] wdata = new WeatherData[len];

            double min = double.MaxValue;
            double max = double.MinValue;

            // fill the array
            for (int i = 0; i < len; i++)
            {
                wdata[i] = new WeatherData(DateTime.Parse(data[i, 0]),
                      double.Parse(data[i, "Max TemperatureF"]),
                      double.Parse(data[i, "Mean TemperatureF"]),
                      double.Parse(data[i, "Min TemperatureF"]));

                min = Math.Min(min, wdata[i].TMin);
                max = Math.Max(max, wdata[i].TMax);
            }

            if (len > 0)
            {
                chart.BeginUpdate();
                chart.Data.Children.Clear();
                chart.Data.ItemsSource = wdata;


                // create data series
                HighLowSeries ds = new HighLowSeries();
                ds.ChartType = ChartType.Gantt;
                ds.Label = "Temp";
                ds.LowValueBinding = new Binding("TMin");
                ds.XValueBinding = new Binding("DateTime");
                ds.HighValueBinding = new Binding("TMax");
                ds.Style = this.Resources["highlowseries"] as Style;

                chart.Data.Children.Add(ds);

                XYDataSeries ds2 = new XYDataSeries();
                ds2.ChartType = ChartType.Line;
                ds2.Label = "Average";
                ds2.ValueBinding = new Binding("TAvg");
                ds2.XValueBinding = new Binding("DateTime");
                chart.Data.Children.Add(ds2);

                // set axis min and max
                chart.View.AxisX.Min = wdata[0].DateTime.Subtract(new TimeSpan(1, 0, 0, 0)).ToOADate();
                chart.View.AxisX.Max = wdata[len - 1].DateTime.ToOADate();

                chart.View.AxisY.Min = min;
                chart.View.AxisY.Max = max;
                
                // style chart
                chart.View.AxisX.IsTime = true;
                chart.View.AxisY.MajorTickThickness = 0;
                chart.View.AxisY.MinorTickThickness = 0;
                chart.View.AxisY.AxisLine = new Line() { StrokeThickness = 0 };
                chart.View.AxisX.Scale = 0.4;
                chart.View.AxisX.ScrollBar = new AxisScrollBar();
                chart.View.AxisX.MajorGridStrokeThickness = 0;
                chart.View.AxisX.MinorGridStrokeThickness = 0;
                chart.View.AxisX.MajorTickThickness = 0;
                chart.View.AxisX.MinorTickThickness = 0;
                chart.View.AxisX.AxisLine = new Line() { StrokeThickness = 0 };
                chart.View.AxisY.Title = new TextBlock() { Text = "Temperature", HorizontalAlignment = System.Windows.HorizontalAlignment.Center };

                chart.EndUpdate();
            }

        }

    }

}
