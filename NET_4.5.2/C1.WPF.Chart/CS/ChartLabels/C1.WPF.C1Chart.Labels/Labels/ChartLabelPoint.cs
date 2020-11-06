using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace C1.WPF.C1Chart.Labels
{
  /// <summary>
  /// Represents chart label attached by data coordinates.
  /// </summary>
  public class ChartLabelPoint : ChartLabelBase
  {
    #region object model

    /// <summary>
    /// Gets or sets the point in data coordinates.
    /// </summary>
    public Point DataPoint { get; set; }

    /// <summary>
    /// Gets or sets the x-axis for the point label.
    /// </summary>
    public string AxisX { get; set; }

    /// <summary>
    /// Gets or sets the y-axis for the point label.
    /// </summary>
    public string AxisY { get; set; }

    #endregion

    #region implementation

    internal override void UpdateAttachPoint(C1Chart chart, Point offset)
    {
      var prect = chart.View.PlotRect;

      var pt = PointFromData(chart, DataPoint, AxisX, AxisY, offset);

      if (double.IsNaN(pt.X) || double.IsNaN(pt.Y))
        return;

      if (Offset.X != 0 || Offset.Y != 0)
      {
        Canvas.SetLeft(this, pt.X + Offset.X);
        Canvas.SetTop(this, pt.Y + Offset.Y);
      }

      if (AttachPoint.X != 0 && AttachPoint.Y != 0)
      {
        var dx = Canvas.GetLeft(this) - AttachPoint.X;
        var dy = Canvas.GetTop(this) - AttachPoint.Y;
        Canvas.SetLeft(this, pt.X + dx);
        Canvas.SetTop(this, pt.Y + dy);
      }
      else
        SetPosition(pt, Offset);

      AttachPoint = pt;
    }

    #endregion
  }
}
