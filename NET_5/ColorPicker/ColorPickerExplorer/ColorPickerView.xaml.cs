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
        }
    }
}
