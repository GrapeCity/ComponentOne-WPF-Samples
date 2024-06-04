using C1.Chart;
using System;
using System.Globalization;
using System.Windows.Data;

namespace FlexRadarIntro
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
