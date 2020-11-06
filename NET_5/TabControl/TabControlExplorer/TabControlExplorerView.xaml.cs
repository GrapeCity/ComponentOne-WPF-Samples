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
        }
    }
}
