using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace Helpers
{
    /// <summary>
    /// Two way binding converter that converts Point to String.
    /// </summary>
    [ValueConversion(typeof(Size), typeof(string))]
    public class SizeToStringConverter : IValueConverter
    {
        public static readonly SizeToStringConverter Default = new SizeToStringConverter();
        
        private static readonly SizeConverter _sizeConverter = new SizeConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            if (value is Size)
            {
                try
                {
                    return _sizeConverter.ConvertToInvariantString(value);
                }
                catch
                {
                }
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            string strValue = value as string;
            if (strValue != null)
            {
                try
                {
                    return _sizeConverter.ConvertFromInvariantString(strValue);
                }
                catch
                {
                }
            }
            
            return Binding.DoNothing;
        }
    }
}
