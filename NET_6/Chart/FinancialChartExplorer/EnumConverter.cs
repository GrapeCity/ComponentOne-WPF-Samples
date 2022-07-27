using System;
using System.Globalization;
using System.Windows.Data;
using C1.Chart;
using C1.Chart.Finance;
using C1.WPF.Chart.Interaction;
using System.Windows;
using System.Linq;

namespace FinancialChartExplorer
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
                return null;

            if(targetType.Name.Equals("Boolean"))
                return bool.Parse(value.ToString());
            
            return Enum.Parse(targetType, value.ToString());
        }
    }

    public class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var paras = parameter.ToString().Split(' ');
            int index = int.Parse(value.ToString());
            return paras.Any(p => index == int.Parse(p)) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IntToSeriesVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = int.Parse(value.ToString());
            int para = int.Parse(parameter.ToString());
            return index == para ? SeriesVisibility.Visible : SeriesVisibility.Hidden;
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
            var selectedIndex = double.Parse(values[0].ToString());
            var isChecked = bool.Parse(values[1].ToString());
            return selectedIndex == 2 && isChecked ? Visibility.Visible : Visibility.Hidden;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ReversedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = bool.Parse(value.ToString());
            return !val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
