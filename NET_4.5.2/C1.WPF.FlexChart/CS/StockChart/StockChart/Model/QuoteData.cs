using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StockChart
{
    public class QuoteData : List<Quote>
    {
        private Reference _referenceValue = new Reference();
        internal Reference ReferenceValue
        {
            get
            {
                return _referenceValue;
            }
        }
    }
}
