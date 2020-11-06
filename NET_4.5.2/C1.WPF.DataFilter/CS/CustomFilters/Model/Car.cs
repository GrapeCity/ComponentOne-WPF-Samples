using System.Drawing;

namespace CustomFilters.Model
{
    public class Car
    {
        public int ID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public double Liter { get; set; }
        public string TransmissAutomatic { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public double Price { get; set; }        
    }
}
