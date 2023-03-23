using GaugesDemo.Resources;
using System.Windows.Controls;

namespace GaugesDemo
{
    public partial class UsingRanges : UserControl
    {
        public UsingRanges()
        {
            InitializeComponent();
            this.lblShowRanges.Content = AppResources.ShowRanges;
            this.lblValue.Content = AppResources.Value;
            Tag = AppResources.UsingRangesDescription;

            DataContext = new SampleViewModel() { Value = 25, ShowRanges = true };

            linearGauge.Pointer.Thickness = 0.5;
            radialGauge.Pointer.Thickness = 0.5;
        }
    }
}
