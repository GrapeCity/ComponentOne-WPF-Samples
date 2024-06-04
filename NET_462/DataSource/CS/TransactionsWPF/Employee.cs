//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientTransactions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    
    public partial class Employee : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    	protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    
        public Employee()
        {
            this.Employees1 = new ObservableCollection<Employee>();
            this.Orders = new ObservableCollection<Order>();
            this.Territories = new ObservableCollection<Territory>();
        }
    
        int _EmployeeID;
        public int EmployeeID 
        {
            get { return _EmployeeID; }
            set
            {
                _EmployeeID = value;
                OnPropertyChanged("EmployeeID");
            }
        }
        string _LastName;
        public string LastName 
        {
            get { return _LastName; }
            set
            {
                _LastName = value;
                OnPropertyChanged("LastName");
            }
        }
        string _FirstName;
        public string FirstName 
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        string _Title;
        public string Title 
        {
            get { return _Title; }
            set
            {
                _Title = value;
                OnPropertyChanged("Title");
            }
        }
        string _TitleOfCourtesy;
        public string TitleOfCourtesy 
        {
            get { return _TitleOfCourtesy; }
            set
            {
                _TitleOfCourtesy = value;
                OnPropertyChanged("TitleOfCourtesy");
            }
        }
        Nullable<System.DateTime> _BirthDate;
        public Nullable<System.DateTime> BirthDate 
        {
            get { return _BirthDate; }
            set
            {
                _BirthDate = value;
                OnPropertyChanged("BirthDate");
            }
        }
        Nullable<System.DateTime> _HireDate;
        public Nullable<System.DateTime> HireDate 
        {
            get { return _HireDate; }
            set
            {
                _HireDate = value;
                OnPropertyChanged("HireDate");
            }
        }
        string _Address;
        public string Address 
        {
            get { return _Address; }
            set
            {
                _Address = value;
                OnPropertyChanged("Address");
            }
        }
        string _City;
        public string City 
        {
            get { return _City; }
            set
            {
                _City = value;
                OnPropertyChanged("City");
            }
        }
        string _Region;
        public string Region 
        {
            get { return _Region; }
            set
            {
                _Region = value;
                OnPropertyChanged("Region");
            }
        }
        string _PostalCode;
        public string PostalCode 
        {
            get { return _PostalCode; }
            set
            {
                _PostalCode = value;
                OnPropertyChanged("PostalCode");
            }
        }
        string _Country;
        public string Country 
        {
            get { return _Country; }
            set
            {
                _Country = value;
                OnPropertyChanged("Country");
            }
        }
        string _HomePhone;
        public string HomePhone 
        {
            get { return _HomePhone; }
            set
            {
                _HomePhone = value;
                OnPropertyChanged("HomePhone");
            }
        }
        string _Extension;
        public string Extension 
        {
            get { return _Extension; }
            set
            {
                _Extension = value;
                OnPropertyChanged("Extension");
            }
        }
        byte[] _Photo;
        public byte[] Photo 
        {
            get { return _Photo; }
            set
            {
                _Photo = value;
                OnPropertyChanged("Photo");
            }
        }
        string _Notes;
        public string Notes 
        {
            get { return _Notes; }
            set
            {
                _Notes = value;
                OnPropertyChanged("Notes");
            }
        }
        Nullable<int> _ReportsTo;
        public Nullable<int> ReportsTo 
        {
            get { return _ReportsTo; }
            set
            {
                _ReportsTo = value;
                OnPropertyChanged("ReportsTo");
            }
        }
        string _PhotoPath;
        public string PhotoPath 
        {
            get { return _PhotoPath; }
            set
            {
                _PhotoPath = value;
                OnPropertyChanged("PhotoPath");
            }
        }
    
        public virtual ObservableCollection<Employee> Employees1 { get; set; }
        public virtual Employee Employee1 { get; set; }
        public virtual ObservableCollection<Order> Orders { get; set; }
        public virtual ObservableCollection<Territory> Territories { get; set; }
    }
}