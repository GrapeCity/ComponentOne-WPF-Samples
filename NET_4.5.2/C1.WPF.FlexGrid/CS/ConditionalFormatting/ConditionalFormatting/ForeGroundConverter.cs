using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;


namespace ConditionalFormatting
{
    public class ForegroundConverter : System.Windows.Data.IValueConverter
    {
        static SolidColorBrush _brBlack = new SolidColorBrush(Colors.Black);
        static SolidColorBrush _brRed = new SolidColorBrush(Colors.Red);
        static SolidColorBrush _brGreen = new SolidColorBrush(Colors.Green);

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var range = (Point)parameter;
            double val = (double)value;
            return
                val < range.X ? _brRed :
                val > range.Y ? _brGreen :
                _brBlack;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
