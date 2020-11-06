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
        }

        private void lbSamples_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SampleItem selectedItem = e.AddedItems[0] as SampleItem;
            grid.DataContext = selectedItem;
        }
    }
}
