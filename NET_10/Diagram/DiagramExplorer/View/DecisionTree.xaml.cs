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
    /// Interaction logic for DecisionTree.xaml
    /// </summary>
    public partial class DecisionTree : UserControl, IDiagramHolder
    {
        public DecisionTree()
        {
            InitializeComponent();

            Common.Samples.LoadDiagramFromResource(diagram, "decision-tree.mermaid");
        }

        public FlexDiagram Diagram => diagram;
    }
}
