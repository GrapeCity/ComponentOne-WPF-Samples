using System.Windows.Controls;

namespace PropertyGridExplorer
{
    public partial class DataObjectProperties : UserControl
    {
        public DataObjectProperties()
        {
            InitializeComponent();
            Tag = Properties.Resources.DataObjectPropertiesDesc;

            listView.ItemsSource = Customer.GetCustomerList(100);
        }
    }
}
