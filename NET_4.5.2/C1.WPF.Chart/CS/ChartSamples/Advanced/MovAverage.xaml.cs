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
  [System.ComponentModel.Description("Shows moving average trend")]
  public partial class MovAverage : UserControl
  {
    Random rnd = new Random();

    public MovAverage()
    {
      InitializeComponent();

      int cnt = 200;

      double[] x = new double[cnt];
      double[] y = new double[cnt];

      for (int i = 0; i < cnt; i++)
      {
        x[i] = i; y[i] = rnd.Next(100);
      }

      chart.Data.Children.Add(new XYDataSeries()
      {
        XValuesSource = x,
        ValuesSource = y,
        Label = "RawData",
        ConnectionStrokeThickness=1,
//        ConnectionStroke = new SolidColorBrush(Colors.LightGray),
      });

      chart.Data.Children.Add(new MovingAverage()
      {
        XValuesSource = x,
        ValuesSource = y,
        Label = "MovAvg 10",
        Period = 10,
//        ConnectionStroke = new SolidColorBrush(Colors.Green),
      });

      chart.Data.Children.Add(new MovingAverage()
      {
        XValuesSource = x,
        ValuesSource = y,
        Label = "MovAvg 40",
        Period = 40,
//        ConnectionStroke = new SolidColorBrush(Colors.Red),
      });

      chart.ChartType = ChartType.Line;

      maEditor1.DataContext = chart.Data.Children[1];
      maEditor2.DataContext = chart.Data.Children[2];
    }
  }
}
