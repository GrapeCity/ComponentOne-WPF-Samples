using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using C1.WPF.C1Chart;
using C1.WPF.C1Chart.Labels;

namespace LabelSamples
{
  /// <summary>
  /// Interaction logic for LabelDecimation.xaml
  /// </summary>
  public partial class DecimatingArrangerSample : UserControl
  {
    ChartLabels lbls;

    public DecimatingArrangerSample()
    {
      InitializeComponent();

      var arranger = new DecimatingLabelArranger();
      lbls = new ChartLabels();

      chart.View.Layers.Add(lbls);
      NewData(2);

      cbDecimation.Checked += (s, e) => lbls.LabelArranger = arranger;
      cbDecimation.Unchecked += (s, e) => lbls.LabelArranger = null;
    }

    void NewData(int nser)
    {
      chart.BeginUpdate();
      chart.Data.Children.Clear();

      int npts = 100;

      for (int i = 0; i < nser; i++)
      {
        chart.Data.Children.Add(SampleData.CreateSeries("series " + i, npts, 0, 100, false));
      }
      chart.EndUpdate();

      lbls.BeginUpdate();
      lbls.Children.Clear();
      for (int i = 0; i < nser; i++)
      {
        for (int j = 0; j < npts; j++)
        {
          var lbl = new ChartLabel()
          {
            Foreground = Brushes.Red,
            Content = "ser " + i,
            SeriesIndex = i,
            PointIndex = j
          };
          lbls.Children.Add(lbl);
        }
      }

      lbls.EndUpdate();
    }

    private void btnNew_Click(object sender, RoutedEventArgs e)
    {
      NewData(2);
    }
  }
}
