using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockChart
{
    public class Quote
    {
        public Quote(Reference value)
        {
            referenceValue = value;
        }

        public DateTime date { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public double volume { get; set; }
        public string events { get; set; }

        internal Reference referenceValue { get; set; }

        public double percentage
        {
            get
            {
                double reValue = referenceValue.Value;
                return reValue == 0 ? .75 : (this.close - reValue) / reValue;
            }
        }
    }
}
