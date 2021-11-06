using System.Windows.Controls;

namespace PropertyGridExplorer
{
    /// <summary>
    /// Interaction logic for PropertyGridExplorerView.xaml
    /// </summary>
    public partial class PropertyGridExplorerView : UserControl
    {
        public PropertyGridExplorerView()
        {
            InitializeComponent();
            DataContext = new SampleDataSource();
            Loaded += PropertyGridExplorerView_Loaded;
        }

        private void PropertyGridExplorerView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
