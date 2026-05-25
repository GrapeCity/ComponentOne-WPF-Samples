using C1.WPF.Chart;
using C1.WPF.Diagram;
using DiagramExplorer.Data;
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

#pragma warning disable 1591
namespace DiagramExplorer.View
{
    /// <summary>
    /// Interaction logic for Animals.xaml
    /// </summary>
    public partial class Animals : UserControl, IDiagramHolder
    {
        public Animals()
        {
            InitializeComponent();

            diagram.ItemsSource = DataService.GetService().GetAnimalData(); 
        }

        public FlexDiagram Diagram => diagram;

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            diagram.NodeTemplate = checkBoxTemplate.IsChecked == true ? FindResource("NodeTemplate") as DataTemplate : null;
            ;
        }
    }
}
