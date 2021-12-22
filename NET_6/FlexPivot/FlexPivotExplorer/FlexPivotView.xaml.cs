using System.Windows.Controls;

namespace FlexPivotExplorer
{
    /// <summary>
    /// Interaction logic for FlexChartView.xaml
    /// </summary>
    public partial class FlexPivotView : UserControl
    {
        public FlexPivotView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
            Loaded += FlexPivotView_Loaded;
        }

        private void FlexPivotView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
