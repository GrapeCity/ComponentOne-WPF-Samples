using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace StockChart.Converters
{
    public class SeriesVisibility2BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            C1.Chart.SeriesVisibility v = (C1.Chart.SeriesVisibility)value;
            return v == C1.Chart.SeriesVisibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = System.Convert.ToBoolean(value);
            return b ? C1.Chart.SeriesVisibility.Visible : C1.Chart.SeriesVisibility.Hidden;
        }
    }
}
