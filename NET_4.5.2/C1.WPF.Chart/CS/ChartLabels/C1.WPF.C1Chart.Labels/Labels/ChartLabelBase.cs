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
  /// Abstract base class for chart label
  /// </summary>
  public abstract class ChartLabelBase : ContentControl, ILabel
  {
    #region fields

    static Point EmptyPoint = new Point(double.NaN, double.NaN);
    Line _line;

    /// <summary>
    /// Store plot center.
    /// </summary>
    protected Point plotCenter = EmptyPoint;

    #endregion

    #region .ctor

    /// <summary>
    /// Creates an instance of ChartLabelBase.
    /// </summary>
    public ChartLabelBase()
    {
      DefaultStyleKey = typeof(ChartLabelBase);
      IsMovable = true;

      SetBinding(ChartLabelBase.ConnectionStrokeProperty, new Binding("Foreground") { Source = this });

      _line = new Line();

      _line.SetBinding(Line.StrokeProperty, new Binding("ConnectionStroke") { Source = this });
      _line.SetBinding(Line.StrokeDashArrayProperty, new Binding("ConnectionStrokeDashArray") { Source = this });
      _line.SetBinding(Line.StrokeThicknessProperty, new Binding("ConnectionStrokeThickness") { Source = this });

      _line.SetBinding(Line.VisibilityProperty, new Binding("Visibility") { Source = this });

      Offset = EmptyPoint;
    }

    #endregion

    #region implementation

    internal Point AttachPoint { get; set; }

    internal Line ConnectingLine
    {
      get { return _line; }
    }

    internal abstract void UpdateAttachPoint(C1Chart chart, Point offset);

    static bool IsEmpty(Point pt)
    {
      return double.IsNaN(pt.X) && double.IsNaN(pt.Y);
    }

    /// <summary>
    /// Sets the position of label.
    /// </summary>
    /// <param name="pt"></param>
    /// <param name="offset"></param>
    protected void SetPosition(Point pt, Point offset)
    {
      Point pos = pt;

      if (!IsEmpty(offset) && offset.X != 0 && offset.Y != 0)
      {
        var sz = ((ILabel)this).Size;

        if (offset.X > 0)
          pos.X += offset.X;
        else if (offset.X < 0)
          pos.X -= -offset.X + sz.Width;
        else
          pos.X -= 0.5 * sz.Width;

        if (offset.Y > 0)
          pos.Y += offset.Y;
        else if (offset.Y < 0)
          pos.Y -= -offset.Y + sz.Height;
        else
          pos.Y -= 0.5 * sz.Height;
      }

      Canvas.SetLeft(this, pos.X);
      Canvas.SetTop(this, pos.Y);
    }

    /// <summary>
    /// Gets the number of data points in the series.
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    protected int GetNumberOfPoints(DataSeries ds)
    {
      var vals = ((IDataSeriesInfo)ds).GetValues();
      if (vals != null)
        return vals.GetLength(1);
      return 0;
    }

    /// <summary>
    /// Performs coordinate conversion from data to chart view.
    /// </summary>
    /// <param name="chart"></param>
    /// <param name="point"></param>
    /// <param name="axisX"></param>
    /// <param name="axisY"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    protected static Point PointFromData(C1Chart chart, Point point, string axisX, string axisY, Point offset)
    {
      var prect = chart.View.PlotRect;
      double x = double.NaN;
      bool hasx = false;
      if (!string.IsNullOrEmpty(axisX))
      {
        var axx = chart.View.Axes[axisX];
        if (axx != null)
        {
          var ptx = axx.PointFromData(point);
          x = ptx.X - prect.X; hasx = true;
        }
      }

      double y = double.NaN;
      bool hasy = false;
      if (!string.IsNullOrEmpty(axisY))
      {
        var axy = chart.View.Axes[axisY];
        if (axy != null)
        {
          var pty = axy.PointFromData(point);
          y = pty.Y - prect.Y; hasy = true;
        }
      }

      var pt = chart.View.PointFromData(point);

      pt.X -= offset.X;
      pt.Y -= offset.Y;

      if (hasx)
        pt.X = x;
      if (hasy)
        pt.Y = y;

      return pt;
    }

    #endregion

    #region object model

    /// <summary>
    /// Gets or set whether the label can be moved during layout.
    /// </summary>
    public bool IsMovable
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the corner radius of label border.
    /// </summary>
    public CornerRadius CornerRadius
    {
      get { return (CornerRadius)GetValue(CornerRadiusProperty); }
      set { SetValue(CornerRadiusProperty, value); }
    }

    /// <summary>
    /// Gets or set the default offset of label from the attached point.
    /// </summary>
    public Point Offset
    {
      get;
      set;
    }

    /// <summary>
    /// Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ChartLabelBase));

    /// <summary>
    /// Gets or sets the stroke brush of connecting line. 
    /// </summary>
    public Brush ConnectionStroke
    {
      get { return (Brush)GetValue(ConnectionStrokeProperty); }
      set { SetValue(ConnectionStrokeProperty, value); }
    }

    /// <summary>
    /// Using a DependencyProperty as the backing store for ConnectionStroke.  This enables animation, styling, binding, etc... 
    /// </summary>
    public static readonly DependencyProperty ConnectionStrokeProperty =
        DependencyProperty.Register("ConnectionStroke", typeof(Brush), typeof(ChartLabelBase));

    /// <summary>
    /// Gets or sets the stroke thickness of connecting line.
    /// </summary>
    public double ConnectionStrokeThickness
    {
      get { return (double)GetValue(ConnectionStrokeThicknessProperty); }
      set { SetValue(ConnectionStrokeThicknessProperty, value); }
    }

    /// <summary>
    /// Using a DependencyProperty as the backing store for ConnectionStrokeThickness.  This enables animation, styling, binding, etc... 
    /// </summary>
    public static readonly DependencyProperty ConnectionStrokeThicknessProperty =
        DependencyProperty.Register("ConnectionStrokeThickness", typeof(double), typeof(ChartLabelBase), new PropertyMetadata(1.0));

    /// <summary>
    /// Gets or sets the dash pattern of connecting line.
    /// </summary>
    public DoubleCollection ConnectionStrokeDashArray
    {
      get { return (DoubleCollection)GetValue(ConnectionStrokeDashArrayProperty); }
      set { SetValue(ConnectionStrokeDashArrayProperty, value); }
    }

    /// <summary>
    /// Using a DependencyProperty as the backing store for ConnectionStrokeDashArray.  This enables animation, styling, binding, etc...
    /// </summary>
    public static readonly DependencyProperty ConnectionStrokeDashArrayProperty =
        DependencyProperty.Register("ConnectionStrokeDashArray", typeof(DoubleCollection), typeof(ChartLabelBase));

    #endregion

    #region ILabel implementation

    Point ILabel.Point
    {
      get { return AttachPoint; }
    }

    Point ILabel.Position
    {
      get
      {
        return new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
      }
      set
      {
        Canvas.SetLeft(this, value.X);
        Canvas.SetTop(this, value.Y);
      }
    }

    Size ILabel.Size
    {
      get
      {
        this.Measure(new Size(10000, 10000));
        return this.DesiredSize;
      }
    }

    Point ILabel.PlotCenter
    {
      get { return plotCenter; }
    }

    bool ILabel.IsVisible
    {
      get
      {
        return Visibility == Visibility.Visible;
      }
      set
      {
        Visibility = value ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    #endregion
  }
}
