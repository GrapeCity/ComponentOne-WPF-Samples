using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using System.Xml;

namespace DataFilterExplorer
{
    internal class DataProvider
    {
        public static IEnumerable<Car> GetCarDataCollection(DataTable carTable)
        {
            Random rnd = new Random();
            foreach (DataRow row in carTable.Rows)
            {
                yield return new Car
                {
                    ID = row.Field<int>("ID"),
                    Brand = row.Field<string>("Brand"),
                    Model = row.Field<string>("Model"),
                    HP = row.Field<Int16>("HP"),
                    Liter = row.Field<double>("Liter"),
                    Cyl = row.Field<Int16>("Cyl"),
                    TransmissSpeedCount = row.Field<Int16>("TransmissSpeedCount").ToString(),
                    TransmissAutomatic = row.Field<string>("TransmissAutomatic"),
                    MPG_City = row.Field<Int16>("MPG_City"),
                    MPG_Highway = row.Field<Int16>("MPG_Highway"),
                    Category = row.Field<string>("Category"),
                    Description = row.Field<string>("Description"),
                    Hyperlink = row.Field<string>("Hyperlink"),
                    Picture = row.Field<byte[]>("Picture"),
                    Price = row.Field<double>("Price")
                };
            }
        }

        public static DataTable GetCarTable()
        {
            DataTable dt = new DataTable("Cars");
            var asm = Assembly.GetExecutingAssembly();
            using (var s = asm.GetManifestResourceStream("DataFilterExplorer.Resources.cars.xml"))
            {
                _= dt.ReadXml(s);
            }
            return dt;
        }

        private static readonly Random s_rnd = new Random();

        public static string[] Colors { get; } = new string[] { "Red", "Green", "Blue", "Black", "Silver", "Gold", "Gray" };

        public static IEnumerable<CountInStore> GetCarsInStores()
        {
            var cars = GetCars();
            var stores = GetStores().ToList();
            var oneCarInStores = stores.Count / 2;

            foreach (var car in cars)
            {
                var colors = GetUniqueRandomIndexes(3, Colors.Length);
                foreach (var colorIndex in colors)
                {
                    var whoHasThisCar = GetUniqueRandomIndexes(oneCarInStores, stores.Count);
                    foreach (var storeIndex in whoHasThisCar)
                    {
                        yield return new CountInStore() { Car = car, Count = s_rnd.Next(1, 100), Store = stores[storeIndex], Color = (Color)ColorConverter.ConvertFromString(Colors[colorIndex]) };
                    }
                }
            }
        }

        public static IEnumerable<Store> GetStores()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream("DataFilterExplorer.Resources.Stores.txt"))
            {
                var shops = new List<Store>();
                using (var sr = new StreamReader(stream))
                {
                    for (; !sr.EndOfStream;)
                    {
                        var store = Store.FromString(sr.ReadLine());
                        if (store != null)
                            shops.Add(store);
                    }
                }

                return shops;
            }
        }

        private static List<int> GetUniqueRandomIndexes(int count, int max, int min = 0)
        {
            var indexes = new List<int>();
            if (count < max - min)
            {
                while (count > 0)
                {
                    var index = s_rnd.Next(min, max);
                    if (!indexes.Contains(index))
                    {
                        indexes.Add(index);
                        count--;
                    }
                }
            }
            return indexes;
        }

        public static IEnumerable<Car> GetCars()
        {
            var carsTable = GetCarTable();
            foreach (DataRow row in carsTable.Rows)
            {
                yield return new Car
                {
                    Brand = row.Field<string>("Brand"),
                    Category = row.Field<string>("Category"),
                    Description = row.Field<string>("Description"),
                    Liter = row.Field<double>("Liter"),
                    Model = row.Field<string>("Model"),
                    Picture = row.Field<byte[]>("Picture"),
                    Price = row.Field<double>("Price"),
                    TransmissAutomatic = row.Field<string>("TransmissAutomatic"),
                    TransmissSpeedCount = row.Field<Int16>("TransmissSpeedCount").ToString(),

                    ID = row.Field<int>("ID")
                };
            }
        }
    }

    internal class TransmissAutomatic
    {
        public string DisplayValue { get; set; }
        public object Value { get; set; }
    }

}
