using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using C1.WPF.C1Chart;

namespace ChartSamples
{
  public partial class StackedPie : UserControl
  {
    public StackedPie()
    {
      InitializeComponent();

      // load data from resource
      var data = new CSVData();
      using (Stream stream = Assembly.GetExecutingAssembly().
          GetManifestResourceStream("ChartSamples.Resources.browsers.csv"))
      {
        data.Read(stream, false, false);
      }

      int len = data.Length;
      var vdata = new VersionInfo[len];

      for (int i = 0; i < len; i++)
      {
        vdata[i] = new VersionInfo()
        {
          Name = data[i, 0],
          Version = data[i, 1],
          Value = double.Parse(data[i, 2], CultureInfo.InvariantCulture)
        };
      }

      chart.BeginUpdate();
      chart.Data.ItemsSource = vdata;
      chart.Data.ItemNameBinding = new Binding("Name");

      chart.Aggregate = Aggregate.Sum;

      // first series - total by browser
      var ds1 = new DataSeries()
      {
        ValueBinding = new Binding("Value"),
        PointLabelTemplate = (DataTemplate)Resources["lbl"],
      };
      ds1.PlotElementLoaded += (PlotElementLoaded);
      chart.Data.Children.Add(ds1);

      // second series - browser versions
      var ds2 = new DataSeries()
      {
        ItemsSource = vdata, // own data source(no aggregates)
        ValueBinding = new Binding("Value"),
        PointLabelTemplate = (DataTemplate)Resources["lbl1"],
      };
      ds2.PlotElementLoaded += (PlotElementLoaded);
      chart.Data.Children.Add(ds2);

      // chart type and direction
      chart.ChartType = ChartType.PieStacked;
      PieOptions.SetDirection(chart, SweepDirection.Counterclockwise);

      chart.EndUpdate();
    }

    void PlotElementLoaded(object sender, EventArgs e)
    {
      var pe = (PlotElement)sender;
      var dp = pe.DataPoint;

      pe.Stroke = new SolidColorBrush(Colors.White);
      pe.StrokeThickness = 1;
    }
  }

  public class VersionInfo
  {
    public string Name { get; set; }
    public string Version { get; set; }
    public double Value { get; set; }
  }

}
