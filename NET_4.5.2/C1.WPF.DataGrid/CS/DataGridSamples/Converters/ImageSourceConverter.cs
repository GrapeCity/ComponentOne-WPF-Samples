using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DataGridSamples
{
    public class ImageSourceConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType.FullName == "System.Windows.Media.ImageSource" && parameter is string)
            {
                BitmapImage bitmap = new BitmapImage(new Uri((parameter as string) + (string)value, UriKind.RelativeOrAbsolute));
                return bitmap;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
