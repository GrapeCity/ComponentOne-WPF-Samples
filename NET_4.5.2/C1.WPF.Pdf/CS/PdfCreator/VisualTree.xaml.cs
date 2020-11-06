using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

using Microsoft.Win32;
using C1.WPF.Pdf;

namespace PdfCreator
{
    public partial class VisualTree : UserControl
    {
        public VisualTree()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            // get stream to save to
            var dlg = new SaveFileDialog();
            dlg.DefaultExt = ".pdf";
            var dr = dlg.ShowDialog();
            if (!dr.HasValue || !dr.Value)
            {
                return;
            }

            // get sender button
            var btn = sender as Button;

            // create document
            var pdf = new C1PdfDocument(PaperKind.Letter);
            pdf.Clear();

            // set document info
            var di = pdf.DocumentInfo;
            di.Author = "ComponentOne";
            di.Subject = "C1.WPF.Pdf demo.";
            di.Title = "Experimental VisualTree Exporter for PDF";

            // walk visual tree
            CreateDocumentVisualTree(pdf, content);

            // render footers
            // this reopens each page and adds content to them (now we know the page count).
            var font = new Font("Arial", 8, PdfFontStyle.Bold);
            var fmt = new StringFormat();
            fmt.Alignment = HorizontalAlignment.Right;
            fmt.LineAlignment = VerticalAlignment.Bottom;
            for (int page = 0; page < pdf.Pages.Count; page++)
            {
                pdf.CurrentPage = page;
                var text = string.Format("C1.WPF.Pdf: {0}, page {1} of {2}",
                    di.Title,
                    page + 1,
                    pdf.Pages.Count);
                pdf.DrawString(
                    text,
                    font,
                    Colors.DarkGray,
                    PdfUtils.Inflate(pdf.PageRectangle, -72, -36),
                    fmt);
            }

            // save document
            using (var stream = dlg.OpenFile())
            {
                pdf.Save(stream);
            }
            MessageBox.Show("Pdf Document saved to " + dlg.SafeFileName);
        }

        public BitmapSource CreateBitmap(FrameworkElement element)
        {
            int width = (int)Math.Ceiling(element.ActualWidth);
            int height = (int)Math.Ceiling(element.ActualHeight);

            width = width == 0 ? 1 : width;
            height = height == 0 ? 1 : height;

            RenderTargetBitmap rtbmp = new RenderTargetBitmap(
                width, height, 96, 96, PixelFormats.Default);
            rtbmp.Render(element);
            return rtbmp;
        }
        void CreateDocumentVisualTree(C1PdfDocument pdf, FrameworkElement targetElement)
        {
            // set up to render
            var font = new Font("Courier", 14);
            var img = new WriteableBitmap(CreateBitmap(targetElement));

            // go render
            bool firstPage = true;
            foreach (Stretch stretch in new Stretch[] { Stretch.Fill, Stretch.None, Stretch.Uniform, Stretch.UniformToFill })
            {
                // add page break
                if (!firstPage)
                {
                    pdf.NewPage();
                }
                firstPage = false;

                // set up to render
                var alignment = ContentAlignment.TopLeft;
                var rc = PdfUtils.Inflate(pdf.PageRectangle, -72, -72);
                rc.Height /= 2;

                // render element as image
                pdf.DrawString("Element as Image, Stretch: " + stretch.ToString(), font, Colors.Black, rc);
                rc = PdfUtils.Inflate(rc, -20, -20);
                pdf.DrawImage(img, rc, alignment, stretch);
                pdf.DrawRectangle(Colors.Green, rc);
                rc = PdfUtils.Inflate(rc, +20, +20);
                pdf.DrawRectangle(Colors.Green, rc);

                // move to bottom of the page
                rc = PdfUtils.Offset(rc, 0, rc.Height + 20);

                // render element 
                pdf.DrawString("Element as VisualTree, Stretch: " + stretch.ToString(), font, Colors.Black, rc);
                rc = PdfUtils.Inflate(rc, -20, -20);
                pdf.DrawElement(targetElement, rc, alignment, stretch);
                pdf.DrawRectangle(Colors.Green, rc);
                rc = PdfUtils.Inflate(rc, +20, +20);
                pdf.DrawRectangle(Colors.Green, rc);
            }

        }
    }
}
