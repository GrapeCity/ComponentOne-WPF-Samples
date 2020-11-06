using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace C1DataCollection101
{
    public class Sample
    {
        public string Name { get; set; }

        public int SampleViewType { get; set; }

        public string Description { get; set; }

        public string Thumbnail { get; set; }

        public ImageSource ThumbnailImageSource
        {
            get
            {
                return GetImageSource(Thumbnail);
            }
        }

        public static ImageSource GetImageSource(string imageName)
        {
            var imgConv = new ImageSourceConverter();
            string path = string.Format("pack://application:,,,/C1DataCollection101;component/Images/{0}", imageName);
            return (ImageSource)imgConv.ConvertFromString(path);
        }
    }
}
