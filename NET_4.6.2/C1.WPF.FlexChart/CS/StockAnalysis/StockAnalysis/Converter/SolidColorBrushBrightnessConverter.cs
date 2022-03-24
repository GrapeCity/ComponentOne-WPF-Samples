using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace StockAnalysis.Converter
{
    public class SolidColorBrushBrightnessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = value as SolidColorBrush;
            var parm = System.Convert.ToDouble(parameter);

            if (brush == null)
            {
                return new SolidColorBrush(Colors.Black);
            }
            var hsb = Utilities.ColorEx.RgbToHsb(brush.Color);
            hsb.B = hsb.B * (1 + parm);

            var color = Utilities.ColorEx.HsbToRgb(hsb);
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
