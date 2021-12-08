using System.Windows;
using System.Windows.Controls;

namespace SpellCheckerExplorer
{
    /// <summary>
    /// Interaction logic for SpellCheckerView.xaml
    /// </summary>
    public partial class SpellCheckerView : UserControl
    {
        public SpellCheckerView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
            Loaded += SpellCheckerView_Loaded;
        }

        private void SpellCheckerView_Loaded(object sender, RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
