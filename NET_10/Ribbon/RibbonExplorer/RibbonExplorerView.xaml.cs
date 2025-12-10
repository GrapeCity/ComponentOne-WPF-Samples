using System.Windows.Controls;

namespace RibbonExplorer
{
    /// <summary>
    /// Interaction logic for RibbonExplorerView.xaml
    /// </summary>
    public partial class RibbonExplorerView : UserControl
    {
        public RibbonExplorerView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
            Loaded += RibbonExplorerView_Loaded;
        }

        private void RibbonExplorerView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
