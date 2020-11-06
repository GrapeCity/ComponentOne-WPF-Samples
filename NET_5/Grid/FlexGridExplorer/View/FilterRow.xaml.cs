using FlexGridExplorer.Resources;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for FilterRow.xaml
    /// </summary>
    public partial class FilterRow : UserControl
    {
        public FilterRow()
        {
            InitializeComponent();
            Tag = AppResources.FilterRowDescription;
            var data = Customer.GetCustomerList(100);
            grid.ItemsSource = data;
        }
    }
}
