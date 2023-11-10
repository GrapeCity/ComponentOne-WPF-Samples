using ColorPickerExplorer.Resources;
using System.Windows.Controls;

namespace ColorPickerExplorer
{
    public partial class Brushes : UserControl
    {
        public Brushes()
        {
            InitializeComponent();
            Tag = AppResources.BrushPickerDescription;
        }
    }
}
