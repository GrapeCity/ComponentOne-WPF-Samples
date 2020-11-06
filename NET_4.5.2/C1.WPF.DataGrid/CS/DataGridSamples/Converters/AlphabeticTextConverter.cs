using System;
using System.Globalization;
using System.Windows.Data;

namespace DataGridSamples
{
    public class AlphabeticTextGroupConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                string text = ((string)value).Trim();
                if (text.Length > 0)
                {
                    return text.Substring(0, 1).ToUpper();
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}
