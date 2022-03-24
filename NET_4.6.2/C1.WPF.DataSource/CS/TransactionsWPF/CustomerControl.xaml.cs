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
using C1.Data.Transactions;
using C1.Data;
using C1.Data.Entities;
using C1.LiveLinq.LiveViews;
using C1.WPF.LiveLinq;


namespace ClientTransactions
{
    // This class is used as the result item type of an order view defined below.
    public class OrderInfo
    {
        public OrderInfo(Order o)
        {
            _order = o;
        }
        private Order _order;
        public Order GetOrder() { return _order; }

        public virtual int OrderID { get; set; }
        public virtual DateTime? OrderDate { get; set; }
        public virtual decimal? Freight { get; set; }
        public virtual string ShipName { get; set; }
        public virtual string ShipCity { get; set; }
    }

    /// <summary>
    /// Interaction logic for CustomerControl.xaml
    /// </summary>
    public partial class CustomerControl : UserControl
    {
        // The customer being edited.
        public Customer Customer { get; private set; }
        // This view contains the orders of the customer.
        View _ordersView;
        // All changes in this control are made in the scope of this client transaction.
        ClientTransaction _transaction;
        EntityClientCache _cache;

        public CustomerControl(Customer customer, EntityClientCache clientCache)
        {
            InitializeComponent();
            _cache = clientCache;
            Customer = customer;
            // Define and bind a live view of orders of the customer.
            _ordersView = from o in customer.Orders.AsLive()
                          select new OrderInfo(o)
                          {
                              OrderID = o.OrderID,
                              OrderDate = o.OrderDate,
                              Freight = o.Freight,
                              ShipName = o.ShipName,
                              ShipCity = o.ShipCity
                          };
            CreateTransaction();
            ordersGrid.ItemsSource = _ordersView;
        }

        void transaction_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // The Undo button is enabled if and only if there are changes made in the transaction scope.
            buttonUndo.IsEnabled = _transaction.HasChanges;
        }

        void CreateTransaction()
        {
            // Initialize the client transaction.
            // Specify that all changes to _ordersView are automatically considered changes made in the scope of this transaction.
            _transaction = _cache.CreateTransaction();
            _ordersView.SetTransaction(_transaction);

            // Enable/disable the Undo button when the _transaction.HasChanges property value changes.
            _transaction.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(transaction_PropertyChanged);
            buttonUndo.IsEnabled = false;
        }

        void undo_Click(object sender, RoutedEventArgs e)
        {
            // Cancel the changes made by this control
            // by rolling _transaction back.
            _transaction.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(transaction_PropertyChanged);
            _transaction.Rollback();
            // Create a new transaction.
            CreateTransaction();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            if (_transaction.HasChanges)
            {
                // Ask the user whether they will save or cancel the changes made by this control.
                switch (MessageBox.Show("Do you want to save changes?", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
                {
                    case MessageBoxResult.Yes:
                        _transaction.Commit();
                        break;
                    case MessageBoxResult.No:
                        _transaction.Rollback();
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                }
            }
            // Raise the CloseRequested event, so MainWindow removes the tab page with this control.
            CloseRequested(this, EventArgs.Empty);
        }

        // MainWindow closes the tab page with this control when this event is raised.
        public event EventHandler CloseRequested = delegate { };

        private void addOrder(object sender, RoutedEventArgs e)
        {
            // Add a new Order in a child transaction scope.
            // Rolling back the child transaction will not roll back the parent transaction.
            using (var tran2 = new ClientTransaction(_transaction)) // Create a child transaction.
            {
                var order = new Order();
                var window = new OrderWindow(order, tran2);
                if (window.ShowDialog() == true)
                {
                    // Add the order in the child transaction scope and commit the transaction.
                    using(tran2.Scope())
                    {
                        Customer.Orders.Add(order);
                        tran2.Commit();
                    }
                }
            } // The child transaction will be rolled back automatically if it is not committed.
        }

        private void editOrder(object sender, RoutedEventArgs e)
        {
            // Edit the currently selected order in a child transaction.

            var order = ordersGrid.SelectedItem as OrderInfo;
            if (order == null)
                return;

            // Create a child transaciton and edit the order in the child transaction scope.
            using (var tran2 = new ClientTransaction(_transaction))
            {
                var window = new OrderWindow(order.GetOrder(), tran2);
                if (window.ShowDialog() == true)
                {
                    tran2.Commit();
                }
            } // The child transaction will be rolled back automatically if it is not committed.
        }

        private void deleteOrder(object sender, RoutedEventArgs e)
        {
            // Remove the selected order.

            var order = ordersGrid.SelectedItem as OrderInfo;
            if (order == null)
                return;
            using (_transaction.Scope())
            {
                // Make the change in the transaction scope.
                Customer.Orders.Remove(order.GetOrder());
            }
        }

        private void orderDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Start editing the order on double click.
            editOrder(sender, e);
        }
    }
}
