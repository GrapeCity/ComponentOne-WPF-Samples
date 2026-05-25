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

using DiagramExplorer.ViewModel;
using C1.WPF.Diagram;

#pragma warning disable 1591
namespace DiagramExplorer.View
{
    /// <summary>
    /// Interaction logic for Styling.xaml
    /// </summary>
    public partial class Styling : UserControl, IDiagramHolder
    {
        public Styling()
        {
            InitializeComponent();
        }

        public FlexDiagram Diagram => diagram;
    }
}
