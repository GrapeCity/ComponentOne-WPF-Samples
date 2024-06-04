using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C1ExpressionEditorSample
{
    class DataCreator
    {
        public static List<DataItem> CreateData()
        {
            var data = new List<DataItem>();
            data.Add(new DataItem("UK", 5, 4));
            data.Add(new DataItem("USA", 7, 3));
            data.Add(new DataItem("Germany", 8, 5));
            data.Add(new DataItem("Japan", 12, 8));
            return data;
        }
    }

    public class DataItem
    {
        public DataItem()
        {
        }

        public DataItem(string country, int sales, int expenses)
        {
            Country = country;
            Sales = sales;
            Expenses = expenses;
        }

        public string Country { get; set; }
        public int Sales { get; set; }
        public int Expenses { get; set; }
    }
}
