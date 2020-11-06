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
using C1.WPF.Word;
using C1.Util;

namespace Word.Creator
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
            dlg.DefaultExt = ".rtf";
            var dr = dlg.ShowDialog();
            if (!dr.HasValue || !dr.Value)
            {
                return;
            }

            // get sender button
            var btn = sender as Button;

            // create document
            var rtf = new C1WordDocument();
            rtf.Clear();

            // set document info
            var di = rtf.Info;
            di.Author = "ComponentOne";
            di.Subject = "C1.WPF.Word demo.";
            di.Title = "Experimental VisualTree Exporter for RTF";
            var count = 5;

            // walk visual tree
            CreateDocumentVisualTree(rtf, content);

            // render footers
            // this reopens each page and adds content to them (now we know the page count).
            var font = new Font("Arial", 8, RtfFontStyle.Bold);
            var fmt = new StringFormat();
            fmt.Alignment = HorizontalAlignment.Right;
            fmt.LineAlignment = VerticalAlignment.Bottom;
            for (int page = 0; page < count; page++)
            {
                //rtf.CurrentPage = page;
                var sz = rtf.PageSize;
                var rc = new Rect(72, 72, sz.Width - 144, sz.Height - 144);
                var text = string.Format("C1.WPF.Rtf: {0}, page {1} of {2}",
                    di.Title,
                    page + 1,
                    count);
                var r = new Rect(72, 36, sz.Width - 144, sz.Height - 72);
                rtf.DrawString(
                    text,
                    font,
                    Colors.DarkGray,
                    r,
                    fmt);
            }

            // save document
            using (var stream = dlg.OpenFile())
            {
                rtf.Save(stream, FileFormat.Rtf);
            }
            MessageBox.Show("Word Document saved to " + dlg.SafeFileName);
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
        void CreateDocumentVisualTree(C1WordDocument rtf, FrameworkElement targetElement)
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
                    //rtf.NewPage();
                }
                firstPage = false;

                // set up to render
                var alignment = ContentAlignment.TopLeft;
                //var rc = WordUtils.Inflate(rtf.PageRectangle, -72, -72);
                var sz = rtf.PageSize;
                var rc = new Rect(72, 72, sz.Width - 144, sz.Height - 144);
                rc.Height /= 2;

                // render element as image
                rtf.DrawString("Element as Image, Stretch: " + stretch.ToString(), font, Colors.Black, rc);
                rc = WordUtils.Inflate(rc, -20, -20);
                //rtf.DrawImage(img, rc, alignment, stretch);
                rtf.DrawImage(img, rc);
                rtf.DrawRectangle(Colors.Green, rc);
                rc = WordUtils.Inflate(rc, +20, +20);
                rtf.DrawRectangle(Colors.Green, rc);

                // move to bottom of the page
                rc = WordUtils.Offset(rc, 0, rc.Height + 20);

                // render element 
                rtf.DrawString("Element as VisualTree, Stretch: " + stretch.ToString(), font, Colors.Black, rc);
                rc = WordUtils.Inflate(rc, -20, -20);
                rtf.DrawElement(targetElement, rc, alignment, stretch);
                rtf.DrawRectangle(Colors.Green, rc);
                rc = WordUtils.Inflate(rc, +20, +20);
                rtf.DrawRectangle(Colors.Green, rc);
            }
        }
    }
}
