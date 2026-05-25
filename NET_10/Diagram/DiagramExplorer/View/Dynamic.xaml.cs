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
using System.Windows.Threading;

# pragma warning disable 1591
namespace DiagramExplorer.View
{
    /// <summary>
    /// Interaction logic for Dynamic.xaml
    /// </summary>
    public partial class Dynamic: UserControl, IDiagramHolder
    {
        public Dynamic()
        {
            InitializeComponent();

            CreateDiagram();

            Loaded += (s, e) => timer.Start();
            Unloaded += (s, e) => timer.Stop();
        }

        DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(1000) };
        Random random = new Random();

        void CreateDiagram()
        {
            var insects = "🐝,🦋,🐞,🐜,🐛,🦗,🦟,🐌".Split(",");

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

                    var text = insects[random.Next(0, insects.Length)].Trim();
                    nodes.Add(new Node() { Title = text, LegendItem = text, Shape = C1.Diagram.Shape.Circle });

                    var i = random.Next(0, nodes.Count - 1);
                    if (i != nodes.Count - 1)
                        edges.Add(new Edge() { Source = nodes[i], Target = nodes[nodes.Count - 1]});
                    i = random.Next(0, nodes.Count - 1);
                    if (i != nodes.Count - 1)
                        edges.Add(new Edge() { Source = nodes[i], Target = nodes[nodes.Count - 1]});

                    diagram.EndUpdate();
                };

            diagram.MouseUp += (s, e) => timer.IsEnabled = !timer.IsEnabled;
        }

        public FlexDiagram Diagram => diagram;
    }
}
