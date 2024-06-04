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
using C1.Silverlight.Chart.Extended;
#else
using C1.WPF.C1Chart;
using C1.WPF.C1Chart.Extended;
#endif

namespace ChartSamples
{
  [System.ComponentModel.Description("Shows trend lines with its options")]
  public partial class TrendLines : UserControl
  {
    Random rnd = new Random();
    double[] x,y;

    public TrendLines()
    {
      InitializeComponent();

      int cnt = 10;
      x = new double[cnt];
      y = new double[cnt];

      for (int i = 0; i < cnt; i++)
      {
        x[i] = i+1; y[i] = rnd.Next(1,100);
      }

      chart.Data.Children.Add(new XYDataSeries()
      {
        XValuesSource = x,
        ValuesSource = y,
        Label = "points"
      });

      chart.View.AxisX.Min = 0; chart.View.AxisX.Max = cnt + 1;
      chart.View.AxisY.Min = 0; chart.View.AxisY.Max = 100;
      chart.ChartType = ChartType.XYPlot;

      AddTrendLine();
    }

    void AddTrendLine()
    {
      TrendLine tl = new TrendLine()
      {
        XValuesSource = x,
        ValuesSource = y,
        Order = 4,
      };

      chart.Data.Children.Add(tl);

      TrendLineEditor tle = new TrendLineEditor() { Margin = new Thickness(4) };
      tle.DataContext = tl;
      sp.Children.Add(tle);
    }
  }
}
