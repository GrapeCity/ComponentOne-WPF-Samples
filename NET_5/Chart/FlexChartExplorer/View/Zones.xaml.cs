using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using C1.Chart;
using C1.WPF.Chart;
using FlexChartExplorer.Resources;

namespace FlexChartExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Zones : UserControl
    {
        const int STUDENTS_COUNT = 200;
        const int MAX_POINT = 1600;
        Random rnd = new Random();
        double[] _zones;
        StudentScore[] _data = new StudentScore[STUDENTS_COUNT];
        Color[] _zones_colors = new Color[]
        {
            Color.FromArgb(64, 255,192,192),
            Color.FromArgb(128, 55,228,228),
            Color.FromArgb(128, 255,228,128),
            Color.FromArgb(128, 128,255,128),
            Color.FromArgb(128, 128,128,225)
        };

        public Zones()
        {
            this.InitializeComponent();

            this.Loaded += Zones_Loaded;
            Tag = AppResources.ZonesTag;
        }

        private void Zones_Loaded(object sender, RoutedEventArgs e)
        {
            SetupChart();
        }

        public void SetupChart()
        {
            if (flexChart != null && flexChart.ItemsSource != null)
                return;

            for (var i = 0; i < STUDENTS_COUNT; i++)
            {
                _data[i] = new StudentScore() { Number = i, Score = (int)(MAX_POINT * 0.5 * (1 + rnd.NextDouble())) };
            }
            flexChart.ItemsSource = _data;
            // Calculate statistics
            var mean = FindMean(_data);
            var stdDev = FindStdDev(_data, mean);
            flexChart.BeginUpdate();
            for (var i = -2; i <= 2; i++)
            {
                var y = mean + i * stdDev;
                var sdata = new Point[]
                {
                    new Point(0, y),
                    new Point(STUDENTS_COUNT - 1, y)
                };

                var series = new Series()
                {
                    BindingX = "X",
                    Binding = "Y",
                    ChartType = ChartType.Line,
                    Style = new ChartStyle()
                    {
                        Stroke = flexChart.Foreground,
                        StrokeThickness = 2
                    },
                    ItemsSource = sdata
                };

                if (Math.Abs(i) == 1)
                    series.Style.StrokeDashArray = new DoubleCollection() { 5, 1 };
                else if (Math.Abs(i) == 2)
                    series.Style.StrokeDashArray = new DoubleCollection() { 2, 2};

                if (i > 0)
                    series.SeriesName = "m+" + i + "s";
                else if (i < 0)
                    series.SeriesName = "m" + i + "s";
                else
                    series.SeriesName = "mean";

                flexChart.Series.Add(series);
            }
            var scores = _data.Select(x => x.Score).OrderByDescending(x => x).ToArray();
            _zones = new double[]
            {
                scores[GetBoundingIndex(scores, 0.95)],
                scores[GetBoundingIndex(scores, 0.75)],
                scores[GetBoundingIndex(scores, 0.25)],
                scores[GetBoundingIndex(scores, 0.05)]
            };

            // Add _zones to legend
            for (var i = 0; i < 5; i++)
            {
                var series = new Series()
                {
                    ChartType = ChartType.Area,
                    SeriesName = "ABCDE"[i].ToString(),
                    Style = new ChartStyle()
                    {
                        Stroke = new SolidColorBrush(Colors.Transparent),
                        Fill = new SolidColorBrush(_zones_colors[4 - i])
                    }
                };
                flexChart.Series.Add(series);
            }

            flexChart.EndUpdate();
        }

        void DrawAlarmZone(FlexChart chart, IRenderEngine engine, double xmin, double ymin, double xmax, double ymax, Color fill)
        {
            var pt1 = chart.DataToPoint(new Point(xmin, ymin));
            var pt2 = chart.DataToPoint(new Point(xmax, ymax));
            engine.SetFill(new SolidColorBrush(fill));
            engine.SetStroke(new SolidColorBrush(Colors.Transparent));
            engine.DrawRect(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y), Math.Abs(pt2.X - pt1.X), Math.Abs(pt2.Y - pt1.Y));
        }

        int GetBoundingIndex(int[] data, double frac)
        {
            var n = data.Length;
            var i = (int)Math.Ceiling(n * frac);
            while (i > data[0] && data[i] == data[i + 1])
                i--;

            return i;
        }

        double FindMean(StudentScore[] data)
        {
            double sum = 0;
            data.ToList().ForEach(x => sum = sum + x.Score);
            return sum / data.Length;
        }

        double FindStdDev(StudentScore[] data, double mean)
        {
            double sum = 0;
            data.ToList().ForEach(x => 
            {
                var d = x.Score - mean;
                sum += d * d;
            });
            return Math.Sqrt(sum / data.Length);
        }

        private void flexChart_Rendering(object sender, RenderEventArgs e)
        {
            var engine = e.Engine;
            var chart = sender as FlexChart;
            if (engine == null || chart == null || _zones == null)
                return;

            for (int i = 0; i < 5; i++)
            {
                var ymin = i == 0 ? ((IAxis)chart.AxisY).GetMin() : _zones[i - 1];
                var ymax = i == 4 ? ((IAxis)chart.AxisY).GetMax() : _zones[i];
                DrawAlarmZone(chart, engine, ((IAxis)chart.AxisX).GetMin(), ymin, ((IAxis)chart.AxisX).GetMax(), ymax, _zones_colors[i]);
            }
        }
    }

    public class StudentScore
    {
        public int Number { get; set; }
        public int Score { get; set; }
    }
}
