using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace DataViewDashboard.Converters
{
    public class ZeroIndexConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int i;
            if(int.TryParse(value.ToString(), out i))
            {
                return i + 1;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int i;
            if (int.TryParse(value.ToString(), out i))
            {
                return i - 1;
            }
            return value;
        }

        #endregion
    }
}
