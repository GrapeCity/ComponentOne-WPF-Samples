using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.WPF.C1Chart;


namespace ChartSamples
{
    [System.ComponentModel.Description("Click on the plot to create an interactive marker with label. You can also drag and remove markers.")]
    public partial class AddRemoveMarkers : UserControl
    {
        ChartPanel pnl = new ChartPanel();

        public AddRemoveMarkers()
        {
            InitializeComponent();


            SampleData.CreateData(chart);
            chart.ChartType = ChartType.LineSymbols;

            chart.MouseLeftButtonDown += (s, e) =>
              {
                  if (e.OriginalSource == pnl)
                  {
                      var pt = pnl.GetDataCoordinates(e);
                      if (pt.X >= chart.View.AxisX.ActualMin && pt.X <= chart.View.AxisX.ActualMax)
                          AddMarker(pt.X);
                  }
              };

            chart.View.Layers.Add(pnl);
        }

        void AddMarker(double x)
        {
            var dt = (DataTemplate)Resources["marker"];
            var cpo = (ChartPanelObject)dt.LoadContent();

            cpo.DataPoint = new Point(x, double.NaN);
            cpo.Action = ChartPanelAction.LeftMouseButtonDrag;

            cpo.Attach = ChartPanelAttach.DataX;

            pnl.Children.Add(cpo);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var obj = (ChartPanelObject)btn.Tag;
            pnl.Children.Remove(obj);
        }
    }
}
