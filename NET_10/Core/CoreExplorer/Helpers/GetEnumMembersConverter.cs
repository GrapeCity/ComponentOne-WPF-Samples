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
    /// Converts an enum member to an array of all members of the enum. This converter is used
    /// in ComboBox controls representing enum type property editors.
    /// </summary>
    public class GetEnumMembersConverter: IValueConverter
    {
        public static readonly GetEnumMembersConverter Default = new GetEnumMembersConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                Type valueType = value.GetType();
                if (valueType.IsEnum)
                    return Enum.GetValues(valueType);
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
