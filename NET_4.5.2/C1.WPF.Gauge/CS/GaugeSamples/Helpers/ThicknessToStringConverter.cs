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
    /// Two way binding converter that converts Thickness to String.
    /// </summary>
    [ValueConversion(typeof(Thickness), typeof(string))]
    public class ThicknessToStringConverter: IValueConverter
    {
        public static readonly ThicknessToStringConverter Default = new ThicknessToStringConverter();
        
        private static readonly ThicknessConverter _thicknessConverter = new ThicknessConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            if (value is Thickness)
            {
                try
                {
                    return _thicknessConverter.ConvertToInvariantString(value);
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
                    return _thicknessConverter.ConvertFromInvariantString(strValue);
                }
                catch
                {
                }
            }
            
            return Binding.DoNothing;
        }
    }
}
