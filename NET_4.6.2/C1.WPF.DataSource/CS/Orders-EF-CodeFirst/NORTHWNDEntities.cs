using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

using C1.Data.Entities;

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orders
{
    public class NORTHWNDEntities : DbContext
    {
        EntityClientCache _clientCache;
        public EntityClientCache ClientCache
        {
            get
            {
                if (_clientCache == null)
                    _clientCache = new EntityClientCache(this);
                return _clientCache;
            }
        }

        // Returns all customer countries.
        public IEnumerable<string> Countries
        {
            get
            {
                return (from c in Customers
                        select c.Country).Distinct().ToList();
            }
        }
        // Returns all customer cities.
        public IEnumerable<string> Cities
        {
            get
            {
                return (from c in Customers
                        select c.City).Distinct().ToList();
            }
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order_Detail> Order_Details { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

    }

    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Customer : ObservableObject
    {
        string _customerId;
        public string CustomerID
        {
            get { return _customerId; }
            set
            {
                _customerId = value;
                OnPropertyChanged("CustomerID");
            }
        }
        string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                _companyName = value;
                OnPropertyChanged("CompanyName");
            }
        }
        string _contactName;
        public string ContactName
        {
            get { return _contactName; }
            set
            {
                _contactName = value;
                OnPropertyChanged("ContactName");
            }
        }
        string _contactTitle;
        public string ContactTitle
        {
            get { return _contactTitle; }
            set
            {
                _contactTitle = value;
                OnPropertyChanged("ContactTitle");
            }
        }
        string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }
        string _city;
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged("City");
            }
        }
        string _region;
        public string Region
        {
            get { return _region; }
            set
            {
                _region = value;
                OnPropertyChanged("Region");
            }
        }
        string _postalCode;
        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                _postalCode = value;
                OnPropertyChanged("PostalCode");
            }
        }
        string _country;
        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                OnPropertyChanged("Country");
            }
        }
        string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged("Phone");
            }
        }
        string _fax;
        public string Fax
        {
            get { return _fax; }
            set
            {
                _fax = value;
                OnPropertyChanged("Fax");
            }
        }

        public virtual ObservableCollection<Order> Orders { get; set; }
    }

    public class Order : ObservableObject
    {
        int _orderId;
        public int OrderID
        {
            get { return _orderId; }
            set
            {
                _orderId = value;
                OnPropertyChanged("OrderID");
            }
        }
        string _customerId;
        public string CustomerID
        {
            get { return _customerId; }
            set
            {
                _customerId = value;
                OnPropertyChanged("CustomerID");
            }
        }
        int? _employeeId;
        public int? EmployeeID
        {
            get { return _employeeId; }
            set
            {
                _employeeId = value;
                OnPropertyChanged("EmployeeID");
            }
        }
        DateTime? _orderDate;
        public DateTime? OrderDate
        {
            get { return _orderDate; }
            set
            {
                _orderDate = value;
                OnPropertyChanged("OrderDate");
            }
        }
        DateTime? _requiredDate;
        public DateTime? RequiredDate
        {
            get { return _requiredDate; }
            set
            {
                _requiredDate = value;
                OnPropertyChanged("RequiredDate");
            }
        }
        DateTime? _shippedDate;
        public DateTime? ShippedDate
        {
            get { return _shippedDate; }
            set
            {
                _shippedDate = value;
                OnPropertyChanged("ShippedDate");
            }
        }
        int? _shipVia;
        public int? ShipVia
        {
            get { return _shipVia; }
            set
            {
                _shipVia = value;
                OnPropertyChanged("ShipVia");
            }
        }
        decimal? _freight;
        public decimal? Freight
        {
            get { return _freight; }
            set
            {
                _freight = value;
                OnPropertyChanged("Freight");
            }
        }
        string _shipName;
        public string ShipName
        {
            get { return _shipName; }
            set
            {
                _shipName = value;
                OnPropertyChanged("ShipName");
            }
        }
        string _shipAddress;
        public string ShipAddress
        {
            get { return _shipAddress; }
            set
            {
                _shipAddress = value;
                OnPropertyChanged("ShipAddress");
            }
        }
        string _shipCity;
        public string ShipCity
        {
            get { return _shipCity; }
            set
            {
                _shipCity = value;
                OnPropertyChanged("ShipCity");
            }
        }
        string _shipRegion;
        public string ShipRegion
        {
            get { return _shipRegion; }
            set
            {
                _shipRegion = value;
                OnPropertyChanged("ShipRegion");
            }
        }
        string _shipPostalCode;
        public string ShipPostalCode
        {
            get { return _shipPostalCode; }
            set
            {
                _shipPostalCode = value;
                OnPropertyChanged("ShipPostalCode");
            }
        }
        string _shipCountry;
        public string ShipCountry
        {
            get { return _shipCountry; }
            set
            {
                _shipCountry = value;
                OnPropertyChanged("ShipCountry");
            }
        }

        Customer _customer;
        public Customer Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                OnPropertyChanged("Customer");
            }
        }
        public virtual ObservableCollection<Order_Detail> Order_Details { get; set; }
    }

    [Table("[Order Details]")]
    public class Order_Detail : ObservableObject
    {
        int _orderId;
        [Key]
        [Column(Order = 1)] 
        public int OrderID
        {
            get { return _orderId; }
            set
            {
                _orderId = value;
                OnPropertyChanged("OrderID");
            }
        }
        int _productId;
        [Key]
        [Column(Order = 2)] 
        public int ProductID
        {
            get { return _productId; }
            set
            {
                _productId = value;
                OnPropertyChanged("ProductID");
            }
        }
        decimal _unitPrice;
        public decimal UnitPrice
        {
            get { return _unitPrice; }
            set
            {
                _unitPrice = value;
                OnPropertyChanged("UnitPrice");
            }
        }
        short _quantity;
        public short Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }
        float _discount;
        public float Discount
        {
            get { return _discount; }
            set
            {
                _discount = value;
                OnPropertyChanged("Discount");
            }
        }

        Order _order;
        public Order Order
        {
            get { return _order; }
            set
            {
                _order = value;
                OnPropertyChanged("Order");
            }
        }
        Product _product;
        public Product Product
        {
            get { return _product; }
            set
            {
                _product = value;
                OnPropertyChanged("Product");
            }
        }
    }

    public class Product : ObservableObject
    {
        int _productId;
        public int ProductID
        {
            get { return _productId; }
            set
            {
                _productId = value;
                OnPropertyChanged("ProductID");
            }
        }
        string _productName;
        public string ProductName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                OnPropertyChanged("ProductName");
            }
        }
        int? _supplierId;
        public int? SupplierID
        {
            get { return _supplierId; }
            set
            {
                _supplierId = value;
                OnPropertyChanged("SupplierID");
            }
        }
        int? _categoryId;
        public int? CategoryID
        {
            get { return _categoryId; }
            set
            {
                _categoryId = value;
                OnPropertyChanged("CategoryID");
            }
        }
        string _quantityPerUnit;
        public string QuantityPerUnit
        {
            get { return _quantityPerUnit; }
            set
            {
                _quantityPerUnit = value;
                OnPropertyChanged("QuantityPerUnit");
            }
        }
        decimal? _unitPrice;
        public decimal? UnitPrice
        {
            get { return _unitPrice; }
            set
            {
                _unitPrice = value;
                OnPropertyChanged("UnitPrice");
            }
        }
        short? _unitsInStock;
        public short? UnitsInStock
        {
            get { return _unitsInStock; }
            set
            {
                _unitsInStock = value;
                OnPropertyChanged("UnitInStock");
            }
        }
        short? _unitsOnOrder;
        public short? UnitsOnOrder
        {
            get { return _unitsOnOrder; }
            set
            {
                _unitsOnOrder = value;
                OnPropertyChanged("UnitsOnOrder");
            }
        }
        short? _reorderLevel;
        public short? ReorderLevel
        {
            get { return _reorderLevel; }
            set
            {
                _reorderLevel = value;
                OnPropertyChanged("ReorderLevel");
            }
        }
        bool _discontinued;
        public bool Discontinued
        {
            get { return _discontinued; }
            set
            {
                _discontinued = value;
                OnPropertyChanged("Discontinued");
            }
        }

        public virtual ObservableCollection<Order_Detail> Order_Details { get; set; }
    }

}