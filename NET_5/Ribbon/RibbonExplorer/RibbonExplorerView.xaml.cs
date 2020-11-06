using System.Windows.Controls;

namespace RibbonExplorer
{
    /// <summary>
    /// Interaction logic for RibbonExplorerView.xaml
    /// </summary>
    public partial class RibbonExplorerView : UserControl
    {
        public RibbonExplorerView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
        }
    }
}
