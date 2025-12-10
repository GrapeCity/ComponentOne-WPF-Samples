using FlexGridExplorer.Resources;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for GettingStarted.xaml
    /// </summary>
    public partial class SelectedItems : UserControl
    {
        public SelectedItems()
        {
            InitializeComponent();
            Tag = AppResources.SelectedItemsDescription;
            grid.ItemsSource = Customer.GetCustomerList(100);
        }
    }
}
