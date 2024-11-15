using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace FlexGridExplorer
{
    /// <summary>
    /// Represents a grid cell that has an image and some text.
    /// </summary>
    public abstract class ImageCell : StackPanel
    {
        ImageSource _imgSrc;

        public ImageCell()
        {
            if (_imgSrc == null)
            {
                _imgSrc = GetImageSource(GetImageResourceName());
            }

            Orientation = Orientation.Horizontal;

            var img = new Image();
            img.Source = _imgSrc;
            img.Width = 25;
            img.Height = 15;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.Stretch = Stretch.None;
            Children.Add(img);

            TextBlock = new TextBlock();
            TextBlock.VerticalAlignment = VerticalAlignment.Center;
            Children.Add(TextBlock);
        }

        public TextBlock TextBlock { get; private set; }

        public abstract string GetImageResourceName();

        public static ImageSource GetImageSource(string imageName)
        {
            var imgConv = new ImageSourceConverter();
            string path = string.Format("pack://application:,,,/FlexGridExplorer;component/Images/{0}", imageName);
            return (ImageSource)imgConv.ConvertFromString(path);
        }
    }

    /// <summary>
    /// Represents a grid cell that is bound to a song name.
    /// </summary>
    public class SongCell : ImageCell
    {
        public override string GetImageResourceName()
        {
            return "Song.png";
        }
    }

    /// <summary>
    /// Represents a grid cell that is bound to an artist.
    /// </summary>
    public class ArtistCell : ImageCell
    {
        public override string GetImageResourceName()
        {
            return "Artist.png";
        }
    }

    /// <summary>
    /// Represents a grid cell that is bound to an album.
    /// </summary>
    public class AlbumCell : ImageCell
    {
        public override string GetImageResourceName()
        {
            return "Album.png";
        }
    }
}
