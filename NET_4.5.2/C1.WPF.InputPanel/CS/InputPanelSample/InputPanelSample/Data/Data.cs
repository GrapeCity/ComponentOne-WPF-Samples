using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using C1.WPF.InputPanel;
using System.Data;
using System.Collections;
using C1.WPF;
using System.Xml.Linq;
using System.Reflection;
using System.IO;
using System.Globalization;

namespace InputPanelSample
{
    public static class ListExtensions
    {
        public static DataTable ToDataTable<T>(this List<T> iList)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);

                DataColumn column = dataTable.Columns.Add(propertyDescriptor.Name, type);
                column.ReadOnly = propertyDescriptor.IsReadOnly;
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }

    public class Data
    {
        static XDocument customersDoc;

        static Data()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string customersResourcestring = assembly.GetManifestResourceNames().FirstOrDefault((resourceName) => resourceName.Contains(".customers.xml"));
            if (customersResourcestring != null)
                customersDoc = XDocument.Load(new StreamReader(assembly.GetManifestResourceStream(customersResourcestring)));
        }

        public static IEnumerable<Customer> GetCustomers()
        {
            var customers = (from customer in customersDoc.Descendants("Customer")
                             select new Customer(customer.Element("ID").Value)
                             {
                                 Country = customer.Element("Country").Value,
                                 First = customer.Element("FirstName").Value,
                                 Last = customer.Element("LastName").Value,
                                 BirthDate = !string.IsNullOrEmpty(customer.Element("BirthDate").Value) ? (DateTime?)DateTime.Parse(customer.Element("BirthDate").Value) : null,
                                 Age = !string.IsNullOrEmpty(customer.Element("Age").Value) ? int.Parse(customer.Element("Age").Value) : 0,
                                 Weight = !string.IsNullOrEmpty(customer.Element("Weight").Value) ? double.Parse(customer.Element("Weight").Value, CultureInfo.InvariantCulture.NumberFormat) : 0,
                                 Occupation = !string.IsNullOrEmpty(customer.Element("Occupation").Value) ? (Occupation)Enum.Parse(typeof(Occupation),customer.Element("Occupation").Value) : Occupation.Other,
                                 Phone = customer.Element("Phone").Value,
                                 Active = !string.IsNullOrEmpty(customer.Element("Active").Value) ? (bool?)bool.Parse(customer.Element("Active").Value) : null,
                             });
            return customers;
        }

        private ObservableCollection<Customer> _customerObservable;
        public ObservableCollection<Customer> CustomerObservable
        {
            get
            {
                if (_customerObservable == null)
                {
                    var list = new ObservableCollection<Customer>();
                    foreach (var customer in GetCustomers())
                    {
                        list.Add(customer);
                    }
                    _customerObservable = list;
                }
                return _customerObservable;
            }
        }

        private DataTable _customerDataTable;
        public DataTable CustomerDataTable
        {
            get
            {
                if (_customerDataTable == null)
                {
                    var list = new List<Customer>();
                    foreach (var customer in GetCustomers())
                    {
                        list.Add(customer);
                    }
                    _customerDataTable = list.ToDataTable<Customer>();
                }
                return _customerDataTable;
            }
        }

        private ICollectionView _customerCollectionView;
        public ICollectionView CustomerCollectionView
        {
            get
            {
                if (_customerCollectionView == null)
                {
                    var list = new ObservableCollection<Customer>();
                    foreach (var customer in GetCustomers())
                    {
                        list.Add(customer);
                    }
                    _customerCollectionView = CollectionViewSource.GetDefaultView(list);
                }
                return _customerCollectionView;
            }
        }
    }

    public enum Occupation
    {
        Doctor,
        Artist,
        Educator,
        Engineer,
        Executive,
        Other
    }

    public class Customer : INotifyPropertyChanged, IEditableObject, ICloneable, IDataErrorInfo
    {
        // ** fields
        string _id;
        string _first, _last;
        string _country;
        DateTime? _birthDate;
        double _weight;
        int _age;
        private Occupation _occupation;
        private string _phone;
        bool? _active;

        // ** data generators
        static Random _rnd = new Random();
        static string[] _firstNames = "Andy|Ben|Charlie|Dan|Ed|Fred|Gil|Herb|Jack|Karl|Larry|Mark|Noah|Oprah|Paul|Quince|Rich|Steve|Ted|Ulrich|Vic|Xavier|Zeb".Split('|');
        static string[] _lastNames = "Ambers|Bishop|Cole|Danson|Evers|Frommer|Griswold|Heath|Jammers|Krause|Lehman|Myers|Neiman|Orsted|Paulson|Quaid|Richards|Stevens|Trask|Ulam".Split('|');
        static string[] _countries = "China|India|United States|Indonesia|Brazil|Pakistan|Bangladesh|Nigeria|Russia|Japan|Mexico|Philippines|Vietnam|Germany|Ethiopia|Egypt|Iran|Turkey|Congo|France|Thailand|United Kingdom|Italy|Myanmar".Split('|');

        // ** ctors
        public Customer() : this(_rnd.Next(100000, 999999).ToString())
        {
        }

        public Customer(string id)
        {
            _id = id;
        }

        public Customer(bool random)
        {
            if (random)
            {
                Country = _countries[_rnd.Next() % _countries.Length];
                First = GetString(_firstNames);
                Last = GetString(_lastNames);
                BirthDate = DateTime.Today.AddDays(-_rnd.Next(1, 365));
                Age = _rnd.Next(10, 70);
                Weight = 50 + _rnd.NextDouble() * 50;
                Phone = _rnd.Next(100000000, 999999999).ToString();
                Active = _rnd.NextDouble() >= .5;
            }
        }

        // ** object model
        [DisplayName("ID")]
        public string ID
        {
            get { return _id; }
        }

        [DisplayName("Country")]
        public string Country
        {
            get { return _country; }
            set
            {
                if (value != _country)
                {
                    _country = value;
                    NotifyPropertyChanged("Country");
                }
            }
        }

        [DisplayName("Name")]
        public string Name
        {
            get
            {
                return First + " " + Last;
            }
        }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "First name is required.")]
        public string First
        {
            get
            {
                return _first;
            }
            set
            {
                if (value != _first)
                {
                    _first = value;

                    // call OnPropertyChanged with null parameter since setting this property
                    // modifies the value of "First" and also the value of "Name".
                    NotifyPropertyChanged(null);
                }
            }
        }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last name is required.")]
        public string Last
        {
            get
            {
                return _last;
            }
            set
            {
                if (value != _last)
                {
                    _last = value;

                    // call OnPropertyChanged with null parameter since setting this property
                    // modifies the value of "Last" and also the value of "Name".
                    NotifyPropertyChanged(null);
                }
            }
        }

        [DisplayName("Birth Date")]
        public DateTime? BirthDate
        {
            get
            {
                return _birthDate;
            }
            set
            {
                if (value != _birthDate)
                {
                    _birthDate = value;
                    NotifyPropertyChanged("BirthDate");
                }
            }
        }

        [DisplayName("Age")]
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                if (value != _age)
                {
                    _age = value;
                    NotifyPropertyChanged("Age");
                }
            }
        }

        [DisplayName("Weight")]
        public double Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                if (value != _weight)
                {
                    _weight = value;
                    NotifyPropertyChanged("Weight");
                }
            }
        }

        [DisplayName("Occupation")]
        public Occupation Occupation
        {
            get
            {
                return _occupation;
            }
            set
            {
                if (value != _occupation)
                {
                    _occupation = value;
                    NotifyPropertyChanged("Occupation");
                }
            }
        }

        [DisplayName("Phone Number")]
        [C1InputPanelMask("(000) 000-0000")]
        [CustomValidation(typeof(CustomValidator), "ValidatePhoneNumber")]
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                if (value != _phone)
                {
                    _phone = value;
                    NotifyPropertyChanged("Phone");
                }
            }
        }

        [DisplayName("Active")]
        public bool? Active
        {
            get
            {
                return _active;
            }
            set
            {
                if (value != _active)
                {
                    _active = value;
                    NotifyPropertyChanged("Active");
                }
            }
        }

        // ** utilities
        static string GetString(string[] arr)
        {
            return arr[_rnd.Next(arr.Length)];
        }
        static string GetName()
        {
            return string.Format("{0} {1}", GetString(_firstNames), GetString(_lastNames));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
        public Customer Clone()
        {
            return (Customer)MemberwiseClone();
        }

        string IDataErrorInfo.Error
        {
            get
            {
                return string.Empty;
            }
        }

        string IDataErrorInfo.this[string fieldName]
        {
            get
            {
                switch (fieldName)
                {
                    case "Last":
                        if (Last != null && Last.Length > 15)
                            return "Last Name should be less than 15 characters.";
                        break;
                    case "Weight":
                        if (Weight > 10000)
                            return "Weight is too high.";
                        break;
                    case "BirthDate":
                        if (BirthDate != null && BirthDate > DateTime.Now)
                            return "The birth date should not late than now.";
                        break;
                    case "Phone":
                        try
                        {
                            if (Phone != null && string.IsNullOrEmpty(Phone.Replace("\0", "")))
                                return "Phone number is required.";
                        }
                        catch { }
                        break;
                }
                return string.Empty;
            }
        }

        #region IEditableObject Members
        // this interface allows transacted edits (user can press escape to restore previous values).
        Customer _clone;
        public void BeginEdit()
        {
            _clone = (Customer)this.MemberwiseClone();
        }
        public void EndEdit()
        {
            _clone = null;
        }
        public void CancelEdit()
        {
            if (_clone != null)
            {
                foreach (var p in this.GetType().GetProperties())
                {
                    if (p.CanRead && p.CanWrite)
                    {
                        p.SetValue(this, p.GetValue(_clone, null), null);
                    }
                }
            }
        }
        #endregion

        public static ObservableCollection<Customer> GetObservableCollection(int count)
        {
            var list = new ObservableCollection<Customer>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new Customer());
            }
            return list;
        }

        public static ICollectionView GetICollectionView(int count)
        {
            var list = new ObservableCollection<Customer>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new Customer());
            }
            return CollectionViewSource.GetDefaultView(list);
        }

        public static List<Customer> GetList(int count)
        {
            var list = new List<Customer>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new Customer());
            }
            return list;
        }

        public static Collection<Customer> GetCollection(int count)
        {
            var list = new Collection<Customer>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new Customer());
            }
            return list;
        }

        public static DataTable GetDataTable(int count)
        {
            var list = new List<Customer>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new Customer());
            }
            return list.ToDataTable<Customer>();
        }
    }
}
