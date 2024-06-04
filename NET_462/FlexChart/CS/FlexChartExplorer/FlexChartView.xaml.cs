using System.Windows.Controls;

namespace FlexChartExplorer
{
    /// <summary>
    /// Interaction logic for FlexChartView.xaml
    /// </summary>
    public partial class FlexChartView : UserControl
    {
        public FlexChartView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
        }
    }
}
