using ListViewExplorer.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace ListViewExplorer
{
    public enum ApplicationState
    {
        Loading,
        Loaded,
        Failed
    }

    /// <summary>
    /// Interaction logic for ListViewDemo.xaml
    /// </summary>
    public partial class Flickr : UserControl
    {
        public Flickr()
        {
            InitializeComponent();
            Tag = AppResources.FlickrDescription;

            LoadPhotos();
        }

        private async void LoadPhotos()
        {
            var source = new List<Photo>();
            var state = ApplicationState.Loading;

            UpdateView(source, state);

            try
            {
                source = await PhotosFromFlickr();
                state = ApplicationState.Loaded;
            }
            catch (Exception)
            {
                state = ApplicationState.Failed;
            }

            UpdateView(source, state);
        }

        private void UpdateView(List<Photo> source, ApplicationState state)
        {
            if (state == ApplicationState.Loading)
            {
                UpdateVisibility(state);
            }
            else if (state == ApplicationState.Loaded)
            {
                listView.ItemsSource = source;
                UpdateVisibility(state);
            }
            else if (state == ApplicationState.Failed)
            {
                MessageBox.Show("There was an error when attempting to download data from Flickr.");
                UpdateVisibility(state);
            }
        }

        private async Task<List<Photo>> PhotosFromFlickr()
        {
            List<Photo> photos = new List<Photo>();

            Uri flickrAPI = new Uri("http://api.flickr.com/services/feeds/photos_public.gne?tags=explore");
            string AtomNS = "http://www.w3.org/2005/Atom";

            HttpClient httpClient = new HttpClient();

            using (HttpResponseMessage httpResponse = await httpClient.GetAsync(flickrAPI, HttpCompletionOption.ResponseHeadersRead))
            {
                httpResponse.EnsureSuccessStatusCode();
                if (httpResponse.Content is not null)
                {
                    Stream stream = await httpResponse.Content.ReadAsStreamAsync();
                    photos = Photo.FromStream(stream, AtomNS);
                }
            }

            return photos;
        }

        private Dictionary<ApplicationState, Dictionary<string, Visibility>> visibilityConfigs
            = new Dictionary<ApplicationState, Dictionary<string, Visibility>>()
        {
            {
                ApplicationState.Loading,
                                        new Dictionary<string, Visibility>
                                        {
                                            {"loading", Visibility.Visible},
                                            {"listView", Visibility.Collapsed},
                                            {"retry", Visibility.Collapsed}
                                        }
            },
            {
                ApplicationState.Loaded,
                                        new Dictionary<string, Visibility>
                                        {
                                            {"loading", Visibility.Collapsed},
                                            {"listView", Visibility.Visible},
                                            {"retry", Visibility.Collapsed}
                                        }
            },
            {
                ApplicationState.Failed,
                                        new Dictionary<string, Visibility>
                                        {
                                            {"loading", Visibility.Collapsed},
                                            {"listView", Visibility.Collapsed},
                                            {"retry", Visibility.Visible}
                                        }
            }
        };

        private void UpdateVisibility(ApplicationState state)
        {
            Dictionary<string, Visibility> visibilitySettings = visibilityConfigs[state];

            loading.Visibility = visibilitySettings["loading"];
            listView.Visibility = visibilitySettings["listView"];
            retry.Visibility = visibilitySettings["retry"];
        }

        private void Retry_Click(object sender, RoutedEventArgs e)
        {
            LoadPhotos();
        }
    }

    public class Photo
    {
        private readonly string _title;
        private readonly string _content;
        private readonly string _thumbnail;

        public Photo(string title, string content, string thumbnail)
        {
            _title = title;
            _content = content;
            _thumbnail = thumbnail;
        }

        public static List<Photo> FromStream(Stream stream, string atomNamespace)
        {
            return XDocument.Load(stream)
                        .Descendants(XName.Get("entry", atomNamespace))
                        .Select(entry => Photo.FromEntry(entry, atomNamespace))
                        .ToList();
        }

        public static Photo FromEntry(XElement entry, string atomNamespace)
        {
            string title = entry.Element(XName.Get("title", atomNamespace)).Value;

            string imageUri = entry.Elements(XName.Get("link", atomNamespace))
                                        .Where(xElement => xElement.Attribute("rel").Value == "enclosure")
                                        .Select(xElement => xElement.Attribute("href").Value)
                                        .FirstOrDefault();

            string thumbnailUri = imageUri.Replace("_b", "_m");

            return new Photo(title, imageUri, thumbnailUri);
        }

        public string Title => _title;
        public string Content => _content;
        public string Thumbnail => _thumbnail;
    }
}