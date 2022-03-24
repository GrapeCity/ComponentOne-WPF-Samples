﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExportExcel
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
    public class Product :
        INotifyPropertyChanged,
        IEditableObject, IDataErrorInfo
    {
        static string[] _lines = "Computers|Washers|Stoves".Split('|');
        static string[] _colors = "Red|Green|Blue|White".Split('|');

        // object model
        [Display(Name = "Line")]
        public string Line
        {
            get { return (string)GetValue("Line"); }
            set { SetValue("Line", value); }
        }

        [Display(Name = "Color")]
        public string Color
        {
            get { return (string)GetValue("Color"); }
            set { SetValue("Color", value); }
        }

        [Display(Name = "Name")]
        public string Name
        {
            get { return (string)GetValue("Name"); }
            set { SetValue("Name", value); }
        }

        [Display(Name = "Price")]
        public double Price
        {
            get { return (double)GetValue("Price"); }
            set { SetValue("Price", value); }
        }

        [Display(Name = "Weight")]
        public double Weight
        {
            get { return (double)GetValue("Weight"); }
            set { SetValue("Weight", value); }
        }

        [Display(Name = "Cost")]
        public double Cost
        {
            get { return (double)GetValue("Cost"); }
            set { SetValue("Cost", value); }
        }

        [Display(Name = "Volume")]
        public double Volume
        {
            get { return (double)GetValue("Volume"); }
            set { SetValue("Volume", value); }
        }

        [Display(Name = "Discontinued")]
        public bool Discontinued
        {
            get { return (bool)GetValue("Discontinued"); }
            set { SetValue("Discontinued", value); }
        }

        [Display(Name = "Rating")]
        public int Rating
        {
            get { return (int)GetValue("Rating"); }
            set { SetValue("Rating", value); }
        }

        // get/set values
        Dictionary<string, object> _values = new Dictionary<string, object>();
        object GetValue(string p)
        {
            object value;
            _values.TryGetValue(p, out value);
            return value;
        }
        void SetValue(string p, object value)
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
        public static IEnumerable<Product> GetProducts(int count)
        {
            var list = new ObservableCollection<Product>();
            var rnd = new Random(0);
            for (int i = 0; i < count; i++)
            {
                var p = new Product();
                p.Line = _lines[rnd.Next() % _lines.Length];
                p.Color = _colors[rnd.Next() % _colors.Length];
                p.Name = string.Format("{0} {1}{2}", p.Line.Substring(0, p.Line.Length - 1), p.Line[0], i);
                p.Price = rnd.Next(1, 1000) + rnd.Next(1, 1000) + rnd.Next(1, 1000);
                p.Cost = rnd.Next(1, 800) + rnd.Next(1, 800) + rnd.Next(1, 800);
                p.Weight = rnd.Next(1, 100) + rnd.Next(1, 1000);
                p.Volume = rnd.Next(500, 5000) + rnd.Next(500, 5000);
                p.Discontinued = rnd.NextDouble() < .1;
                p.Rating = rnd.Next(0, 5) + rnd.Next(0, 5) + rnd.Next(0, 5);
                list.Add(p);
            }
            return list;
        }
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

        #region IDataErrorInfo members
        [Display(Name = "Error")]
        public string Error
        {
            // Return If(Price < Cost, "Price must be > Cost", Nothing)
            get {
                if (Price < Cost)
                {
                    return "Price must be > Cost";
                }
                else
                {
                    return string.Empty;
                }
                }
        }

        public string this[string columnName]
        {
            get {
                switch (columnName)
                {
                    case "Price":
                        {
                            if (Price < 0)
                            {
                                return "Price must be greater than 0";
                            }
                            break;
                        }
                    case "Cost" :
                        {
                            if(Cost<0)
                            {
                                return "Cost must be greater than 0";
                            }
                            break;
                        }
                }
                return string.Empty;
            }
        } 
        #endregion
    }
}