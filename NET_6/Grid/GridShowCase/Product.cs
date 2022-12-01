using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GridShowCase
{
    public class Product : BaseObject
    {
        const int HISTORY_SIZE = 5;
        List<decimal> _productPriceHistory = new List<decimal>();

        string _product, _color;
        decimal _price, _history;
        int _id, _countryId;
        DateTime _dateTime, _time;
        bool _active;

        static Random _rnd = new Random();
        static string[] _names = "Macko|Surfair|Pocohey|Studeby".Split('|');
        static string[] _colors = "Red|Green|Blue|Black|Orange".Split('|');

        public Product(int id)
        {
            ID = id;
            ProductName = PickOne(_names);
            Color = PickOne(_colors);
            Price = 30 + (decimal)_rnd.NextDouble() * 1000;
            CreatedDate = DateTime.Today.AddDays(-_rnd.Next(1, 365)).AddHours(_rnd.Next(0, 24)).AddMinutes(_rnd.Next(0, 60));
            History = Price;
            for (int i = 0; i < HISTORY_SIZE; i++)
                _productPriceHistory.Add(Price * (decimal)(1 + (_rnd.NextDouble() * .11 - .05)));
            Discount = _rnd.Next(2, 50) / 100.0;
            Size = 120;
            Weight = 911;
            Quantity = 2;
            Description = " ...Lorem Ipsum Dollect";
            Rating = _rnd.Next(0, 5);
            Active = _rnd.NextDouble() >= .5;
        }
        string PickOne(string[] options)
        {
            return options[_rnd.Next() % options.Length];
        }

        public int ID
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                }
            }
        }

        public DateTime CreatedDate
        {
            get { return _dateTime; }
            set
            {
                if (value != _dateTime)
                {

                    SetProperty("CreatedDate", ref _dateTime, value);
                    OnPropertyChanged(new PropertyChangedEventArgs("CreatedDate"));
                }
            }
        }

        public string ProductName
        {
            get { return _product; }
            set { SetProperty("Product", ref _product, value); }
        }


        public Country Country { get; set; }
        public int CountryId { get; set; }

        public string Color
        {
            get { return _color; }
            set { SetProperty("Color", ref _color, value); }
        }

        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public decimal History
        {
            get { return _history; }
            set { _history = value; }
        }

        public double Discount { get; set; }
        public int Rating { get; set; }
        public bool Active
        {
            get { return _active; }
            set
            {
                if (value != _active)
                {
                    _active = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Size { get; set; }
        public int Weight { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public List<decimal> GetHistory()
        {
            return _productPriceHistory;
        }

        //public List<object> Histories
        //{

        //    get
        //    {
        //        List<object> _histories = new List<object>();
        //        foreach (var item in _productPriceHistory)
        //        {
        //            _histories.Add(new { Value = item });
        //        }

        //        return _histories;
        //    }
        //}

        //void AddHistory(List<decimal> list, decimal value)
        //{
        //    while (list.Count >= HISTORY_SIZE)
        //    {
        //        list.RemoveAt(0);
        //    }
        //    while (list.Count < HISTORY_SIZE)
        //    {
        //        list.Add(value);
        //    }
        //}
    }
    public class Country
    {
        public string Name { get; set; }
        public string Image { get; set; }

        public static ObservableCollection<Country> ReadAll()
        {
            var result = new ObservableCollection<Country>();
            string codes = null;
            using (var manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GridShowCase.Resources.countries.json"))
            {
                using (var streamReader = new StreamReader(manifestResourceStream))
                {
                    codes = streamReader.ReadToEnd();
                }
            }

            dynamic sources = JsonConvert.DeserializeObject(codes);
            foreach (dynamic item in sources)
            {
                result.Add(new Country { Name = ((string)item.name), Image = "/GridShowCase;component/Resources/flags/" + ((string)item.code) + ".png" }); ;
            }

            return result;
        }

        public override string ToString()
        {
            return Name;
        }
        
        class ApiHelper
        {
            public static string GetJsonString(string api)
            {
                string jsonString = string.Empty;
                var webRequest = WebRequest.Create(api) as HttpWebRequest;
                if (webRequest == null)
                {
                    return null;
                }

                webRequest.ContentType = "application/json";
                webRequest.UserAgent = ".";

                using (var s = webRequest.GetResponse().GetResponseStream())
                {
                    using (var sr = new StreamReader(s))
                    {
                        jsonString = sr.ReadToEnd();
                    }
                }
                return jsonString;
            }
        }
    }
    /// <summary>
    /// BaseObject provides INotifyPropertyChanged and IEditableObject.
    /// </summary>
    public class BaseObject : INotifyPropertyChanged, IEditableObject
    {
        protected bool SetProperty<T>(string propName, ref T field, T value)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propName);
            return true;
        }

        #region ** INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion

        #region ** IEditableObject

        object _clone = null;
        public void BeginEdit()
        {
            _clone = this.MemberwiseClone();
        }
        public void CancelEdit()
        {
            foreach (var p in this.GetType().GetProperties())
            {
                var value = p.GetValue(_clone, null);
                p.SetValue(this, value, null);
            }
        }
        public void EndEdit()
        {
            _clone = null;
        }

        #endregion
    }
}
