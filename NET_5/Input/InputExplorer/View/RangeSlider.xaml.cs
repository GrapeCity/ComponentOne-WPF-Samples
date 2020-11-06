using System.Windows.Controls;

namespace InputExplorer
{
    /// <summary>
    /// Interaction logic for InputView.xaml
    /// </summary>
    public partial class RangeSlider : UserControl
    {
        public RangeSlider()
        {
            InitializeComponent();
            Tag = "The C1RangeSlider control extends the basic slider control and provides two thumbs, making it possible for users to select a range. Users can drag each thumb individually or drag the center to modify both at the same time.";
        }
    }
}
