using System.Windows.Controls;

namespace TabControlExplorer
{
    /// <summary>
    /// Interaction logic for DockingView.xaml
    /// </summary>
    public partial class TabControlExplorerView : UserControl
    {
        public TabControlExplorerView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
            Loaded += TabControlExplorerView_Loaded;
        }

        private void TabControlExplorerView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
