using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace StockChart.Converters
{
    public class Value2BrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double d = 0;
            d = System.Convert.ToDouble(value);
            return new SolidColorBrush(d >= 0 ? Color.FromArgb(255, 18, 155, 20) : Color.FromArgb(255, 255, 30, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            C1.Chart.SeriesVisibility v = (C1.Chart.SeriesVisibility)value;
            return v == C1.Chart.SeriesVisibility.Visible;
        }
    }
}
