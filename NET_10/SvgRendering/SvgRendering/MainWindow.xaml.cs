using C1.WebStandards.Svg;
using C1.WPF.Core.Svg;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SvgRendering
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public class SvgSurface : Control
    {
        private C1SvgDoc _doc;

        public SvgSurface()
        {
            var resourceStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/SvgRendering;component/car.svg")).Stream;
            _doc = C1SvgDoc.Parse(resourceStream);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawSvg(_doc, Foreground, new Rect(0, 0, ActualWidth, ActualHeight));
        }
    }
}