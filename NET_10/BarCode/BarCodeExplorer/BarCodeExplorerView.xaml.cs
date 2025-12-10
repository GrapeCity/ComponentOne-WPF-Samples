using System.Windows.Controls;

namespace BarCodeExplorer
{
    /// <summary>
    /// Interaction logic for BarCodeExplorerView.xaml
    /// </summary>
    public partial class BarCodeExplorerView : UserControl
    {
        public BarCodeExplorerView()
        {
            InitializeComponent();
            DataContext = new SampleDataSource();
            Loaded += View_Loaded;
        }

        private void View_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                lbSamples.SelectedItem = lbSamples.Items[0];
            });
        }
    }
}
