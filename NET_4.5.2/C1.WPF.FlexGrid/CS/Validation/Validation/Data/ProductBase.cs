using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Validation
{
    /// <summary>
    /// Simple data class that implements two important binding interfaces:
    /// 
    /// INotifyPropertyChanged:
    ///   Notifies bound controls of changes to the object properties so they can synchronize
    ///   the UI with the current object state.
    ///   
    /// IEditableObject:
    ///   Provides transacted edits by saving the object state when editing starts and 
    ///   committing or rolling back when editing ends.
    ///   
    /// </summary>
    public class ProductBase :
        INotifyPropertyChanged,
        IEditableObject
    {
        static string[] _lines = "Computers|Washers|Stoves".Split('|');
        static string[] _colors = "Red|Green|Blue|White".Split('|');
        static Random _rnd = new Random();

        // ctor
        public ProductBase()
        {
            Line = _lines[_rnd.Next() % _lines.Length];
            Color = _colors[_rnd.Next() % _colors.Length];
            Name = string.Format("{0} {1}{2}", Line.Substring(0, Line.Length - 1), Line[0], _rnd.Next(1, 1000));
            Price = _rnd.Next(1, 1000);
            Cost = _rnd.Next(1, 600);
            Weight = _rnd.Next(1, 100);
            Volume = _rnd.Next(500, 5000);
            Discontinued = _rnd.NextDouble() < .1;
            Rating = _rnd.Next(0, 5);
        }

        // object model
        [Display(Name = "Line")]
        public virtual string Line
        {
            get { return (string)GetValue("Line"); }
            set { SetValue("Line", value); }
        }

        [Display(Name = "Color")]
        public virtual string Color
        {
            get { return (string)GetValue("Color"); }
            set { SetValue("Color", value); }
        }

        [Display(Name = "Name")]
        public virtual string Name
        {
            get { return (string)GetValue("Name"); }
            set { SetValue("Name", value); }
        }

        [Display(Name = "Price")]
        public virtual double? Price
        {
            get { return (double?)GetValue("Price"); }
            set { SetValue("Price", value); }
        }

        [Display(Name = "Cost")]
        public virtual double? Cost
        {
            get { return (double?)GetValue("Cost"); }
            set { SetValue("Cost", value); }
        }

        [Display(Name = "Weight")]
        public virtual double? Weight
        {
            get { return (double?)GetValue("Weight"); }
            set { SetValue("Weight", value); }
        }

        [Display(Name = "Volume")]
        public virtual double? Volume
        {
            get { return (double?)GetValue("Volume"); }
            set { SetValue("Volume", value); }
        }

        [Display(Name = "Discontinued")]
        public virtual bool? Discontinued
        {
            get { return (bool?)GetValue("Discontinued"); }
            set { SetValue("Discontinued", value); }
        }

        [Display(Name = "Rating")]
        public virtual int? Rating
        {
            get { return (int?)GetValue("Rating"); }
            set { SetValue("Rating", value); }
        }

        // get/set values
        Dictionary<string, object> _values = new Dictionary<string, object>();
        protected virtual object GetValue(string p)
        {
            object value;
            _values.TryGetValue(p, out value);
            return value;
        }
        protected virtual void SetValue(string p, object value)
        {
            if (!object.Equals(value, GetValue(p)))
            {
                _values[p] = value;
                OnPropertyChanged(p);
            }
        }
        protected virtual void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        // factory
        public static string[] GetLines()
        {
            return _lines;
        }
        public static string[] GetColors()
        {
            return _colors;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IEditableObject Members

        static Dictionary<string, object> _clone;
        public void BeginEdit()
        {
            if (_clone == null)
            {
                _clone = new Dictionary<string, object>();
            }
            _clone.Clear();
            foreach (var key in _values.Keys)
            {
                _clone[key] = _values[key];
            }
        }
        public void CancelEdit()
        {
            _values.Clear();
            foreach (var key in _clone.Keys)
            {
                _values[key] = _clone[key];
            }
            OnPropertyChanged(null);
        }
        public void EndEdit()
        {
        }

        #endregion
    }
}
