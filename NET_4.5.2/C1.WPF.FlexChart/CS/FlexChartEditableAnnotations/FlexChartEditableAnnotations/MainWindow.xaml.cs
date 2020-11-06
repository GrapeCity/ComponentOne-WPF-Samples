using C1.Chart.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using C1.WPF.Toolbar;
using C1.WPF.Chart.Interaction;
using System.IO;
using System.Windows.Controls.Primitives;
using C1.WPF.Chart.Annotation;

namespace FlexChartEditableAnnotations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EditableAnnotationLayer al;
        private List<CustomC1ToolbarButton> _lsAttachmentGroup, _lsAnnotationGroup;
        private C1AxisScrollbar _scrollbar;
        private ContextMenu _flexChartContextMenu;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;

            Init();
        }

        private void Init()
        {
            _lsAttachmentGroup = new List<CustomC1ToolbarButton>{
                c1tbAbsolute,c1tbDataCoordinate,c1tbRelative
            };

            _lsAnnotationGroup = new List<CustomC1ToolbarButton>{
                c1tbText,c1tbCircle,c1tbEllipse,c1tbRectangle,c1tbSquare,c1tbLine,c1tbPolygon
            };

            this.Title = Localizer.GetItem("introduction", "title");
            tbTitle.Text = Localizer.GetItem("introduction", "title");

            c1tbAbsolute.ToolTip = Localizer.GetItem("Absolute", "description");
            c1tbRelative.ToolTip = Localizer.GetItem("Relative", "description");
            c1tbDataCoordinate.ToolTip = Localizer.GetItem("DataCoordinate", "description");
            c1tbAllowMove.ToolTip = Localizer.GetItem("allowMove", "description");
            c1tbText.ToolTip = Localizer.GetItem("textAnno", "description");
            c1tbLine.ToolTip = Localizer.GetItem("lineAnno", "description");
            c1tbCircle.ToolTip = Localizer.GetItem("circleAnno", "description");
            c1tbEllipse.ToolTip = Localizer.GetItem("ellipseAnno", "description");
            c1tbRectangle.ToolTip = Localizer.GetItem("rectAnno", "description");
            c1tbSquare.ToolTip = Localizer.GetItem("squareAnno", "description");
            c1tbPolygon.ToolTip = Localizer.GetItem("polygonAnno", "description");

            c1tbAbsolute.Tag = AnnotationAttachment.Absolute;
            c1tbRelative.Tag = AnnotationAttachment.Relative;
            c1tbDataCoordinate.Tag = AnnotationAttachment.DataCoordinate;
            c1tbAllowMove.Tag = "AllowMove";
            c1tbText.Tag = "Text";
            c1tbLine.Tag = "Line";
            c1tbCircle.Tag = "Circle";
            c1tbEllipse.Tag = "Ellipse";
            c1tbRectangle.Tag = "Rectangle";
            c1tbSquare.Tag = "Square";
            c1tbPolygon.Tag = "Polygon";

            string rtf = Localizer.GetItem("introduction", "description").MakeRtf();
            Localizer.ClearDocument();

            rtbDescription.Selection.Load(new MemoryStream(Encoding.UTF8.GetBytes(rtf)), DataFormats.Rtf);
            rtbDescription.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, 10d);
            rtbDescription.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, "Segoe UI");

            InitChart();

            c1tbAbsolute.Checked = true;
            c1tbAllowMove.Checked = true;
        }

        private void InitChart()
        {
            var ls = new List<Point>();
            Random r = new Random();

            for (int i = 0; i < 100; i++)
            {
                ls.Add(new Point(i, r.Next(0, 1000)));
            }

            _flexChartContextMenu = new ContextMenu();
            MenuItem mi = new MenuItem() { Header = "Delete" };
            mi.Click += OnDeleteClicked;
            _flexChartContextMenu.Items.Add(mi);

            flexChart1.ItemsSource = ls;
            flexChart1.ChartType = C1.Chart.ChartType.Scatter;
            flexChart1.ToolTip.Visibility = Visibility.Collapsed;
            flexChart1.Series.Clear();
            flexChart1.Series.Add(new C1.WPF.Chart.Series
            {
                Binding = "Y",
                BindingX = "X"
            });

            #region Annotations Setup

            al = new EditableAnnotationLayer(flexChart1);

            al.Annotations.Add(new C1.WPF.Chart.Annotation.Text
            {
                Attachment = AnnotationAttachment.Relative,
                Content = "Text Annotation",
                Location = new Point(0.55, 0.15),
                Style = new ChartStyle() { FontSize = 12, Stroke = Brushes.Black }
            });

            al.Annotations.Add(new C1.WPF.Chart.Annotation.Rectangle
            {
                Attachment = AnnotationAttachment.Absolute,
                Content = "Absolute",
                Location = new Point(100, 150),
                Width = 100,
                Height = 50,
                Style = new ChartStyle() { Fill = Brushes.Red }
            });

            al.Annotations.Add(new C1.WPF.Chart.Annotation.Ellipse
            {
                Attachment = AnnotationAttachment.Relative,
                Location = new Point(0.5f, 0.35f),
                Content = "Relative",
                Width = 100,
                Height = 50,
                Style = new ChartStyle() { Fill = Brushes.Red }
            });

            al.Annotations.Add(new C1.WPF.Chart.Annotation.Square
            {
                Attachment = AnnotationAttachment.DataCoordinate,
                Content = "DataCoordinate",
                Length = 100,
                Location = new Point(20, 200),
                Style = new ChartStyle() { Fill = Brushes.Red }
            });

            al.Annotations.Add(new C1.WPF.Chart.Annotation.Polygon("Polygon")
            {
                Attachment = AnnotationAttachment.Absolute,
                Points =
                {
                    new Point(300, 25),
                    new Point(250, 70),
                    new Point(275, 115),
                    new Point(325, 115),
                    new Point(350, 70)
                },
                Style = new ChartStyle() { Fill = Brushes.Red }
            });

            al.Annotations.Add(new C1.WPF.Chart.Annotation.Line
            {
                Attachment = AnnotationAttachment.Relative,
                Content = "Horizontal Line",
                Start = new Point(0, 0.5f),
                End = new Point(1.2f, 0.5f),
                Style = new ChartStyle() { Fill = Brushes.Red }
            });

            al.Annotations.Add(new C1.WPF.Chart.Annotation.Line
            {
                Attachment = AnnotationAttachment.Relative,
                Content = "Vertical Line",
                Start = new Point(0.7f, 0),
                End = new Point(0.7f, 1.2f),
                Style = new ChartStyle() { Fill = Brushes.Red }
            });

            al.Annotations.Add(new C1.WPF.Chart.Annotation.Line
            {
                Attachment = AnnotationAttachment.Absolute,
                Content = "Slanted Line",
                Start = new Point(100, 70),
                End = new Point(200, 170),
                Style = new ChartStyle() { Fill = Brushes.Red }
            });

            al.PolygonAddFunc = (pt) =>
            {
                return new C1.WPF.Chart.Annotation.Polygon(string.Empty)
                {
                    Points =
                    {
                        pt,pt,pt
                    }
                };
            };

            flexChart1.Layers.Add(al);

            al.PolygonReSizeFunc = (poly, rectangle) =>
            {
                var top = new Point((float)(rectangle.Left + rectangle.Width / 2), rectangle.Y);
                var left = new Point(rectangle.Left, rectangle.Bottom);
                var right = new Point(rectangle.Right, rectangle.Bottom);
                poly.Points[0] = Helpers.CoordsToAnnoPoint(flexChart1, poly, top);
                poly.Points[1] = Helpers.CoordsToAnnoPoint(flexChart1, poly, left);
                poly.Points[2] = Helpers.CoordsToAnnoPoint(flexChart1, poly, right);
            };

            al.ContentEditor = new AnnotationEditor();
            #endregion

            flexChart1.Rendered += FlexChart1_Rendered;
            flexChart1.MouseDown += FlexChart1_MouseDown;
            flexChart1.KeyDown += FlexChart1_KeyDown;
        }

        private void FlexChart1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                al.Annotations.Remove(al.SelectedAnnotation);
            }
        }

        private void FlexChart1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //flexChart1.Focus(); //KeyDown does not trigger without this            
            if (e.RightButton == MouseButtonState.Pressed)
            {
                var selectedAnno = al.HitTest(e.GetPosition(flexChart1));
                if (selectedAnno != null)
                {                    
                    flexChart1.ContextMenu = _flexChartContextMenu;
                    flexChart1.ContextMenu.IsOpen = true;
                }
                else
                    flexChart1.ContextMenu = null;
            }
        }

        private void FlexChart1_Rendered(object sender, RenderEventArgs e)
        {
            SetupAxisScrollbar();
        }

        private void SetupAxisScrollbar()
        {
            if (flexChart1.AxisX.Scrollbar != null)
                return;
            flexChart1.AxisX.Scrollbar = new C1AxisScrollbar();
        }

        private void C1ToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            var clickedItem = sender as CustomC1ToolbarButton;

            //Handle Annotation Attachments
            if (_lsAttachmentGroup.Contains(clickedItem))
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    foreach (var item in _lsAttachmentGroup)
                        item.Checked = item == clickedItem ? !item.Checked : false;
                    al.Attachment = (AnnotationAttachment)clickedItem.Tag;
                }));
            }

            //Handle adding new Annotations 
            else if (_lsAnnotationGroup.Contains(clickedItem))
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    foreach (var item in _lsAnnotationGroup)
                        item.Checked = item == clickedItem ? !item.Checked : false;
                    var annoToAdd = (string)clickedItem.Tag;
                    var asm = typeof(AnnotationBase).Assembly;
                    al.NewAnnotationType = asm.GetType(string.Format("C1.WPF.Chart.Annotation.{0}", annoToAdd));
                    al.AllowAdd = clickedItem.Checked;
                }));
            }
            //Allow Moving Annotations at runtime
            else if ((string)clickedItem.Tag == "AllowMove")
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    clickedItem.Checked = !clickedItem.Checked;
                    al.AllowMove = clickedItem.Checked;
                }));
            }
        }
        private void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            al.Annotations.Remove(al.SelectedAnnotation);
        }
    }
}
