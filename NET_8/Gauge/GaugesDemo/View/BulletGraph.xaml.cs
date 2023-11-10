using C1.WPF.Gauge;
using GaugesDemo.Resources;
using System.Windows.Controls;

namespace GaugesDemo
{
    public partial class BulletGraph : UserControl
    {
        public BulletGraph()
        {
            InitializeComponent();
            Tag = AppResources.BulletGraphDescription;
            this.lblBad.Content = AppResources.Bad;
            this.lblGood.Content = AppResources.Good;
            this.lblTarget.Content = AppResources.Target;
            DataContext = new SampleViewModel() { Value = 72, ShowText = GaugeTextVisibility.All };
        }
    }
}
