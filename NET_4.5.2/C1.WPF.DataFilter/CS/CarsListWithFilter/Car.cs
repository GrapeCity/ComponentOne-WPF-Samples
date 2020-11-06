using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarsListWithFilter
{
    public class Car
    {
        [Browsable(false)]
        public int ID { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        [Browsable(false)]
        public Int16 HP { get; set; }

        public double Liter { get; set; }
        [Browsable(false)]
        public Int16 Cyl { get; set; }
        public string TransmissSpeedCount { get; set; }
        public string TransmissAutomatic { get; set; }

        [Browsable(false)]
        public Int16 MPG_City { get; set; }
        [Browsable(false)]
        public Int16 MPG_Highway { get; set; }
        public string Category { get; set; }
        [Browsable(false)]
        public string Description { get; set; }
        [Browsable(false)]
        public string Hyperlink { get; set; }
        public double Price { get; set; }
    }
}