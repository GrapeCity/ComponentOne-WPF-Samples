using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockChart
{
    public static class ReferanceExtension
    {
        public static double GetValue(this object obj, string property)
        {
            return Convert.ToDouble(obj.GetType().GetProperty(property).GetValue(obj, null));
        }
        public static object GetRefValue(this object obj, string property)
        {
            return obj.GetType().GetProperty(property).GetValue(obj, null);
        }
    }
}
