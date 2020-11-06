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
        }

        private void lbSamples_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SampleItem selectedItem = e.AddedItems[0] as SampleItem;
            grid.DataContext = selectedItem;
        }
    }
}
