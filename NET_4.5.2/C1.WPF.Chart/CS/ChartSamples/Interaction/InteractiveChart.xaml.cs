using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
  [System.ComponentModel.Description("Interactive chart which uses built-in zooming and panning features")]
  public partial class InteractiveChart : UserControl
  {
//    Color[] _clrs = new Color[] { Colors.Red, Colors.Green, Colors.Blue };

    public InteractiveChart()
    {
      InitializeComponent();

      Loaded += new RoutedEventHandler(InteractiveChart_Loaded);

//      c1chart.View.PlotBackground = new SolidColorBrush(Color.FromArgb(255,80,80,80));

      c1chart.ActionEnter += new EventHandler(Actions_Enter);
      c1chart.ActionLeave += new EventHandler(Actions_Leave);

      CreataSampleData(3, 200);

      UpdateScrollbars();
    }

    void InteractiveChart_Loaded(object sender, RoutedEventArgs e)
    {
      rbX.Foreground = Foreground;
      rbY.Foreground = Foreground;
      rbXY.Foreground = Foreground;
    }

    void Actions_Leave(object sender, EventArgs e)
    {
      c1chart.Cursor = Cursors.Arrow;

      UpdateScrollbars();
    }

    void Actions_Enter(object sender, EventArgs e)
    {
      if( sender is ScaleAction)
        c1chart.Cursor = Cursors.SizeNS;
      else if (sender is TranslateAction)
#if (SILVERLIGHT)
      c1chart.Cursor = Cursors.Stylus;
#else
      c1chart.Cursor = Cursors.SizeAll;
#endif
      else
        c1chart.Cursor = Cursors.Hand;
    }

    Random rnd = new Random();

    void CreataSampleData(int nser, int npts)
    {
      c1chart.BeginUpdate();
      DataSeriesCollection sc = c1chart.Data.Children;
      sc.Clear();

      for (int iser = 0; iser < nser; iser++)
      {
        double phase = 0.05 + 0.1 * rnd.NextDouble();
        double amp = 1 + 5 * rnd.NextDouble();

        DataSeries ds = new DataSeries();
        double[] vals = new double[npts];
        for (int i = 0; i < npts; i++)
          vals[i] = amp * Math.Sin(i * phase);

        ds.ValuesSource = vals;
        ds.Label = "S " + iser.ToString();

//        ds.ConnectionStroke = new SolidColorBrush(_clrs[iser%3]);
        ds.ConnectionStrokeThickness = 2;

        sc.Add(ds);
      }
      c1chart.EndUpdate();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      c1chart.BeginUpdate();
      c1chart.View.AxisX.Scale = 1;
      c1chart.View.AxisX.Value = 0.5;
      c1chart.View.AxisY.Scale = 1;
      c1chart.View.AxisY.Value = 0.5;

      UpdateScrollbars();
      c1chart.EndUpdate();
    }

    private void rb_Checked(object sender, RoutedEventArgs e)
    {
      if (c1chart == null)
        return;

      c1chart.BeginUpdate();
      
      if (sender == rbX)
      {
        // limit scale of Y-axis
        c1chart.View.AxisY.MinScale = 1;
        c1chart.View.AxisY.Scale = 1;
        c1chart.View.AxisY.Value = 0.5;
        c1chart.View.AxisX.MinScale = 0.001;
      }
      else if (sender == rbY)
      {
        // limit scale of X-axis
        c1chart.View.AxisX.MinScale = 1;
        c1chart.View.AxisX.Scale = 1;
        c1chart.View.AxisX.Value = 0.5;
        c1chart.View.AxisY.MinScale = 0.001;
      }
      else
      {
        // allow smaller scales for both axes
        c1chart.View.AxisX.MinScale = 0.001;
        c1chart.View.AxisY.MinScale = 0.001;
      }

      UpdateScrollbars();
      c1chart.EndUpdate();
    }

    private void UpdateScrollbars()
    {
      double sx = c1chart.View.AxisX.Scale;
      AxisScrollBar sbx = (AxisScrollBar)c1chart.View.AxisX.ScrollBar;
      if (sx >= 1.0)
        sbx.Visibility = Visibility.Collapsed;
      else
        sbx.Visibility = Visibility.Visible;

      double sy = c1chart.View.AxisY.Scale;
      AxisScrollBar sby = (AxisScrollBar)c1chart.View.AxisY.ScrollBar;
      if (sy >= 1.0)
        sby.Visibility = Visibility.Collapsed;
      else
        sby.Visibility = Visibility.Visible;
    }
  }
}