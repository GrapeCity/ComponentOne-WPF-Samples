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
  [System.ComponentModel.Description("Shows a complex stacked bar chart with multiple data series.")]
  public partial class ComplexChart : UserControl
  {
    Random rnd = new Random();
//    Brush[] brushes = new Brush[]
//    {
//      new SolidColorBrush(Color.FromArgb(255, 21, 72, 123) ),
//      CreateBrush( Color.FromArgb(255, 21, 72, 123) ),
//      new SolidColorBrush(Color.FromArgb(255, 239, 239, 231)),
//      CreateBrush( Color.FromArgb(255, 239, 239, 231)),
//      new SolidColorBrush(Color.FromArgb(255, 74, 130, 189)),
//      CreateBrush( Color.FromArgb(255, 74, 130, 189)),
//      new SolidColorBrush(Color.FromArgb(255, 198, 80, 74)),
//      CreateBrush( Color.FromArgb(255, 198, 80, 74)),
//      new SolidColorBrush(Color.FromArgb(255, 156, 186, 90)),
//      CreateBrush( Color.FromArgb(255, 156, 186, 90)),
//    };

    public ComplexChart()
    {
      InitializeComponent();
      
      chart.Data.ItemNames = new string[] { "Cat.1", "Cat.2", "Cat.3", "Cat.4", "Cat.5" };
      CreateData();
    }

    void CreateData()
    {
      chart.Data.Children.Clear();

      for (int i = 0; i < 10; i++)
      {
        DataSeries ds = new DataSeries() { ValuesSource = CreateRandomArray(5) };
//        ds.SymbolFill = brushes[i];

        BarColumnOptions.SetStackGroup(ds, i % 2);
        chart.Data.Children.Add(ds);
      }

      chart.Data.Children.Add(new DataSeries() 
        { ChartType = ChartType.LineSymbols,  ValuesSource = CreateRandomArray(5),
        ConnectionStrokeThickness = 5, ConnectionStroke=new SolidColorBrush(Colors.Orange)});
    }

    double[] CreateRandomArray(int cnt)
    {
      double[] vals = new double[cnt];

      for (int i = 0; i < cnt; i++)
        vals[i] = rnd.Next(10, 100);

      return vals;
    }

    static LinearGradientBrush CreateBrush(Color clr)
    {
      LinearGradientBrush lgb = new LinearGradientBrush()
      {
        EndPoint = new Point(2,2), MappingMode= BrushMappingMode.Absolute,
        SpreadMethod = GradientSpreadMethod.Reflect
      };
      lgb.GradientStops = new GradientStopCollection()
      {
        new GradientStop() { Color= clr, Offset = 0},
        new GradientStop() { Color= Color.FromArgb(64,255,255,255), Offset = 1},
      };

      return lgb;
    }
  }
}
