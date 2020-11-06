using System;
using System.Linq;
using System.Windows;
using System.Globalization;
using System.Windows.Data;
using System.Collections.Generic;
using C1.WPF.Chart.Interaction;

namespace LineMarkerSample
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type type = value.GetType();
            if (type == typeof(LineMarkerAlignment))
            {
                var model = parameter as LineMarkerViewModel;
                var alignments = model.LineMarkerAlignments;
                var alignment = (LineMarkerAlignment)Enum.Parse(typeof(LineMarkerAlignment), value.ToString());
                int index = 0;
                foreach (var val in alignments.Values)
                {
                    if (val == alignment)
                    {
                        return alignments.ElementAt(index);
                    }
                    index++;
                }
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(bool))
                return bool.Parse(value.ToString());
            else
            {
                if (targetType == typeof(LineMarkerAlignment))
                {
                    KeyValuePair<string, LineMarkerAlignment> pair = (KeyValuePair<string, LineMarkerAlignment>)value ;
                    return pair.Value;
                }
                else
                {
                    return Enum.Parse(targetType, value.ToString());
                }
            }
        }
    }

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (LineMarkerInteraction)Enum.Parse(typeof(LineMarkerInteraction), value.ToString());
            return val == LineMarkerInteraction.Drag ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
