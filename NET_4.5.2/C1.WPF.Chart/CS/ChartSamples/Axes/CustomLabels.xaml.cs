using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#if (SILVERLIGHT)
using C1.Silverlight.Chart;
#else
using C1.WPF.C1Chart;
#endif

namespace ChartSamples
{
  [System.ComponentModel.Description("Custom axis labels")]
  public partial class CustomLabels : UserControl
  {
    public CustomLabels()
    {
      InitializeComponent();

      if(! DesignerProperties.GetIsInDesignMode(this))
        chart.Loaded += new RoutedEventHandler(chart_Loaded);
    }

    void chart_Loaded(object sender, RoutedEventArgs e)
    {
      // read data from resource
      CSVData data = new CSVData();

      using (Stream stream = Assembly.GetExecutingAssembly().
        GetManifestResourceStream("ChartSamples.Resources.weather.csv"))
      {
        data.Read(stream, true, false);
      }

      int len = data.Length;
      DateTime[] dts = new DateTime[len];
      double[] t = new double[len];
      Brush red = new SolidColorBrush(Colors.Red);
      Brush blue = new SolidColorBrush(Colors.Blue);

      DateTime day = new DateTime(), dtmin = new DateTime(), dtmax = new DateTime();
      double tmax = double.MinValue, tmin = double.MaxValue;

      // collection for min/max values axis source
      List<KeyValuePair<object, double>> kvals = new List<KeyValuePair<object, double>>();

      // fill up the time and temperature arrays
      // and calculate daily min/max
      for (int i = 0; i < len; i++)
      {
        dts[i] = DateTime.Parse(data[i, "date"] + " " + data[i, "time"],CultureInfo.InvariantCulture);
        t[i] = double.Parse(data[i, "T"],CultureInfo.InvariantCulture);

        if (i == 0)
        {
          day = dts[i].Date; dtmax = dtmin = dts[i]; tmax = tmin = t[i];
        }

        if (dts[i].Date == day)
        {
          if (t[i] > tmax)
          {
            tmax = t[i];
            dtmax = dts[i];
          }
          if (t[i] < tmin)
          {
            tmin = t[i];
            dtmin = dts[i];
          }
        }
        else
        {
          kvals.Add(new KeyValuePair<object, double>(
              new TextBlock() { Text = "low", Foreground = blue }, dtmin.ToOADate()));
          kvals.Add(new KeyValuePair<object, double>(
              new TextBlock() { Text = "high", Foreground = red }, dtmax.ToOADate()));
          day = dts[i].Date; dtmax = dtmin = dts[i]; tmax = tmin = t[i];
        }
      }

      chart.BeginUpdate();

      // create data series
      XYDataSeries ds = new XYDataSeries()
      {
        XValuesSource = dts,
        ValuesSource = t,
        ConnectionStrokeThickness = 2,
//        ConnectionStroke = new SolidColorBrush(Colors.Green)
      };
      chart.Data.Children.Add(ds);
      chart.ChartType = ChartType.Line;

      double xsc = 0.05;

      // main x-axis for time
      Axis axx = chart.View.AxisX;
      axx.Min = dts[0].ToOADate();
      axx.Max = dts[len - 1].ToOADate();
      axx.Value = 0;
      axx.Scale = xsc;
      axx.IsTime = true;
      axx.MajorUnit = 0.25;
      axx.AnnoFormat = "HH";
      axx.MajorGridStroke = null;
      axx.AnnoTemplate = new TextBlock() { FontSize = 8, Foreground=Foreground};
      axx.Foreground = Foreground;
      axx.ScrollBar = new AxisScrollBar();

      // auxiliary x-axis for dates
      Axis ax = new Axis()
      {
        AxisType = AxisType.X,
        AnnoFormat = "d MMM",
        AnnoPosition = AnnoPosition.Near,
        IsDependent = true,
        MajorUnit = 1,
        IsTime = true,
        MajorGridStroke = Foreground,
        MajorGridStrokeThickness = 1.5
      };
      chart.View.Axes.Add(ax);

      // create auxiliary x-axis for daily min/max
      ax = new Axis()
      {
        AxisType = AxisType.X,
        Position = AxisPosition.Far,
        IsDependent = true,
        //AnnoAngle = -90,
        ItemsSource = kvals,
        MajorGridStroke = new SolidColorBrush(Colors.LightGray),
        MajorGridStrokeThickness = 1,
        MajorGridStrokeDashes = new DoubleCollection() { 1, 2 },
      };
      chart.View.Axes.Add(ax);

      chart.EndUpdate();

      chart.Loaded -= new RoutedEventHandler(chart_Loaded);
    }
  }
}
