using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Data;
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
  [System.ComponentModel.Description("Interactive line chart with 50K data points")]
  public partial class LargeData : UserControl
  {
    public LargeData()
    {
      InitializeComponent();

      LineAreaOptions.SetOptimizationRadius(chart, 2);
      chart.Actions.Add(new ZoomAction());
      CreateChart();
    }

    void CreateChart()
    {
      chart.BeginUpdate();
      int cnt = 50000;
      double[] x = new double[cnt];
      double[] y = new double[cnt];

      for (int i = 0; i < cnt; i++)
      {
        x[i] = i;
        y[i] = 10*Math.Sin(0.001 * i) + Math.Cos(0.03 * i);
      }

      XYDataSeries ds = new XYDataSeries()
      {
        XValuesSource = x,
        ValuesSource = y,
        ConnectionStrokeThickness = 1
      };

      chart.Data.Children.Add(ds);
      chart.ChartType = ChartType.Line;

      chart.View.AxisX.ScrollBar = new AxisScrollBar();
      //chart.View.AxisX.Scale = 0.1;

      chart.View.AxisY.MinScale = 1;

      SetScale(0.25);
      chart.EndUpdate();
    }

    private void OriginalSize_Click(object sender, RoutedEventArgs e)
    {
      SetScale(1.0);
    }

    private void ZoomIn_Click(object sender, RoutedEventArgs e)
    {
      SetScale(chart.View.AxisX.Scale * 0.5);
    }

    private void ZoomOut_Click(object sender, RoutedEventArgs e)
      {
      SetScale( chart.View.AxisX.Scale * 2);
      }

    private void SetScale(double scale)
    {
      chart.View.AxisX.Scale = scale;
      btn1_1.IsEnabled = scale != 1;
      btnZoomIn.IsEnabled = scale > 0.002;
      btnZoomOut.IsEnabled = scale < 1;
    }
  }
}
