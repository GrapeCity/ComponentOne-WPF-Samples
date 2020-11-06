using C1.WPF.Chart;
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

namespace WeatherChart
{
    /// <summary>
    /// Interaction logic for WeatherChartDemo.xaml
    /// </summary>
    public partial class WeatherChartDemo : UserControl
    {
        public WeatherChartDemo()
        {
            InitializeComponent();
        }

        void OnChartRendered(object sender, RenderEventArgs e)
        {
            var flexChart = sender as C1FlexChart;
            if (flexChart == null)
                return;

            var rect = flexChart.PlotRect;
            e.Engine.SetFill(Colors.Transparent);
            e.Engine.SetStroke(new SolidColorBrush(Colors.DimGray));
            e.Engine.SetStrokeThickness(1d);
            e.Engine.DrawRect(rect.X, rect.Y, rect.Width, rect.Height);
        }

        private void rangeSelector_Loaded(object sender, RoutedEventArgs e)
        {
            rangeSelector.UpperValue = 42550;
        }
    }
}
