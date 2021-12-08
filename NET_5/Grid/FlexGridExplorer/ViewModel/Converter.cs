using C1.WPF.Grid;
using C1.WPF.Input;
using System;
using System.Globalization;
using System.Windows.Data;

namespace FlexGridExplorer
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            string strVal = null;
            if (value is C1ComboBoxItem cbItem)
            {
                strVal = cbItem.Content.ToString();
            }
            else
            {
                strVal = value.ToString();
            }
            return Enum.Parse(targetType, strVal);
        }
    }
}
