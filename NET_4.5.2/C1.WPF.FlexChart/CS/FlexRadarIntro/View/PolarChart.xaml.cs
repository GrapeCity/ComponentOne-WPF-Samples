using C1.WPF;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;

namespace FlexRadarIntro
{
    /// <summary>
    /// Interaction logic for PolarChart.xaml
    /// </summary>
    public partial class PolarChart : UserControl
    {
        public PolarChart()
        {
            InitializeComponent();
            this.Loaded += PolarChart_Loaded;
        }

        private void PolarChart_Loaded(object sender, RoutedEventArgs e)
        {
            polarChart.ItemsSource = CreateData(10, 2);
        }

        List<Point> CreateData(double k, double a)
        {
            var pts = new List<Point>();
            int n = 360;
            for (var i = 0; i < n; i++)
            {
                var angle = Math.PI * i / 180;
                pts.Add(new Point() { X = i, Y = (float)(Math.Cos(k * angle) + a) });
            }
            return pts;
        }

        void OnNumericBoxValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            if (numericUpDown2 != null && numericUpDown2 != null && polarChart != null)
            {
                polarChart.ItemsSource = CreateData((double)numericUpDownStartAngle.Value, (double)numericUpDown2.Value);
            }
        }
    }
}
