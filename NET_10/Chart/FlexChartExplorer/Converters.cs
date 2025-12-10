using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

using C1.Chart;

namespace FlexChartExplorer
{
    public class BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return Enum.Parse(targetType, value.ToString());
        }
    }

    public class ChartTypeConverter : DependencyObject, IValueConverter
    {
        public List<string> ChartTypes
        {
            get { return (List<string>)GetValue(ChartTypesProperty); }
            set { SetValue(ChartTypesProperty, value); }
        }

        public static readonly DependencyProperty ChartTypesProperty =
            DependencyProperty.Register("ChartTypes", typeof(List<string>), typeof(ChartTypeConverter), 
            new PropertyMetadata(null));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ChartTypes.IndexOf(value?.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = (int)value;
            return ChartTypes[index];
        }
    }

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            var position = (PieLabelPosition)Enum.Parse(typeof(PieLabelPosition), value.ToString());
            return position == PieLabelPosition.None ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OrderVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fitType = (FitType)Enum.Parse(typeof(FitType), value?.ToString());
            return fitType == FitType.Fourier || fitType == FitType.Polynom ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            return isChecked ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AnimationSettingsToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var settings = (AnimationSettings)value;
            return settings == AnimationSettings.All;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enable = (bool)value;
            return enable ? AnimationSettings.All : AnimationSettings.None;
        }
    }

}
