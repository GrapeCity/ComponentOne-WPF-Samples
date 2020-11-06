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
        }

        private void lbSamples_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainWindow.RemoveFloatingWindow();
        }
    }
}
