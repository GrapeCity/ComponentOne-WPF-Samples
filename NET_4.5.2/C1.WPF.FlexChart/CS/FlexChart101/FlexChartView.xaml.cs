using System.Windows.Controls;

namespace FlexChart101
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
