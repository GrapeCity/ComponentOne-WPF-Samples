using System.Windows.Controls;
using DiagramExplorer.ViewModel;
using C1.WPF.Diagram;
using DiagramExplorer.Data;
using C1.WPF.Chart;

#pragma warning disable 1591
namespace DiagramExplorer.View
{
    /// <summary>
    /// Interaction logic for NodeTemplate.xaml
    /// </summary>
    public partial class NodeTemplate : UserControl, IDiagramHolder
    {
        public NodeTemplate()
        {
            InitializeComponent();

            diagram.BeginUpdate();
            diagram.ItemsSource = new List<TypeInfo> { new TypeInfo(typeof(FlexChartBase), new[] { typeof(FlexDiagram).Assembly }) };
            diagram.ChildItemsPath = "Childs";
            diagram.EndUpdate();
        }

        public FlexDiagram Diagram => diagram;
    }
}
