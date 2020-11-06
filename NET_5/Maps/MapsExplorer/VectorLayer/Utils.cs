using System;
using System.IO;
using System.Net;
using System.Reflection;
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
  public class Utils
  {
    public static Geometry CreateBaloon()
    {
      PathGeometry pg = new PathGeometry();
      pg.Transform = new TranslateTransform() { X = -10, Y = -24.14 };
      PathFigure pf = new PathFigure() { StartPoint = new Point(10, 24.14), IsFilled = true, IsClosed = true };
      pf.Segments.Add(new ArcSegment() { SweepDirection = SweepDirection.Counterclockwise, Point = new Point(5, 19.14), RotationAngle = 45, Size = new Size(10, 10) });
      pf.Segments.Add(new ArcSegment() { SweepDirection = SweepDirection.Clockwise, Point = new Point(15, 19.14), RotationAngle = 270, Size = new Size(10, 10), IsLargeArc = true });
      pf.Segments.Add(new ArcSegment() { SweepDirection = SweepDirection.Counterclockwise, Point = new Point(10, 24.14), RotationAngle = 45, Size = new Size(10, 10) });
      pg.Figures.Add(pf);
      return pg;
    }

    public static void LoadShapeFromResource(VectorLayer vl, string resname, string dbfname, Location location, bool clear, ProcessShapeItem pv)
    {
        using (Stream shapeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resname))
        {
            using (Stream dbfStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(dbfname))
            {
                if (shapeStream != null)
                {
                    if (clear)
                        vl.Children.Clear();

                    vl.LoadShape(shapeStream, dbfStream, location, true, pv);
                }
            }
        }
    }
    
    public static void LoadKMZFromResources(VectorLayer vl, string resname, bool clear, ProcessVectorItem pv)
    {
      using (Stream zipStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resname))
      {
        if (zipStream != null)
        {
          if( clear)
            vl.Children.Clear();

          vl.LoadKMZ(zipStream, true, pv);
        }
      }
    }

    public static void LoadKMLFromResources(VectorLayer vl, string resname, bool clear, ProcessVectorItem pv)
    {
        using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resname))
        {
            if (stream != null)
            {
                if (clear)
                    vl.Children.Clear();

                vl.LoadKML(stream, false, pv);
            }
        }
    }

    static Random rnd = new Random();

    public static Color GetRandomColor(byte a)
    {
      return Color.FromArgb(a, (byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255));
    }

    public static Color GetRandomColor(byte min, byte a)
    {
      return Color.FromArgb(a, (byte)( min + rnd.Next(255-min)), 
        (byte)(min + rnd.Next(255-min)), (byte)(min + rnd.Next(255-min)));
    }
  }
}
