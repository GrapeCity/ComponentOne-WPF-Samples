using System;
using System.Collections.Generic;
using System.Text;
#if WinForms
using System.Drawing;
#endif
#if WPF || WinForms
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
#elif WINDOWS_UWP
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
#endif

namespace DashboardModel
{
    public class UnitConverter : IValueConverter
    {
        public UnitConverter()
        {

        }
#if WPF || WinForms
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#elif WINDOWS_UWP
        public object Convert(object value, Type targetType, object parameter, string language)
#endif
        {
            double convertValue = (double)value;
            convertValue /= 1000000;
            return convertValue;
        }

#if WPF || WinForms
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#elif WINDOWS_UWP
        public object ConvertBack(object value, Type targetType, object parameter, string language)
#endif
        {
#if WPF || WinForms
            return Convert(value, targetType, parameter, culture);
#elif WINDOWS_UWP
            return Convert(value, targetType, parameter, language);
#endif
        }
    }

    public class BarColorConverter : IValueConverter
    {
#if WPF || WinForms
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#elif WINDOWS_UWP
        public object Convert(object value, Type targetType, object parameter, string language)
#endif
        {
            double precent = (double)value;
#if WinForms
            return precent < 0.8 ? Color.Red : Color.FromArgb(255, 0, 133, 199);
#else
            return precent < 0.8 ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Color.FromArgb(255, 0, 133, 199));
#endif
        }

#if WPF || WinForms
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#elif WINDOWS_UWP
        public object ConvertBack(object value, Type targetType, object parameter, string language)
#endif
        {
#if WPF || WinForms
            return Convert(value, targetType, parameter, culture);
#elif WINDOWS_UWP
            return Convert(value, targetType, parameter, language);
#endif
        }
    }

    public class GoodValueConverter : IValueConverter
    {
        public GoodValueConverter()
        {

        }

#if WPF || WinForms
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#elif WINDOWS_UWP
        public object Convert(object value, Type targetType, object parameter, string language)
#endif
        {
            double goodValue = (double)value;
            goodValue *= 0.0000008;
            return goodValue;
        }

#if WPF || WinForms
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#elif WINDOWS_UWP
        public object ConvertBack(object value, Type targetType, object parameter, string language)
#endif
        {
#if WPF || WinForms
        return Convert(value, targetType, parameter, culture);
#elif WINDOWS_UWP
            return Convert(value, targetType, parameter, language);
#endif
        }
    }

    public class BadValueConverter : IValueConverter
    {
        public BadValueConverter()
        {

        }

#if WPF || WinForms
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#elif WINDOWS_UWP
        public object Convert(object value, Type targetType, object parameter, string language)
#endif
        {
            double badValue = (double)value;
            badValue *= 0.0000005;
            return badValue;
        }

#if WPF || WinForms
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#elif WINDOWS_UWP
        public object ConvertBack(object value, Type targetType, object parameter, string language)
#endif
        {
#if WPF || WinForms
        return Convert(value, targetType, parameter, culture);
#elif WINDOWS_UWP
            return Convert(value, targetType, parameter, language);
#endif
        }
    }
}
