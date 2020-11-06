using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MainTestApplication
{
    /// <summary>
    /// Class that represents some generic data object to use as a sample.
    /// </summary>
    public class Product : BaseObject
    {
        string _name, _color, _line;
        double _price, _cost;
        DateTime _date;
        bool _discontinued;

        static Random _rnd = new Random();
        static string[] _names = "Macko|Surfair|Pocohey|Studeby".Split('|');
        static string[] _lines = "Computers|Washers|Stoves|Cars".Split('|');
        static string[] _colors = "Red|Green|Blue|White".Split('|');

        public Product()
        {
            Name = PickOne(_names);
            Line = PickOne(_lines);
            Color = PickOne(_colors);
            Price = 30 + _rnd.NextDouble() * 1000;
            Cost = 3 + _rnd.NextDouble() * 300;
            Discontinued = _rnd.NextDouble() < .2;
            Introduced = DateTime.Today.AddDays(_rnd.Next(-600, 0));
        }
        string PickOne(string[] options)
        {
            return options[_rnd.Next() % options.Length];
        }

        [Display(Name = "Name")]
        public string Name
        {
            get { return _name; }
            set { SetProperty("Name", ref _name, value); }
        }

        [Display(Name = "Color")]
        public string Color
        {
            get { return _color; }
            set { SetProperty("Color", ref _color, value); }
        }

        [Display(Name = "Line")]
        public string Line
        {
            get { return _line; }
            set { SetProperty("Line", ref _line, value); }
        }

        [Display(Name = "Price")]
        public double Price
        {
            get { return _price; }
            set { SetProperty("Price", ref _price, value); }
        }

        [Display(Name = "Cost")]
        public double Cost
        {
            get { return _cost; }
            set { SetProperty("Cost", ref _cost, value); }
        }

        [Display(Name = "Introduced")]
        public DateTime Introduced
        {
            get { return _date; }
            set { SetProperty("Introduced", ref _date, value); }
        }

        [Display(Name = "Discontinued")]
        public bool Discontinued
        {
            get { return _discontinued; }
            set { SetProperty("Discontinued", ref _discontinued, value); }
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
        void OnPropertyChanged(string propName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propName));
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
