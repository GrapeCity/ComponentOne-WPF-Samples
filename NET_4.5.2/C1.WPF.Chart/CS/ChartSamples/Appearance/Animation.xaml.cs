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
using C1.WPF.C1Chart;


namespace ChartSamples
{
  [System.ComponentModel.Description("Shows how to provide a transitional animation when loading new data")]
  public partial class Animation : UserControl
  {
    Dictionary<string, PlotElement> _dict = new Dictionary<string, PlotElement>();

    public Animation()
    {
      InitializeComponent();

      chart.View.AxisY.Min = 0;
      chart.View.AxisY.Max = 100;
      BarColumnOptions.SetRadiusX(chart, 2);
      BarColumnOptions.SetRadiusY(chart, 2);
      chart.ChartType = ChartType.Column;

      NewData();
    }

    Random rnd = new Random();

    void NewData()
    {
      // create new random data values
      int cnt = 12;
      double[] vals = new double[cnt];
      for (int i = 0; i < cnt; i++)
        vals[i] = rnd.Next(10, 100);

      DataSeries ds;
      if (chart.Data.Children.Count == 0)
      {
        ds = new DataSeries();
        ds.PlotElementLoaded += new EventHandler(ds_PlotElementLoaded);
        chart.Data.Children.Add(ds);
      }
      chart.Data.Children[0].ValuesSource = vals;
     }

    void ds_PlotElementLoaded(object sender, EventArgs e)
    {
      var pe = (PlotElement)sender;
      string key = GetKey(pe);
      if (_dict.ContainsKey(key))
      {
        var pe0 = _dict[key];

#if (SILVERLIGHT)
        Rect rc0 = ((Path)pe0.Shape).Data.Bounds;
        Rect rc = ((Path)pe.Shape).Data.Bounds;
#else
        Rect rc0 = pe0.RenderedGeometry.Bounds;
        Rect rc = pe.RenderedGeometry.Bounds;
#endif

        if (rc0 != rc) // different values
        {
          // transform to fit into the previous value
          ScaleTransform st = new ScaleTransform()
          {
            ScaleX = rc0.Width / rc.Width,
            ScaleY = rc0.Height / rc.Height
          };
          pe.RenderTransform = st;
          pe.RenderTransformOrigin = new Point(0,1);

          // animate to the current value
          DoubleAnimation da1 = new DoubleAnimation() { To = 1.0 };
          DoubleAnimation da2 = new DoubleAnimation() { To = 1.0 };

#if (SILVERLIGHT)
          Storyboard sb = new Storyboard() {Duration = new Duration(TimeSpan.FromSeconds(3)) };
          Storyboard.SetTarget(da1, st);
          Storyboard.SetTargetProperty(da1, new PropertyPath("ScaleX"));
          sb.Children.Add(da1);
          Storyboard.SetTarget(da2, st);
          Storyboard.SetTargetProperty(da2, new PropertyPath("ScaleY"));
          sb.Children.Add(da2);
          sb.Begin();
          st.BeginAnimation(ScaleTransform.ScaleXProperty, da1);
          st.BeginAnimation(ScaleTransform.ScaleYProperty, da2);
#else
          st.BeginAnimation(ScaleTransform.ScaleXProperty, da1);
          st.BeginAnimation(ScaleTransform.ScaleYProperty, da2);
#endif
        }
      }
      // store the previous element into the dictionary
      _dict[key] = pe;
    }

    static string GetKey(PlotElement pe)
    {
      return string.Format("s{0}p{1}", pe.DataPoint.SeriesIndex, pe.DataPoint.PointIndex);
    }

    private void btnNew_Click(object sender, RoutedEventArgs e)
    {
      NewData();
    }
  }
}
