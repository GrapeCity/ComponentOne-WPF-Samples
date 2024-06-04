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
  public partial class TrendLineEditor : UserControl
  {
    public TrendLineEditor()
    {
      InitializeComponent();

      cbFit.ItemsSource = Utils.GetEnumValues(typeof(FitType));
      cbFit.SelectedIndex = 0;

      DataContextChanged += (s, e) =>
        {
          TrendLine tl = DataContext as TrendLine;
          if (tl != null)
          {
            cbFit.SelectedIndex = (int)tl.FitType;
            nbOrder.Value = tl.Order;
            if (tl.Label == null)
              tl.Label = CreateLabel(tl);
          }
        };
    }

    private void cbFit_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      TrendLine tl = DataContext as TrendLine;
      if (tl != null)
      {
        tl.FitType = (FitType)cbFit.SelectedIndex;
        tl.Label = CreateLabel(tl);
        UpdateControls();
      }
    }

    private void nbOrder_ValueChanged(object sender, C1.WPF.PropertyChangedEventArgs<double> e)
    {
      TrendLine tl = DataContext as TrendLine;
      if (tl != null)
      {
        tl.Order = (int)nbOrder.Value;
        tl.Label = CreateLabel(tl);
      }
    }

    string CreateLabel(TrendLine tl)
    {
      if( tl.FitType == FitType.Polynom || tl.FitType == FitType.Fourier)
        return string.Format("{0} {1}", tl.FitType, tl.Order);
      else
        return string.Format("{0}", tl.FitType);
    }

    void UpdateControls()
    {
      TrendLine tl = DataContext as TrendLine;
      if (tl != null)
      {
        if (tl.FitType == FitType.Polynom || tl.FitType == FitType.Fourier)
          hOrder.Visibility = Visibility.Visible;
        else
          hOrder.Visibility = Visibility.Collapsed;
      }
    }
  }
}
