using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

#if (SILVERLIGHT)
using C1.Silverlight.Chart;
using C1.Silverlight.Chart.Extended;
#else
using C1.WPF.C1Chart;
using C1.WPF.C1Chart.Extended;
#endif

namespace ChartSamples
{
  [System.ComponentModel.Description("Live line chart with automatic calculation of min/max/avarage values")]
  public partial class Dynamic : UserControl
  {
    ObservableCollection<Point> _pts = new ObservableCollection<Point>();
    int counter = 0;
    Random rnd = new Random();
    DispatcherTimer dt;
    int nMaxPoints = 60;
    int nAddPoints = 1;
    TrendLine _tlmin, _tlmax, _tlavg;

    public Dynamic()
    {
      InitializeComponent();

      chart.Data.ItemsSource = _pts;
      chart.ChartType = ChartType.Line;

      XYDataSeries ds = new XYDataSeries()
      {
        XValueBinding = new Binding("X"),
        ValueBinding = new Binding("Y"),
        ConnectionStrokeThickness = 2,
        Label = "raw",
//        ConnectionStroke = new SolidColorBrush(Colors.DarkGray)
      };

      chart.Data.Children.Add(ds);

      _tlmin = new TrendLine() 
      { 
        FitType = FitType.MinY,
        XValueBinding = new Binding("X"),
        ValueBinding = new Binding("Y"),
        Label = "min",
//        ConnectionStroke = new SolidColorBrush(Colors.Blue)
      };

      _tlmax = new TrendLine()
      {
        FitType = FitType.MaxY,
        XValueBinding = new Binding("X"),
        ValueBinding = new Binding("Y"),
        Label = "max",
//        ConnectionStroke = new SolidColorBrush(Colors.Red)
      };

      _tlavg = new TrendLine()
      {
        FitType = FitType.AverageY,
        XValueBinding = new Binding("X"),
        ValueBinding = new Binding("Y"),
        Label = "avg",
//        ConnectionStroke = new SolidColorBrush(Colors.Green)
      };

      chart.View.AxisY.Min = -1000;
      chart.View.AxisY.Max = 1000;

      dt = new DispatcherTimer() 
        { Interval = TimeSpan.FromSeconds(0.2) };
      dt.Tick += (s, e) => Update();
      dt.Start();
    }

    void Update()
    {
      chart.BeginUpdate();

      int cnt = nAddPoints;
      for (int i = 0; i < cnt; i++)
      {
        double r = rnd.NextDouble();
        double y = (10 * r * Math.Sin(0.1 * counter) * Math.Sin(0.6 * rnd.NextDouble() * counter));
        _pts.Add(new Point(counter++, y*100));
      }

      int ndel = _pts.Count - nMaxPoints;
      if (ndel > 0)
        for (int i = 0; i < ndel; i++)
          _pts.RemoveAt(0);

      chart.EndUpdate();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      Button btn = (Button)sender;

      if (dt.IsEnabled)
      {
        dt.Stop();
        btn.Content = "Start";
      }
      else
      {
        dt.Start();
        btn.Content = "Stop";
      }
    }

    internal void StopTimer()
    {
      if (dt.IsEnabled)
      {
        dt.Stop();
        btnTimer.Content = "Start";
      }
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
      CheckBox cb = (CheckBox)sender;
      if( cb.Content as string == "Minimum")
        chart.Data.Children.Add( _tlmin);
      else if (cb.Content as string == "Maximum")
        chart.Data.Children.Add(_tlmax);
      else if (cb.Content as string == "Average")
        chart.Data.Children.Add(_tlavg);
    }

    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
      CheckBox cb = (CheckBox)sender;
      if (cb.Content as string == "Minimum")
        chart.Data.Children.Remove(_tlmin);
      else if (cb.Content as string == "Maximum")
        chart.Data.Children.Remove(_tlmax);
      else if (cb.Content as string == "Average")
        chart.Data.Children.Remove(_tlavg);
    }
  }
}
