using ListViewExplorer.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;

namespace ListViewExplorer
{
    /// <summary>
    /// Interaction logic for ListBoxDemo.xaml
    /// </summary>
    public partial class Flickr : UserControl
    {
        public Flickr()
        {
            InitializeComponent();
            Tag = AppResources.FlickrDescription;

            LoadPhotos();
        }

        private void LoadPhotos()
        {
            var flickrUrl = "http://api.flickr.com/services/feeds/photos_public.gne?tags=animals";
            var AtomNS = "http://www.w3.org/2005/Atom";
            loading.Visibility = Visibility.Visible;
            listView.Visibility = Visibility.Collapsed;
            retry.Visibility = Visibility.Collapsed;

            var photos = new List<Photo>();
            var client = new WebClient();
            client.OpenReadCompleted += (s, e) =>
            {
                try
                {
                    #region ** parse flickr data
                    var doc = XDocument.Load(new XmlTextReader(e.Result));
                    foreach (var entry in doc.Descendants(XName.Get("entry", AtomNS)))
                    {
                        var title = entry.Element(XName.Get("title", AtomNS)).Value;
                        var enclosure = entry.Elements(XName.Get("link", AtomNS)).Where(elem => elem.Attribute("rel").Value == "enclosure").FirstOrDefault();
                        var contentUri = enclosure.Attribute("href").Value;
                        photos.Add(new Photo() { Title = title, Content = contentUri, Thumbnail = contentUri.Replace("_b", "_m") });
                    }
                    #endregion

                    listView.ItemsSource = photos;
                    loading.Visibility = Visibility.Collapsed;
                    listView.Visibility = Visibility.Visible;
                    retry.Visibility = Visibility.Collapsed;
                }
                catch
                {
                    MessageBox.Show("There was an error when attempting to download data from Flickr.");
                    listView.Visibility = Visibility.Collapsed;
                    loading.Visibility = Visibility.Collapsed;
                    retry.Visibility = Visibility.Visible;
                }
            };
            client.OpenReadAsync(new Uri(flickrUrl));
        }

        private void Retry_Click(object sender, RoutedEventArgs e)
        {
            LoadPhotos();
        }
    }

    public class Photo
    {
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string Content { get; set; }
    }
}
