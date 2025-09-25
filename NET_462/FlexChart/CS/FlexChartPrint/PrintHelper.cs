using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

using System.Windows.Xps.Packaging;

using C1.WPF.Chart;

namespace FlexChartPrint
{
    public class PrintPageEventArgs : EventArgs
    {
        private int pageNumber;
        private UIElement visual;
        private Rect printableArea;

        public PrintPageEventArgs(int pageNumber, Rect printableArea)
        {
            this.pageNumber = pageNumber;
            this.printableArea = printableArea;
        }

        public int PageNumber
        {
            get
            {
                return pageNumber;
            }
        }

        public UIElement Visual
        {
            get
            {
                return visual;
            }
            set
            {
                visual = value;
            }
        }

        public Rect PrintableArea
        {
            get
            {
                return printableArea;
            }
        }
    }

    public class PrintHelper
    {
        private FlexChartBase chart;
        private int npages;
        private string _previewWindowXaml =
@"<Window
        xmlns                 ='http://schemas.microsoft.com/netfx/2007/xaml/presentation'
        xmlns:x               ='http://schemas.microsoft.com/winfx/2006/xaml'
        Title                 ='Print Preview - @@TITLE'
        WindowStartupLocation ='CenterOwner'>
        <DocumentViewer Name='dv1'/>
     </Window>";

        public PrintHelper(int npages = 1, FlexChartBase chart = null)
        {
            this.npages = npages;
            this.chart = chart;
        }

        public Canvas ChartToCanvas(FlexChartBase chart, Rect rect)
        {
            // save chart position
            var visualParent = chart.Parent as Visual;
            var chartRect = new Rect(0, 0, chart.ActualWidth, chart.ActualHeight);
            if (visualParent != null)
                chartRect = chart.TransformToVisual(visualParent).TransformBounds(chartRect);

            chartRect.X -= chart.Margin.Left;
            chartRect.Y -= chart.Margin.Top;
            chartRect.Width += chart.Margin.Left + chart.Margin.Right;
            chartRect.Height += chart.Margin.Top + chart.Margin.Bottom;

            chart.Measure(rect.Size);
            chart.Arrange(rect);
            chart.UpdateLayout();

            var canvasChild = FindVisualChild<Canvas>(chart);

            var clone = Clone(canvasChild);
            Canvas.SetLeft(clone, rect.Left);
            Canvas.SetTop(clone, rect.Top);

            // restore chart position
            chart.Measure(chartRect.Size);
            chart.Arrange(chartRect);
            chart.UpdateLayout();

            return clone;
        }

        public void PrintPreview(string title = "FlexChart")
        {
            var dlg = new PrintDialog();

            var caps = dlg.PrintQueue.GetPrintCapabilities(dlg.PrintTicket);
            var rect = new Rect(caps.PageImageableArea.OriginWidth, caps.PageImageableArea.OriginHeight,
                caps.PageImageableArea.ExtentWidth, caps.PageImageableArea.ExtentHeight);

            var fileName = System.IO.Path.GetRandomFileName();

            try
            {
                var fdoc = new FlowDocument();

                var sz = rect.Size;
                fdoc.PagePadding = new Thickness(0);
                fdoc.PageWidth = sz.Width;
                fdoc.PageHeight = sz.Height;

                for (var i = 0; i < npages; i++)
                {
                    var sec = new Section();
                    sec.BreakPageBefore = true;

                    var args = new PrintPageEventArgs(i+1, rect);
                    OnPagePrinting(this, args);

                    var visual = args.Visual;
                    if(visual == null && chart!=null)
                        visual = ChartToCanvas(chart, rect);
                    sec.Blocks.Add(new BlockUIContainer(visual));
                    fdoc.Blocks.Add(sec);

                    OnPagePrinted(this, args);
                }

                // write the XPS document
                using (var doc = new XpsDocument(fileName, FileAccess.ReadWrite))
                {
                    var writer = XpsDocument.CreateXpsDocumentWriter(doc);
                    DocumentPaginator paginator = ((IDocumentPaginatorSource)fdoc).DocumentPaginator;
                    writer.Write(paginator);
                }

                // Read the XPS document into a dynamically generated
                // preview Window 
                using (var doc = new XpsDocument(fileName, FileAccess.Read))
                {
                    FixedDocumentSequence fds = doc.GetFixedDocumentSequence();

                    string s = _previewWindowXaml;
                    s = s.Replace("@@TITLE", title.Replace("'", "&apos;"));

                    using (var reader = new System.Xml.XmlTextReader(new StringReader(s)))
                    {
                        var preview = System.Windows.Markup.XamlReader.Load(reader) as Window;

                        var dv1 = LogicalTreeHelper.FindLogicalNode(preview, "dv1") as DocumentViewer;
                        dv1.Document = fds as IDocumentPaginatorSource;

                        preview.ShowDialog();
                    }
                }
            }
            finally
            {
                if (File.Exists(fileName))
                {
                    try
                    {
                        File.Delete(fileName);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public event EventHandler<PrintPageEventArgs> PagePrinting;
        public event EventHandler<PrintPageEventArgs> PagePrinted;

        private void OnPagePrinting(object sender, PrintPageEventArgs args)
        {
            if(PagePrinting!=null)
                PagePrinting.Invoke(sender, args);
        }

        private void OnPagePrinted(object sender, PrintPageEventArgs args)
        {
            if(PagePrinted!=null)
                PagePrinted.Invoke(sender, args);
        }

        private Canvas Clone(Canvas source)
        {
            var canvas = new Canvas() { Width = source.ActualWidth, Height = source.ActualHeight };

            foreach (UIElement child in source.Children)
            {
                var xaml = System.Windows.Markup.XamlWriter.Save(child);
                var deepCopy = System.Windows.Markup.XamlReader.Parse(xaml) as UIElement;
                canvas.Children.Add(deepCopy);
            }

            return canvas;
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                {
                    return child as T;
                }
                else
                {
                    T result = FindVisualChild<T>(child);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }
   }
}
