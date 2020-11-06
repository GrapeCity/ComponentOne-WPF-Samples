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

namespace GaugeSamples
{
    /// <summary>
    /// Interaction logic for DemoLinearGauge.xaml
    /// </summary>
    public partial class DemoLinearGauge : UserControl
    {
        bool _isPressed;
        double _orginWidth, _orginHeight;

        public DemoLinearGauge()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _orginWidth = linearGauge.ActualWidth;
            _orginHeight = linearGauge.ActualHeight;
            gauge.To = linearGauge.Value;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            linearGauge.CaptureMouse();
            _isPressed = true;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isPressed = false;
            linearGauge.ReleaseMouseCapture();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isPressed)
            {
                linearGauge.Value = GaugeUtil.LinearPointToValue(linearGauge, e.GetPosition(linearGauge));
                gauge.To = linearGauge.Value;
            }
        }

        private void Orientation_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGaugeSize();
        }

        private void UpdateGaugeSize()
        {
            if (!IsLoaded)
                return;
            double width = (double)FindResource("gaugeWidth");
            double height = (double)FindResource("gaugeHeight");
            if (linearGauge.Orientation == Orientation.Horizontal)
            {
                linearGauge.Width = _orginWidth;
                linearGauge.Height = _orginHeight;
                linearGauge.RenderTransformOrigin = new Point(0, 0);
                linearGauge.RenderTransform = null;
                linearGauge.FaceTemplate = (DataTemplate)FindResource("ValueFaceTemplateHorizontal");
                GaugeValue.Margin = new Thickness(0, 40, 0, -40);
            }
            else
            {
                linearGauge.Width = width;
                linearGauge.Height = height;
                linearGauge.RenderTransformOrigin = new Point(0.5, 0.5);
                linearGauge.RenderTransform = new ScaleTransform(1, -1);
                linearGauge.FaceTemplate = (DataTemplate)FindResource("ValueFaceTemplateVertical");
                GaugeValue.Margin = new Thickness(50, 0, -50, 0);
            }
        }

    }
}
