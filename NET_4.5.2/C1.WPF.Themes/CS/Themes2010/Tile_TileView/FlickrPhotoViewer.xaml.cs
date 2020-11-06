using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Themes
{
    /// <summary>
    /// Interaction logic for FlickrPhotoViewer.xaml
    /// </summary>
    public partial class FlickrPhotoViewer : UserControl
    {
        private bool _isContentLoaded = false;
        
        public FlickrPhotoViewer()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!_isContentLoaded && this.IsVisible)
            {
                // load content once
                foreach (C1.WPF.Tile.C1Tile tile in tilePanel.Children)
                {
                    FlickrPhoto.Load((string)tile.Header, tile);
                }
                _isContentLoaded = true;
            }
        }
    }

    public class FlickrPhoto : C1.WPF.Tile.ILoadable
    {
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public ImageSource ImageSource { get; private set; }

        #region ** ILoadable implementation
        public bool IsLoaded
        {
            get { return _isLoaded; }
        }
        private bool _isLoaded = false;

        public bool Load()
        {
            if (!_isLoaded && !string.IsNullOrEmpty(Thumbnail))
            {
                BitmapImage img = new BitmapImage(new Uri(Thumbnail, UriKind.Absolute));
                img.UriCachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.CacheIfAvailable);
                img.DownloadCompleted += (s, e) =>
                {
                    _isLoaded = true;
                };
                img.DownloadFailed += (s, e) =>
                {
                    _isLoaded = true;
                };
                if (!img.IsDownloading)
                {
                    // the message was taken from cache. In such case DownloadCompleted won't fire
                    _isLoaded = true;
                }
                ImageSource = img;
            }
            return _isLoaded;
        }
        #endregion

        static string flickrUrl = "http://api.flickr.com/services/feeds/photos_public.gne";
        static string AtomNS = "http://www.w3.org/2005/Atom";

        /// <summary>
        /// Loads public photos from flickr.
        /// </summary>
        /// <param name="tag">If set, method uses it to load photos only with this specific tag.</param>
        /// <returns></returns>
        public static void Load(string tag, Control control)
        {

            List<FlickrPhoto> result = new List<FlickrPhoto>();

            string uri = string.IsNullOrEmpty(tag) ? flickrUrl : flickrUrl + "?tags=" + tag;
            var client = new WebClient();
            client.OpenReadCompleted += (s, e) =>
            {
                try
                {
                    #region ** parse flickr data
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(e.Result))
                    {
                        var doc = XDocument.Load(reader);
                        foreach (var entry in doc.Descendants(XName.Get("entry", AtomNS)))
                        {
                            var title = entry.Element(XName.Get("title", AtomNS)).Value;
                            var author = entry.Element(XName.Get("author", AtomNS)).Element(XName.Get("name", AtomNS)).Value;
                            var enclosure = entry.Elements(XName.Get("link", AtomNS)).Where(elem => elem.Attribute("rel").Value == "enclosure").FirstOrDefault();
                            var contentUri = enclosure.Attribute("href").Value;
                            result.Add(new FlickrPhoto() { Title = title, Content = contentUri, Thumbnail = contentUri.Replace("_b", "_m"), Author = author });
                        }
                        if (control is C1.WPF.Tile.C1Tile)
                        {
                            ((C1.WPF.Tile.C1Tile)control).ContentSource = result;
                        }
                        else if (control is ItemsControl)
                        {
                            ((ItemsControl)control).ItemsSource = result;
                        }
                    }
                    #endregion
                }
                catch
                {
                    MessageBox.Show("There was an error when attempting to download data from Flickr.");
                }
            };
            client.OpenReadAsync(new Uri(uri));
        }
    }
}
