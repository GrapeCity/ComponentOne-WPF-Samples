using System;
using System.Collections.Generic;
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

using C1.Chart;
using C1.WPF.Chart;
using C1.WPF.Chart.Extended;

namespace ExtendedFeatures.Samples
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : UserControl
    {
        public Loading()
        {
            InitializeComponent();
        }

        private void ChartLoaded(object sender, RoutedEventArgs e)
        {
            if (chart.Series.Count > 0)
                return;

            chart.BeginUpdate();

            var scale = new DiscreteColorScale() { Intervals = new List<DiscreteColorScale.Interval>() };
            scale.Intervals.Add(new DiscreteColorScale.Interval(0, 10, Color.FromRgb(0x05,0x71,0xB0), "Very Low"));
            scale.Intervals.Add(new DiscreteColorScale.Interval(10, 25, Color.FromRgb(0x92, 0xC5, 0xDE), "Low"));
            scale.Intervals.Add(new DiscreteColorScale.Interval(25, 75, Color.FromRgb(0xD7, 0xD7, 0xD7), "Normal"));
            scale.Intervals.Add(new DiscreteColorScale.Interval(75, 90, Color.FromRgb(0xF4, 0xA5, 0x82), "High"));
            scale.Intervals.Add(new DiscreteColorScale.Interval(90, 100, Color.FromRgb(0xCA, 0x00, 0x20), "Critical"));

            var rnd = new Random();
            var data = new double[24, 7];
            for (var j = 0; j < 7; j++)
                for (var i = 0; i < 24; i++)
                    data[i, j] = 10 * Math.Exp(-(i - 12) * (i - 12) / (2 * 4.0 * 4.0)) / Math.Sqrt(2 * Math.PI * 4.0 * 4.0) * ((j == 5 || j == 6) ? 50 : 100) * rnd.NextDouble();

            var hmap = new Heatmap();
            hmap.ItemsSource = data;
            hmap.ColorScale = scale;
            chart.Series.Add(hmap);

            var times = new string[24];
            for (var i = 0; i < 24; i++)
                times[i] = new DateTime(2000, 1, 1, i, 0, 0).ToShortTimeString();
            chart.AxisX.ItemsSource = times;

            chart.AxisY.ItemsSource = new string[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

            chart.EndUpdate();
        }
    }
}
