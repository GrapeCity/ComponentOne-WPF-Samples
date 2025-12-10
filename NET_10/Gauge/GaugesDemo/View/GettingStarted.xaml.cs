using C1.WPF.Gauge;
using GaugesDemo.Resources;
using System.Windows.Controls;

namespace GaugesDemo
{
    public partial class GettingStarted : UserControl
    {
        public GettingStarted()
        {
            InitializeComponent();
            Tag = AppResources.GettingStartedDescription;
            this.lblValue.Content = AppResources.Value;
            DataContext = new SampleViewModel() { Value = 25, ShowText = GaugeTextVisibility.None, IsReadOnly = false };

        }

    }
}
