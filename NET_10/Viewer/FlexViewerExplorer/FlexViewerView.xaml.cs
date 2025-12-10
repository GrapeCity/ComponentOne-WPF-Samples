using System.Windows.Controls;

namespace FlexViewerExplorer
{
    /// <summary>
    /// Interaction logic for FlexChartView.xaml
    /// </summary>
    public partial class FlexViewerView : UserControl
    {
        public FlexViewerView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
        }
    }
}
