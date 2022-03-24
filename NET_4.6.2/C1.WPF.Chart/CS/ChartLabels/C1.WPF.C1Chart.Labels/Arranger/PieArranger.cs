using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace C1.WPF.C1Chart.Labels
{
  /// <summary>
  /// Arranges labels for pie chart.
  /// </summary>
  public class PieArranger : BaseLabelArranger
  {
      // available angles 
      double[] angles = { 45, -45, 0, 90, -90, 180, 135, -135 };

      double offset_start = 20;

      // step for increasing label offset from the anchor point
      double offset_step = 5;

      // maximal label offset
      double max_offset = 140;

      /// <summary>
      /// Performs layout of labels.
      /// </summary>
      /// <param name="labels"></param>
      /// <param name="rectangles"></param>
      /// <param name="border"></param>
      public override void Arrange(IList<ILabel> labels, IList<Rect> rectangles, System.Windows.Rect border)
      {
        // copy original labels to the temporary working set
        var state = CreateState(labels);

        // find the distance from border
        int cnt = labels.Count;

        // sort according to the distance
        // the labels which are closer to the border have less freedom
        LabelState[] array = state.ToArray<LabelState>();

        // main positioning cycle 
        for (int i = 0; i < cnt; i++)
        {
          var lbl = array[i];

          if (lbl.IsVisible)
          {
            bool good = false;

            // first try to put label closely to anchor point
            // the anchor is on the label corner(4 possible states)
#if( false) 
            for (int pos = 0; pos < 4; pos++)
            {
              SetPosition(lbl, (LabelPosition)pos);
              good = IsGoodPosition(lbl, border, array, rectangles, i);
              if (good)
                break;
            }
#endif

            // next
            if (!good)
            {
              // try to increase offset
              for (double offset = offset_start; offset < max_offset; offset += offset_step)
              {
                // and different angles
                var angle = 180.0 / Math.PI * Math.Atan2(lbl.Point.Y - lbl.PlotCenter.Y, lbl.Point.X - lbl.PlotCenter.X);
                var angles = new double[] { angle, angle - 10, angle + 10, angle - 20, angle + 20, };

                for (int ia = 0; ia < angles.Length; ia++)
                {
                  SetPosition(lbl, angles[ia], offset);
                  good = IsGoodPosition(lbl, border, array, rectangles, i);
                  if (good)
                    break;
                }

                if (good)
                  break;
              }
            }
          }
        }

        // position real labels
        for (int i = 0; i < cnt; i++)
          state[i].CopyState(labels[i]);
      }

      // verify whether the current label position is good relatively to the other labels
      static bool IsGoodPosition(LabelState lbl, Rect border, LabelState[] labels, IList<Rect> rects, int index)
      {
        // check border
        //if (!Contains(lbl, border))
        //  return false;

        if (rects != null)
        {
          int cnt = rects.Count;
          for (int i = 0; i < cnt; i++)
          {
            if (Intersect(lbl, rects[i]))
              return false;
          }
        }

        // check other anchor points
        int len = labels.Length;
        for (int i = 0; i < len; i++)
        {
          if (i != index)
          {
            if (IntersectPoint(lbl, labels[i].Point))
              return false;
          }
        }

        for (int i = 0; i < index; i++)
        {
          // check already placed labels
          if (labels[i].IsVisible)
          {
            // label rectangle
            if (Intersect(lbl, labels[i]))
              return false;

            // connecting lines
            if (!LabelHelper.Intersect(lbl.Point, lbl.GetConnectingPoint(), labels[i].Point, labels[i].GetConnectingPoint()).IsEmpty())
              return false;
          }
        }

        return true;
      }

      // set label position relatively to anchor point
      static void SetPosition(LabelState label, LabelPosition position)
      {
        if (!label.IsMovable)
          return;

        var sz = label.Size;

        switch (position)
        {
          case LabelPosition.TopLeft:
            label.Position = label.Point;
            break;
          case LabelPosition.TopRight:
            label.Position = new Point(label.Point.X - sz.Width, label.Point.Y);
            break;
          case LabelPosition.BottomLeft:
            label.Position = new Point(label.Point.X, label.Point.Y - sz.Height);
            break;
          case LabelPosition.BottomRight:
            label.Position = new Point(label.Point.X - sz.Width, label.Point.Y - sz.Height);
            break;
        }
      }

      // set label position relatively to anchor point with specified angle and offset
      static void SetPosition(LabelState label, double angle, double offset)
      {
        if (!label.IsMovable)
          return;

        angle = angle * Math.PI / 180;

        var sz = label.Size;
        var center = new Point(label.Point.X + offset * Math.Cos(angle), label.Point.Y + offset * Math.Sin(angle));

        label.Position = new Point(center.X - 0.5 * sz.Width, center.Y - 0.5 * sz.Height);
      }

    }
}
