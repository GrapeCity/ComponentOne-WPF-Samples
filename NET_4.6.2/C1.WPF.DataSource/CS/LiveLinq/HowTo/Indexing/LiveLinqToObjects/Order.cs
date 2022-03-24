using System;
using C1.LiveLinq.Collections;
using C1.LiveLinq.Indexing;

namespace LiveLinqToObjects
{
    public class Order : IndexableObject
    {
        int orderID;
        public int OrderID
        {
            get { return orderID; }
            set
            {
                if (orderID == value)
                    return;

                orderID = value;
                OnPropertyChanged("OrderID");
            }
        }

        string customerID;
        public string CustomerID
        {
            get { return customerID; }
            set
            {
                if (customerID == value)
                    return;

                customerID = value;
                OnPropertyChanged("CustomerID");

            }
        }

        DateTime orderDate;
        public DateTime OrderDate
        {
            get { return orderDate; }
            set
            {
                if (orderDate == value)
                    return;

                orderDate = value;
                OnPropertyChanged("OrderDate");
            }
        }

        decimal freight;
        public decimal Freight
        {
            get { return freight; }
            set
            {
                if (freight == value)
                    return;

                freight = value;
                OnPropertyChanged("Freight");
            }
        }

        string shipCity;
        public string ShipCity
        {
            get { return shipCity; }
            set
            {
                if (shipCity == value)
                    return;

                shipCity = value;
                OnPropertyChanged("ShipCity"); ;
            }
        }
    }
}