using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SalesAnalysis
{
    public class ChartDataPoint
    {
        public ChartDataPoint() { }
        [Key]
        public string Item { get; set; }
        public decimal? Amount { get; set; }
    }
}
