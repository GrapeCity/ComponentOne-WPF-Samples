using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace GapAnalysis
{
    /// <summary>
    /// GroupHeaderConverter used to return only the group name 
    /// (no property name, no counts)
    /// </summary>
    public class GroupHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var cvg = value as CollectionViewGroup;
            return cvg != null ? cvg.Name : value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
