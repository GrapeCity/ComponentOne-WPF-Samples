using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace StockChart
{
    public class QuoteInfo
    {
        public string Code { get; set; }
        public Color Color { get; set; }
        public double Value { get; set; }
        public int Volume { get; set; }
        public DateTime Date { get; set; }
    }
}
