using C1.Diagram;
using C1.Chart.Standard;
using C1.Chart.Drawing;
using C1.WPF.Diagram;
using DiagramExplorer.Data;
using DiagramExplorer.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for FlexChartFamily.xaml
    /// </summary>
    public partial class FlexChartFamily : UserControl, IDiagramHolder
    {
        public FlexChartFamily()
        {
            InitializeComponent();

            CreateFlexChartFamily(diagram);
        }

        public FlexDiagram Diagram => diagram;

        static Random rnd = new Random();

        public static void CreateFlexChartFamily(FlexDiagram diagram)
        {
            diagram.FontSize = 12;
            //diagram.Header.Content = "FlexChart Family";

            var nodes = diagram.Nodes;
            var edges = diagram.Edges;

            var w = 120;
            var h = 80;

            nodes.Add(new Node() { Text = "📈 FlexChart", Content = "6 Charting Components" });// 0
            nodes.Add(new Node() { Text = "📋 Table-based Data" }); // 1
            nodes.Add(new Node() { Appearance = NodeAppearance.Hidden });//
            nodes.Add(new Node() { Text = "🌳 Hierarchical Data" }); // 2

            nodes.Add(new Node() { Text = "📈 FlexChart", TitleImage = CreateFlexChart(w, h, Colors.Black, Colors.Transparent) });//3
            nodes.Add(new Node() { Text = "🥧 FlexPie", TitleImage = CreateFlexPie(w, h, Colors.Black, Colors.Transparent) });// 4
            nodes.Add(new Node() { Text = "📡 FlexRadar", 
                TitleImage = CreateRadarChart(w, h, Colors.Black, Colors.Transparent) 
            }); // 5

            nodes.Add(new Node() { Text = "🧱 TreeMap", 
                TitleImage = CreateTreeMap(w, h, Colors.Black, Colors.Transparent) 
            }); // 6
            nodes.Add(new Node() { Text = "💥 Sunburst", 
                TitleImage = CreateSunburstChart(w, h, Colors.Black, Colors.Transparent) 
            });// 7
            nodes.Add(new Node() { Text = "🔀 FlexDiagram", 
                TitleImage = CreateDiagram(w, h, Colors.Black, Colors.Transparent) 
            });// 7

            edges.Add(new Edge() { Source = nodes[0], Target = nodes[1] });
            edges.Add(new Edge() { Source = nodes[0], Target = nodes[2] });
            edges.Add(new Edge() { Source = nodes[0], Target = nodes[3] });

            edges.Add(new Edge() { Source = nodes[1], Target = nodes[4] });
            edges.Add(new Edge() { Source = nodes[1], Target = nodes[5] });
            edges.Add(new Edge() { Source = nodes[1], Target = nodes[6] });

            edges.Add(new Edge() { Source = nodes[3], Target = nodes[7] });
            edges.Add(new Edge() { Source = nodes[3], Target = nodes[8] });
            edges.Add(new Edge() { Source = nodes[3], Target = nodes[9] });

            foreach (var node in nodes)
            {
                node.TitleDirection = C1.Chart.Direction.Vertical;
                node.TitleOrder = C1.Chart.LabelOrder.TextImage;
                node.Shape = C1.Diagram.Shape.RoundedRectangle;
            }
            diagram.ScaleMode = C1.Diagram.ScaleMode.ScaleToFit;
        }

        static ImageSource CreateFlexChart(int w, int h, Color foreColor, Color backColor)
        {
            var pts = new List<Point>();
            for (var i = 0; i < 6; i++)
                pts.Add(new Point(i, rnd.Next(100)));

            var chart = new FlexChart() { Binding = "Y", BindingX = "X", DataSource = pts, 
                //ForeColor = foreColor, 
                //BackColor = backColor 
            };
            chart.Series.Add(new Series());
            //chart.Margin = new Padding(0);
            var ms = new MemoryStream();
            chart.SavePng(ms, w, h);
            return BitmapFrame.Create(ms, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
        }

        static ImageSource CreateFlexPie(int w, int h, Color foreColor, Color backColor)
        {
            var pts = new List<Point>();
            for (var i = 0; i < 4; i++)
                pts.Add(new Point(i, rnd.Next(100)));

            var chart = new FlexPie() { Binding = "Y", BindingName = "X", DataSource = pts, 
                //ForeColor = foreColor,
                //BackColor = backColor
            };
            chart.Legend.Position = C1.Chart.Position.None;
            //chart.Margin = new Padding(0);
            var ms = new MemoryStream();
            chart.SavePng(ms,  w, h);
            return BitmapFrame.Create(ms, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
        }

        static ImageSource CreateTreeMap(int w, int h, Color foreColor, Color backColor)
        {
            var treeMap = new TreeMap() { Binding = "Value", BindingName = "Name", 
                //ForeColor = foreColor, 
                //BackColor = backColor 
            };

            treeMap.DataSource = new object[] {
                            new { Name = "Group1", Value = 15 },
                            new { Name = "Group2", Value = 12},
                            new { Name = "Group3", Value = 8},
                        };
            //treeMap.Margin = new Padding(0);
            var ms = new MemoryStream();
            treeMap.SavePng(ms, w, h);
            return BitmapFrame.Create(ms, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad); 
        }

        static ImageSource CreateRadarChart(int w, int h, Color foreColor, Color backColor)
        {
            var chart = new FlexRadar() { 
                //ForeColor = foreColor, 
                //BackColor = backColor 
            };
            chart.Binding = "Value";
            chart.BindingX = "Name";

            for (int iser = 0; iser < 3; iser++)
            {
                var data = new List<object>();
                for (var i = 0; i < 6; i++)
                    data.Add(new { Name = $"S{i}", Value = rnd.NextDouble() });
                var ser = new RadarSeries() { Name = $"ser {iser}", DataSource = data };

                chart.Series.Add(ser);
            }
            //chart.Margin = new Padding(0);
            chart.Legend.Position = C1.Chart.Position.None;
            var ms = new MemoryStream();
            chart.SavePng(ms, w, h);
            return BitmapFrame.Create(ms, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
        }

        static ImageSource CreateSunburstChart(int w, int h, Color foreColor, Color backColor)
        {
            var sunburst = new Sunburst()
            {
                Binding = "sales",
                BindingName = "type",
                ChildItemsPath = "items",
                //ForeColor = foreColor,
                //BackColor = backColor
            };
            sunburst.Offset = 0.2;
            sunburst.DataLabel.Position = C1.Chart.PieLabelPosition.None;
            sunburst.Legend.Position = C1.Chart.Position.None;
            sunburst.DataSource = CreateHierarchicalData();
            //sunburst.Margin = new Padding(0);
            var ms = new MemoryStream();
            sunburst.SavePng(ms, w, h);
            return BitmapFrame.Create(ms, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
        }

        public class SalesDataItem
        {
            public string? type { get; set; }
            public double sales { get; set; }

            public SalesDataItem[]? items { get; set; }
        }

        static int rand() => rnd.Next(10, 100);

        public static SalesDataItem[] CreateHierarchicalData()
        {
            var data = new SalesDataItem[] {
                    new SalesDataItem {
                        type = "Electronics",
                        items = new SalesDataItem[] {
                            new SalesDataItem{
                                type = "Camera",
                                items = new SalesDataItem[]
                                {
                                        new SalesDataItem{ type = "Digital", sales = rand() },
                                        new SalesDataItem{ type = "Film", sales = rand() },
                                }
                            },
                            new SalesDataItem{
                                type = "Headphones",
                                items = new SalesDataItem[]
                                {
                                        new SalesDataItem{ type = "Earbud", sales = rand() },
                                        new SalesDataItem{ type = "Over-ear", sales = rand() },
                                        new SalesDataItem{ type = "On-ear", sales = rand() },
                                }
                            }
                        }
                    },
                    new SalesDataItem{
                        type = "Computers\n& Tablets",
                        items = new SalesDataItem[]
                        {
                            new SalesDataItem
                            {
                                type = "Desktops",
                                items = new SalesDataItem[]
                                {
                                    new SalesDataItem{ type = "All-in-ones", sales = rand() },
                                    new SalesDataItem{ type = "Minis", sales = rand() },
                                }
                            },
                            new SalesDataItem
                            {
                                type = "Laptops",
                                items = new SalesDataItem[]
                                {
                                    new SalesDataItem{ type = "2 in 1", sales = rand() },
                                    new SalesDataItem{ type = "Traditional", sales = rand() }
                                }
                            },
                        }
                    }
                };
            return data;
        }

        static ImageSource CreateDiagram(int w, int h, Color foreColor, Color backColor)
        {
            var diagram = new C1.Diagram.Standard.FlexDiagram() { 
                //ForeColor = foreColor, 
                //BackColor = backColor 
            };
            diagram.Legend.Position = C1.Chart.Position.None;
            var nodes = diagram.Nodes;
            var edges = diagram.Edges;

            for (var i = 0; i < 3; i++)
                nodes.Add(new C1.Diagram.Standard.Node() { Shape = C1.Diagram.Shape.Circle, LegendItem = $"{i + 1}" });
            edges.Add(new C1.Diagram.Standard.Edge() { Source = nodes[0], Target = nodes[1] });
            edges.Add(new C1.Diagram.Standard.Edge() { Source = nodes[0], Target = nodes[2] });
            var ms = new MemoryStream();
            diagram.SavePng(ms, w, h);
            return BitmapFrame.Create(ms, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
        }
    }
}
