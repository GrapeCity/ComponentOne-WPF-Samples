using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.FlexGrid;
using System.IO;
using C1.WPF.Pdf;
using System.Windows.Controls;
using System.Windows;

namespace MultiGridPdf
{
    internal static class GridExport
    {
        //-------------------------------------------------------------------------------------------------------
        #region ** object model

        public static void SavePdf(C1FlexGrid flex, Stream s)
        {
            var options = new PdfExportOptions();
            SavePdf(flex, s, options);
        }
        public static void SavePdf(C1FlexGrid flex, Stream s, PdfExportOptions options)
        {
            var pdf = new C1PdfDocument();
            options.KnownPageCount = false;
            RenderGrid(pdf, flex, options);

            // save the PDF document and close the stream
            pdf.Save(s);
            s.Close();
        }

        public static void RenderGrid(C1PdfDocument pdf, C1FlexGrid flex)
        {
            RenderGrid(pdf, flex, null);
        }
        public static void RenderGrid(C1PdfDocument pdf, C1FlexGrid flex, PdfExportOptions options)
        {
            // get rendering options
            if (options == null)
            {
                options = new PdfExportOptions();
            }

            // get root element to lay out the PDF pages
            Panel root = null;
            for (var parent = flex.Parent as FrameworkElement; parent != null; parent = parent.Parent as FrameworkElement)
            {
                if (parent is Panel)
                {
                    root = parent as Panel;
                }
            }

            // get page size
            var rc = pdf.PageRectangle;

            // create panel to hold elements while they render
            var pageTemplate = new PageTemplate();
            pageTemplate.Width = rc.Width;
            pageTemplate.Height = rc.Height;
            pageTemplate.SetPageMargin(options.Margin);
            root.Children.Add(pageTemplate);

            // render grid into PDF document
            var m = options.Margin;
            var sz = new Size(rc.Width - m.Left - m.Right, rc.Height - m.Top - m.Bottom);
            var pages = flex.GetPageImages(options.ScaleMode, sz, 100);
            for (int i = 0; i < pages.Count; i++)
            {
                // skip a page when necessary
                if (i > 0)
                {
                    pdf.NewPage();
                }

                // set content
                pageTemplate.PageContent.Child = pages[i];
                pageTemplate.PageContent.Stretch = options.ScaleMode == ScaleMode.ActualSize
                    ? System.Windows.Media.Stretch.None
                    : System.Windows.Media.Stretch.Uniform;

                // set header/footer text
                pageTemplate.HeaderLeft.Text = options.DocumentTitle;
                if (options.KnownPageCount)
                {
                    pageTemplate.FooterRight.Text = string.Format("Page {0} of {1}",
                        pdf.CurrentPage + 1, pages.Count);
                }
                else
                {
                    pageTemplate.FooterRight.Text = string.Format("Page {0}",
                        pdf.CurrentPage + 1);
                }

                // measure page element
                pageTemplate.UpdateLayout();
                pageTemplate.Arrange(new Rect(0, 0, rc.Width, rc.Height));

                // add to PDF
                pdf.DrawElement(pageTemplate, rc);
            }

            // done with template
            root.Children.Remove(pageTemplate);
        }

        #endregion
    }
}
