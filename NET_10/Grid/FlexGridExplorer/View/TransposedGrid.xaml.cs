using FlexGridExplorer.Resources;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    public partial class TransposedGrid : UserControl
    {
        public TransposedGrid()
        {
            InitializeComponent();
            Tag = AppResources.TransposedGridDescription;

            grid.ItemsSource = Customer.GetCustomerList(100);
            grid.MinColumnWidth = 85;
        }
    }
}
