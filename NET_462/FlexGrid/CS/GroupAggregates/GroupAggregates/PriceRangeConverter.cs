using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace GroupAggregates
{
    // converter used to group prices into ranges
    class PriceRangeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                var p = (double)value;
                if (p < 50) return "Low";
                if (p < 100) return "Medium";
                if (p < 500) return "High";
                return "Very High";
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
