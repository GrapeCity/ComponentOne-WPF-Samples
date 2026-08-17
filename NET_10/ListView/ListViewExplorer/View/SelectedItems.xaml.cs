using ListViewExplorer.Resources;
using System.Windows.Controls;

namespace ListViewExplorer
{
    public partial class SelectedItems : UserControl
    {
        public SelectedItems()
        {
            InitializeComponent();

            Tag = AppResources.SelectedItemsDescription;
        }
    }
}
