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
    /// Interaction logic for C1RulerGaugePage.xaml
    /// </summary>
    public partial class C1RulerGaugePage : UserControl
    {
        public C1RulerGaugePage()
        {
            InitializeComponent();
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
            if (linearGauge.Orientation == Orientation.Vertical)
            {
                linearGauge.Width = width;
                linearGauge.Height = height;
            }
            else
            {
                linearGauge.Width = height;
                linearGauge.Height = width;
            }
        }

    }
}
