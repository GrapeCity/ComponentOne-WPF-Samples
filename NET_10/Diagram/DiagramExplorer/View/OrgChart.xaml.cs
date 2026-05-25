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

# pragma warning disable 1591
namespace DiagramExplorer.View
{
    /// <summary>
    /// Interaction logic for OrgChart.xaml
    /// </summary>
    public partial class OrgChart : UserControl, IDiagramHolder
    {
        static Data.DataService dataService = new Data.DataService();

        public OrgChart()
        {
            InitializeComponent();

            var data = dataService.GetOrgChartData();

            var fontFamily = new FontFamily("Segoe UI");

            var nodeStyle = new ChartStyle() { FontFamily = fontFamily, FontSize = 12 };
            var nodeContentStyle = new ChartStyle()
            {
                FontFamily = fontFamily,
                FontSize = 10,
                FontStyle = FontStyles.Italic,
                StrokeThickness = 0f,
            };

            var headerStyle = new ChartStyle()
            {
                FontFamily = fontFamily,
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                StrokeThickness = 0,
            };

            diagram.NodeCreated += (s, a) =>
            {
                var orgNode = a.Data as OrgNode;
                var node = a.Node as C1.WPF.Diagram.Node;

                if (orgNode == null || node == null)
                    return;

                if (!string.IsNullOrEmpty(orgNode.FirstName))
                {
                    node.Title = $"{orgNode.FirstName}\n{orgNode.LastName}";
                    node.Content = orgNode.JobTitle;

                    node.TitleImage = orgNode.Image;
                    node.TitleImageSize = new Size(60, 80);
                    node.TitleStyle = nodeStyle;
                    node.NodeStyle = nodeContentStyle;
                }
                else
                {
                    node.Title = orgNode.Name;
                    node.TitleStyle = node.NodeStyle = headerStyle;
                }

                node.LegendItem = orgNode.Department;
                node.Shape = C1.Diagram.Shape.RoundedRectangle;
            };

            diagram.ItemsSource = data;
        }

        public FlexDiagram Diagram => diagram;
    }
}
