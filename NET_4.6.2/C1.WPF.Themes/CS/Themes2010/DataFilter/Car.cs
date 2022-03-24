using System.ComponentModel;

namespace Themes
{
    public class Car
    {
        [Browsable(false)]
        public int ID { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        [Browsable(false)]
        public double Liter { get; set; }

        [Browsable(false)]
        public string TransmissAutomatic { get; set; }

        public string Category { get; set; }

        [Browsable(false)]
        public string Description { get; set; }

        [Browsable(false)]
        public byte[] Picture { get; set; }

        
        public double Price { get; set; }
    }
}