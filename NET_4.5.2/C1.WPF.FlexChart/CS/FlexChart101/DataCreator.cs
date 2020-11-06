using System;
using System.Collections.Generic;

namespace FlexChart101
{
    class DataCreator
    {
        public delegate double MathActionDouble(double num);
        public delegate double MathActionInt(int num);

        public static List<string> CreateSimpleChartTypes()
        {
            return new List<string> { "Column", "Bar", "Line", "Scatter", "LineSymbols", "Area", "Spline", "SplineArea", "SplineSymbols", "Step", "StepArea", "StepSymbols" };
        }

        public static List<FruitDataItem> CreateFruit()
        {
            var fruits = new string[] { "Oranges", "Apples", "Pears", "Bananas" };
            var count = fruits.Length;
            var result = new List<FruitDataItem>();
            var rnd = new Random();
            for (var i = 0; i < count; i++)
                result.Add(new FruitDataItem()
                {
                    Fruit = fruits[i],
                    March = rnd.Next(20),
                    April = rnd.Next(20),
                    May = rnd.Next(20),
                });
            return result;
        }

        public static List<DataItem> CreateData()
        {
            var data = new List<DataItem>();
            data.Add(new DataItem("UK", 5, 4));
            data.Add(new DataItem("USA", 7, 3));
            data.Add(new DataItem("Germany", 8, 5));
            data.Add(new DataItem("Japan", 12, 8));
            return data;
        }

        public static List<GroupDataItem> CreateGroupData()
        {
            var data = new List<GroupDataItem>();
            data.Add(new GroupDataItem("UK", 5, 4, 6, 5));
            data.Add(new GroupDataItem("USA", 7, 6, 3, 2));
            data.Add(new GroupDataItem("Germany", 8, 5, 10, 6));
            data.Add(new GroupDataItem("Japan", 12, 8, 10, 7));
            return data;
        }

        public static List<DataItem> CreateFunnelData()
        {
            var data = new List<DataItem>();
            var countries = "US,Germany,UK,Japan,Italy,Greece".Split(',');
            var sales = 10000;
            var rnd = new Random();
            for (var i = 0; i < countries.Length; i++)
            {
                var item = new DataItem(countries[i], sales, 0);
                sales = sales - (int)Math.Round(rnd.NextDouble() * 2000);
                data.Add(item);
            }

            return data;
        }
    }

    public class FruitDataItem
    {
        public string Fruit { get; set; }
        public double March { get; set; }
        public double April { get; set; }
        public double May { get; set; }
    }

    public class DataItem
    {
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

    public class GroupDataItem
    {
        public GroupDataItem(string country, int domesticSales, int domesticExpenses, int exportSales, int exportExpenses)
        {
            Country = country;
            DomesticSales = domesticSales;
            DomesticExpenses = domesticExpenses;
            ExportSales = exportSales;
            ExportExpenses = exportExpenses;
        }

        public string Country { get; set; }
        public int DomesticSales { get; set; }
        public int DomesticExpenses { get; set; }
        public int ExportSales { get; set; }
        public int ExportExpenses { get; set; }
    }

    public class DynamicItem
    {
        public int Day { get; set; }
        public int Trucks { get; set; }
        public int Ships { get; set; }
        public int Planes { get; set; }
    }
}
