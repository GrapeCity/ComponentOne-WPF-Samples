﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
#if SILVERLIGHT
using C1.Silverlight.FlexGrid;
#else
#endif

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

        //public virtual GridRow Row
        //{
        //    set { BindCell(value.DataItem); }
        //}
        //void BindCell(object dataItem)
        //{
        //    var binding = new Binding("Name");
        //    binding.Source = dataItem;
        //    TextBlock.SetBinding(TextBlock.TextProperty, binding);
        //}
        public abstract string GetImageResourceName();

        public static ImageSource GetImageSource(string imageName)
        {
#if SILVERLIGHT
            var bmp = new BitmapImage();
            bmp.CreateOptions = BitmapCreateOptions.None;
            var fmt = "Resources/{0}";
            bmp.UriSource = new Uri(string.Format(fmt, imageName), UriKind.Relative);
            return bmp;
#else
            var imgConv = new ImageSourceConverter();
            string path = string.Format("pack://application:,,,/FlexGridExplorer;component/Images/{0}", imageName);
            return (ImageSource)imgConv.ConvertFromString(path);
#endif
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
    /// Represents a grid cell that has a collapse/expand icon, an image, and some text.
    /// </summary>
    public abstract class NodeCell : ImageCell
    {
        const double ALPHA = 0.5;
        Image _nodeImage;
        static ImageSource _bmpExpanded, _bmpCollapsed;

        public NodeCell()
        {
            // create collapsed/expanded images
            if (_bmpExpanded == null)
            {
                _bmpExpanded = ImageCell.GetImageSource("Expanded.png");
                _bmpCollapsed = ImageCell.GetImageSource("Collapsed.png");
            }

            // store reference to row

            // initialize collapsed/expanded image
            _nodeImage = new Image();
            _nodeImage.Width = _nodeImage.Height = 9;
            _nodeImage.VerticalAlignment = VerticalAlignment.Center;
            _nodeImage.Stretch = Stretch.None;
            _nodeImage.MouseLeftButtonDown += img_MouseLeftButtonDown;
            _nodeImage.MouseEnter += img_MouseEnter;
            _nodeImage.MouseLeave += img_MouseLeave;
            _nodeImage.Opacity = ALPHA;
            _nodeImage.Source = _isCollapsed ? _bmpCollapsed : _bmpExpanded;
            Children.Insert(0, _nodeImage);

            // make text bold
            TextBlock.FontWeight = FontWeights.Bold;
        }
        void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var img = sender as Image;
            var cell = img.Parent as NodeCell;
            cell.IsCollapsed = !cell.IsCollapsed;
        }
        void img_MouseEnter(object sender, MouseEventArgs e)
        {
            var img = sender as Image;
            img.Opacity = 1;
        }
        void img_MouseLeave(object sender, MouseEventArgs e)
        {
            var img = sender as Image;
            img.Opacity = ALPHA;
        }
        //public override GridRow Row
        //{
        //    set
        //    {
        //        // update image
        //        _gr = value as GridGroupRow;
        //        _nodeImage.Source = _gr.IsCollapsed ? _bmpCollapsed : _bmpExpanded;
        //        _nodeImage.Opacity = ALPHA;

        //        // update text
        //        base.Row = value;
        //    }
        //}

        private bool _isCollapsed;
        public event EventHandler IsCollapsedChanged;

        public bool IsCollapsed
        {
            get { return _isCollapsed; }
            set
            {
                if (_isCollapsed != value)
                {
                    _isCollapsed = value;
                    _nodeImage.Source = value ? _bmpCollapsed : _bmpExpanded;
                    IsCollapsedChanged?.Invoke(this, new EventArgs());
                }
            }
        }
    }

    /// <summary>
    /// Represents a grid cell that is bound to an artist.
    /// </summary>
    public class ArtistCell : NodeCell
    {
        public override string GetImageResourceName()
        {
            return "Artist.png";
        }
    }

    /// <summary>
    /// Represents a grid cell that is bound to an album.
    /// </summary>
    public class AlbumCell : NodeCell
    {
        public override string GetImageResourceName()
        {
            return "Album.png";
        }
    }
}
