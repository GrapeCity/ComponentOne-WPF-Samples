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
        }
    }
}
