using C1.WPF.Maps;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MapsExplorer
{
    public partial class Grid : UserControl
    {
        VectorLayer vl;

        public Grid()
        {
            InitializeComponent();
            Tag = Properties.Resources.Grid;
            this.Loaded += Grid_Loaded;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            maps.Layers.Clear();
            Color fc = Color.FromArgb(0xff, 0xC0, 0x50, 0x4d);
            maps.Foreground = new SolidColorBrush(fc);

            vl = new VectorLayer();

            SolidColorBrush stroke = new SolidColorBrush(Colors.LightGray);

            for (int lon = -180; lon <= 180; lon += 30)
            {
                DoubleCollection dc = new DoubleCollection();
                dc.Add(1); dc.Add(2);

                VectorPolyline pl = new VectorPolyline() { Stroke = stroke, StrokeDashArray = dc };
                PointCollection pc = new PointCollection();
                pc.Add(new Point(lon, -85));
                pc.Add(new Point(lon, +85));
                pl.Points = pc;
                vl.Children.Insert(0, pl);

                string lbl = Math.Abs(lon).ToString() + "°";
                if (lon > 0)
                    lbl += "E";
                else if (lon < 0)
                    lbl += "W";

                VectorPlacemark pm = new VectorPlacemark()
                {
                    GeoPoint = new Point(lon, 0),
                    Label = lbl,
                    LabelPosition = LabelPosition.Top
                };
                vl.Children.Add(pm);
            }

            for (int lat = -80; lat <= 80; lat += 20)
            {
                DoubleCollection dc = new DoubleCollection();
                dc.Add(1); dc.Add(2);

                VectorPolyline pl = new VectorPolyline() { Stroke = stroke, StrokeDashArray = dc };
                PointCollection pc = new PointCollection();
                pc.Add(new Point(-180, lat));
                pc.Add(new Point(180, lat));
                pl.Points = pc;
                vl.Children.Insert(0, pl);

                string lbl = Math.Abs(lat).ToString() + "°";
                if (lat > 0)
                    lbl += "N";
                else if (lat < 0)
                    lbl += "S";

                VectorPlacemark pm = new VectorPlacemark()
                {
                    GeoPoint = new Point(0, lat),
                    Label = lbl,
                    LabelPosition = LabelPosition.Right
                };
                vl.Children.Add(pm);
            }

            maps.Layers.Add(vl);
        }
    }
}
