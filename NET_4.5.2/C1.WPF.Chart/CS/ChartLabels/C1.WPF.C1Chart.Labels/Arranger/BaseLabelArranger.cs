using System;
using System.Collections.Generic;
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
  /// <summary>
  /// Base class for label arranger.
  /// </summary>
  public class BaseLabelArranger : ILabelArranger
  {
    /// <summary>
    /// Performs label layout.
    /// </summary>
    /// <param name="labels"></param>
    /// <param name="rectangles"></param>
    /// <param name="border"></param>
    public virtual void Arrange(IList<ILabel> labels, IList<Rect> rectangles, Rect border)
    {
      throw new NotImplementedException();
    }

    internal static bool Contains(Rect bdr, Rect rect)
    {
      return bdr.Contains(new Point( rect.X,rect.Y)) && bdr.Contains( new Point( rect.Right,rect.Bottom));
    }

    internal static bool Intersect(ILabel label1, ILabel label2)
    {
      var rect1 = label1.RectFromLabel();

      var rect2 = label2.RectFromLabel();
      rect1.Intersect(rect2);

      return (!rect1.IsEmpty);
    }

    internal static bool Intersect(ILabel label, Rect rect)
    {
      var rect1 = label.RectFromLabel();
      rect1.Intersect(rect);
      return (!rect1.IsEmpty);
    }

    internal static bool Contains(ILabel label, Rect rect)
    {
      return rect.Contains( label.Position) && 
        rect.Contains( new Point(label.Position.X + label.Size.Width, label.Position.Y + label.Size.Height));
    }

    internal static bool IntersectPoint(ILabel label, Point point)
    {
      var rect = label.RectFromLabel();
      return rect.Contains(point);
    }

    internal static LabelState[] CreateState(IList<ILabel> labels)
    {
      int cnt = labels.Count;
      var state = new LabelState[cnt];
      for (int i = 0; i < cnt; i++)
        state[i] = new LabelState(labels[i]);
      return state;
    }
  }
}
