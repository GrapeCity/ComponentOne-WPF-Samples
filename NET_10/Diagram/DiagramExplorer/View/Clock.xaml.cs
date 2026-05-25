using C1.Chart;
using C1.Diagram;
using C1.WPF.Chart;
using C1.WPF.Diagram;
using DiagramExplorer.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Brushes = System.Windows.Media.Brushes;
using NodeShape = C1.Diagram.Shape;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;

# pragma warning disable 1591
namespace DiagramExplorer.View
{
    /// <summary>
    /// Interaction logic for Tooltips.xaml
    /// </summary>
    public partial class Clock : UserControl, IDiagramHolder
    {
        public Clock()
        {
            InitializeComponent();
            
            timer.Tick += (s,e) => UpdateTimeDiagram(diagram);

            Loaded += (s, e) => timer.Start();
            Unloaded += (s, e) => timer.Stop();
        }

        public FlexDiagram Diagram => diagram;

        DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500) };

        static void UpdateTimeDiagram(FlexDiagram diagram)
        {
            diagram.BeginUpdate();

            var nodes = diagram.Nodes;
            var edges = diagram.Edges;
            nodes.Clear();
            edges.Clear();

            var time = DateTime.Now;

            var styleActive = new ChartStyle() { Stroke = diagram.Foreground, StrokeThickness = 2 };
            var styleBlink = new ChartStyle() { Fill = (time.Second % 2) == 0 ? diagram.Foreground : diagram.Background, StrokeThickness = 2 };

            var minuteNode = new Node() { Shape = NodeShape.Circle, NodeStyle = styleBlink };
            nodes.Add(minuteNode);
            var secondNode = new Node() { Shape = NodeShape.Circle, NodeStyle = styleBlink };
            nodes.Add(secondNode);

            for (var j = 0; j < 10; j++)
                AddTimeNode(diagram, null, minuteNode, time, (t) => t.Hour, "h", j, styleActive);

            for (var j = 0; j < 10; j++)
                AddTimeNode(diagram, minuteNode, secondNode, time, (t) => t.Minute, "m", j, styleActive);

            for (var j = 0; j < 10; j++)
                AddTimeNode(diagram, secondNode, null, time, (t) => t.Second, "s", j, styleActive);

            diagram.EndUpdate();
        }

        private static void AddTimeNode(FlexDiagram diagram, Node? from, Node? to, DateTime time, Func<DateTime, int> getValue,
            string suffix, int i, ChartStyle styleActive)
        {
            var val = getValue(time);
            var node = new Node() { Title = $"{10 * (val / 10) + i: 00}{suffix}", Shape = NodeShape.RoundedRectangle };

            var pos = 1 - (double)Math.Abs((i - (val % 10)) / 9.0);
            var color = Common.Samples.Interpolate( Color.White , Color.Black, pos * pos * pos);

            var active = val % 10 == i;
            node.NodeStyle = node.TitleStyle = active ? styleActive : new ChartStyle() { 
                Stroke = new SolidColorBrush( System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B)),
            }; 

            diagram.Nodes.Add(node);

            if (from != null)
            {
                diagram.Edges.Add(new Edge()
                {
                    Source = from,
                    Target = node,
                    EdgeStyle = node.TitleStyle,
                    TargetArrow = active ? ArrowStyle.Normal : ArrowStyle.None
                });
            }

            if (to != null)
            {
                diagram.Edges.Add(new Edge()
                {
                    Source = node,
                    Target = to,
                    EdgeStyle = node.TitleStyle,
                    TargetArrow = active ? ArrowStyle.Normal : ArrowStyle.None
                });
            }
        }
    }
}
