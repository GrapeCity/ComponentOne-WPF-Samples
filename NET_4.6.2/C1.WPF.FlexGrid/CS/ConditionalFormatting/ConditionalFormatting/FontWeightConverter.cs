using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ConditionalFormatting
{
    public class FontWeightConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var range = (Point)parameter;
            double val = (double)value;
            return
                val < range.X ? FontWeights.Bold :
                val > range.Y ? FontWeights.Bold :
                FontWeights.Normal;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
