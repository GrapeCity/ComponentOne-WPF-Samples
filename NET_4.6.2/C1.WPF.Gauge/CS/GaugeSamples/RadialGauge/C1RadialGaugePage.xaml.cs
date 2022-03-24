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
    /// Interaction logic for DemoRadialGauge.xaml
    /// </summary>
    public partial class DemoRadialGauge : UserControl
    {
        bool _isPressed;

        public DemoRadialGauge()
        {
            InitializeComponent();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            myGauge.CaptureMouse();
            _isPressed = true ;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isPressed = false;
            myGauge.ReleaseMouseCapture();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if(_isPressed)
            {
                myGauge.Value = GaugeUtil.RadialPointToValue(myGauge, e.GetPosition(myGauge));
                gauge.To = myGauge.Value;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            gauge.To = myGauge.Value;
        }
    }
}
