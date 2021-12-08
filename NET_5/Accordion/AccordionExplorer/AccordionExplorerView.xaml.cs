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
            Loaded += AccordionExplorerView_Loaded;
        }

        private void AccordionExplorerView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
