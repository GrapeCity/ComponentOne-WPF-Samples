using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using C1.WPF;

namespace BasicControls
{
    /// <summary>
    /// Interaction logic for TileListBoxSample.xaml
    /// </summary>
    public partial class TileListBoxSample : UserControl
    {
        public TileListBoxSample()
        {
            InitializeComponent();

            LoadVideos();
        }

        private void LoadVideos()
        {
            var youtubeUrl = "https://gdata.youtube.com/feeds/api/videos?q=wpf";
            var AtomNS = "http://www.w3.org/2005/Atom";
            var MediaNS = "http://search.yahoo.com/mrss/";
            loading.Visibility = Visibility.Visible;
            listBox.Visibility = Visibility.Collapsed;
            retry.Visibility = Visibility.Collapsed;

            var videos = new List<Video>();
            var client = new WebClient();
            client.OpenReadCompleted += (s, e) =>
            {
                if (e.Error == null)
                {
                    #region ** parse you tube data
                    var doc = XDocument.Load(new XmlTextReader(e.Result));
                    foreach (var entry in doc.Descendants(XName.Get("entry", AtomNS)))
                    {
                        var title = entry.Element(XName.Get("title", AtomNS)).Value;
                        var thumbnail = "";
                        var group = entry.Element(XName.Get("group", MediaNS));
                        var thumbnails = group.Elements(XName.Get("thumbnail", MediaNS));
                        var thumbnailElem = thumbnails.FirstOrDefault();
                        if (thumbnailElem != null)
                            thumbnail = thumbnailElem.Attribute("url").Value;
                        var alternate = entry.Elements(XName.Get("link", AtomNS)).Where(elem => elem.Attribute("rel").Value == "alternate").FirstOrDefault();
                        var link = alternate.Attribute("href").Value;
                        videos.Add(new Video() { Title = title, Link = link, Thumbnail = thumbnail });
                    }
                    #endregion

                    listBox.ItemsSource = videos;
                    loading.Visibility = Visibility.Collapsed;
                    listBox.Visibility = Visibility.Visible;
                    retry.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("There was an error when attempting to download data from you tube.");
                    listBox.Visibility = Visibility.Collapsed;
                    loading.Visibility = Visibility.Collapsed;
                    retry.Visibility = Visibility.Visible;
                }
            };
            client.OpenReadAsync(new Uri(youtubeUrl));
        }

        private void Retry_Click(object sender, RoutedEventArgs e)
        {
            LoadVideos();
        }

        private void tileListBox_ItemClick(object sender, EventArgs e)
        {
            var video = (sender as C1ListBoxItem).Content as Video;
            NavigateUrl(video.Link);
        }

        public void NavigateUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch { }
        }
        #region ** public properties
        public Orientation Orientation
        {
            get
            {
                return listBox.Orientation;
            }
            set
            {
                listBox.Orientation = value;
            }
        }

        public double ItemWidth
        {
            get
            {
                return listBox.ItemWidth;
            }
            set
            {
                listBox.ItemWidth = value;
            }
        }

        public double ItemHeight
        {
            get
            {
                return listBox.ItemHeight;
            }
            set
            {
                listBox.ItemHeight = value;
            }
        }

        public ZoomMode ZoomMode
        {
            get
            {
                return listBox.ZoomMode;
            }
            set
            {
                listBox.ZoomMode = value;
            }
        }

        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get
            {
                return listBox.HorizontalScrollBarVisibility;
            }
            set
            {
                listBox.HorizontalScrollBarVisibility = value;
            }
        }

        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get
            {
                return listBox.VerticalScrollBarVisibility;
            }
            set
            {
                listBox.VerticalScrollBarVisibility = value;
            }
        }
        
        #endregion
    }

    public class Video
    {
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string Link { get; set; }
    }
}
