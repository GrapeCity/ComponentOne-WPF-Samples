using System;
using System.Collections.Generic;
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
using System.Reflection;
using C1.WPF.C1Chart;


namespace ChartSamples
{

  [System.ComponentModel.Description("This sample shows how you can aggregate each data series into one total sum, count, average and so on.")]
  public partial class AggregateChart : UserControl
  {
    Dictionary<string, Brush> _dict = new Dictionary<string, Brush>();

    public AggregateChart()
    {
      InitializeComponent();

      _dict["red"] = CreateBrush(Colors.Red);
      _dict["blue"] = CreateBrush(Color.FromArgb(255, 2, 150, 252));
      _dict["yellow"] = CreateBrush(Colors.Yellow);

      cb.ItemsSource = Utils.GetEnumValues(typeof(Aggregate));
      cb.SelectedIndex = 0;
      cb.SelectionChanged += (s, e) =>
        chart.Aggregate = (Aggregate)cb.SelectedIndex;

      int cnt = 30;
      chart.Data.ItemNames = CreateRandomStrings(cnt,
        new string[] { "blue", "red", "yellow" }); ;
      chart.View.AxisX.AnnoVisibility = AnnoVisibility.ShowAll;

      var vals = CreateRandomValues(cnt);

      var ds = new DataSeries() { ValuesSource = vals };

      ds.PlotElementLoaded += (s, e) =>
      {
        PlotElement pe = (PlotElement)s;
        if (_dict.ContainsKey(pe.DataPoint.Name))
          pe.Fill = _dict[pe.DataPoint.Name];
        pe.StrokeThickness = 0;
      };

      chart.Data.Children.Add(ds);
      chart.View.AxisX.AnnoTemplate = chart.Resources["al_tmpl"];
      BarColumnOptions.SetRadiusX(chart, 4);
      BarColumnOptions.SetRadiusY(chart, 4);
    }

    Random rnd = new Random();

    double[] CreateRandomValues(int cnt)
    {
      double[] vals = new double[cnt];
      for (int i = 0; i < cnt; i++)
        vals[i] = rnd.NextDouble() * 100;
      return vals;
    }

    string[] CreateRandomStrings(int cnt, string[] list)
    {
      string[] vals = new string[cnt];
      for (int i = 0; i < cnt; i++)
        vals[i] = list[rnd.Next(0, list.Length)];
      return vals;
    }

    Brush CreateBrush(Color clr)
    {
      return new LinearGradientBrush()
      {
        GradientStops = 
        { 
          //new GradientStop() { Color= Colors.Black,  Offset = 0},
          new GradientStop() { Color= Color.FromArgb(128, clr.R,clr.G,clr.B) , Offset = 0},
          new GradientStop() { Color= clr, Offset = 0.2},
          new GradientStop() { Color= clr, Offset = 0.7},
          new GradientStop() { Color= Color.FromArgb(128, clr.R,clr.G,clr.B) , Offset = 1},
        },
        StartPoint = new Point(0, 0),
        EndPoint = new Point(1, 1),
        //Opacity = 0.8
      };
    }
  }

  public class AxisLabelConverter : IValueConverter
  {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (value != null)
        return string.Format("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";Component/Resources/flower_{0}.png", value.ToString());

      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    #endregion
  }

  class Utils
  {
    public static T[] GetEnumValues<T>()
    {
      var type = typeof(T);
      if (!type.IsEnum)
        throw new ArgumentException("Type '" + type.Name + "' is not an enum");

      return (
        from field in type.GetFields(BindingFlags.Public | BindingFlags.Static)
        where field.IsLiteral
        select (T)field.GetValue(null)
      ).ToArray();
    }

    public static object[] GetEnumValues(Type type)
    {
      List<object> list = new List<object>();
      for (int i = 0; ; i++)
      {
        string val = Enum.GetName(type, i);
        if (!string.IsNullOrEmpty(val))
          list.Add(val);
        else
          break;
      }

      return list.ToArray();
    }
  }

}
