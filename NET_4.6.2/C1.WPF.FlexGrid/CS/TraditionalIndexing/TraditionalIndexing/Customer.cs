using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TraditionalIndexing
{
    public class Customer : INotifyPropertyChanged, IEditableObject
    {
        int _id;
        string _first, _last, _country;
        bool _active;
        DateTime _hired;
        double _weight;

        static Random _rnd = new Random();
        static string[] _firstNames = "Andy|Ben|Charlie|Dan|Ed|Fred|Gil|Herb|Jack|Karl|Larry|Mark|Noah|Oprah|Paul|Quince|Rich|Steve|Ted|Ulrich|Vic|Xavier|Zeb".Split('|');
        static string[] _lastNames = "Ambers|Bishop|Cole|Danson|Evers|Frommer|Griswold|Heath|Jammers|Krause|Lehman|Myers|Neiman|Orsted|Paulson|Quaid|Richards|Stevens|Trask|Ulam".Split('|');
        static string[] _countries = "China|India|United States|Indonesia|Brazil|Pakistan|Bangladesh|Nigeria|Russia|Japan|Mexico|Philippines|Vietnam|Germany|Ethiopia|Egypt|Iran|Turkey|Congo|France|Thailand|United Kingdom|Italy|Myanmar".Split('|');

        public Customer()
            : this(_rnd.Next())
        {

        }
        public Customer(int id)
        {
            ID = id;

            First = GetString(_firstNames);
            Last = GetString(_lastNames);
            Country = GetString(_countries);
            Active = _rnd.NextDouble() >= .5;
            Hired = DateTime.Today.AddDays(-_rnd.Next(1, 365));
            Weight = 50 + _rnd.NextDouble() * 50;
            Father = GetName();
            Brother = GetName();
            Cousin = GetName();
            Boss = GetName();
            Friend = GetName();
            Driver = GetName();
            Enemy = GetName();
        }

        [Display(Name = "ID")]
        public int ID
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        [Display(Name = "Name")]
        public string Name
        {
            get { return string.Format("{0} {1}", First, Last); }
        }

        [Display(Name = "Country")]
        public string Country
        {
            get { return _country; }
            set
            {
                if (value != _country)
                {
                    _country = value;
                    OnPropertyChanged("Country");
                }
            }
        }

        [Display(Name = "Active")]
        public bool Active
        {
            get { return _active; }
            set
            {
                if (value != _active)
                {
                    _active = value;
                    OnPropertyChanged("Active");
                }
            }
        }

        [Display(Name = "First")]
        public string First
        {
            get { return _first; }
            set
            {
                if (value != _first)
                {
                    _first = value;
                    OnPropertyChanged("First");
                }
            }
        }

        [Display(Name = "Last")]
        public string Last
        {
            get { return _last; }
            set
            {
                if (value != _last)
                {
                    _last = value;
                    OnPropertyChanged("Last");
                }
            }
        }

        [Display(Name = "Hired")]
        public DateTime Hired
        {
            get { return _hired; }
            set
            {
                if (value != _hired)
                {
                    _hired = value;
                    OnPropertyChanged("Hired");
                }
            }
        }

        [Display(Name = "Weight")]
        public double Weight
        {
            get { return _weight; }
            set
            {
                if (value != _weight)
                {
                    _weight = value;
                    OnPropertyChanged("Weight");
                }
            }
        }

        // some read-only stuff
        [Display(Name = "Father")]
        public string Father { get; set; }

        [Display(Name = "Brother")]
        public string Brother { get; set; }

        [Display(Name = "Cousin")]
        public string Cousin { get; set; }

        [Display(Name = "Boss")]
        public string Boss { get; set; }

        [Display(Name = "Friend")]
        public string Friend { get; set; }

        [Display(Name = "Driver")]
        public string Driver { get; set; }

        [Display(Name = "Enemy")]
        public string Enemy { get; set; }

        static string GetString(string[] arr)
        {
            return arr[_rnd.Next(arr.Length)];
        }
        static string GetName()
        {
            return string.Format("{0} {1}", GetString(_firstNames), GetString(_lastNames));
        }

        //-----------------------------------------------------------------------------------
        #region INotifyPropertyChanged Members

        void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion

        //-----------------------------------------------------------------------------------
        #region IEditableObject Members

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

    }
}
