using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for FlexChartView.xaml
    /// </summary>
    public partial class FlexGridView : UserControl
    {
        public FlexGridView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
            Loaded += FlexGridView_Loaded;
        }

        private void FlexGridView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
