using System.Windows.Controls;

namespace ColorPickerExplorer
{
    /// <summary>
    /// Interaction logic for ColorPickerView.xaml
    /// </summary>
    public partial class ColorPickerView : UserControl
    {
        public ColorPickerView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
            Loaded += ColorPickerView_Loaded;
        }

        private void ColorPickerView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
