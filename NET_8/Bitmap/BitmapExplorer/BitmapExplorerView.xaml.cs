using System.Windows.Controls;

namespace BitmapExplorer
{
    /// <summary>
    /// Interaction logic for MenuExplorerView.xaml
    /// </summary>
    public partial class BitmapExplorerView : UserControl
    {
        public BitmapExplorerView()
        {
            InitializeComponent();
            DataContext = new SampleDataSource();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
