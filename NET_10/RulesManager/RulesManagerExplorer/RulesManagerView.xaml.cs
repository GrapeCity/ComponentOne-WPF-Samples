using System.Windows.Controls;

namespace RulesManagerExplorer
{
    public partial class RulesManagerView : UserControl
    {
        public RulesManagerView()
        {
            InitializeComponent();
            DataContext = new SampleDataSource();
            Loaded += OnLoaded;
        }
        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
