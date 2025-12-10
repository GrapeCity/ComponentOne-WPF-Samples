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
            Loaded += InputExplorerView_Loaded;
        }

        private void InputExplorerView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
