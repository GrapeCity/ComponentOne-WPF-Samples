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
  [System.ComponentModel.Description("This sample shows how to create static labels and tooltips for data points. It also shows the built-in percentage series label.")]
  public partial class Labels : UserControl
  {
    public Labels()
    {
      InitializeComponent();

      Loaded += new RoutedEventHandler(Labels_Loaded);

      c1chart.BeginUpdate();
      c1chart.View.AxisY.Min = 0; c1chart.View.AxisY.Max = 10;

      cbChartType.ItemsSource = new string[]{ "Column", "LineSymbols", "Pie" };
      cbChartType.SelectedItem = "Column";
      c1chart.EndUpdate();
    }

    void Labels_Loaded(object sender, RoutedEventArgs e)
    {
      // seems radiobutton does not use parent Foreground
      rbLabels.Foreground = Foreground;
      rbTooltips.Foreground = Foreground;
    }

    private void rbLabel_Checked(object sender, RoutedEventArgs e)
    {
      if (c1chart == null)
        return;

      DataTemplate lbl = null;
      if (sender == rbLabels)
      {
        lbl = (DataTemplate)Resources["lbl"];
      }

      foreach (DataSeries ds in c1chart.Data.Children)
        ds.PointLabelTemplate = lbl;

      // force repainting
      var ds1 = c1chart.Data.Children[1];
      c1chart.Data.Children.Remove(ds1);
      c1chart.Data.Children.Add(ds1);
    }

    private void cbChartType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (cbChartType.SelectedItem is string)
         c1chart.ChartType = (ChartType)Enum.Parse(typeof(ChartType),
           (string)cbChartType.SelectedItem, false);
    }

    private void DataSeries_PlotElementLoaded(object sender, EventArgs e)
    {
      var pe = (PlotElement)sender;
      if (pe.DataPoint.Series.PointLabelTemplate == null)
      {
        var dt = (DataTemplate)Resources["lbl"];
        pe.ToolTip = dt.LoadContent();
      }
    }
  }
}
