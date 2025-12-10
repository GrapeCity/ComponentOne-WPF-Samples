using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ThemeExplorer
{
    public class ScheduleStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts the values of C1.WPF.Schedule.ViewType in String
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;

            string key = $"{value.GetType().Name}_{value}";
            return Properties.Resources.ResourceManager.GetString(key) ?? value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
