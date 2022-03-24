using System;

namespace AnnotationExplorer
{
    public class DataItem
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class FinancialItem
    {
        public double Hight { get; set; }
        public double Low { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public int Volume { get; set; }
        public DateTime Date { get; set; }
    }
}
