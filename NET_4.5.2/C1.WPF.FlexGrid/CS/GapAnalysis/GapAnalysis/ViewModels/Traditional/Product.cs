using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace GapAnalysis
{
    public class Product :
        BaseEditableObject,
        IDataErrorInfo
    {
        static string[] _lines = "Computers|Washers|Stoves".Split('|');
        static string[] _colors = "Red|Green|Blue|White".Split('|');

        int _id;
        string _line, _color, _name;
        double _price, _weight, _cost;
        bool _discontinued;
        int? _rating;

        // object model
        [Display(Name = "ID")]
        public int ID
        {
            get { return _id; }
            set { SetValue(ref _id, value, "ID"); }
        }

        [Display(Name = "Name")]
        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value, "Name"); }
        }

        [Display(Name = "Line")]
        public string Line
        {
            get { return _line; }
            set { SetValue(ref _line, value, "Line"); }
        }

        [Display(Name = "Color")]
        public string Color
        {
            get { return _color; }
            set { SetValue(ref _color, value, "Color"); }
        }

        [Display(Name = "Price")]
        public double Price
        {
            get { return _price; }
            set { SetValue(ref _price, value, "Price"); }
        }

        [Display(Name = "Weight")]
        public double Weight
        {
            get { return _weight; }
            set { SetValue(ref _weight, value, "Weight"); }
        }

        [Display(Name = "Cost")]
        public double Cost
        {
            get { return _cost; }
            set { SetValue(ref _cost, value, "Cost"); }
        }

        [Display(Name = "Discontinued")]
        public bool Discontinued
        {
            get { return _discontinued; }
            set { SetValue(ref _discontinued, value, "Discontinued"); }
        }

        [Display(Name = "Rating")]
        public int? Rating
        {
            get { return _rating; }
            set { SetValue(ref _rating, value, "Rating"); }
        }

        // factory
        public static ObservableCollection<Product> GetProducts(int count)
        {
            var list = new ObservableCollection<Product>();
            var rnd = new Random(0);
            for (int i = 0; i < count; i++)
            {
                var p = new Product();
                p.ID = i;
                p.Line = _lines[rnd.Next() % _lines.Length];
                p.Color = _colors[rnd.Next() % _colors.Length];
                p.Name = string.Format("{0} {1}{2}", p.Line.Substring(0, p.Line.Length - 1), p.Line[0], i);
                p.Price = rnd.Next(1, 1000);
                p.Cost = rnd.Next(1, 600);
                p.Weight = rnd.Next(1, 100);
                p.Discontinued = rnd.NextDouble() < .1;
                p.Rating = rnd.Next(0, 5);
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

        #region ** IDataErrorInfo

        string IDataErrorInfo.Error
        {
            get { return Price < Cost ? "Price must be > Cost" : null; }
        }
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Price":
                        return Price < 0 ? "Price must be > 0." : null;
                    case "Cost":
                        return Cost < 0 ? "Cost must be > 0." : null;
                    case "Rating":
                        return Rating < 0 || Rating > 5 ? "Rating should be between 0 and 5." : null;
                }
                return null;
            }
        }

        #endregion
    }
}
