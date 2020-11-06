using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using C1.WPF;
using C1.WPF.C1Chart;

namespace ChartSamples
{
  /// <summary>
  /// Interaction logic for ZoomChart.xaml
  /// </summary>
  public partial class ZoomChartControl : UserControl
  {
    // ** fields
    double _min = double.NaN, _max = double.NaN;
    double _minRange = 7.0 / 365;
    DispatcherTimer _timer;

    // ** ctor
    public ZoomChartControl()
    {
      InitializeComponent();

      Loaded += new RoutedEventHandler(ZoomChart_Loaded);

      // read data
      CSVData data = new CSVData();
      using (Stream stream = Assembly.GetExecutingAssembly().
          GetManifestResourceStream("ChartSamples.Resources.weatherYear.csv"))
      {
        data.Read(stream, true, false);
      }

      int len = data.Length;
      WeatherData[] wdata = new WeatherData[len];

      double min = double.MaxValue;
      double max = double.MinValue;

      // fill array
      for (int i = 0; i < len; i++)
      {
        wdata[i] = new WeatherData(DateTime.Parse(data[i, 0]),
            double.Parse(data[i, "Max TemperatureF"]),
            double.Parse(data[i, "Mean TemperatureF"]),
            double.Parse(data[i, "Min TemperatureF"]));

        min = Math.Min(min, wdata[i].TMin);
        max = Math.Max(max, wdata[i].TMax);
      }

      _min = wdata[0].DateTime.ToOADate();
      _max = wdata[len - 1].DateTime.ToOADate();

      if (len > 0)
      {
        chart.BeginUpdate();
        CreateDataSeries(chart, wdata);

        chart.ChartType = ChartType.Line;
        chart.View.AxisX.AnnoFormat = "MMM";
        chart.View.AxisX.MinorTickHeight = 0;
        chart.View.AxisX.Min = _min;
        chart.View.AxisX.Max = _max;

        chart.EndUpdate();

        chartZoom.BeginUpdate();
        CreateDataSeries(chartZoom, wdata);
        chartZoom.ChartType = ChartType.Line;
        chartZoom.View.AxisX.AnnoFormat = "dd-MMM";
        chartZoom.View.AxisX.Max = _min + 0.25 * (_max - _min);
        chartZoom.View.AxisX.MinorTickHeight = 0;
        chartZoom.EndUpdate();
      }
    }

    void ZoomChart_Loaded(object sender, RoutedEventArgs e)
    {
      if (Foreground is SolidColorBrush)
      {
        Color clr = ((SolidColorBrush)Foreground).Color;
        slider.Box.Fill = new SolidColorBrush(Color.FromArgb(32, clr.R, clr.G, clr.B));
        slider.Box.Stroke = new SolidColorBrush(Color.FromArgb(200, clr.R, clr.G, clr.B));
      }
      legend.DataContext = chart;
    }

    // ** zooming
    double Minimum
    {
      get { return double.IsNaN(_min) ? chart.View.AxisX.Min : _min; }
      set
      {
        if (value != _min)
        {
          _min = value;
          Update();
        }
      }
    }
    double Maximum
    {
      get { return double.IsNaN(_max) ? chart.View.AxisX.Max : _max; }
      set
      {
        if (value != _max)
        {
          _max = value;
          Update();
        }
      }
    }
    
    // timer update exclude excessive repaints(useful for large data)
    void Update()
    {
      if (_timer == null)
      {
        _timer = new DispatcherTimer() { Interval=TimeSpan.FromSeconds(0.5)};
        _timer.Tick += (s, e) =>
          {
            PerformUpdate();
            _timer.Stop();
          };

      }
      if (!_timer.IsEnabled)
        _timer.Start();
    }
    
    void PerformUpdate()
    {
      chartZoom.BeginUpdate();
      chartZoom.View.AxisX.Min = Minimum;
      chartZoom.View.AxisX.Max = Maximum;
      chartZoom.EndUpdate();
    }

    // ** overrides
    protected override Size MeasureOverride(Size availableSize)
    {
      // adjust margin if necessary
      {
        Thickness mars = chart.View.Margin;
        double w = 0.5 * slider.ThumbWidth;
        mars.Left -= w; mars.Right -= w;
        slider.Margin = mars;
      }

      return base.MeasureOverride(availableSize);
    }

    // ** event handlers
    void slider_LowerValueChanged(object sender, EventArgs e)
    {
      if (chart != null && chartZoom != null)
      {
        if (slider.UpperValue - slider.LowerValue < _minRange)
          slider.LowerValue = slider.UpperValue - _minRange;

        Axis ax = chart.View.AxisX;
        if (ax.Max != ax.Min)
        {
          double min = ax.Min + slider.LowerValue * (ax.Max - ax.Min);
          Minimum = min;
        }
      }
    }
    
