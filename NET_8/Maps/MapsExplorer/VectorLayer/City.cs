using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using C1.WPF.Maps;

namespace MapsExplorer
{
  public class City
  {
    public string Name { get; set; }
    public Point GeoPoint { get; set; }
    public string Country { get; set; }
    public int Population { get; set; }

    public override string ToString()
    {
      return string.Format("{0}\t{1}\t{2}\t{3}", Name, GeoPoint, Population, Country);
    }

    public static City FromString(string s)
    {
      City city = null;
      if (!string.IsNullOrEmpty(s))
      {
        string[] ss = s.Split('\t');
        if (ss.Length == 4)
        {
          city = new City()
          {
            Name = ss[0],
            GeoPoint = PointFromString(ss[1]),
            Population = int.Parse(ss[2]),
            Country = ss[3]
          };
        }
      }

      return city;
    }

    static Point PointFromString(string s)
    {
      string[] ss = s.Split(',');
      return new Point(double.Parse(ss[1],CultureInfo.InvariantCulture),
        double.Parse(ss[0], CultureInfo.InvariantCulture));
    }

    public static List<City> Read(Stream stream)
    {
      List<City> cities = new List<City>();

      using (StreamReader sr = new StreamReader(stream))
      {
        for (; !sr.EndOfStream ; )
        {
          City city = FromString(sr.ReadLine());
          if (city != null)
            cities.Add(city);
        }
      }

      return cities;
    }
  }

  public class GeometryConverter : IValueConverter
  {
    static Brush stroke = new SolidColorBrush(Colors.Black);
    static Brush fill = new SolidColorBrush(Colors.White);

    public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
      int population = (int)value;

      EllipseGeometry ell = new EllipseGeometry();

      if (population >= 2000000)
        ell.RadiusX = ell.RadiusY = 6;
      else if (population >= 1000000)
        ell.RadiusX = ell.RadiusY = 5;
      else if (population >= 7500000)
        ell.RadiusX = ell.RadiusY = 4;
      else if (population >= 500000)
        ell.RadiusX = ell.RadiusY = 3;
      else if (population >= 250000)
        ell.RadiusX = ell.RadiusY = 2;
      else
        ell.RadiusX = ell.RadiusY = 1;

      return ell;
    }

    public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  public class LODConverter : IValueConverter
  {
    public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
      int population = (int)value;

      if (population >= 2000000)
        return new LOD(0, 0, 0, 20);

      if (population >= 1000000)
        return new LOD(0, 0, 1, 20);

      if (population >= 7500000)
        return new LOD(0, 0, 2, 20);

      if (population >= 500000)
        return new LOD(0, 0, 3, 20);

      if (population >= 250000)
        return new LOD(0, 0, 4, 20);

      return new LOD(0, 0, 5, 20);
    }

    public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
