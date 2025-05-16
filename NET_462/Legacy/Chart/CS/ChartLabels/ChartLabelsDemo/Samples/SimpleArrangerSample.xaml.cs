using System;
using System.Diagnostics;
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
  /// Interaction logic for LabelsCustomArranger.xaml
  /// </summary>
  public partial class SimpleArrangerSample : UserControl
  {
    ChartLabels lbls;
    ChartLabels lbls2;

    public SimpleArrangerSample()
    {
      InitializeComponent();

      // first layer with automatic label positioning
      var arranger = new SimpleArranger();
      lbls = new ChartLabels() { LabelArranger = arranger };
      chart.View.Layers.Add(lbls);

      // second layer with user-movable labels 
      lbls2 = new ChartLabels();
      chart.View.Layers.Add(lbls2);
      lbls2.LayoutUpdated += new EventHandler(lbls2_LayoutUpdated);

      NewData(5);

      chart.Actions.Add(new ZoomAction());
      chart.Actions.Add(new TranslateAction() { Modifiers = ModifierKeys.Shift });
      chart.Actions.Add(new ScaleAction() { Modifiers = ModifierKeys.Control });

      chart.MouseLeftButtonDown += new MouseButtonEventHandler(chart_MouseLeftButtonDown);
    }

    void lbls2_LayoutUpdated(object sender, EventArgs e)
    {
      // optimized updating
      var list = new List<Rect>();
      foreach (var lbl in lbls2.Children)
      {
        var ilbl = (ILabel)lbl;
        list.Add(new Rect(ilbl.Position, ilbl.Size));
      }

      int cnt = list.Count;

      if (cnt == lbls.OccupiedRects.Count)
      {
        bool changed = false;

        for (int i = 0; i < cnt; i++)
        {
          if (list[i] != lbls.OccupiedRects[i])
          {
            changed = true;
            break;
          }
        }

        if (!changed)
          return;
      }

      // update occupied rectangles
      lbls.BeginUpdate();
      lbls.OccupiedRects.Clear();

      for(int i=0; i<cnt; i++)
        lbls.OccupiedRects.Add(list[i]);

      lbls.EndUpdate();
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
            PointIndex = j,
          };
          lbls.Children.Add(lbl);
        }
      }

      lbls.EndUpdate();

      lbls2.Children.Clear();
    }

    void chart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (Keyboard.Modifiers == ModifierKeys.Control)
      {
        e.Handled = true;

        var pt = chart.View.PointToData(e.GetPosition(chart.View));

        lbls2.Children.Add(new MovableChartLabelPoint()
        {
          Content = string.Format("{0:0.00},{1:0.00}", pt.X, pt.Y),
          CornerRadius = new CornerRadius(2),
          DataPoint = pt,
          Padding = new Thickness(2),
          Foreground = Brushes.Blue,
          BorderThickness = new Thickness(1),
          BorderBrush = Brushes.Blue,
          Offset = new Point()
        });
      }
    }

    private void btnNew_Click(object sender, RoutedEventArgs e)
    {
      NewData(5);
    }
  }
}
