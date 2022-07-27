using C1.WPF.Core;
using C1.WPF.Docking;
using C1.WPF.Maps;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Linq;
using Res = MapsExplorer.Properties.Resources;

namespace MapsExplorer
{
    public partial class Flicker : UserControl
    {
        DispatcherTimer _timer;

        public Flicker()
        {
            InitializeComponent();
            Tag = Res.Flicker;
            txt.Text = Res.LoadingData;
            this.Loaded += Flicker_Loaded;
        }

        private void Flicker_Loaded(object sender, RoutedEventArgs e)
        {
            if (maps.Layers.Count > 0)
            {
                vl = maps.Layers[0] as VectorLayer;
            }
            else
            {
                vl = new VectorLayer() { ItemStyle = this.FindResource("style") as Style };
                maps.Layers.Add(vl);
            }
            vl.UriSourceLoaded += vl_UriSourceLoaded;
            vl.UriSource = new Uri("http://api.flickr.com/services/feeds/geo/Uruguay/Montevideo");

            vl.UriSourceFailed += (s, e) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    txt.Text = Res.CannotLoadData;
                    btnLoad.IsEnabled = tb.IsEnabled = true;
                }));
            };

            // shuffle images in z-order
            _timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.5) };
            _timer.Tick += (s, e) =>
            {
                int cnt = vl.Children.Count;
                if (cnt >= 2)
                {
                    vl.BeginUpdate();

                    VectorItemBase item = vl.Children[0];
                    vl.Children.RemoveAt(0);
                    vl.Children.Add(item);

                    vl.EndUpdate();
                }
            };

            btnLoad.IsEnabledChanged += (s, e) =>
            {
                btnLoad.Content = btnLoad.IsEnabled ? Res.Load : Res.LoadingData;
            };
            maps.Zoom = 4;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tb.Text))
            {
                string source = string.Format("http://api.flickr.com/services/feeds/geo/{0}", tb.Text);
                if (vl != null)
                {
                    _timer.Stop();
                    vl.UriSource = new Uri(source);
                    btnLoad.IsEnabled = tb.IsEnabled = false;
                    txt.Text = Res.LoadingData;
                    txt.Visibility = Visibility.Visible;
                    maps.Opacity = 0.5;
                }
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border bdr = (Border)sender;
            C1Window wnd = new C1Window()
            {
                ShowMaximizeButton = false,
                ShowMinimizeButton = false,
                IsResizable = false
            };
            Image contImg = new Image()
            {
                MaxWidth = 400,
                MaxHeight = 400,
                Stretch = Stretch.None
            };
            wnd.Content = contImg;
            ShowImage(wnd, contImg, bdr, "");
            wnd.ShowModal();

            // Find other images covered by the current images
            Point pt0 = e.GetPosition(bdr);

            Point pt = e.GetPosition(maps);
            Point pt1 = pt, pt2 = pt;
            pt1.X -= pt0.X;
            pt1.Y -= pt0.Y;
            pt2.X = pt1.X + bdr.ActualWidth;
            pt2.Y = pt1.Y + bdr.ActualWidth;

            pt1 = maps.ScreenToGeographic(pt1);
            pt2 = maps.ScreenToGeographic(pt2);

            Point min = new Point(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y));
            Point max = new Point(Math.Max(pt1.X, pt2.X), Math.Max(pt1.Y, pt2.Y));

            List<VectorPlacemark> list = new List<VectorPlacemark>();

            foreach (VectorPlacemark pm in vl.Children)
            {
                if (pm.GeoPoint.X >= min.X && pm.GeoPoint.X <= max.X &&
                  pm.GeoPoint.Y >= min.Y && pm.GeoPoint.Y <= max.Y)
                {
                    list.Add(pm);
                }
            }

            // start "slideshow"
            if (list.Count > 1)
            {
                DispatcherTimer dp;
                int tcnt = 0;

                dp = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(2) };
                dp.Tick += (se, ea) =>
                  {
                      tcnt++;
                      if (tcnt >= list.Count)
                          tcnt = 0;

                      ShowImage(wnd, contImg, list[tcnt].LabelUI as FrameworkElement,
                string.Format("{0}/{1} ", tcnt + 1, list.Count));
                  };

                wnd.Closing += (s1, e1) =>
                  {
                      dp.Stop();
                  };

                dp.Start();
            }
        }

        private void ShowImage(C1Window wnd, Image contImg, FrameworkElement lbl, string header)
        {
            if (lbl != null)
            {
                Image im = lbl.FindName("img") as Image;
                if (im != null)
                    contImg.Source = im.Source;
                TextBlock tb = lbl.FindName("txt") as TextBlock;
                if (tb != null)
                    wnd.Header = header + tb.Text;
            }
        }

        double _zoom = 0;

        private void vl_UriSourceLoaded(object sender, EventArgs e)
        {
            VectorLayer vl = (VectorLayer)sender;
            Rect bnds = vl.Children.GetBounds();
            if (!bnds.IsEmpty)
            {
                maps.TargetCenter = new Point(bnds.Left + 0.5 * bnds.Width, bnds.Top + 0.5 * bnds.Height);

                double w = maps.ActualWidth > 0 ? maps.ActualWidth : 500;
                double h = maps.ActualHeight > 0 ? maps.ActualHeight : 500;

                double scale = Math.Max(bnds.Width / 360 * w,
                      bnds.Height / 180 * h); ;
                _zoom = Math.Log(512 / Math.Max(scale, 1), 2.0);
                maps.CenterChanged += (maps_CenterChanged);
            }
            Dispatcher.BeginInvoke(new Action(() =>
            {
                txt.Visibility = Visibility.Collapsed;
                maps.Opacity = 1;
                btnLoad.IsEnabled = tb.IsEnabled = true;
                _timer.Start();
            }));
        }

        void maps_CenterChanged(object sender, PropertyChangedEventArgs<Point> e)
        {
            if (maps.TargetCenter == maps.Center)
            {
                maps.CenterChanged -= (maps_CenterChanged);
                maps.TargetZoom = _zoom;
            }
        }

        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Button_Click(btnLoad, new RoutedEventArgs());
        }
    }

    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            XElement xe = value as XElement;

            if (xe != null)
            {
                var content = xe.Element(xe.GetDefaultNamespace() + "content");
                if (content != null)
                {
                    string s = content.Value;
                    if (!string.IsNullOrEmpty(s))
                    {
                        int i1 = s.IndexOf("http://farm");
                        if (i1 >= 0)
                        {
                            int i2 = s.IndexOf(".jpg", i1);
                            if (i2 >= 0)
                            {
                                return s.Substring(i1, i2 - i1 + ".jpg".Length);
                            }
                        }
                    }
                }
            }

            return null;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
