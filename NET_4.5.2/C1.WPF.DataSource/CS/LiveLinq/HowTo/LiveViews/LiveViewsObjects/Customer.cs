using System;
using System.ComponentModel;
using C1.LiveLinq.Collections;
using C1.LiveLinq.Indexing;

namespace LiveViewsObjects
{
    public class Customer : IndexableObject
    {
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

        string city;
        public string City
        {
            get { return city; }
            set
            {
                if (city == value)
                    return;
                city = value;
                OnPropertyChanged("City");
            }
        }


        DateTime dateRegistered;
        public DateTime DateRegistered
        {
            get { return dateRegistered; }
            set
            {
                if (dateRegistered == value)
                    return;

                dateRegistered = value;
                OnPropertyChanged("DateRegistered");
            }
        }
    }
}