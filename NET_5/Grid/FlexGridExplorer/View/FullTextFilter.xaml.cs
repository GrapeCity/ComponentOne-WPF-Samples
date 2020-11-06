using FlexGridExplorer.Resources;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for FullTextFilter.xaml
    /// </summary>
    public partial class FullTextFilter : UserControl
    {
        public FullTextFilter()
        {
            InitializeComponent();
            Tag = AppResources.FullTextFilterDescription;
            var data = Customer.GetCustomerList(100);
            grid.ItemsSource = data;
            grid.MinColumnWidth = 85;
        }
    }
}
