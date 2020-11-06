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
    /// Interaction logic for Temperature.xaml
    /// </summary>
    public partial class Temperature : UserControl
    {
        public Temperature()
        {
            InitializeComponent();
        }

        private void ChartLoaded(object sender, RoutedEventArgs e)
        {
            if (chart.Series.Count > 0)
                return;

            chart.BeginUpdate();
            chart.AxisY.MajorGrid = false;

            var scale = new GradientColorScale() { Min = -30, Max = 30 };
            scale.Colors = new List<Color> { Colors.Blue, Colors.White, Colors.Red };

            var hmap = new Heatmap();
            hmap.ItemsSource = new double[,] {
                {  3.0, 3.1, 5.7, 8.2, 12.5, 15.0, 17.1, 17.1, 14.3, 10.6, 6.6, 4.3 },
                { -9.3, -7.7, -2.2, 5.8, 13.1, 16.6, 18.2, 16.4, 11.0, 5.1, -1.2, -6.1},
                 { -15.1, -12.5, -5.2, 3.1, 10.1, 15.5, 18.3, 15.0, 9.4, 1.4, -5.6, -11.4},
                };
            hmap.ColorScale = scale;
            chart.Series.Add(hmap);

            chart.AxisX.ItemsSource = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            chart.AxisY.ItemsSource = new string[] { "Amsterdam", "Moscow", "Perm" };

            chart.EndUpdate();
        }
    }
}
