using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace C1.WPF.C1Chart.Labels
{

  /// <summary>
  /// Represent chart label attached by data point.
  /// The label can be dragged with left mouse button.
  /// </summary>
  public class MovableChartLabelPoint : ChartLabelPoint
  {
    bool isMoving = false;
    Point startPt = new Point();

    //public bool Moving { get { return isMoving; } }

    /// <summary>
    /// Creates an instance of MovableChartLabelPoint
    /// </summary>
    public MovableChartLabelPoint()
    {
      IsMovable = false;
      MouseLeftButtonDown += new MouseButtonEventHandler(MovableChartLabelPoint_MouseLeftButtonDown);
      MouseLeftButtonUp += new MouseButtonEventHandler(MovableChartLabelPoint_MouseLeftButtonUp);
      MouseMove += new MouseEventHandler(MovableChartLabelPoint_MouseMove);
    }

    void MovableChartLabelPoint_MouseMove(object sender, MouseEventArgs e)
    {
      if (isMoving && startPt.X != 0 && startPt.Y != 0)
      {
        Offset = (Point)((e.GetPosition(null) - startPt));
        ChartLabels lbls = this.Parent as ChartLabels;
        lbls.ForceArrange();
      }
    }

    void MovableChartLabelPoint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      isMoving = false;
      ReleaseMouseCapture();
      startPt = new Point();
    }

    void MovableChartLabelPoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      isMoving = true;
      e.Handled = true;
      CaptureMouse();
      startPt = e.GetPosition(null);
      if (!double.IsNaN(Offset.X) && !double.IsNaN(Offset.Y))
        startPt.Offset(-Offset.X, -Offset.Y);
    }
  }
}
