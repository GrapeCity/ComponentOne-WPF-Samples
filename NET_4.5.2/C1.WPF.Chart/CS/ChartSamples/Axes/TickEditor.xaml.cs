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
using C1.WPF;
using C1.WPF.C1Chart;
#endif

namespace ChartSamples
{
  public partial class TickEditor : UserControl
  {
    public TickEditor()
    {
      InitializeComponent();
    }

    private void nbTickHeight_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
    {
      Axis ax = DataContext as Axis;
      if (ax != null)
      {
        ax.MajorTickHeight = nbMajHeight.Value;
        ax.MajorTickOverlap = nbMajOverlap.Value;

        ax.MinorTickHeight = nbMinHeight.Value;
        ax.MinorTickOverlap = nbMinOverlap.Value;
      }
    }
  }
}
