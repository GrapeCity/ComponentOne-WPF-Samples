using CustomFilters.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Media;

namespace CustomFilters
{
    public class DataProvider
    {
        private static readonly Random s_rnd= new Random();

        public string[] Colors { get; } = new string[] { "Red", "Green", "Blue", "Black", "Silver", "Gold", "Gray" };
      
        public IEnumerable<CountInStore> GetCarsInStores()
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

        public IEnumerable<Store> GetStores()
        {
            using (var stream = File.OpenRead("..\\..\\Resources\\Stores.txt"))
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

        private List<int> GetUniqueRandomIndexes(int count, int max, int min = 0)
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

        public IEnumerable<Car> GetCars()
        {
            var carsTable = GetDataTableCars();
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
                    ID = row.Field<int>("ID")
                };
            }
        }

        private static string GetConnectionString()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\ComponentOne Samples\Common";
            string conn = @"provider=microsoft.jet.oledb.4.0;data source={0}\c1nwind.mdb;";
            return string.Format(conn, path);
        }

        private static DataTable GetDataTableCars()
        {
            string rs = "select * from Cars;";
            string cn = GetConnectionString();
            OleDbDataAdapter da = new OleDbDataAdapter(rs, cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}
