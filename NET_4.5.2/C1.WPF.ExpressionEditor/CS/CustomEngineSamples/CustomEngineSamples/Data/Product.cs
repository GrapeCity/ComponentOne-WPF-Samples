using System;

namespace CustomEngineSamples
{
    public class Product
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }

        public Product(string name, double price, int count)
        {
            ID = Guid.NewGuid();
            Name = name;
            Price = price;
            Count = count;
        }

        /// <summary>
        /// Returns string representation.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}
