using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

using C1.WPF.C1Chart;

namespace C1.WPF.C1Chart.Labels
{
  /// <summary>
  /// Represents chart label attached by series and point indices.
  /// </summary>
  public class ChartLabel : ChartLabelBase
  {
    #region object model
    
    /// <summary>
    /// Gets or sets the index of data series.
    /// </summary>
    public int SeriesIndex { get; set; }
    
    /// <summary>
    /// Gets or sets the index of data point in the series.
    /// </summary>
    public int PointIndex { get; set; }

    /// <summary>
    /// Gets or sets data series of the point. If Series = null the data series
    /// with SeriesIndex from Chart.Data.Children collection is used. 
    /// </summary>
    public DataSeries Series { get; set; }
    
    #endregion

    #region implementation
    
    Point DataIndexToPoint(C1Chart chart, int seriesIndex, int pointIndex)
    {
      if (chart.ChartType.ToString().StartsWith("Pie"))
      {
        var slices = chart.FindChildren<PieSlice>();

        foreach (var slice in slices)
        {
          if (slice.DataPoint.SeriesIndex == seriesIndex && slice.DataPoint.PointIndex == pointIndex)
          {
            plotCenter = slice.PieCenter;
            var geom = slice.RenderedGeometry as PathGeometry;
            var arc = geom.Figures[0].Segments[1] as ArcSegment;

            var angle = slice.Angle/180*Math.PI;
            return new Point(slice.PieCenter.X + arc.Size.Width*Math.Cos(angle),
              slice.PieCenter.Y + arc.Size.Height * Math.Sin(angle));
          }
        }
        return new Point();
      }
      else
        return chart.View.DataIndexToPoint(seriesIndex, pointIndex);
    }

    internal override void UpdateAttachPoint(C1Chart chart, Point offset)
    {
      var prect = chart.View.PlotRect;

      DataSeries ds = Series;

      if (ds == null)
      {
        var series = chart.Data.Children;
        if (SeriesIndex >= series.Count)
          return;
        ds = series[SeriesIndex];
      }

      int npts = GetNumberOfPoints(ds);

      // negative means offset from the end
      if (PointIndex < 0)
        PointIndex = npts + PointIndex;

      if (PointIndex < 0 || PointIndex >= npts)
        return;

      Point pt = new Point(double.NaN, double.NaN);

      if (Series == null)
      {
        pt = DataIndexToPoint(chart, SeriesIndex, PointIndex);
        pt.X -= offset.X;
        pt.Y -= offset.Y;
      }
      else
      {
        var vals = ((IDataSeriesInfo)ds).GetValues();
        if (vals != null)
        {
          var x = vals.GetLength(0) >= 2 ? vals[1, PointIndex] : PointIndex;
          var y = vals[0, PointIndex];

          pt = PointFromData(chart, new Point(x, y), ds.AxisX, ds.AxisY, offset);
        }
      }
      
      if (double.IsNaN(pt.X) || double.IsNaN(pt.Y))
        return;

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
