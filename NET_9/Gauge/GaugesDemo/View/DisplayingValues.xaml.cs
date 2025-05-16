using GaugesDemo.Resources;
using System.Windows.Controls;

namespace GaugesDemo
{
    public partial class DisplayingValues : UserControl
    {
        public DisplayingValues()
        {
            InitializeComponent();
            this.lblShowText.Content = AppResources.ShowText;
            this.lblValue.Content = AppResources.Value;
            Tag = AppResources.DisplayingValuesDescription;
            DataContext = new SampleViewModel() { Max = 1, Value = .25, Step = .01, Format = "P0", ShowRanges = false };
        }
    }
}
