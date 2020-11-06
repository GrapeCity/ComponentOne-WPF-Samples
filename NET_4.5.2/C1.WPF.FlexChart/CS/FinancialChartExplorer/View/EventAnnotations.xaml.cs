using C1.Chart;
using C1.Chart.Annotation;
using C1.WPF.Chart;
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
using Rectangle = C1.WPF.Chart.Annotation.Rectangle;

namespace FinancialChartExplorer
{
    /// <summary>
    /// Interaction logic for EventAnnotations.xaml
    /// </summary>
    public partial class EventAnnotations : UserControl
    {
        DataService dataService = DataService.GetService();
        private List<Quote> _data;

        public EventAnnotations()
        {
            InitializeComponent();
            this.Loaded += EventAnnotations_Loaded;
        }

        private void EventAnnotations_Loaded(object sender, RoutedEventArgs e)
        {
            var annotations = dataService.GetData<Annotation>("box-annotations");
            foreach (var anno in annotations)
            {
                var rectangle = new Rectangle()
                {
                    Content = "E",
                    Width = 20,
                    Height = 20,
                    Attachment = AnnotationAttachment.DataIndex,
                    PointIndex = anno.DataIndex,
                    TooltipText = anno.Description == null ? "[b]" + anno.Title + "[/b]" : ("[b]" + anno.Title + "[/b]" + "\n" + anno.Description)
                };
                var fillColor = (Color)ColorConverter.ConvertFromString("#88bde6");
                rectangle.Style = new ChartStyle()
                {
                    Fill = new SolidColorBrush(fillColor)
                };
                annotationLayer.Annotations.Add(rectangle);
            }
        }

        public List<Quote> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = dataService.GetSymbolData("box");
                }

                return _data;
            }
        }

        private void rangeChanged(object sender, EventArgs e)
        {
            var yRange = dataService.FindRenderedYRange(Data, selector.LowerValue, selector.UpperValue);
            financialChart.BeginUpdate();
            financialChart.AxisX.Min = selector.LowerValue;
            financialChart.AxisX.Max = selector.UpperValue;
            financialChart.AxisY.Min = yRange.Min;
            financialChart.AxisY.Max = yRange.Max;
            financialChart.EndUpdate();
        }

        private void selectorChart_Rendered(object sender, RenderEventArgs e)
        {
            if (selector != null)
            {
                var axis = selectorChart.AxisX as IAxis;
                var min = axis.GetMin();
                var max = axis.GetMax();
                var range = dataService.FindApproxRange(min, max, 0.45);
                selector.LowerValue = range.Min;
                selector.UpperValue = range.Max;
            }
        }
    }
}
