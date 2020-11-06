using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace StockAnalysis.Converter
{
    public class SolidColorBrush2ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Media.SolidColorBrush brush = value as System.Windows.Media.SolidColorBrush;
            return brush == null ? System.Windows.Media.Colors.Transparent : brush.Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Media.Color color = (System.Windows.Media.Color)value;
            return new System.Windows.Media.SolidColorBrush(color);
        }
    }
}
