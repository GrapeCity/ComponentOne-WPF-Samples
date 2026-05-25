using C1.Diagram;
using C1.WPF.Chart;
using C1.WPF.Diagram;
using DiagramExplorer.Data;
using DiagramExplorer.ViewModel;
using System.Windows;
using System.Windows.Controls;

#pragma warning disable 1591
namespace DiagramExplorer.View
{
    /// <summary>
    /// Interaction logic for Collapsible.xaml
    /// </summary>
    public partial class Collapsible : UserControl, IDiagramHolder
    {
        public Collapsible()
        {
            InitializeComponent();

            diagram.NodeCreated += (s, args) =>
            {
                var node = args.Node;
                var country = args.Data as Country;
                node.Shape = C1.Diagram.Shape.RoundedRectangle;

                if (country != null && country.Flag != null)
                {
                    node.Tooltip = country.Name;
                    node.TitleImage = country.Flag;
                    node.TitleImageAlignment = C1.Chart.ContentAlignment.MiddleCenter;
                    node.TitleImageSize = new Size(16, 16);
                }

                if (country != null)
                {
                    args.ParentNode.LegendItem = node.LegendItem = country.Continent;
                    // hide low-level nodes
                    args.ParentNode.Appearance = node.Appearance = NodeAppearance.None;
                }
                else
                {
                    var text = args.Data.ToString();
                    node.Content = text == "World" ? "➖" : "➕";
                    node.LegendItem = text;
                }
            };

            diagram.ItemsSource = new DataService().GetCountryData();
            diagram.Binding = "World,Continent,Region,Code";

            diagram.MouseUp += (s, e) =>
            {
                var position = e.GetPosition(diagram);
                var info = diagram.HitTest(position.X, position.Y);
                if (info != null && info.Distance == 0 && info.Element is INode node)
                {
                    diagram.BeginUpdate();
                    ToggleNodeState(node);
                    diagram.EndUpdate();
                }
            };
        }

        public FlexDiagram Diagram => diagram;

        // expand or collapse the node
        static void ToggleNodeState(INode node)
        {
            var childs = GetChilds(node);

            if (childs.Count > 0)
            {
                var collapsed = childs[0].Appearance != NodeAppearance.Visible;

                if (collapsed)
                {
                    foreach (var child in childs)
                        child.Appearance = NodeAppearance.Visible;
                }
                else
                {
                    SetChildAppearance(node, NodeAppearance.None,
                        (n) =>
                        {
                            if (n.Content.Text == "➖")
                                n.Content.Text = "➕";
                        }
                    );

                    // the first child is hidden to keep node on the same layout layer
                    childs[0].Appearance = NodeAppearance.Hidden;
                }

                node.Content.Text = collapsed ? "➖" : "➕";
            }
        }

        static void SetChildAppearance(INode node, NodeAppearance appearance, Action<INode>? action = null)
        {
            var childNodes = GetChilds(node);

            foreach (var child in childNodes)
            {
                child.Appearance = appearance;
                SetChildAppearance(child, appearance, action);
                action?.Invoke(child);
            }
        }

        static List<INode> GetChilds(INode node)
        {
            var list = new List<INode>();
            var diagram = node.Diagram;
            var edges = diagram.GetEdges();

            foreach (var edge in edges)
            {
                if (edge.Source == node && edge.Target != null)
                    list.Add(edge.Target);
            }

            return list;
        }
    }
}