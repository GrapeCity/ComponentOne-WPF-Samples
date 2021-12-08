using System.Windows.Controls;

namespace MapsExplorer
{
    /// <summary>
    /// Interaction logic for FlexChartView.xaml
    /// </summary>
    public partial class MapsView : UserControl
    {
        public MapsView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
            Loaded += MapsView_Loaded;
        }

        private void MapsView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
