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
  [System.ComponentModel.Description("This sample shows a bubble chart with a custom style applied to the plot elements.")]
  public partial class Bubble : UserControl
  {
    public Bubble()
    {
      InitializeComponent();

      chart.BeginUpdate();

      chart.Data.ItemNames = new string[] 
      { 
        "Sydney - 2000",
        "Athens - 2004",
        "Beijing - 2008",
        "London - 2010"
      };
      chart.View.AxisY.ItemsSource = new string[] { "USA", "China", "Russia" };
      chart.View.AxisX.Min = -0.5; chart.View.AxisX.Max = 3.5;
      chart.View.AxisY.Min = -0.5; chart.View.AxisY.Max = 2.5;
      chart.EndUpdate();
    }
  }

  public class ToolTipConverter : System.Windows.Data.IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      DataPoint dp = value as DataPoint;

      if (dp != null)
        return dp["SizeValues"].ToString() + " medals";

      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
