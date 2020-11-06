using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using C1.WPF.Maps;

namespace MapsSamples
{
  public partial class Grid : UserControl
  {
    C1VectorLayer vl;

    public Grid()
    {
      InitializeComponent();

      Color fc = Colors.LightGray;// Color.FromArgb(0xff, 0xC0, 0x50, 0x4d);
      Color bk = Color.FromArgb(0xff, 0x44, 0x44, 0x44);
      Color bb = Colors.Black;
      maps.Foreground = new SolidColorBrush(fc);
      maps.Background = new SolidColorBrush(bk);
      maps.BorderBrush = new SolidColorBrush(bb);

      vl = new C1VectorLayer();

      SolidColorBrush stroke = new SolidColorBrush(Colors.LightGray);
      
      for (int lon = -180; lon <= 180; lon += 30)
      {
        DoubleCollection dc = new DoubleCollection();
        dc.Add(1); dc.Add(2);

        C1VectorPolyline pl = new C1VectorPolyline() { Stroke=stroke, StrokeDashArray=dc};
        PointCollection pc = new PointCollection();
        pc.Add( new Point(lon, -85));
        pc.Add( new Point(lon, +85));
        pl.Points = pc;
        vl.Children.Insert(0,pl);

        string lbl = Math.Abs(lon).ToString() + "°";
        if (lon > 0)
          lbl += "E";
        else if (lon < 0)
          lbl += "W";

        C1VectorPlacemark pm = new C1VectorPlacemark()
        {
          GeoPoint = new Point(lon,0),
          Label = lbl,
          LabelPosition = LabelPosition.Top
        };
        vl.Children.Add(pm);
      }

      for (int lat = -80; lat <= 80; lat += 20)
      {
        DoubleCollection dc = new DoubleCollection();
        dc.Add(1); dc.Add(2);

        C1VectorPolyline pl = new C1VectorPolyline() { Stroke = stroke, StrokeDashArray=dc };
        PointCollection pc = new PointCollection();
        pc.Add(new Point(-180,lat));
        pc.Add(new Point(180, lat));
        pl.Points = pc;
        vl.Children.Insert(0,pl);

        string lbl = Math.Abs(lat).ToString() + "°";
        if (lat > 0)
          lbl += "N";
        else if( lat<0)
          lbl += "S";

        C1VectorPlacemark pm = new C1VectorPlacemark()
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