    void slider_UpperValueChanged(object sender, EventArgs e)
    {
      if (chart != null && chartZoom != null)
      {
        if (slider.UpperValue - slider.LowerValue < _minRange)
          slider.UpperValue = slider.LowerValue + _minRange;

        Axis ax = chart.View.AxisX;
        if (ax.Max != ax.Min)
        {
          double max = ax.Min + slider.UpperValue * (ax.Max - ax.Min);
          Maximum = max;
        }
      }
    }

    // ** utilities
    void CreateDataSeries(C1Chart chart, WeatherData[] data)
    {
      chart.Data.Children.Clear();

#if DataBinding
      chart.Data.ItemsSource = data;
#else
      int len = data.Length;
      DateTime[] days = new DateTime[len];
      double[] tmin = new double[len];
      double[] tmax = new double[len];
      double[] tavg = new double[len];
      for (int i = 0; i < len; i++)
      {
        days[i] = data[i].DateTime;
        tmin[i] = data[i].TMin;
        tmax[i] = data[i].TMax;
        tavg[i] = data[i].TAvg;
      }
#endif

      XYDataSeries ds = new XYDataSeries(); ds.Label = "Minimum";
#if DataBinding
      ds.ValueBinding = new Binding("TMin");
      ds.XValueBinding = new Binding("DateTime");
#else
      ds.XValuesSource = days;
      ds.ValuesSource = tmin;
#endif
      ds.ConnectionStroke = new SolidColorBrush(Colors.Blue);
      chart.Data.Children.Add(ds);

      ds = new XYDataSeries(); ds.Label = "Maximum";
#if DataBinding
      ds.ValueBinding = new Binding("TMax");
      ds.XValueBinding = new Binding("DateTime");
#else
      ds.XValuesSource = days;
      ds.ValuesSource = tmax;
#endif
      ds.ConnectionStroke = new SolidColorBrush(Colors.Red);
      chart.Data.Children.Add(ds);

      ds = new XYDataSeries(); ds.Label = "Average";
#if DataBinding
      ds.ValueBinding = new Binding("TAvg");
      ds.XValueBinding = new Binding("DateTime");
#else
      ds.XValuesSource = days;
      ds.ValuesSource = tavg;
#endif
      chart.Data.Children.Add(ds);

      foreach (DataSeries ser in chart.Data.Children)
        ser.ConnectionStrokeThickness = 1;

      chart.View.AxisX.IsTime = true;
      chart.View.AxisX.AnnoPosition = AnnoPosition.Near;
    }
  }

  public class ChartSlider : C1RangeSlider
  {
    // ** fields
    Rectangle _box;
    Grid _root;
    double _tw;

    // ** ctor
    public ChartSlider()
    {
      _box = new Rectangle();
      _box.Fill = new SolidColorBrush(Color.FromArgb(32, 255, 255, 255));
      _box.Stroke = new SolidColorBrush(Color.FromArgb(200, 255, 255, 255));
      _box.IsHitTestVisible = false;
      Grid.SetColumn(_box, 2);
    }

    // ** object model
    public double ThumbWidth
    {
      get { return _tw; }
    }

    public Rectangle Box
    {
      get { return _box; }
    }

    // ** overrides
    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      _root = GetTemplateChild("HorizontalTemplate") as Grid;
      if (_root != null)
      {
        if (_root.Children.Count > 2)
        {
          Rectangle r = _root.Children[0] as Rectangle;
          if (r != null)
            r.Visibility = Visibility.Collapsed;
          r = _root.Children[1] as Rectangle;
          if (r != null)
            r.Visibility = Visibility.Collapsed;
        }

        _root.Children.Insert(0, _box);
      }

      Thumb thumb = GetTemplateChild("LowerThumb") as Thumb;
      if (thumb != null)
      {
        _tw = thumb.Width;
        if (double.IsNaN(_tw))
        {
          thumb.Measure(new Size(1000, 1000));
          _tw = thumb.DesiredSize.Width;
        }
        double w = Math.Ceiling(0.5 * _tw);
        _box.Margin = new Thickness(-w, 0, -w, 0);
      }

      Rectangle focus = GetTemplateChild("FocusVisual") as Rectangle;
      if (focus != null)
        focus.Visibility = Visibility.Hidden;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      Thumb thumb = GetTemplateChild("MiddleThumb") as Thumb;
      if (thumb != null)
      {
        thumb.Height = finalSize.Height;
      }
      return base.ArrangeOverride(finalSize);
    }
  }
}
