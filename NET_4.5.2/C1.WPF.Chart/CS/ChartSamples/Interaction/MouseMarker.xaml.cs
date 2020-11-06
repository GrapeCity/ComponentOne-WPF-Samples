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
    [System.ComponentModel.Description("This sample shows a marker that moves with the mouse cursor.")]
    public partial class MouseMarker : UserControl
    {
        public MouseMarker()
        {
            InitializeComponent();

            SampleData.CreateData(chart);
            chart.ChartType = ChartType.LineSymbols;

            var pnl = new ChartPanel();

            var obj = new ChartPanelObject()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom
            };

            var bdr = new Border()
            {
                Background = new SolidColorBrush(Colors.Green) { Opacity = 0.4 },
                BorderBrush = new SolidColorBrush(Colors.Green),
                BorderThickness = new Thickness(1, 1, 3, 3),
                CornerRadius = new CornerRadius(6, 6, 0, 6),
                Padding = new Thickness(3)
            };
            //bdr.Padding = new Thickness(16, 16, 0, 0);

            var sp = new StackPanel();

            var tb1 = new TextBlock();
            var bind1 = new Binding();
            bind1.Source = obj;
            bind1.StringFormat = "x={0:#.##}";
            bind1.Path = new PropertyPath("DataPoint.X");
            tb1.SetBinding(TextBlock.TextProperty, bind1);

            var tb2 = new TextBlock();
            var bind2 = new Binding();
            bind2.Source = obj;
            bind2.StringFormat = "y={0:#.##}";
            bind2.Path = new PropertyPath("DataPoint.Y");
            tb2.SetBinding(TextBlock.TextProperty, bind2);

            sp.Children.Add(tb1);
            sp.Children.Add(tb2);

            bdr.Child = sp;

            obj.Content = bdr;
            obj.DataPoint = new Point();
            obj.Action = ChartPanelAction.MouseMove;

            pnl.Children.Add(obj);

            chart.View.Layers.Add(pnl);

            cbAttach.ItemsSource = Utils.GetEnumValues(typeof(ChartPanelAttach));
            cbAttach.SelectedIndex = 0;
            cbAttach.SelectionChanged += (s, e) =>
            {
                obj.Attach =
                  (ChartPanelAttach)Enum.Parse(typeof(ChartPanelAttach), cbAttach.SelectedValue.ToString(), true);
            };
        }
    }
}
