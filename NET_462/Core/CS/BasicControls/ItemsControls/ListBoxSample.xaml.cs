using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using C1.WPF;

namespace BasicControls
{
    /// <summary>
    /// Interaction logic for ListBoxDemo.xaml
    /// </summary>
    public partial class ListBoxSample : UserControl
    {
        public ListBoxSample()
        {
            InitializeComponent();

            Loaded += new System.Windows.RoutedEventHandler(ListBoxSample_Loaded);
        }

        void ListBoxSample_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPhotos();
        }

        private void LoadPhotos()
        {
            var flickrUrl = "http://api.flickr.com/services/feeds/photos_public.gne?tags=animals";
            var AtomNS = "http://www.w3.org/2005/Atom";
            loading.Visibility = Visibility.Visible;
            listBox.Visibility = Visibility.Collapsed;
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

                    listBox.ItemsSource = photos;
                    loading.Visibility = Visibility.Collapsed;
                    listBox.Visibility = Visibility.Visible;
                    retry.Visibility = Visibility.Collapsed;
                }
                catch
                {
                    MessageBox.Show("There was an error when attempting to download data from Flickr.");
                    listBox.Visibility = Visibility.Collapsed;
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

        public C1SelectionMode SelectionMode
        {
            get
            {
                return listBox.SelectionMode;
            }
            set
            {
                listBox.SelectionMode = value;
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

        public Brush DEMO_Background
        {
            get
            {
                return listBox.Background;
            }
            set
            {
                listBox.Background = value;
            }
        }

        public Brush DEMO_Foreground
        {
            get
            {
                return listBox.Foreground;
            }
            set
            {
                listBox.Foreground = value;
            }
        }

        public Brush DEMO_BorderBrush
        {
            get
            {
                return listBox.BorderBrush;
            }
            set
            {
                listBox.BorderBrush = value;
            }
        }

        public Brush SelectedBackground
        {
            get
            {
                return listBox.SelectedBackground;
            }
            set
            {
                listBox.SelectedBackground = value;
            }
        }

        public Brush ButtonBackground
        {
            get
            {
                return listBox.ButtonBackground;
            }
            set
            {
                listBox.ButtonBackground = value;
            }
        }

        public Brush ButtonForeground
        {
            get
            {
                return listBox.ButtonForeground;
            }
            set
            {
                listBox.ButtonForeground = value;
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return listBox.MouseOverBrush;
            }
            set
            {
                listBox.MouseOverBrush = value;
            }
        }

        public Brush PressedBrush
        {
            get
            {
                return listBox.PressedBrush;
            }
            set
            {
                listBox.PressedBrush = value;
            }
        }

        public Thickness DEMO_BorderThickness
        {
            get
            {
                return listBox.BorderThickness;
            }
            set
            {
                listBox.BorderThickness = value;
            }
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return listBox.CornerRadius;
            }
            set
            {
                listBox.CornerRadius = value;
            }
        }

        #endregion
    }

    public class Photo
    {
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string Content { get; set; }
    }
}
