using System.Collections.Generic;

namespace DrillDown
{
    public class DataItem
    {
        List<DataItem> _items;

        public string Year { get; set; }
        public string Quarter { get; set; }
        public string Month { get; set; }
        public double Value { get; set; }
        public List<DataItem> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new List<DataItem>();
                }

                return _items;
            }
        }
    }

    public class Item
    {
        public int ID { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public double Amount { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public int Day { get; set; }
    }

    public class Leaf
    {
        public string Type { get; set; }
        public Leaf[] Items { get; set; }
        public int Sales { get; set; }
    }

    public class FlexPoint
    {
        public string XValue { get; set; }
        public double YValue { get; set; }
    }
}
