using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace DrillDown
{
    public class ChartTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            C1.Chart.ChartType result = C1.Chart.ChartType.Column;
            Enum.TryParse(value.ToString(), out result);
            return result;
        }
    }

    public class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var index = int.Parse(value.ToString());
            var preIndexes = parameter.ToString().Split(' ');
            return preIndexes.Any(preIndex => int.Parse(preIndex) == index) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ShowNavigateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse(value.ToString()) >= int.Parse(parameter.ToString()) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MultipleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var val1 = (Visibility)values[0];
            var val2 = bool.Parse(values[1].ToString());
            return val1 == Visibility.Visible && val2 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
