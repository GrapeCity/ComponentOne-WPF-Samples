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
using C1.Silverlight.Chart.Extended;
#else
using C1.WPF.C1Chart.Extended;
#endif

namespace ChartSamples
{
  public partial class MovingAverageEditor : UserControl
  {
    public MovingAverageEditor()
    {
      InitializeComponent();
      DataContextChanged += (s,e) =>
      {
        MovingAverage ms = DataContext as MovingAverage;
        if (ms != null)
          nbPeriod.Value = ms.Period;
      };
    }

    private void nbPeriod_ValueChanged(object sender, C1.WPF.PropertyChangedEventArgs<double> e)
    {
      MovingAverage ms = DataContext as MovingAverage;
      if (ms != null)
      {
        ms.Period = (int)nbPeriod.Value;
        ms.Label = string.Format("MovAvg {0}", ms.Period);
      }
    }
  }
}
