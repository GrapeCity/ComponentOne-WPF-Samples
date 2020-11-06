using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockAnalysis.Object
{
    public class Quote
    {
        public string Name
        {
            get;
            set;
        }
        public string Symbol
        {
            get;
            set;
        }

        public IEnumerable<QuoteData> Data
        {
            get;
            set;
        }

        private double? rate = null;
        public double Rate
        {
            get
            {
                if (rate == null && Data != null)
                {
                    QuoteData last = Data.Last();
                    QuoteData secondLast = Data.Skip(Data.Count() - 2).Take(1).FirstOrDefault();
                    if (last != null && secondLast != null)
                    {
                        rate = (last.Close - secondLast.Close) / secondLast.Close;
                    }
                }
                return rate.Value;
                
            }
        }

        private bool? updown = null;
        public bool Updown
        {
            get
            {
                if (updown == null && Data != null)
                {
                    QuoteData last = Data.Last();
                    QuoteData secondLast = Data.Skip(Data.Count() - 2).Take(1).FirstOrDefault();
                    if (last != null && secondLast != null)
                    {
                        updown = last.Close >= secondLast.Close;
                    }
                }
                return updown.Value;
            }
        }

        private double? price = null;
        public double Price
        {
            get
            {

                if (price == null && Data != null)
                {
                    QuoteData last = Data.Last();
                    price = last.Close;
                }
                return price.Value;
            }
        }
    }
}
