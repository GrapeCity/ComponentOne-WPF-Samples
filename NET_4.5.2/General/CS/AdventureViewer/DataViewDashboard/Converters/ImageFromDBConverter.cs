using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media.Imaging;
using System.IO;

namespace DataViewDashboard.Converters
{
    public class ImageFromDBConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] bytes = value as byte[];
            if(bytes != null)
            {
                MemoryStream ms = new MemoryStream(bytes);
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = ms;
                bmp.EndInit();
                return bmp;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
