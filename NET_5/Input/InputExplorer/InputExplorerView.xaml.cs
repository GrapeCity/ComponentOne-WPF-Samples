using System.Windows.Controls;

namespace InputExplorer
{
    /// <summary>
    /// Interaction logic for InputExplorerView.xaml
    /// </summary>
    public partial class InputExplorerView : UserControl
    {
        public InputExplorerView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
        }
    }
}
