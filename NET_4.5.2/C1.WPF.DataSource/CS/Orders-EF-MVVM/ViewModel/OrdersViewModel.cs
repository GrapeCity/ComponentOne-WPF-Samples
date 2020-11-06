using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

using C1.LiveLinq.LiveViews;
using C1.WPF.LiveLinq;
using C1.Data.Entities;
using C1.Data;

namespace Orders.ViewModel
{
    using Orders.Model;

    // This file contains View Models used in the OrdersView.

    // The properties of the following two view models 
    // must be virtual to make live view items modifiable.
    public class OrderViewModel
    {
        public virtual int OrderID { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual DateTime? OrderDate { get; set; }
        public virtual decimal TotalCost { get; set; }
        public virtual IList<OrderDetailViewModel> OrderDetails { get; set; }
    }

    public class OrderDetailViewModel
    {
        public virtual int OrderID { get; set; }
        public virtual int ProductID { get; set; }
        public virtual string ProductName { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual decimal Discount { get; set; }
        public virtual decimal TotalCost { get; set; }
    }

    public class OrdersViewModel : WorkspaceViewModel
    {
        // Data grids in this view are bound to these data sources.
        public IList Orders { get; private set; }
        public ICollectionView Cities { get; private set; }

        public OrdersViewModel(NORTHWNDContext data, bool showDiscontinued)
        {
            base.DisplayName = showDiscontinued ? "Discontinued" : "Edit Orders";

            Cities = new CollectionViewSource() { Source = data.Cities }.View;

            // Define a live view of orders filtered by Customer on the server,
            // containing order details
            Orders =  from ord in data.Orders.AsFiltered(o => o.Customer != null)
                          .AsFilteredBound(o => o.Customer.City).BindFilterKey(Cities, "CurrentItem")
                          .Include("Customer").Include("Order_Details.Product")
                      let orderDetails = 
                            from od in ord.Order_Details.AsLive()
                            where !showDiscontinued || od.Product.Discontinued // show only discontinued products if showDiscontinued==true
                            select new OrderDetailViewModel()
                            {
                                OrderID = ord.OrderID,
                                ProductID = od.ProductID,
                                ProductName = od.Product.ProductName,
                                Quantity = od.Quantity,
                                UnitPrice = od.UnitPrice,
                                Discount = (decimal)od.Discount,
                                TotalCost = od.Quantity * od.UnitPrice * (1m - (decimal)od.Discount)
                            }
                      where orderDetails.Count() != 0 // filter out orders without details
                      select new OrderViewModel()
                      {
                          OrderID = ord.OrderID,
                          OrderDate = ord.OrderDate,
                          CustomerName = ord.Customer.CompanyName + " (" + ord.Customer.CustomerID + ", " + ord.Customer.City + ")",
                          TotalCost = orderDetails.Sum(od => od.Quantity * od.UnitPrice * (1m - od.Discount)), // Order cost is the sum of order detail costs.
                          OrderDetails = orderDetails
                      };
        }
    }
}
