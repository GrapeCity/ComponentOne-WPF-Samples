using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using C1.WPF.C1Chart;

namespace ChartSamples
{
  /// <summary>
  /// Interaction logic for MultipleAxes.xaml
  /// </summary>

    public partial class MultipleAxes : System.Windows.Controls.UserControl
  {
    public MultipleAxes()
    {
      InitializeComponent();

      // read data
      CSVData data = new CSVData();
      using (Stream stream = Assembly.GetExecutingAssembly().
          GetManifestResourceStream("ChartSamples.Resources.weatherYear.csv"))
      {
          data.Read(stream, true, true);
      }

      int len = data.Length;
      WeatherData[] wdata = new WeatherData[len];

      double min = double.MaxValue;
      double max = double.MinValue;

      // fill the array
      for (int i = 0; i < len; i++)
      {
        wdata[i] = new WeatherData(DateTime.Parse(data[i, 0]),
              double.Parse(data[i, "Max TemperatureF"]),
              double.Parse(data[i, "Mean TemperatureF"]),
              double.Parse(data[i, "Min TemperatureF"]));

          min = Math.Min(min, wdata[i].TMin);
          max = Math.Max(max, wdata[i].TMax);
      }

      if (len > 0)
      {
          chart.BeginUpdate();
          chart.Data.Children.Clear();
          chart.Data.ItemsSource = wdata;

          Style ss = (Style)Resources["sstyle"];

        // create data series
          XYDataSeries ds = new XYDataSeries(); ds.Label = "Minimum";
          ds.ValueBinding = new Binding("TMin");
          ds.XValueBinding = new Binding("DateTime");
          ds.SymbolStyle = ss;
          chart.Data.Children.Add(ds);

          ds = new XYDataSeries(); ds.Label = "Maximum";
          ds.ValueBinding = new Binding("TMax");
          ds.XValueBinding = new Binding("DateTime");
          ds.SymbolStyle = ss;
          chart.Data.Children.Add(ds);

        //ds = new XYDataSeries(); ds.Label = "Average";
        //ds.ValueBinding = new Binding("TAvg");
        //ds.XValueBinding = new Binding("DateTime");
        //ds.SymbolStyle = ss;
          //chart.Data.Children.Add(ds);

          chart.View.AxisX.Min = wdata[0].DateTime.ToOADate();
        chart.View.AxisX.Max = wdata[len - 1].DateTime.ToOADate();

          chart.View.AxisY.Min = min;
          chart.View.AxisY.Max = max;

          chart.EndUpdate();
      }

      WaitAndDisableAnimation();
    }

    bool _showAnim = true;

    private void Symbol_Loaded(object sender, RoutedEventArgs e)
    {
        if (!_showAnim)
            return;

        PlotElement pe = (PlotElement)sender;

        Storyboard stb = new Storyboard();

        DoubleAnimation da = new DoubleAnimation();
        da.From = -Canvas.GetTop(pe); da.To = -Canvas.GetTop(pe); da.Duration = new Duration(TimeSpan.FromSeconds(0.01));
        Storyboard.SetTargetProperty(da, new PropertyPath("RenderTransform.Children[1].Y"));
        stb.Children.Add(da);

        da = new DoubleAnimation();
        da.From = -Canvas.GetTop(pe); da.To = 0; 
        if (pe.DataPoint != null && (sb.Value == 0 || chart.View.AxisX.Scale == 1.0))
        {
            da.BeginTime = TimeSpan.FromSeconds(0.04 * (pe.DataPoint.PointIndex));
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
        }
        else
            da.Duration = new Duration(TimeSpan.FromSeconds(1.5));

        Storyboard.SetTargetProperty(da, new PropertyPath("RenderTransform.Children[1].Y"));
        stb.Children.Add(da);

        stb.Begin(pe);
    }

    private void scale_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sb != null && scale.SelectedValue is double)
        {
            _showAnim = true;

        double val = (double)scale.SelectedValue;
            if (val >= 1.0)
                sb.ViewportSize = double.MaxValue;
            else
                sb.ViewportSize = 1.0 / (1.0 - val) - 1.0;
            sb.LargeChange = sb.ViewportSize;
            sb.SmallChange = 0.5 * sb.ViewportSize;

            chart.View.AxisX.Scale = val;
            WaitAndDisableAnimation();
        }
    }

    private void WaitAndDisableAnimation()
    {
        Storyboard stb = new Storyboard();
        DoubleAnimation da = new DoubleAnimation();
        da.Duration = new Duration(TimeSpan.FromSeconds(1));
        Storyboard.SetTargetProperty(da, new PropertyPath("Opacity"));

        stb.Children.Add(da);
      stb.Completed += (s, a) =>
        {
          _showAnim = false;
        };
        stb.Begin(this);
    }
  }

  public class AxisAnnoConverter : IValueConverter
  {
      public static readonly AxisAnnoConverter Default =
         new AxisAnnoConverter();

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
          if (value is double)
          {
             double dv = (double)value;
             if (dv > 0)
                 return Brushes.Red;
             else if (dv < 0)
                return Brushes.Blue;
          }
          return value;
      }
      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
          return value;
      }
  }

  public class Far2CelConverter : IValueConverter
  {
      public static readonly Far2CelConverter Default =
         new Far2CelConverter();

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
          if (value is double)
          {
              double dv = (double)value;
        return (dv - 32) * 5 / 9;
          }
          return value;
      }
      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
          return value;
      }
  }
}