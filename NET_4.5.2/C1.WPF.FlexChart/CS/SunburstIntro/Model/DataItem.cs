using System.Collections.Generic;

namespace SunburstIntro
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

    public class FlatDataItem
    {
        public string Year { get; set; }
        public string Quarter { get; set; }
        public string Month { get; set; }
        public double Value { get; set; }
    }

    public class Item
    {
        public int Year { get; set; }
        public string Quarter { get; set; }
        public string MonthName { get; set; }
        public int MonthValue { get; set; }
        public double Value { get; set; }
    }
}
