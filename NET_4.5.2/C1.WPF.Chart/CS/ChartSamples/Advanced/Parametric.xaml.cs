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
using C1.Silverlight.Chart.Extended;
#else
using C1.WPF.C1Chart;
using C1.WPF.C1Chart.Extended;
#endif

namespace ChartSamples
{
  [System.ComponentModel.Description("Shows plotting of parametric function")]
  public partial class Parametric : UserControl
  {
    ParametricFunctionSeries _pfs;

    public Parametric()
    {
      InitializeComponent();

      _pfs = new ParametricFunctionSeries()
      {
        SampleCount = 1000,
        Max = 2*Math.PI,
        XFunction = (t) => Math.Cos(xPar.Value * t),
        YFunction = (t) => Math.Sin(yPar.Value * t),
      };
      chart.Data.Children.Add(_pfs);

      chart.ChartType = ChartType.Line;
    }

    private void Par_ValueChanged(object sender, C1.WPF.PropertyChangedEventArgs<double> e)
    {
      if(_pfs!=null)
        _pfs.UpdateData();
    }
  }
}
