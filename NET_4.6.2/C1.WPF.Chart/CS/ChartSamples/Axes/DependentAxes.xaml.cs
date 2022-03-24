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
  [System.ComponentModel.Description("Multiple Y-axes with different units of measurement")]
  public partial class DependentAxes : UserControl
  {
    public DependentAxes()
    {
      InitializeComponent();

      chart.Data.Children.Add(CreateTestData(new DateTime(2007,3,1), 50));
      chart.View.AxisX.IsTime = true;

      CreateTitle( chart.View.AxisY ,"°C", null);

      Axis axf = new Axis()
        {
          AxisType = AxisType.Y,
          IsDependent = true,
          Foreground = new SolidColorBrush(Colors.Red),
          DependentAxisConverter= (x) =>  x*9/5 + 32
        };
      CreateTitle(axf, "°F", new SolidColorBrush(Colors.Red));
      chart.View.Axes.Add(axf);

      Axis axk = new Axis()
      {
        IsDependent = true,
        Foreground = new SolidColorBrush(Colors.Purple),
        DependentAxisConverter = (x) => x + 273.15
      };
      CreateTitle(axk, "K", new SolidColorBrush(Colors.Purple));
      chart.View.Axes.Add(axk);
    }

    Random rnd = new Random();

    XYDataSeries CreateTestData(DateTime start, int cnt)
    {
      DateTime[] x = new DateTime[cnt];
      double[] y = new double[cnt];

      for (int i = 0; i < cnt; i++)
      {
        x[i] = start.AddDays(i);
        y[i] = -10 + 20 * rnd.NextDouble();
      }

      return new XYDataSeries() { XValuesSource = x, ValuesSource = y };
    }

    void CreateTitle(Axis ax, string text, SolidColorBrush fg)
    {
      TextBlock tb = new TextBlock()
        {
          TextAlignment = TextAlignment.Right,
          Text = text,
          HorizontalAlignment = HorizontalAlignment.Center,
          VerticalAlignment = VerticalAlignment.Bottom,
          Width = 20,
          RenderTransform = new RotateTransform() { Angle = 90 },
          RenderTransformOrigin = new Point(0.5,0.5) // new Point(0.8,1.1),
        };

      if (fg != null)
        tb.Foreground = fg;

      ax.Title = new Border()
      {
        HorizontalAlignment = HorizontalAlignment.Left,
        VerticalAlignment = VerticalAlignment.Top,
        Child = tb,
      };
    }
  }
}
