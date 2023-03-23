using C1.WPF.Gauge;
using GaugesDemo.Resources;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GaugesDemo
{
    public partial class AutomaticScaling : UserControl
    {
        public AutomaticScaling()
        {
            InitializeComponent();
            Tag = AppResources.AutomaticScalingDescription;
            this.lblStartAngle.Content = AppResources.StartAngle;
            this.lblSweepAngle.Content = AppResources.SweepAngle;
            this.lblReversed.Content = AppResources.Reversed;
            DataContext = new SampleViewModel() { Max = 200, Value = 60, ShowText = GaugeTextVisibility.All };
        }
    }
}
