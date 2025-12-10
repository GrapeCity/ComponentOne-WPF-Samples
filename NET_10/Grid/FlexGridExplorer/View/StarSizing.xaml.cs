using FlexGridExplorer.Resources;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for StarSizing.xaml
    /// </summary>
    public partial class StarSizing : UserControl
    {
        public StarSizing()
        {
            InitializeComponent();
            Tag = AppResources.StarSizingDescription;
            var data = Customer.GetCustomerList(100);
            grid.ItemsSource = data;
        }
    }
}
