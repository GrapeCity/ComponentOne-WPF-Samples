using System.Windows.Media;

namespace RibbonExplorer
{
    public class Sample
    {
        public string Name { get; set; }

        public int SampleViewType { get; set; }

        public string Description { get; set; }

        public string Thumbnail { get; set;}

        public ImageSource ThumbnailImageSource
        {
            get
            {
                return null;
                //return GetImageSource(Thumbnail);
            }
        }

        public static ImageSource GetImageSource(string imageName)
        {
            var imgConv = new ImageSourceConverter();
            string path = string.Format("pack://application:,,,/RibbonExplorer;component/Images/{0}", imageName);
            return (ImageSource)imgConv.ConvertFromString(path);
        }
    }
}
