using System;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using C1.WPF.LiveLinq;

using C1.LiveLinq;
using C1.LiveLinq.LiveViews;
using C1.Data;
using C1.Data.Entities;

namespace Orders
{
    public partial class EditOrders : System.Windows.Controls.UserControl
    {
        // These two classes are used as the result item types of the two live views defined below.
        public class OrderInfo
        {
            public virtual Orders.Order Order { get; set; }
            public virtual int OrderID { get; set; }
            public virtual DateTime? OrderDate { get; set; }
            public virtual Customer Customer { get; set; }
            public virtual decimal TotalCost { get; set; }
            public virtual View<Order_Detail> OrderDetails { get; set; }
        }

        public class OrderDetailInfo
        {
            public virtual int ProductID { get; set; }
            public virtual Product Product { get; set; }
            public virtual int Quantity { get; set; }
            public virtual decimal UnitPrice { get; set; }
            public virtual float Discount { get; set; }
            public virtual decimal TotalCost { get; set; }
        }

        // These live views are bound to the data grids.
        C1.LiveLinq.LiveViews.View _ordersView;
        C1.LiveLinq.LiveViews.View _orderDetailsView;

        public EditOrders()
        {
            InitializeComponent();

            // Define and bind a live view on top of the Orders EntityViewSource 
            // sorted by the total cost in descending order.
            dataGrid1.ItemsSource = _ordersView =
                from o in dataSource["Orders"].AsLive<Orders.Order>()
                // Order cost is the sum of order detail costs.
                let totalCost = o.Order_Details.Sum(od => od.Quantity * od.UnitPrice * (1m - (decimal)od.Discount))
                orderby totalCost descending
                select new OrderInfo()
                {
                    Order = o,
                    OrderID = o.OrderID,
                    OrderDate = o.OrderDate,
                    Customer = o.Customer,
                    TotalCost = totalCost,
                    OrderDetails = o.Order_Details.AsLive()
                };
        }

        private void DataGrid1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Rebind the data grid, display the order details of the selected order.
            // If the "checkDiscontinued" check box is checked, show only the order details for discontinued products.
            var item = dataGrid1.SelectedItem as OrderInfo;
            if (item != null)
                dataGrid2.ItemsSource = _orderDetailsView =
                    from od in item.OrderDetails
                    where (!checkDiscontinued.IsChecked ?? true) || od.Product.Discontinued
                    select new OrderDetailInfo()
                    {
                        ProductID = od.ProductID,
                        Product = od.Product,
                        Quantity = od.Quantity,
                        UnitPrice = od.UnitPrice,
                        Discount = od.Discount,
                        TotalCost = od.Quantity * od.UnitPrice * (1m - (decimal)od.Discount)
                    };
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Update the Order filter, display the orders of customers in the selected city.
            var city = comboBox.SelectedValue as string;
            if (city != null)
            {
                // Update the filter value.
                dataSource.ViewSources["Orders"].FilterDescriptors[0].Value = city;
                // Manually reload the EntityViewSource because auto-loading is turned off (AutoLoad=False).
                dataSource.ViewSources["Orders"].Load();
            }
        }

        private void addOrder(object sender, System.Windows.RoutedEventArgs e)
        {
            // Add a new Order.

            if (dataGrid1.SelectedItem == null)
            {
                // The user must select an Order to create a new one.
                MessageBox.Show("Must be positioned on an order to be able to create a new one");
                return;                
            }
            var order = ((OrderInfo)dataGrid1.SelectedItem).Order;
            var newOrder = new Orders.Order();
            // Assign the new Order to the Customer of the currently selected Order.
            newOrder.Customer = order.Customer;
            newOrder.OrderID = 99999;
        }

        private void deleteOrder(object sender, System.Windows.RoutedEventArgs e)
        {
            // Remove the selected Order.
            ((IEditableCollectionView)dataGrid1.ItemsSource).Remove(((ICollectionView)dataGrid1.ItemsSource).CurrentItem);
        }

        private void addOrderDetail(object sender, System.Windows.RoutedEventArgs e)
        {
            // Add a new Order Detail.
            ((IEditableCollectionView)dataGrid2.ItemsSource).AddNew();
        }

        private void deleteOrderDetail(object sender, System.Windows.RoutedEventArgs e)
        {
            // Remove the selected Order Detail.
            ((IEditableCollectionView)dataGrid2.ItemsSource).Remove(((ICollectionView)dataGrid2.ItemsSource).CurrentItem);
        }

        private void checkDiscontinued_Changed(object sender, RoutedEventArgs e)
        {
            // Rebuild the order detail view to reapply filtering.
            _orderDetailsView.Rebuild();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Select the first city in the combo box.
            if (comboBox.Items.Count != 0)
                comboBox.SelectedIndex = 0;
        }
    }
}
