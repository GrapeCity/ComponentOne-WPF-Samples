using PropertyGridExplorer.Resources;
using System.Windows.Controls;

namespace PropertyGridExplorer
{
    public partial class DataObjectProperties : UserControl
    {
        public DataObjectProperties()
        {
            InitializeComponent();
            Tag = AppResources.DataObjectPropertiesDesc;

            listView.ItemsSource = Customer.GetCustomerList(100);
        }
    }
}
