using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.WPF.C1Chart;


namespace ChartSamples
{
    [System.ComponentModel.Description("This sample shows vertical and horizontal markers.")]
    public partial class Markers : UserControl
    {
        public Markers()
        {
            InitializeComponent();

            SampleData.CreateData(chart);
            chart.ChartType = ChartType.XYPlot;
            chart.Palette = ColorGeneration.Urban;

            var pnl = new ChartPanel();

            var vmarker = CreateMarker(false);
            pnl.Children.Add(vmarker);
            vmarker.Action = ChartPanelAction.MouseMove;

            var hmarker = CreateMarker(true);
            pnl.Children.Add(hmarker);
            hmarker.Action = ChartPanelAction.MouseMove;

            chart.View.Layers.Add(pnl);

            cbAttach.ItemsSource = Utils.GetEnumValues(typeof(ChartPanelAttach));
            cbAttach.SelectedIndex = 0;
            cbAttach.SelectionChanged += (s, e) =>
              {
                  hmarker.Attach = vmarker.Attach =
                    (ChartPanelAttach)Enum.Parse(typeof(ChartPanelAttach), cbAttach.SelectedValue.ToString(), true);
              };
        }

        ChartPanelObject CreateMarker(bool isHorizontal)
        {
            var obj = new ChartPanelObject();
            var bdr = new Border()
            {
                BorderBrush = Background = new SolidColorBrush(Color.FromArgb(150, 255, 2, 2)),
                Padding = new Thickness(2),
            };
            var tb = new TextBlock();
            var bind = new Binding();
            bind.Source = obj;

            if (isHorizontal)
            {
                bdr.BorderThickness = new Thickness(0, 4, 0, 0);
                bdr.Margin = new Thickness(0, -1, 0, 0);
                obj.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                bind.StringFormat = "y={0:#.#}";
                bind.Path = new PropertyPath("DataPoint.Y");
                obj.DataPoint = new Point(double.NaN, 0.5);
            }
            else
            {
                bdr.BorderThickness = new Thickness(4, 0, 0, 0);
                bdr.Margin = new Thickness(-1, 0, 0, 0);
                obj.VerticalContentAlignment = VerticalAlignment.Stretch;
                bind.StringFormat = "x={0:#.#}";
                bind.Path = new PropertyPath("DataPoint.X");
                obj.DataPoint = new Point(0.5, double.NaN);
            }

            tb.SetBinding(TextBlock.TextProperty, bind);
            bdr.Child = tb;

            bdr.IsHitTestVisible = false;

            obj.Content = bdr;

            return obj;
        }
    }
}
