using C1.WPF.Gauge;
using C1.WPF.Core;
using C1.WPF.Input;
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

        private void NumericBox_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            if (sender == null) return;

            var nb = (C1NumericBox)sender;

            if (nb?.Value == null)
                return;

            double val = nb.Value;

            if (val < nb.Minimum) nb.Value = nb.Minimum;
            else if (val > nb.Maximum) nb.Value = nb.Maximum;
        }
    }
}
