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
            Loaded += FlexChartView_Loaded;
        }

        private void FlexChartView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
