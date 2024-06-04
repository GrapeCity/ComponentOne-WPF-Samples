using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Xps.Packaging;
using C1.Chart;
using C1.WPF.Chart;

namespace FlexChartPrint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FlexChartBase[] charts = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnPrintSingle_Click(object sender, RoutedEventArgs e)
        {
            var printHelper = new PrintHelper(1);

            printHelper.PagePrinting += (s, a) =>
            {
                var rect = a.PrintableArea;
                var cnv = new Canvas();

                cnv.Children.Add(printHelper.ChartToCanvas(lineChart, new Rect(0, 0, rect.Width, 0.5 * rect.Height)));
                cnv.Children.Add(printHelper.ChartToCanvas(columnChart, new Rect(0, 0.5 * rect.Height, 0.5*rect.Width, 0.5 * rect.Height)));
                cnv.Children.Add(printHelper.ChartToCanvas(pieChart, new Rect(0.5 * rect.Width, 0.5 * rect.Height, 0.5*rect.Width, 0.5 * rect.Height)));

                a.Visual = cnv;
            };

            printHelper.PrintPreview();
        }

        private void btnPrintMulti_Click(object sender, RoutedEventArgs e)
        {
            var printHelper = new PrintHelper(charts.Length);

            printHelper.PagePrinting += (s, a) =>
            {
                a.Visual = printHelper.ChartToCanvas(charts[a.PageNumber-1], a.PrintableArea);
            };

            printHelper.PrintPreview();
        }

        private void btnPrintMultiLine_Click(object sender, RoutedEventArgs e)
        {
            double actualMin = ((IAxis)lineChart.AxisX).GetMin();
            double actualMax = ((IAxis)lineChart.AxisX).GetMax();
            double range = actualMax - actualMin;

            var printHelper = new PrintHelper(10, lineChart);

            printHelper.PagePrinting += (s, a) =>
            {
                lineChart.AxisX.Min = actualMin + (a.PageNumber - 1) * range / 10;
                lineChart.AxisX.Max = actualMin + a.PageNumber * range / 10;
            };

            printHelper.PagePrinted += (s, a) =>
            {
                lineChart.AxisX.Min = double.NaN;
                lineChart.AxisX.Max = double.NaN;
            };

            printHelper.PrintPreview();
        }

        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            // init charts
            if (charts == null)
            {
                SetupLineChart();
                SetupColumnChart();
                SetupPieChart();
                charts = new FlexChartBase[] { lineChart, columnChart, pieChart };
            }
        }

        #region Sample charts

        private void SetupPieChart()
        {
            pieChart.BeginUpdate();
            var len = 10;
            var data = new object[len];
            for (var i = 0; i < 4; i++)
                data[i] = new { Name = "Q " + (i + 1).ToString(), Value = i + 1 };

            pieChart.Binding = "Value";
            pieChart.BindingName = "Name";
            pieChart.ItemsSource = data;
            pieChart.EndUpdate();
        }

        private void SetupColumnChart()
        {
            columnChart.BeginUpdate();

            var len = 10;
            for (var j = 0; j < 2; j++)
            {
                var pts = new Point[len];
                for (int i = 0; i < pts.Length; i++)
                    pts[i] = new Point(i, 2.0 + Math.Sin(i - 0.5 * j));
                var ser = new Series() { ItemsSource = pts, SeriesName = "S " + (j + 1).ToString() };
                ser.BindingX = "X";
                ser.Binding = "Y";
                columnChart.Series.Add(ser);
            }

            columnChart.EndUpdate();
        }

        private void SetupLineChart()
        {
            var rnd = new Random();
            var pts1 = new Point[1001];
            for (int i = 0; i < pts1.Length; i++)
            {
                var r = rnd.NextDouble();
                pts1[i] = new Point(i, 10 * r * Math.Sin(0.1 * i) * Math.Sin(0.6 * rnd.NextDouble() * i));
            }

            var pts2 = new Point[1001];
            for (int i = 0; i < pts2.Length; i++)
            {
                var r = rnd.NextDouble();
                pts2[i] = new Point(i, 10 * r * Math.Sin(0.1 * i) * Math.Sin(0.6 * rnd.NextDouble() * i));
            }

            lineChart.BeginUpdate();

            var ser1 = new Series() { ItemsSource = pts1 };
            ser1.BindingX = "X";
            ser1.Binding = "Y";
            ser1.SeriesName = "s1";
            lineChart.Series.Add(ser1);

            var ser2 = new Series() { ItemsSource = pts2 };
            ser2.BindingX = "X";
            ser2.Binding = "Y";
            ser2.SeriesName = "s2";
            lineChart.Series.Add(ser2);

            lineChart.ChartType = ChartType.Line;
            lineChart.LegendPosition = Position.Bottom;
            lineChart.EndUpdate();
        }

        #endregion
    }

}
