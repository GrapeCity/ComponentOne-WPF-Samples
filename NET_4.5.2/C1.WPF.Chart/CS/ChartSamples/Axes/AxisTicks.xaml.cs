using System;
using System.ComponentModel;
using System.Collections;
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
  [System.ComponentModel.Description("Axis ticks customization")]
  public partial class AxisTicks : UserControl
  {
    public AxisTicks()
    {
      InitializeComponent();

      XYDataSeries ds = Function.CreateDataSeries(0, 5, 200,
        (val) => Math.Cos(4 * val) * Math.Sin(val), null);

      ds.ConnectionStrokeThickness = 3;
      //ds.ConnectionStroke = new SolidColorBrush(Colors.Green);
      chart.Data.Children.Add(ds);

      chart.View.AxisX.MajorTickHeight = 6;
      chart.View.AxisX.MinorTickHeight = 4;
      chart.View.AxisY.MajorTickHeight = 6;
      chart.View.AxisY.MinorTickHeight = 4;

      tex.DataContext = chart.View.AxisX;
      tey.DataContext = chart.View.AxisY;
    }
  }
}
