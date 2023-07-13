using C1.WPF.Input;
using System.Linq;
using System.Windows.Controls;

namespace InputExplorer
{
    public partial class StopsSlider : UserControl
    {
        public StopsSlider()
        {
            InitializeComponent();
            Tag = Properties.Resources.StopsSliderDescription;
            UpdateLabel();
        }

        private void OnStopsChanged(object sender, StopsChangedEventArgs e)
        {
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            label.Text = string.Join(" - ", stops.Stops.Select(s => $"{s:N2}"));
        }
    }
}
