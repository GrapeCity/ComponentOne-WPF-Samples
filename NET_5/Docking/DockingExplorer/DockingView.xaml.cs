using System.Windows.Controls;

namespace DockingExplorer
{
    /// <summary>
    /// Interaction logic for DockingView.xaml
    /// </summary>
    public partial class DockingView : UserControl
    {
        public DockingView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
            Loaded += DockingView_Loaded;
        }

        private void DockingView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }

        private void lbSamples_SelectionChanged_1(object sender, C1.WPF.Core.SelectionChangedEventArgs<int> e)
        {
            MainWindow.RemoveFloatingWindow();
        }
    }
}
