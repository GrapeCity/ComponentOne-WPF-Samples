using System.Windows.Controls;

namespace MapsExplorer
{
    /// <summary>
    /// Interaction logic for FlexChartView.xaml
    /// </summary>
    public partial class MapsView : UserControl
    {
        public MapsView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
        }
    }
}
