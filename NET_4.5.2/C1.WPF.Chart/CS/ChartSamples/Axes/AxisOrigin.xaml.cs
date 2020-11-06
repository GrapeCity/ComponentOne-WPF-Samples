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
  [System.ComponentModel.Description("Controls the axis position with its origin")]
  public partial class AxisOrigin : UserControl
  {
    public AxisOrigin()
    {
      InitializeComponent();

      chart.View.AxisX.Position = AxisPosition.OverData;
      chart.View.AxisY.Position = AxisPosition.OverData;
      chart.View.AxisY.Min = -1;
      chart.View.AxisY.Max = 1;
      
      chart.View.AxisX.MajorGridFill = new SolidColorBrush(Colors.LightGray) { Opacity = 0.1 };
      chart.View.AxisY.MajorGridFill = new SolidColorBrush(Colors.LightGray) { Opacity = 0.1 };

      XYDataSeries ds = Function.CreateDataSeries(0, Math.PI, 200,
        (val) => Math.Cos(7 * val) * Math.Cos(val),
        (val) => Math.Cos(7 * val) * Math.Sin(val), null);
      ds.ConnectionStrokeThickness = 4;
//      ds.ConnectionStroke = new SolidColorBrush(Colors.Red);
      chart.Data.Children.Add(ds);
    }

    private void vs_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      if( chart!=null )
        chart.View.AxisX.Origin = 1 - 2 * 0.01 * vs.Value; 
    }

    private void hs_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      if (chart != null)
        chart.View.AxisY.Origin = -1 + 2 * 0.01 * hs.Value;
    }
  }
}
