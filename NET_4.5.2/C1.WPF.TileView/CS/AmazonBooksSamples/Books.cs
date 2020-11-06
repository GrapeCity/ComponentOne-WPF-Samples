using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmazonBooksSamples
{
    public class Book
    {
        public string Title { get; set; }
        public string CoverUri { get; set; }
        public string Id { get; set; }
        public string Price { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int StockAmount { get; set; }
    }
}
