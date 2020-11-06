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
using System.Windows.Shapes;
using C1.Data.Transactions;
using C1.Data.Entities;
using C1.WPF.LiveLinq;

namespace ClientTransactions
{
    // This class is used as the result item type of an order view defined below.
    public class OrderDetailsInfo
    {
        public virtual int OrderID { get; set; }
        public virtual int ProductID { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual float Discount { get; set; }
        public virtual decimal Quantity { get; set; }
    }

    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        // All changes in this window are made in the scope of this client transaction.
        ClientTransaction _transaction;

        public OrderWindow(Order order, ClientTransaction transaction)
        {
            InitializeComponent();
            _transaction = transaction;

            // Create a proxy for the order that automatically opens the transaction scope
            // each time a value is assigned to a property; and use it for data binding.
            DataContext = transaction.ScopeDataContext(order);

            // Define a live view of order details.
            var orderDetailsView = from od in order.Order_Details.AsLive()
                                   select new OrderDetailsInfo
                                   {
                                       OrderID = od.OrderID,
                                       ProductID = od.ProductID,
                                       UnitPrice = od.UnitPrice,
                                       Discount = od.Discount,
                                       Quantity = od.Quantity
                                   };
            // All changes in this view are made in the transaction scope.
            orderDetailsView.SetTransaction(_transaction);
            orderDetailsGrid.ItemsSource = orderDetailsView;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
