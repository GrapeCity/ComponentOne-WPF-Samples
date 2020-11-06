using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace C1.WPF.C1Chart.Labels
{
  static class LabelHelper
  {
    public static Rect RectFromLabel(this ILabel label)
    {
      return new Rect(label.Position, label.Size);
    }

    public static Point GetConnectingPoint(this ILabel label)
    {
      Point center = new Point(label.Position.X + 0.5 * label.Size.Width, label.Position.Y + 0.5 * label.Size.Height);

      Point point = label.Point;
      
      var rect = label.RectFromLabel();

			Point[] pts = new Point[4];

			double k = ( center.Y - point.Y) / ( center.X - point.X);

			pts[0].X = rect.Left;
			pts[0].Y = k * (pts[0].X - center.X) + center.Y;

			pts[1].X = rect.Right;
			pts[1].Y = k * (pts[1].X - center.X) + center.Y;

			k = 1 / k;

			pts[2].Y = rect.Top;
			pts[2].X = k * (pts[2].Y - center.Y) + center.X;

			pts[3].Y = rect.Bottom;
			pts[3].X = k * (pts[3].Y - center.Y) + center.X;

			var min_dist = double.MaxValue;
			int imin = 0;

			Rect r = rect;
			r.Inflate( 2, 2);

			for( int i = 0; i<4; i++)
			{
				if( !r.Contains(pts[i]))
					continue;

				var dx = point.X - pts[i].X;
				var dy = point.Y - pts[i].Y;

				var dist = dx*dx + dy*dy;
				if( dist < min_dist)
				{
					min_dist = dist;
					imin = i;
				}
			}

			return pts[imin];
		}

    public static Point Intersect(Point start1, Point end1, Point start2, Point end2)
    {
      var dir1 = Point.Subtract( end1, start1);
      var dir2 = Point.Subtract( end2, start2);

      double a1 = -dir1.Y;
      double b1 = +dir1.X;
      double d1 = -(a1 * start1.X + b1 * start1.Y);

      double a2 = -dir2.Y;
      double b2 = +dir2.X;
      double d2 = -(a2 * start2.X + b2 * start2.Y);

      double seg1_line2_start = a2 * start1.X + b2 * start1.Y + d2;
      double seg1_line2_end = a2 * end1.X + b2 * end1.Y + d2;

      double seg2_line1_start = a1 * start2.X + b1 * start2.Y + d1;
      double seg2_line1_end = a1 * end2.X + b1 * end2.Y + d1;

      if (seg1_line2_start * seg1_line2_end >= 0 || seg2_line1_start * seg2_line1_end >= 0)
        return EmptyPoint;

      double u = seg1_line2_start / (seg1_line2_start - seg1_line2_end);
      
      return new Point(start1.X + u * dir1.X, start1.Y + u * dir1.Y);
    }

    public static Point EmptyPoint = new Point(double.NaN, double.NaN);

    public static bool IsEmpty(this Point point)
    {
      return double.IsNaN(point.X) && double.IsNaN(point.Y);
    }

    public static IEnumerable<T> FindChildren<T>(this DependencyObject source) where T : DependencyObject
    {
      if (source != null)
      {
        var childs = GetChildObjects(source);
        foreach (DependencyObject child in childs)
        {
          //analyze if children match the requested type
          if (child != null && child is T)
          {
            yield return (T)child;
          }

          //recurse tree
          foreach (T descendant in FindChildren<T>(child))
          {
            yield return descendant;
          }
        }
      }
    }


    /// <summary>
    /// This method is an alternative to WPF's
    /// <see cref="VisualTreeHelper.GetChild"/> method, which also
    /// supports content elements. Keep in mind that for content elements,
    /// this method falls back to the logical tree of the element.
    /// </summary>
    /// <param name="parent">The item to be processed.</param>
    /// <returns>The submitted item's child elements, if available.</returns>
    public static IEnumerable<DependencyObject> GetChildObjects(this DependencyObject parent)
    {
      if (parent == null) yield break;

      if (parent is ContentElement || parent is FrameworkElement)
      {
        //use the logical tree for content / framework elements
        foreach (object obj in LogicalTreeHelper.GetChildren(parent))
        {
          var depObj = obj as DependencyObject;
          if (depObj != null) yield return (DependencyObject)obj;
        }
      }
      else
      {
        //use the visual tree per default
        int count = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < count; i++)
        {
          yield return VisualTreeHelper.GetChild(parent, i);
        }
      }
    }

   }
}
