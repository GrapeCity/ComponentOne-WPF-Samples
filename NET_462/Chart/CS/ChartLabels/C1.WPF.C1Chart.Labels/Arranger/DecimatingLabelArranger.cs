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
  /// Label layout engine that hides overlapping labels.
  /// </summary>
  public class DecimatingLabelArranger : BaseLabelArranger
  {
    /// <summary>
    /// Whether the engine can hide the labels.
    /// </summary>
    public bool CanHideLabels { get; set; }
    
    /// <summary>
    /// Whether the labels can overlap border.
    /// </summary>
    public bool CanOverlapBorder { get; set; }

    /// <summary>
    /// Creates an instance of DecimationLabelArranger.
    /// </summary>
    public DecimatingLabelArranger()
    {
      CanHideLabels = true;
    }

    /// <summary>
    /// Performs label layout.
    /// </summary>
    /// <param name="labels"></param>
    /// <param name="rectangles"></param>
    /// <param name="border"></param>
    public override void Arrange(IList<ILabel> labels, IList<Rect> rectangles, Rect border)
    {
      var processed = new List<ILabel>();

      foreach (var clbl in labels)
      {
        if (clbl.IsVisible)
        {
          bool hide = false;
          var crect = clbl.RectFromLabel();

          foreach (var lbl in processed)
          {
            var rect = lbl.RectFromLabel();
            rect.Intersect(crect);
            if (!rect.IsEmpty)
            {
              hide = true; break;
            }
          }

          if (!hide && !CanOverlapBorder)
          {
            if (!Contains(border, crect))
              hide = true;
          }

          if (hide)
            HideLabel(clbl);

          if (clbl.IsVisible)
            processed.Add(clbl);
        }
      }
    }

    void HideLabel(ILabel label)
    {
      if (CanHideLabels)
        label.IsVisible = false;
    }
  }
}
