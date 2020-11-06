using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.Data.Entities;
using C1.Data;


namespace ClientTransactions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // A client cache and a client scope that are used to load the data.
        EntityClientCache _clientCache;
        EntityClientScope _scope;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the client cache and the client scope.
            _clientCache = new EntityClientCache(new NORTHWNDEntities());
            _scope = new EntityClientScope(_clientCache);

            // Bind a combo box to the list of cities
            // using a live view of customers grouped by city.
            comboCity.ItemsSource =
                (from c in _scope.GetItems<Customer>()
                 group c by c.City into g
                 orderby g.Key
                 select new
                 {
                     City = g.Key,
                     Customers = g
                 }).AsDynamic();
        }

        private void EditOrdersForCustomer()
        {
            // Start editing the selected customer.

            var customer = listCustomers.SelectedItem as Customer;
            if (customer == null)
                return;

            // If the customer is already being edited, then select the tab page associated with that customer.
            var tab = (from TabItem t in tabs.Items
                       let productCtl = (CustomerControl)t.Content
                       where productCtl.Customer == customer
                       select t).FirstOrDefault();
            if (tab != null)
            {
                tab.IsSelected = true;
                return;
            }

            // Create a CustomerControl for the current customer
            // and display it in the tab control.
            var customerControl = new CustomerControl(customer, _clientCache);
            var tabItem = new TabItem
            {
                Header = customer.ContactName,
                Content = customerControl
            };
            tabs.Items.Add(tabItem);
            tabItem.IsSelected = true;

            // Remove the created tab page when the Close button is pressed.
            customerControl.CloseRequested += delegate
            {
                tabs.Items.Remove(tabItem);
            };
        }

        private void listCustomers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Start editing the selected customer on double click.
            EditOrdersForCustomer();
        }
    }
}
