using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace CarsListWithFilter
{
    public class DataProvider
    {
        public IEnumerable<Car> GetCarDataCollection(DataTable carTable)
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
                    Price = row.Field<double>("Price")
                };
            }
        }

        public DataTable GetCarTable()
        {
            string rs = "select * from Cars;";
            string cn = GetConnectionString();
            OleDbDataAdapter da = new OleDbDataAdapter(rs, cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private string GetConnectionString()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\ComponentOne Samples\Common");
            string conn = @"provider=microsoft.jet.oledb.4.0;data source=|DataDirectory|\c1nwind.mdb;";
            return conn;
        }
    }
}
