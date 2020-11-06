using System.Windows.Controls;

namespace DataFilterExplorer
{
    /// <summary>
    /// Interaction logic for FlexChartView.xaml
    /// </summary>
    public partial class DataFilterView : UserControl
    {
        public DataFilterView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
        }
    }
}
