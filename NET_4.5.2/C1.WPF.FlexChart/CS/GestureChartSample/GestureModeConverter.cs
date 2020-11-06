using C1.WPF.Chart.Interaction;
using System;
using System.Globalization;
using System.Windows.Data;

namespace GestureChartSample
{
    class GestureModeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (GestureMode)Enum.Parse(typeof(GestureMode), value.ToString());
        }
    }
}
