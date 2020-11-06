using System;
using System.Windows;
using System.Windows.Data;

namespace GroupPanel
{
    /// <summary>
    /// Group converter for amounts.
    /// </summary>
    /// <remarks>
    /// This converter converts amounts into one of four categories:
    /// "High", "Moderate", "Low", and "Very Low". The category is calculated
    /// based on a maximum value provided to the constructor.
    /// </remarks>
    public class AmountGroupConverter : IValueConverter
    {
        double _maxValue;
        public AmountGroupConverter(double maxValue)
        {
            _maxValue = maxValue;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var pct = (double)value / _maxValue;
            if (pct < .25) return "Very Low";
            if (pct < .50) return "Low";
            if (pct < .75) return "Moderate";
            return "High";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
