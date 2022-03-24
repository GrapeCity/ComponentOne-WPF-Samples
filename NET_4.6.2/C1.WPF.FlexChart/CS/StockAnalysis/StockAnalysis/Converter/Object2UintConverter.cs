using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace StockAnalysis.Converter
{
    public class Object2UintConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            uint i = 1;
            double d = double.MinValue;
            if (value != null  && double.TryParse(value.ToString(), out d))
            {
                if (d >= 0)
                {
                    i = System.Convert.ToUInt32(d);
                }
            }
            
            return i;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
