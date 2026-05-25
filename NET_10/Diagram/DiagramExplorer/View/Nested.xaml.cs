using System.Windows.Controls;
using DiagramExplorer.ViewModel;
using C1.Diagram.Parser;
using C1.WPF.Diagram;
using DiagramExplorer.Common;

#pragma warning disable 1591
namespace DiagramExplorer.View
{
    /// <summary>
    /// Interaction logic for Nested.xaml
    /// </summary>
    public partial class Nested : UserControl, IDiagramHolder
    {
        public Nested()
        {
            InitializeComponent();
        }

        public FlexDiagram Diagram => diagram;

        private void FlexDiagram_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var diagram = sender as FlexDiagram;

            var node = diagram.DataContext as Node;

            if (node.Title != "System")
                Samples.LoadDiagramFromResource(diagram, $"{node.ID}.mermaid");
            else
                diagram.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
