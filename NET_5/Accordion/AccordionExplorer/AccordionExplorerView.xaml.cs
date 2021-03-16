using System.Windows.Controls;

namespace AccordionExplorer
{
    /// <summary>
    /// Interaction logic for MenuExplorerView.xaml
    /// </summary>
    public partial class AccordionExplorerView : UserControl
    {
        public AccordionExplorerView()
        {
            InitializeComponent();
            DataContext = new SampleDataSource();
        }
    }
}
