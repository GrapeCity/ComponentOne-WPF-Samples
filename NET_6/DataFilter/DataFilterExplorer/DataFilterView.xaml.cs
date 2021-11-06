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
            Loaded += DataFilterView_Loaded;
        }

        private void DataFilterView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
