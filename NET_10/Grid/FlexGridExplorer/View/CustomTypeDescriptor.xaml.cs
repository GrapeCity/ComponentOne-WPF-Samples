using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using FlexGridExplorer.ViewModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlexGridExplorer.View
{
    /// <summary>
    /// Interaction logic for CustomTypeDescriptor.xaml
    /// </summary>
    public partial class CustomTypeDescriptor : UserControl
    {
        public CustomTypeDescriptor()
        {
            InitializeComponent();

            Tag = AppResources.CustomTypeDescriptorDescription;

            var customerList = Customer.GetCustomerList(100);
            flexGrid.Columns.Add(new GridColumn
            {
                Header = "Full Name",
                Binding = "[FullName]"
            });

            flexGrid.Columns.Add(new GridColumn
            {
                Header = "Email",
                Binding = "[Email]"
            });
            var items = new List<CustomTypeCustomer>();

            foreach (var customer in customerList)
            {
                var dynCust = new CustomTypeCustomer();
                dynCust.AddProperty("FullName", String.Concat(customer.FirstName," ",customer.LastName));
                dynCust.AddProperty("Email", customer.Email);
                items.Add(dynCust);
            }
            flexGrid.ItemsSource = items;
            flexGrid.MinColumnWidth = 85;
            flexGrid.AllowDragging = GridAllowDragging.Both;
            flexGrid.AllowResizing = GridAllowResizing.Both;
        }
    }
}
