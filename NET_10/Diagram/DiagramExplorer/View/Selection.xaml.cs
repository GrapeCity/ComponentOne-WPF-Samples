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

using C1.WPF.Chart;
using C1.WPF.Diagram;
using DiagramExplorer.ViewModel;

# pragma warning disable 1591
namespace DiagramExplorer.View
{
    /// <summary>
    /// Interaction logic for Selection.xaml
    /// </summary>
    public partial class Selection : UserControl, IDiagramHolder
    {
        public Selection()
        {
            InitializeComponent();

            var node = new Node() { Title = "1.1", Shape = C1.Diagram.Shape.Circle };
            diagram.Nodes.Add(node);
            CreateSubNodes(diagram, node, 0, () => 3, 1);
        }

        public FlexDiagram Diagram => diagram;

        public static void CreateSubNodes(FlexDiagram diagram, Node node, int level, Func<int> n, int maxLevel = 2, C1.Diagram.Shape shape = C1.Diagram.Shape.Circle)
        {
            for (var i = 0; i < n(); i++)
            {
                var subNode = new Node() { Title = $"{level + 2}.{i + 1}", Shape = shape };
                diagram.Nodes.Add(subNode);
                diagram.Edges.Add(new Edge() { Source = node, Target = subNode });

                if (level < maxLevel)
                    CreateSubNodes(diagram, subNode, level + 1, n, maxLevel, shape);
            }
        }
    }
}
