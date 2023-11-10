using System.Windows.Controls;

namespace CoreExplorer
{
    /// <summary>
    /// Interaction logic for CoreView.xaml
    /// </summary>
    public partial class CoreView : UserControl
    {
        public CoreView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
            Loaded += CoreView_Loaded;
        }

        private void CoreView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
