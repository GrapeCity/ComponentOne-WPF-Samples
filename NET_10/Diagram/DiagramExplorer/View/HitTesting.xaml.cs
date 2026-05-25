using C1.WPF.Chart;
using C1.WPF.Diagram;
using DiagramExplorer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

# pragma warning disable 1591
namespace DiagramExplorer.View
{
    /// <summary>
    /// Interaction logic for HitTesting.xaml
    /// </summary>
    public partial class HitTesting : UserControl, IDiagramHolder
    {
        public HitTesting()
        {
            InitializeComponent();

            CreateRandomPersonDiagram(diagram);

            var nodeStyle = new ChartStyle();
            var edgeStyle = new ChartStyle();

            diagram.MouseMove += (s, e) =>
            {
                var point = e.GetPosition(diagram);
                var info = diagram.HitTest(point.X, point.Y);

                if (info?.Distance <= 3)
                {


                    diagram.BeginUpdate();

                    foreach (var node in diagram.Nodes)
                        node.NodeStyle = nodeStyle;
                    foreach (var edge in diagram.Edges)
                    {
                        edge.EdgeStyle = edgeStyle;
                        edge.SourceArrow = edge.TargetArrow = C1.Chart.ArrowStyle.None;
                    }



                    if (info?.Element is Node node1)
                    {
                        node1.NodeStyle = new ChartStyle() { StrokeThickness = 4 };
                    }
                    else if (info?.Element is Edge edge)
                    {
                        edge.EdgeStyle = new ChartStyle() { StrokeThickness = 2 };
                        edge.SourceArrow = edge.TargetArrow = C1.Chart.ArrowStyle.Normal;
                    }

                    diagram.EndUpdate();
                }
            };

            diagram.MouseUp += (s, e) => CreateRandomPersonDiagram(diagram);
        }

        public FlexDiagram Diagram => diagram;


        static Random random = new Random();

        static void CreateRandomPersonDiagram(FlexDiagram diagram)
        {
            var nodes = diagram.Nodes;
            var edges = diagram.Edges;

            diagram.BeginUpdate();

            nodes.Clear();
            edges.Clear();

            var persons = "👨,👩,👦,👧".Split(",");

            for (var i = 0; i < 15; i++)
            {
                var text = persons[random.Next(0, persons.Length)];
                var node = new Node() { Title = text, LegendItem = text, Shape = C1.Diagram.Shape.RoundedRectangle };
                nodes.Add(node);

                if (i == 0)
                    continue;

                var k = random.Next(0, nodes.Count - 2);
                edges.Add(new Edge() { Source = nodes[k], Target = nodes[nodes.Count - 1] });
                k = random.Next(0, nodes.Count - 1);
                edges.Add(new Edge() { Source = nodes[k], Target = nodes[nodes.Count - 1] });

                diagram.EndUpdate();
            }
        }

    }
}
