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
  /// Interaction logic for Labels.xaml
  /// </summary>
  public partial class OptimalArrangerSample : UserControl
  {
    ChartLabels lbls;

    public OptimalArrangerSample()
    {
      InitializeComponent();

      var arranger = new OptimalLabelArranger();
      lbls = new ChartLabels() { LabelArranger = arranger };

      chart.View.Layers.Add(lbls);
      chart.MouseLeftButtonDown += new MouseButtonEventHandler(chart_MouseLeftButtonDown);

      NewData(5);

      cbHideOutside.Checked += (s, e) => arranger.HideLabelsOutsideBorder = true;
      cbHideOutside.Unchecked += (s, e) => arranger.HideLabelsOutsideBorder = false;
    }

    void chart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var pt = chart.View.PointToData(e.GetPosition(chart.View));

      lbls.Children.Add(new ChartLabelPoint() 
      { 
        Content = string.Format("{0:0.00},{1:0.00}", pt.X, pt.Y), CornerRadius = new CornerRadius(2),
        DataPoint = pt, Padding = new Thickness(2),
        Foreground = Brushes.Blue, BorderThickness = new Thickness(1), BorderBrush = Brushes.Blue });
    }

    void NewData(int nser)
    {
      chart.BeginUpdate();
      chart.Data.Children.Clear();
      
      int npts = 10;

      for (int i = 0; i < nser; i++)
      {
        chart.Data.Children.Add(SampleData.CreateSeries("series " + i, npts));
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
      NewData(5);
    }
  }

  class SampleData
  {
    static Random rnd = new Random();

    public static XYDataSeries CreateSeries(string label, int npts)
    {
      return CreateSeries(label, npts, 0, 1);
    }

    public static XYDataSeries CreateSeries(string label, int npts, double min, double max, bool random=true)
    {
      var x = new double[npts];
      var y = new double[npts];

      var d = max - min;
      var phase = Math.PI * 2 *rnd.NextDouble();
      var amp = rnd.NextDouble();

      for (int i = 0; i < npts; i++)
      {
        x[i] = i;

        if (random)
          y[i] = min + d * rnd.NextDouble();
        else
          y[i] = min + amp * d * Math.Sin(0.1*i+phase);
      }

      return new XYDataSeries() { SymbolSize = new Size(6,6), Label = label, XValuesSource = x, ValuesSource = y };
    }
  }
}
