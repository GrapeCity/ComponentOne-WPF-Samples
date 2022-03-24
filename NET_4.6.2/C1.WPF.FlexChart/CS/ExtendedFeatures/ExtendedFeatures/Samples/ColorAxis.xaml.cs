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
    /// Interaction logic for ColorAxis.xaml
    /// </summary>
    public partial class ColorAxis : UserControl
    {
        public ColorAxis()
        {
            InitializeComponent();
        }

        private void ChartLoaded(object sender, RoutedEventArgs e)
        {
            if (chart.Series.Count > 0)
                return;

            chart.BeginUpdate();

            var scale = new GradientColorScale()
            {
                Min = 0,
                Max = 4,
                Axis = new C1.WPF.Chart.Extended.ColorAxis() {
                    Position = Position.Right,
                    Title = "deformation, mm"
                },
                Colors = new List<Color> { Colors.Yellow, Colors.Red }
            };

            var data = new double[50, 50];
            for (var i = 0; i < 50; i++)
                for (var j = 0; j < 50; j++)
                    data[i, j] = 2 + (Math.Sin(0.1 * i) + Math.Cos(0.1 * j));

            var hmap = new Heatmap();
            hmap.ItemsSource = data;
            hmap.ColorScale = scale;
            hmap.StartX = 0;
            hmap.StartY = 0;
            chart.Series.Add(hmap);

            chart.EndUpdate();
        }
    }
}
