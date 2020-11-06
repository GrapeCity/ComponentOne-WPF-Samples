using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace StockChart.Converters
{
    public class Color2BrushConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Media.Color c = (System.Windows.Media.Color)value;
            return new System.Windows.Media.SolidColorBrush(c);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Media.SolidColorBrush b = (System.Windows.Media.SolidColorBrush)value;
            return b == null ? System.Windows.Media.Colors.White: b.Color;
        }
    }
}
