using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GaugesDemo
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
                return new BitmapImage(new Uri("Images/" + Thumbnail, UriKind.Relative)) { CreateOptions = BitmapCreateOptions.None };
            }
        }
    }
}
