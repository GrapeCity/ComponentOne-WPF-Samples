using System.Windows;
using System.Windows.Controls;

namespace SparklineExplorer
{
    /// <summary>
    /// Interaction logic for SpellCheckerView.xaml
    /// </summary>
    public partial class SparklineView: UserControl
    {
        public SparklineView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
