using System.Windows.Controls;

namespace MenuExplorer
{
    /// <summary>
    /// Interaction logic for MenuExplorerView.xaml
    /// </summary>
    public partial class MenuExplorerView : UserControl
    {
        public MenuExplorerView()
        {
            InitializeComponent();
            DataContext = new SampleDataSource();
            Loaded += MenuExplorerView_Loaded;
        }

        private void MenuExplorerView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
