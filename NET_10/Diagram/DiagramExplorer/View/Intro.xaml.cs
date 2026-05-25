using System.Windows.Controls;
using DiagramExplorer.ViewModel;
using C1.WPF.Diagram;

#pragma warning disable 1591
namespace DiagramExplorer.View
{
    /// <summary>
    /// Interaction logic for Intro.xaml
    /// </summary>
    public partial class Intro : UserControl, IDiagramHolder
    {
        public Intro()
        {
            InitializeComponent();
        }

        public FlexDiagram Diagram => diagram;
    }
}
