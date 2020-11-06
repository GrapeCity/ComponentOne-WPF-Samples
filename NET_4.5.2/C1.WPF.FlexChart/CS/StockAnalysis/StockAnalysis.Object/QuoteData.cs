using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockAnalysis.Object
{
    public class QuoteData
    {
        public string Date { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }

        public DateTime LocalDate { get; set; }
    }
}
