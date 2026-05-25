using C1.Diagram;
using C1.WPF.Diagram;
using C1.WPF.Chart;
using DiagramExplorer.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;

#pragma warning disable 1591
namespace DiagramExplorer.View
{
    /// <summary>
    /// Interaction logic for Intro.xaml
    /// </summary>
    public partial class CustomShape : UserControl, IDiagramHolder
    {
        public CustomShape()
        {
            InitializeComponent();

            // handle timer
            Loaded += (s, e) => timer.Start();
            Unloaded += (s, e) => timer.Stop();
            diagram.MouseUp += (s, e) => timer.IsEnabled = !timer.IsEnabled;

            // draw custom shape
            diagram.NodeRendering += (s, e) =>
            {
                var d = diagram as IDiagram;
                var (left, top, width, height) = d.GetRect(e.Node);

                e.Engine.SetStrokeThickness(0);
                e.Engine.SetFill(e.Node.NodeStyle.Fill);
                DrawBlob(e.Engine, left, top, width, height, d.GetNodes().IndexOf(e.Node));
            };

            // dynamically create diagram
            timer.Tick += (s, e) =>
            {
                diagram.BeginUpdate();

                var nodes = diagram.Nodes;
                var edges = diagram.Edges;

                if (nodes.Count >= 50)
                {
                    nodes.Clear();
                    edges.Clear();
                }

                nodes.Add(new Node()
                {
                    Text = $"{nodes.Count}",
                    Shape = Shape.None,
                    TitleStyle = new ChartStyle() { Stroke = Brushes.Black },
                    NodeStyle = new ChartStyle() { Fill = palette[rnd.Next(0, palette.Count)] }
                });

                var i = rnd.Next(0, nodes.Count - 1);
                if (i != nodes.Count - 1)
                    edges.Add(new Edge() { Source = nodes[i], Target = nodes[^1] });

                diagram.EndUpdate();
            };
        }

        IList<Brush> palette = C1.WPF.Chart.Palettes.Qualitative.Pastel1;
        Random rnd = new Random();
        DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(1000) };

        public FlexDiagram Diagram => diagram;

        static void DrawBlob(C1.Chart.IRenderEngine e, double left, double top, double width, double height, int seed = 1)
        {
            var rnd = new Random(seed);

            int f1 = 3 + rnd.Next(0, 3);
            int f2 = 5 + rnd.Next(0, 3);
            int f3 = 7 + rnd.Next(0, 3);

            double p1 = rnd.NextDouble() * Math.PI * 2;
            double p2 = rnd.NextDouble() * Math.PI * 2;
            double p3 = rnd.NextDouble() * Math.PI * 2;

            const int steps = 120;

            var rx = 0.5 * width;
            var ry = 0.5 * height;

            var cx = left + rx;
            var cy = top + ry;

            var xs = new double[steps];
            var ys = new double[steps];

            for (int i = 0; i < steps; i++)
            {
                double t = i * 2.0 * Math.PI / steps;
                double r = 1.0 + 0.11 * Math.Sin(f1 * t + p1) + 0.06 * Math.Cos(f2 * t + p2) + 0.04 * Math.Sin(f3 * t + p3);
                xs[i] = cx + rx * r * Math.Cos(t);
                ys[i] = cy + ry * r * Math.Sin(t);
            }

            e.DrawPolygon(xs, ys);
        }
    }
}
