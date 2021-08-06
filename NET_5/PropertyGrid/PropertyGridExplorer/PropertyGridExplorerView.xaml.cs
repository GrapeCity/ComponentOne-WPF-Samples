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
        }
    }
}
