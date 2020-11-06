using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace StockAnalysis.Converter
{
    public class NewAnnotationType2VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string param = parameter==null ? null : parameter.ToString();
            var type = (Data.NewAnnotationType)value;
            var visibility = type == Data.NewAnnotationType.None ? Visibility.Collapsed : Visibility.Visible;
            if (type == Data.NewAnnotationType.Text && param == "Shape")
            {
                visibility = Visibility.Collapsed;
            }
            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
