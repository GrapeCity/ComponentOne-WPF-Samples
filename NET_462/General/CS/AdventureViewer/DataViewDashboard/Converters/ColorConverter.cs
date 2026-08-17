using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;

namespace DataViewDashboard.Converters
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value.ToString().Equals("White"))
                {
                    return new SolidColorBrush(Colors.White);
                }
                else if (value.ToString().Equals("Black"))
                {
                    return new SolidColorBrush(Colors.Black);
                }
                else if (value.ToString().Equals("Silver"))
                {
                    return new SolidColorBrush(Colors.Silver);
                }
                else if (value.ToString().Equals("Blue"))
                {
                    return new SolidColorBrush(Colors.Blue);
                }
                else if (value.ToString().Equals("Red"))
                {
                    return new SolidColorBrush(Colors.Red);
                }
                else if (value.ToString().Equals("Yellow"))
                {
                    return new SolidColorBrush(Colors.Yellow);
                }
                else if (value.ToString().Equals("Multi"))
                {
                    GradientStopCollection gradients = new GradientStopCollection();
                    gradients.Add(new GradientStop(Colors.Red, 0.0));
                    gradients.Add(new GradientStop(Colors.Yellow, 0.25));
                    gradients.Add(new GradientStop(Colors.Green, 0.50));
                    gradients.Add(new GradientStop(Colors.Blue, 0.75));
                    gradients.Add(new GradientStop(Colors.Purple, 1.0));
                    return new LinearGradientBrush(gradients);
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
