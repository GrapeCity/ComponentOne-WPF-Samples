using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Reflection;
using C1.WPF.Chart;

namespace CurrencyComparison
{
    public class WatermarkAdorner : Adorner
    {
        FlexChart chart;
        BitmapImage bmp;

        public WatermarkAdorner(UIElement adornedElement) : base(adornedElement)
        {
            chart = adornedElement as FlexChart;
            var resource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/Resources/Img_WaterMark_C1Logo.png", UriKind.Relative));
            var stream = resource.Stream;
            if (stream != null)
            {
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = stream;
                bmp.EndInit();
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var plotRect = chart.PlotRect;
            double width = bmp.PixelWidth;
            double height = bmp.PixelHeight;
            double x = plotRect.X + (plotRect.Width - width) / 2;
            double y = plotRect.Y + (plotRect.Height - height) / 2;
            drawingContext.DrawImage(bmp, new Rect(x, y, width, height));
        }
    }
}
