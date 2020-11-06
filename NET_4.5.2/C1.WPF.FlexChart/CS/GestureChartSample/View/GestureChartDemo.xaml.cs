using System.Windows;
using System.Windows.Controls;

namespace GestureChartSample
{
    public sealed partial class GestureChartDemo : UserControl
    {
        public GestureChartDemo()
        {
            this.InitializeComponent();
        }

        private void OnResetButtonClick(object sender, RoutedEventArgs e)
        {
            gestureChart.AxisX.Min = double.NaN;
            gestureChart.AxisX.Max = double.NaN;
            gestureChart.AxisY.Min = double.NaN;
            gestureChart.AxisY.Max = double.NaN;
        }
    }
}
