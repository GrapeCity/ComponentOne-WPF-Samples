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
  /// Interaction logic for PieLabels.xaml
  /// </summary>
  public partial class PieArrangerSample : UserControl
  {
    Random rnd = new Random();
    ChartLabels lbls;

    public PieArrangerSample()
    {
      InitializeComponent();

      chart.ChartType = ChartType.Pie;

      var arranger = new PieArranger();
      lbls = new ChartLabels() { LabelArranger = arranger };
      chart.View.Layers.Add(lbls);

      NewData();

      SizeChanged += (s, e) => lbls.EndUpdate();
    }

    void NewData(int cnt = 0)
    {
      if (cnt == 0)
        cnt = rnd.Next(40, 80);

      chart.Data.Children.Clear();

      var values = new double[cnt];
      for (int i = 0; i < cnt; i++)
        values[i] = rnd.Next(1, 100);

      chart.Data.Children.Add(new DataSeries() { ValuesSource = values });

      lbls.BeginUpdate();
      lbls.Children.Clear();
      for (int i = 0; i < cnt; i++)
      {
        lbls.Children.Add(new ChartLabel()
        {
          SeriesIndex = 0,
          PointIndex = i,
          Content = "lbl" + i,
          //Padding = new Thickness(1),
          BorderThickness = new Thickness(0.5),
          BorderBrush = Brushes.LightGray,
          Offset = new Point()
        });
      }
      lbls.EndUpdate();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      NewData();
    }
  }
}
