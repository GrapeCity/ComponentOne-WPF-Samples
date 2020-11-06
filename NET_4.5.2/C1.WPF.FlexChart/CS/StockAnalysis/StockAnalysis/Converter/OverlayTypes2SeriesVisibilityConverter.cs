using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace StockAnalysis.Converter
{
    public class OverlayTypes2SeriesVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var types = value as System.Collections.ObjectModel.ObservableCollection<Data.OverlayType>;
            var type = (Data.OverlayType)parameter;
            if (types.Contains(type))
            {
                return C1.Chart.SeriesVisibility.Visible;
            }
            return C1.Chart.SeriesVisibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
