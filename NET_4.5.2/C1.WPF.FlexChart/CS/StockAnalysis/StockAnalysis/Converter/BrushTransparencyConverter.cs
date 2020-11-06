using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace StockAnalysis.Converter
{
    public class BrushTransparencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = value as SolidColorBrush;
            if (brush == null)
            {
                return new SolidColorBrush(Colors.Black);
            }
            var color = brush.Color;

            double tv = 1;

            if (double.TryParse(parameter.ToString(), out tv))
            {
                color.A = System.Convert.ToByte(color.A * tv);
            }

            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
