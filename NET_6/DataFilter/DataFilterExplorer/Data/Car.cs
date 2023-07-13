using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataFilterExplorer
{
    public class Car
    {
        public Car()
        {
            Random gen = new Random();
            int range = 25 * 365;
            DateProductionLine = DateTime.Today.AddDays(-gen.Next(range));
        }

        public string Brand { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string TransmissSpeedCount { get; set; }
        public string TransmissAutomatic { get; set; }
        public DateTime DateProductionLine { get; set; }

        //[Browsable(false)]
        public int ID { get; set; }
        [Browsable(false)]
        public Int16 HP { get; set; }
        public double Liter { get; set; }
        [Browsable(false)]
        public Int16 Cyl { get; set; }
        [Browsable(false)]
        public Int16 MPG_City { get; set; }
        [Browsable(false)]
        public Int16 MPG_Highway { get; set; }
        [Browsable(false)]
        public string Description { get; set; }
        [Browsable(false)]
        public string Hyperlink { get; set; }
        [Browsable(false)]
        [Display(AutoGenerateField =false)]
        public byte[] Picture { get; set; }
    }
}