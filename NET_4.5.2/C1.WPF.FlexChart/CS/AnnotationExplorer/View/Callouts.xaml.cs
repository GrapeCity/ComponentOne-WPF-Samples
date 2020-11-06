using C1.Chart.Annotation;
using C1.WPF.Chart;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Annotation = C1.WPF.Chart.Annotation;
using C1.Chart;

namespace AnnotationExplorer
{
    /// <summary>
    /// Interaction logic for Callouts.xaml
    /// </summary>
    public partial class Callouts : UserControl
    {
        AnnotationViewModel _model = new AnnotationViewModel();
        IRenderEngine _engine;

        public Callouts()
        {
            InitializeComponent();
            DataContext = _model;
            flexChart.Rendering += FlexChart_Rendering;
        }

        private void FlexChart_Rendering(object sender, RenderEventArgs e)
        {
            if (_engine == null)
            {
                _engine = e.Engine;
                SetupAttotations();
            }
        }

        void SetupAttotations()
        {
            annotationLayer.Annotations.Clear();

            var contentCenter = new Point(25, -50);
            var arrowCallout = new Annotation.Polygon()
            {
                Content = "Low",
                Style = new ChartStyle()
                {
                    Fill = new SolidColorBrush(Colors.Green) { Opacity = 200.0 / 255 },
                    Stroke = new SolidColorBrush(Colors.Green),
                },
                Attachment = AnnotationAttachment.DataIndex,
                PointIndex = 1,
                ContentCenter = contentCenter,
                Points = GetPointsForArrowCallout(contentCenter.X, contentCenter.Y, "Low")
            };

            var lineCallout = new Annotation.Polygon()
            {
                Content = "High",
                Style = new ChartStyle()
                {
                    Fill = new SolidColorBrush(Colors.Red) { Opacity = 200.0 / 255 },
                    Stroke = new SolidColorBrush(Colors.Red),
                },
                Attachment = AnnotationAttachment.DataIndex,
                PointIndex = 4,
                ContentCenter = new Point(-50, 75),
                Points = new PointCollection(new Point[] { new Point(0, 0), new Point(-50, 50), new Point(-100, 50), new Point(-100, 100), new Point(0, 100), new Point(0, 50), new Point(-50, 50) })
            };

            annotationLayer.Annotations.Add(arrowCallout);
            annotationLayer.Annotations.Add(lineCallout);
        }

        /// <summary>
        /// This is used to automatically generate the collection of endpoints of an arrow callout. 
        /// </summary>
        /// <param name="centerX">The center x of the content rectangle.</param>
        /// <param name="centerY">The center y of the content rectangle.</param>
        /// <param name="content">The content of this callout. Will automatically adjust the content rectangle according to this.</param>
        /// <returns>The endpoints of an arrow callout.</returns>
        PointCollection GetPointsForArrowCallout(double centerX, double centerY, string content)
        {
            _Size size = _engine.MeasureString(content);
            return GetPointsForArrowCallout(centerX, centerY, (float)size.Width + 10, (float)size.Height + 10);
        }

        /// <summary>
        /// This is used to automatically generate the collection of endpoints of an arrow callout. 
        /// </summary>
        /// <param name="centerX">The center x of the content rectangle.</param>
        /// <param name="centerY">The center y of the content rectangle.</param>
        /// <param name="rectWidth">The width of the content rectangle.</param>
        /// <param name="rectHeight">The height of the content rectangle.</param>
        /// <returns>The endpoints of an arrow callout.</returns>
        PointCollection GetPointsForArrowCallout(double centerX, double centerY, double rectWidth, double rectHeight)
        {
            var points = new PointCollection();

            double rectLeft = centerX - rectWidth / 2;
            double rectRight = centerX + rectWidth / 2;
            double rectTop = centerY - rectHeight / 2;
            double rectBottom = centerY + rectHeight / 2;

            double angle = Math.Atan2(-centerY, centerX);
            double angleOffset1 = 0.4;
            double angleOffset2 = 0.04;
            double arrowHeight = 0.4 * rectHeight;
            double hypotenuse = arrowHeight / Math.Cos(angleOffset1);
            double subHypotenuse = arrowHeight / Math.Cos(angleOffset2);

            bool isNearBottom = Math.Abs(rectTop) > Math.Abs(rectBottom);
            double nearHorizontalEdge = isNearBottom ? rectBottom : rectTop;
            bool isNearRight = Math.Abs(rectLeft) > Math.Abs(rectRight);
            double nearVerticalEdge = isNearRight ? rectRight : rectLeft;
            bool isHorizontalCrossed = Math.Abs(nearHorizontalEdge) > Math.Abs(nearVerticalEdge);
            double nearEdge = isHorizontalCrossed ? nearHorizontalEdge : nearVerticalEdge;

            int factor = nearEdge > 0 ? -1 : 1;
            double crossedPointOffsetToCenter = isHorizontalCrossed ? 
                rectHeight / (2 * Math.Tan(angle)) * factor : rectWidth * Math.Tan(angle) * factor / 2;

            // Arrow points
            points.Add(new Point(0, 0));
            points.Add(new Point(Math.Cos(angle + angleOffset1) * hypotenuse, -Math.Sin(angle + angleOffset1) * hypotenuse));
            points.Add(new Point(Math.Cos(angle + angleOffset2) * subHypotenuse, -Math.Sin(angle + angleOffset2) * subHypotenuse));

            // Rectangle points
            if (isHorizontalCrossed)
            {
                points.Add(new Point(-nearEdge / Math.Tan(angle + angleOffset2), nearEdge));
                if (isNearBottom)
                {
                    points.Add(new Point(rectLeft, rectBottom));
                    points.Add(new Point(rectLeft, rectTop));
                    points.Add(new Point(rectRight, rectTop));
                    points.Add(new Point(rectRight, rectBottom));
                }
                else
                {
                    points.Add(new Point(rectRight, rectTop));
                    points.Add(new Point(rectRight, rectBottom));
                    points.Add(new Point(rectLeft, rectBottom));
                    points.Add(new Point(rectLeft, rectTop));
                }
                points.Add(new Point(-nearEdge / Math.Tan(angle - angleOffset2), nearEdge));
            }
            else
            {
                points.Add(new Point(nearEdge, - nearEdge * Math.Tan(angle + angleOffset2)));
                if (isNearRight)
                {
                    points.Add(new Point(rectRight, rectBottom));
                    points.Add(new Point(rectLeft, rectBottom));
                    points.Add(new Point(rectLeft, rectTop));
                    points.Add(new Point(rectRight, rectTop));
                }
                else
                {
                    points.Add(new Point(rectLeft, rectTop));
                    points.Add(new Point(rectRight, rectTop));
                    points.Add(new Point(rectRight, rectBottom));
                    points.Add(new Point(rectLeft, rectBottom));
                }
                points.Add(new Point(nearEdge, - nearEdge * Math.Tan(angle - angleOffset2)));
            }

            // Arrow points
            points.Add(new Point(Math.Cos(angle - angleOffset2) * subHypotenuse, -Math.Sin(angle - angleOffset2) * subHypotenuse));
            points.Add(new Point(Math.Cos(angle - angleOffset1) * hypotenuse, -Math.Sin(angle - angleOffset1) * hypotenuse));
            return points;
        }
    }
}
