using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using C1.Chart;

namespace FlexChart101
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.Parse(targetType, value.ToString());
        }
    }

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = (SeriesVisibility)Enum.Parse(typeof(SeriesVisibility), value.ToString());
            return visibility == SeriesVisibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? SeriesVisibility.Visible : SeriesVisibility.Legend;
        }
    }

    public class FormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = double.Parse(value.ToString());
            return val;
        }
    }
}
