using C1.WPF.Chart;
using C1.WPF.Chart.Palettes;
using FlexChartExplorer.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FlexChartExplorer
{
    /// <summary>
    /// Interaction logic for Waterfall.xaml
    /// </summary>
    public partial class ContourChart : UserControl
    {
        public ContourChart()
        {
            InitializeComponent();
            Tag = AppResources.ContourTag;
        }

        private void ChartLoaded(object sender, RoutedEventArgs e)
        {
            if (chart.Series.Count > 0)
                return;

            chart.BeginUpdate();
            chart.RenderMode = RenderMode.Direct2D;

            var contour = new Contour();
            var scale = new GradientColorScale() { Min = 0, Max = 100 };
            scale.Colors = Diverging.RdBu.Select(b => ((SolidColorBrush)b).Color).Reverse().ToList();
            contour.ColorScale = scale;

            var data = MonkeySaddleData();
            contour.ItemsSource = data;
            chart.Series.Add(contour);

            chart.AxisX.Min = 0;
            chart.AxisX.Max = data.GetLength(0) - 1;
            chart.AxisY.Min = 0;
            chart.AxisY.Max = data.GetLength(1) - 1;

            chart.EndUpdate();
        }

        private static double[,] MonkeySaddleData()
        {
            int width = 20;
            int height = 20;
            double[,] data = new double[width, height];
            double minValue = 0;
            double maxValue = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Center the coordinates
                    double xc = x - width / 2.0;
                    double yc = y - height / 2.0;

                    // Monkey saddle equation: z = x³ - 3xy²
                    data[x, y] = (xc * xc * xc - 3 * xc * yc * yc);

                    // set initial min and max value, can't be 0.
                    if (x == 0 && y == 0)
                    {
                        maxValue = data[0, 0];
                        minValue = data[0, 0];
                    }

                    // Track min and max values
                    minValue = Math.Min(minValue, data[x, y]);
                    maxValue = Math.Max(maxValue, data[x, y]);
                }
            }

            // normalize to range [0,100] 
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    data[x,y] = 100 * (data[x, y] - minValue) / (maxValue - minValue);

            return data;
        }
    }
}
