using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace StockChart.Converters
{
    public class IsComparisonModeToDisplayModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = System.Convert.ToBoolean(value);
            return b ? DisplayMode.Comparison : DisplayMode.Independent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DisplayMode dm = (DisplayMode)value;
            return dm == DisplayMode.Comparison;
        }
    }
}
