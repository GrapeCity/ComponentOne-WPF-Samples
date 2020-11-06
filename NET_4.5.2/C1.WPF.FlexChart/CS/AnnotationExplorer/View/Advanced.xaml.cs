using C1.Chart.Annotation;
using C1.WPF.Chart;
using C1.WPF.Chart.Interaction;
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
using Annotation = C1.WPF.Chart.Annotation;

namespace AnnotationExplorer
{
    /// <summary>
    /// Interaction logic for Advanced.xaml
    /// </summary>
    public partial class Advanced : UserControl
    {
        AnnotationViewModel _model = new AnnotationViewModel();
        Annotation.Rectangle infoAnnotation;

        public Advanced()
        {
            InitializeComponent();
            this.DataContext = _model;
            this.Loaded += OnAdvancedLoaded;
        }

        private void OnAdvancedLoaded(object sender, RoutedEventArgs e)
        {
            SetupAttotations();
        }

        void SetupAttotations()
        {
            var financialData = _model.FinancialData;
            annotationLayer.Annotations.Clear();
            annotationLayer.Annotations.Add(new Annotation.Rectangle("", 10580, 1285)
            {
                Style = new ChartStyle()
                {
                    Fill = new SolidColorBrush(Colors.Green){ Opacity = 25.0 / 255 },
                    Stroke = new SolidColorBrush(Colors.Transparent),
                },
                ContentStyle = new ChartStyle()
                {
                    FontSize = 10,
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("GenericSansSerif")
                },
                Location = new Point((float)financialData[20].Date.ToOADate(), 100),
                Attachment = AnnotationAttachment.DataCoordinate,
                Position = AnnotationPosition.Right,
                
            });

            foreach (var data in financialData)
            {
                if (data.Volume >= 9)
                    annotationLayer.Annotations.Add(new Annotation.Square("D", 20)
                    {
                        Style = new ChartStyle()
                        {
                            Fill = new SolidColorBrush(Colors.Blue) { Opacity = 150.0 / 255 },
                            Stroke = new SolidColorBrush(Colors.Transparent),
                        },
                        ContentStyle = new ChartStyle()
                        {
                            FontSize = 10,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("GenericSansSerif"),
                            Stroke = new SolidColorBrush(Colors.White),
                        },
                        SeriesIndex = 1,
                        PointIndex = financialData.IndexOf(data),
                        Attachment = AnnotationAttachment.DataIndex,
                        TooltipText = "Dividend"
                    });
                if (data.Date.Day % 10 == 0)
                    annotationLayer.Annotations.Add(new Annotation.Square("E", 20)
                    {
                        Style = new ChartStyle()
                        {
                            Fill = new SolidColorBrush(Colors.Aqua) { Opacity = 150.0 / 255 },
                            
                        },
                        ContentStyle = new ChartStyle()
                        {
                            FontSize = 10,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("GenericSansSerif")
                        },
                        SeriesIndex = 0,
                        PointIndex = financialData.IndexOf(data),
                        Attachment = AnnotationAttachment.DataIndex,
                        TooltipText = "Close"
                    });
            }
            annotationLayer.Annotations.Add(new Annotation.Line("Rising wedge")
            {
                Style = new ChartStyle()
                {
                    Fill = new SolidColorBrush(Colors.Black),
                    Stroke = new SolidColorBrush(Colors.Aqua),
                },
                ContentStyle = new ChartStyle()
                {
                    FontSize = 12,
                    FontFamily = new FontFamily("GenericSansSerif")
                },
                Start = new Point((int)financialData[10].Date.ToOADate(), 20),
                End = new Point((int)financialData[40].Date.ToOADate(), 100),
                Attachment = AnnotationAttachment.DataCoordinate,
            });
            annotationLayer.Annotations.Add(new Annotation.Line("")
            {
                Style = new ChartStyle()
                {
                    Stroke = new SolidColorBrush(Colors.Aqua),
                },
                Start = new Point((int)financialData[20].Date.ToOADate(), 0),
                End = new Point((int)financialData[50].Date.ToOADate(), 80),
                Attachment = AnnotationAttachment.DataCoordinate,
            });
            annotationLayer.Annotations.Add(new Annotation.Image("pack://application:,,,/Resources/flag.png")
            {
                SeriesIndex = 0,
                PointIndex = 20,
                Attachment = AnnotationAttachment.DataIndex,
                Position = AnnotationPosition.Top
            });
            annotationLayer.Annotations.Add(new Annotation.Text("Facebook Inc to acquire LiveRail.")
            {
                Style = new ChartStyle()
                {
                    FontFamily = new FontFamily("GenericSansSerif"),
                    FontSize = 12
                },
                SeriesIndex = 0,
                PointIndex = 20,
                Attachment = AnnotationAttachment.DataIndex,
                Position = AnnotationPosition.Left,
            });
            annotationLayer.Annotations.Add(new Annotation.Image("pack://application:,,,/Resources/flag.png")
            {
                SeriesIndex = 0,
                PointIndex = 70,
                Attachment = AnnotationAttachment.DataIndex,
                Position = AnnotationPosition.Top,
            });
            annotationLayer.Annotations.Add(new Annotation.Text("Alibaba Group Holding Ltd")
            {
                Style = new ChartStyle()
                {
                    FontFamily = new FontFamily("GenericSansSerif"),
                    FontSize = 12
                },
                SeriesIndex = 0,
                PointIndex = 70,
                Attachment = AnnotationAttachment.DataIndex,
                Position = AnnotationPosition.Left,
            });
            annotationLayer.Annotations.Add(new Annotation.Image("pack://application:,,,/Resources/arrowDOWN.png")
            {
                SeriesIndex = 0,
                PointIndex = 30,
                Attachment = AnnotationAttachment.DataIndex,
                TooltipText = "Bid: $73.59"
            });
            annotationLayer.Annotations.Add(new Annotation.Image("pack://application:,,,/Resources/arrowUP.png")
            {
                SeriesIndex = 0,
                PointIndex = 50,
                Attachment = AnnotationAttachment.DataIndex,
                TooltipText = "Bid: $73.59"
            });
            infoAnnotation = new Annotation.Rectangle("", 120, 100)
            {
                Style = new ChartStyle()
                {
                    Fill = new SolidColorBrush(Colors.SandyBrown) { Opacity = 200.0 / 255 },
                    Stroke = new SolidColorBrush(Colors.Chocolate),
                },
                ContentStyle = new ChartStyle()
                {
                    FontFamily = new FontFamily("GenericSansSerif"),
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Stroke = new SolidColorBrush(Colors.Chocolate),
                },
                Location = new Point(130, 60),
                Attachment = AnnotationAttachment.Absolute,
            };
        }

        private void flexChart_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(flexChart);
            var dataList = _model.FinancialData;
            if (flexChart.PlotRect.Contains(pos))
            {
                var ht = flexChart.HitTest(pos);
                var low = dataList[ht.PointIndex].Low;
                var hight = dataList[ht.PointIndex].Hight;
                var open = dataList[ht.PointIndex].Open;
                var close = dataList[ht.PointIndex].Close;
                var volume = dataList[ht.PointIndex].Volume;
                infoAnnotation.Content = string.Format(
                    "Low={0}\r\nHigh={1}\r\nOpen={2}\r\nClose={3}\r\nVolume={4}", low, hight, open, close, volume
                    );

                if (!annotationLayer.Annotations.Contains(infoAnnotation))
                {
                    annotationLayer.Annotations.Add(infoAnnotation);
                }
                flexChart.Invalidate();
            }
        }

        private void flexChart_Rendered(object sender, RenderEventArgs e)
        {
            if (flexChart.AxisX.Scrollbar == null)
                flexChart.AxisX.Scrollbar = new C1AxisScrollbar();
        }
    }
}
