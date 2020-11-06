using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
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
  [System.ComponentModel.Description("Shows various options of logarithmic chart axes")]
  public partial class LogAxes : UserControl
  {
    public LogAxes()
    {
      InitializeComponent();

      templ.Visibility = Visibility.Collapsed;
      templ.Checked += (s, a) => { UpdateTemplate(chart.View.AxisX); UpdateTemplate(chart.View.AxisY); };
      templ.Unchecked += (s, a) => { UpdateTemplate(chart.View.AxisX); UpdateTemplate(chart.View.AxisY); };

      xbase.ItemsSource = new string[] { "None", "e", "10" };
      xbase.Tag = chart.View.AxisX;
      xbase.SelectionChanged += new SelectionChangedEventHandler(base_SelectionChanged);
      xbase.SelectedIndex = 2;

      ybase.ItemsSource = new string[] { "None", "e", "10" };
      ybase.Tag = chart.View.AxisY;
      ybase.SelectionChanged += new SelectionChangedEventHandler(base_SelectionChanged);
      ybase.SelectedIndex = 2;

      templ.IsChecked = true;

      chart.ChartType = ChartType.LineSmoothed;

      // create data series
      chart.Data.Children.Add(Function.CreateDataSeries(0.01, 10, 100, (val) => Math.Log10(val), "y(x)=log(x)"));
      chart.Data.Children.Add(Function.CreateDataSeries(0.01, 10, 100, (val) => val, "y(x)=x"));
      chart.Data.Children.Add(Function.CreateDataSeries(0.01, 1, 100, (val) => Math.Pow(10, val), "y(x)=10^x"));

      // minor grid
      chart.View.AxisX.MinorUnit = 1;
      chart.View.AxisX.MinorGridStroke = new SolidColorBrush(Colors.LightGray);
      chart.View.AxisX.MinorGridStrokeThickness = 0.5;

      // major grid
      chart.View.AxisX.MajorGridStroke = new SolidColorBrush(Colors.DarkGray);
      chart.View.AxisX.MajorGridStrokeDashes = null;

      chart.View.AxisY.MajorGridStroke = new SolidColorBrush(Colors.DarkGray);
      chart.View.AxisY.MajorGridStrokeDashes = null;
    }

    void base_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ComboBox cb = (ComboBox)sender;
      Axis ax = cb.Tag as Axis;
      if (ax != null)
      {
        switch (cb.SelectedIndex)
        {
          case 0:
            ax.LogBase = double.NaN;
            break;
          case 1:
            ax.LogBase = Math.E;
            break;
          case 2:
            ax.LogBase = 10;
            break;
        }

        UpdateTemplate(ax);
      }
    }

    void UpdateTemplate(Axis ax)
    {
      if (!double.IsNaN(ax.LogBase) && templ.IsChecked == true)
        ax.AnnoTemplate = Resources["ax"];
      else
        ax.AnnoTemplate = null;

      if (double.IsNaN(chart.View.AxisX.LogBase) && double.IsNaN(chart.View.AxisY.LogBase))
        templ.Visibility = Visibility.Collapsed;
      else
        templ.Visibility = Visibility.Visible;
    }
  }

  public static class Function
  {
    public static XYDataSeries CreateDataSeries(double xmin, double xmax, int npts, Func<double, double> func, string label)
    {
      double[] x = new double[npts];
      double[] y = new double[npts];

      for (int i = 0; i < npts; i++)
      {
        x[i] = xmin + (xmax - xmin) * i / (npts - 1);
        y[i] = func(x[i]);
      }

      return new XYDataSeries() { ValuesSource = y, XValuesSource = x, Label = label };
    }

    public static XYDataSeries CreateDataSeries(double tmin, double tmax, int npts,
      Func<double, double> xfunc, Func<double, double> yfunc, string label)
    {
      double[] x = new double[npts];
      double[] y = new double[npts];

      for (int i = 0; i < npts; i++)
      {
        double t = tmin + (tmax - tmin) * i / (npts - 1);
        x[i] = xfunc(t);
        y[i] = yfunc(t);
      }

      return new XYDataSeries() { ValuesSource = y, XValuesSource = x, Label = label };
    }
  }
}
