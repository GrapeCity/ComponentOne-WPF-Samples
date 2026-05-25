using C1.WPF.Gauge;
using System.Windows.Controls;

namespace SimpleGauges
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SimpleGaugesDemo : UserControl
    {
        public SimpleGaugesDemo()
        {
            InitializeComponent();
            this.DataContext = new SampleViewModel() { Value = 25, TextVisibility = GaugeTextVisibility.All };
            Tag = Properties.Resources.Tag;
        }
    }
}
