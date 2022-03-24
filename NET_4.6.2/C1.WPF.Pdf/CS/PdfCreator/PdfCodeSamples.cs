using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Globalization;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.IO;

using System.Windows.Media.Imaging;
using System.Reflection;

namespace PdfCreator
{
    using C1.WPF.Pdf;

    public static class PdfCodeSamples
    {
        public static void HandleButtonClick(object sender, RoutedEventArgs e)
        {
            // get sender button
            var btn = sender as Button;

            // create document
            var pdf = new C1PdfDocument(PaperKind.Letter);
            pdf.Clear();

            // set document info
            var di = pdf.DocumentInfo;
            di.Author = "ComponentOne";
            di.Subject = "C1.WPF.Pdf demo.";
            di.Title = (string)btn.Content;

            //// add some security
            //if (false)
            //{
            //    var si = pdf.Security;
            //    si.AllowPrint = false;
            //    si.AllowEditAnnotations = false;
            //    si.AllowEditContent = false;
            //    si.AllowCopyContent = false;
            //    //si.UserPassword = "test";
            //    //si.OwnerPassword = "test";
            //}

            //// set viewer preferences
            //if (false)
            //{
            //    var vp = pdf.ViewerPreferences;
            //    vp.CenterWindow = true;
            //    vp.FitWindow = true;
            //    vp.PageLayout = PageLayout.TwoColumnLeft;
            //    vp.PageMode = PageMode.FullScreen;
            //}

            // create document
            switch (di.Title)
            {
                case "Quotes":
                    CreateDocumentQuotes(pdf);
                    break;
                case "Tables":
                    CreateDocumentTables(pdf);
                    break;
                case "Images":
                    CreateDocumentImages(pdf);
                    break;
                case "Paper Sizes":
                    CreateDocumentPaperSizes(pdf);
                    break;
                case "Table of Contents":
                    CreateDocumentTOC(pdf);
                    break;
                case "Text Flow":
                    CreateDocumentTextFlow(pdf);
                    break;
                case "Text":
                    CreateDocumentText(pdf);
                    break;
                case "Graphics":
                    CreateDocumentGraphics(pdf);
                    break;
            }

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

            var w = new Preview();
            w.PdfDocument = pdf;
            w.Show();
        }

        //---------------------------------------------------------------------------------
        #region ** quotes

        static void CreateDocumentQuotes(C1PdfDocument pdf)
        {
            // calculate page rect (discounting margins)
            Rect rcPage = PdfUtils.PageRectangle(pdf);
            Rect rc = rcPage;

            // initialize output parameters
            Font hdrFont = new Font("Arial", 14, PdfFontStyle.Bold);
            Font titleFont = new Font("Arial", 24, PdfFontStyle.Bold);
            Font txtFont = new Font("Times New Roman", 10, PdfFontStyle.Italic);

            // add title
            rc = PdfUtils.RenderParagraph(pdf, pdf.DocumentInfo.Title, titleFont, rcPage, rc);

            // build document
            foreach (string s in GetQuotes())
            {
                string[] authorQuote = s.Split('\t');

                // render header (author)
                var author = authorQuote[0];
                rc.Y += 20;
                rc = PdfUtils.RenderParagraph(pdf, author, hdrFont, rcPage, rc, true);

                // render body text (quote)
                string text = authorQuote[1];
                rc.X = rcPage.X + 36; // << indent body text by 1/2 inch
                rc.Width = rcPage.Width - 40;
                rc = PdfUtils.RenderParagraph(pdf, text, txtFont, rcPage, rc);
                rc.X = rcPage.X; // << restore indent
                rc.Width = rcPage.Width;
                rc.Y += 12; // << add 12pt spacing after each quote
            }
        }

        static List<string> GetQuotes()
        {
            var list = new List<string>();

            using (var sr = new StreamReader(DataAccess.GetStream("quotes.txt")))
            {
                var quotes = sr.ReadToEnd();
                foreach (string quote in quotes.Split('*'))
                {
                    int pos = quote.IndexOf("\r\n");
                    if (pos > -1)
                    {
                        var q = string.Format("{0}\t{1}", quote.Substring(0, pos), quote.Substring(pos + 2).Trim());
                        list.Add(q);
                    }
                }
            }

            return list;
        }

        #endregion

        //---------------------------------------------------------------------------------
        #region ** tables

        static void CreateDocumentTables(C1PdfDocument pdf)
        {
            // get the data
            var ds = DataAccess.GetDataSet();

            // calculate page rect (discounting margins)
            Rect rcPage = PdfUtils.PageRectangle(pdf);
            Rect rc = rcPage;

            // add title
            Font titleFont = new Font("Tahoma", 24, PdfFontStyle.Bold);
            rc = PdfUtils.RenderParagraph(pdf, pdf.DocumentInfo.Title, titleFont, rcPage, rc, false);

            // render some tables
            RenderTable(pdf, rc, rcPage, ds.Tables["Customers"], new string[] { "CompanyName", "ContactName", "Country", "Address", "Phone" });
            pdf.NewPage();
            rc = rcPage;
            RenderTable(pdf, rc, rcPage, ds.Tables["Products"], new string[] { "ProductName", "QuantityPerUnit", "UnitPrice", "UnitsInStock", "UnitsOnOrder" });
            pdf.NewPage();
            rc = rcPage;
            RenderTable(pdf, rc, rcPage, ds.Tables["Employees"], new string[] { "FirstName", "LastName", "Country", "Notes" });
        }


        static Rect RenderTable(C1PdfDocument pdf, Rect rc, Rect rcPage, DataTable table, string[] fields)
        {
            // select fonts
            Font hdrFont = new Font("Tahoma", 10, PdfFontStyle.Bold);
            Font txtFont = new Font("Tahoma", 8);

            // build table
            pdf.AddBookmark(table.TableName, 0, rc.Y);
            rc = PdfUtils.RenderParagraph(pdf, "NorthWind " + table.TableName, hdrFont, rcPage, rc, false);

            // build table
            rc = RenderTableHeader(pdf, hdrFont, rc, fields);
            foreach (DataRow dr in table.Rows)
            {
                rc = RenderTableRow(pdf, txtFont, hdrFont, rcPage, rc, fields, dr);
            }

            // done
            return rc;
        }

        static Rect RenderTableHeader(C1PdfDocument pdf, Font font, Rect rc, string[] fields)
        {
            // calculate cell width (same for all columns)
            Rect rcCell = rc;
            rcCell.Width = rc.Width / fields.Length;
            rcCell.Height = 0;

            // calculate cell height (max of all columns)
            foreach (string field in fields)
            {
                var height = pdf.MeasureString(field, font, rcCell.Width).Height;
                rcCell.Height = Math.Max(rcCell.Height, height);
            }
            rcCell.Height += 6; // add 6 point margin

            // render header cells
            var fmt = new StringFormat();
            fmt.LineAlignment = VerticalAlignment.Center;
            foreach (string field in fields)
            {
                pdf.FillRectangle(Colors.Black, rcCell);
                pdf.DrawString(field, font, Colors.White, rcCell, fmt);
                rcCell = PdfUtils.Offset(rcCell, rcCell.Width, 0);
            }

            // update rectangle and return it
            return PdfUtils.Offset(rc, 0, rcCell.Height);
        }

        static Rect RenderTableRow(C1PdfDocument pdf, Font font, Font hdrFont, Rect rcPage, Rect rc, string[] fields, DataRow dr)
        {
            // calculate cell width (same for all columns)
            Rect rcCell = rc;
            rcCell.Width = rc.Width / fields.Length;
            rcCell.Height = 0;

            // calculate cell height (max of all columns)
            rcCell = PdfUtils.Inflate(rcCell, -4, 0);
            foreach (string field in fields)
            {
                string text = dr[field].ToString();
                var height = pdf.MeasureString(text, font, rcCell.Width).Height;
                rcCell.Height = Math.Max(rcCell.Height, height);
            }
            rcCell = PdfUtils.Inflate(rcCell, 4, 0); // add 4 point margin
            rcCell.Height += 2;

            // break page if we have to
            if (rcCell.Bottom > rcPage.Bottom)
            {
                pdf.NewPage();
                rc = RenderTableHeader(pdf, hdrFont, rcPage, fields);
                rcCell.Y = rc.Y;
            }

            // center vertically just to show how
            StringFormat fmt = new StringFormat();
            fmt.LineAlignment = VerticalAlignment.Center;

            // render data cells
            foreach (string field in fields)
            {
                // get content
                string text = dr[field].ToString();

                // set horizontal alignment
                double d;
                fmt.Alignment = (double.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out d))
                    ? HorizontalAlignment.Right
                    : HorizontalAlignment.Left;

                // render cell
                pdf.DrawRectangle(Colors.LightGray, rcCell);
                rcCell = PdfUtils.Inflate(rcCell, -4, 0);
                pdf.DrawString(text, font, Colors.Black, rcCell, fmt);
                rcCell = PdfUtils.Inflate(rcCell, 4, 0);
                rcCell = PdfUtils.Offset(rcCell, rcCell.Width, 0);
            }

            // update rectangle and return it
            return PdfUtils.Offset(rc, 0, rcCell.Height);
        }

        #endregion

        //---------------------------------------------------------------------------------
        #region ** images

        static void CreateDocumentImages(C1PdfDocument pdf)
        {
            // calculate page rect (discounting margins)
            Rect rcPage = PdfUtils.PageRectangle(pdf);

            // load image into writeable bitmap
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = DataAccess.GetStream("borabora.jpg");
            bi.EndInit();
            var wb = new WriteableBitmap(bi);

            // center image on page preserving aspect ratio
            pdf.DrawImage(wb, rcPage, ContentAlignment.MiddleCenter, Stretch.Uniform);
        }

        #endregion

        //---------------------------------------------------------------------------------
        #region ** paper sizes

        static void CreateDocumentPaperSizes(C1PdfDocument pdf)
        {
            // add title
            Font titleFont = new Font("Tahoma", 24, PdfFontStyle.Bold);
            Rect rc = PdfUtils.PageRectangle(pdf);
            PdfUtils.RenderParagraph(pdf, pdf.DocumentInfo.Title, titleFont, rc, rc, false);

            // create constant font and StringFormat objects
            Font font = new Font("Tahoma", 18);
            StringFormat sf = new StringFormat();
            sf.Alignment = HorizontalAlignment.Center;
            sf.LineAlignment = VerticalAlignment.Center;

            // create one page with each paper size
            bool firstPage = true;
            foreach (var fi in typeof(PaperKind).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                // Silverlight/Phone doesn't have Enum.GetValues
                PaperKind pk = (PaperKind)fi.GetValue(null);

                // skip custom size
                if (pk == PaperKind.Custom) continue;

                // add new page for every page after the first one
                if (!firstPage) pdf.NewPage();
                firstPage = false;

                // set paper kind and orientation
                pdf.PaperKind = pk;
                pdf.Landscape = !pdf.Landscape;

                // draw some content on the page
                rc = PdfUtils.PageRectangle(pdf);
                rc = PdfUtils.Inflate(rc, -6, -6);
                string text = string.Format("PaperKind: [{0}];\r\nLandscape: [{1}];\r\nFont: [Tahoma 18pt]",
                    pdf.PaperKind, pdf.Landscape);
                pdf.DrawString(text, font, Colors.Black, rc, sf);
                pdf.DrawRectangle(Colors.Black, rc);
            }
        }

        #endregion

        //---------------------------------------------------------------------------------
        #region ** table of contents

        static void CreateDocumentTOC(C1PdfDocument pdf)
        {
            // create pdf document
            pdf.DocumentInfo.Title = "Document with Table of Contents";

            // add title
            Font titleFont = new Font("Tahoma", 24, PdfFontStyle.Bold);
            Rect rcPage = PdfUtils.PageRectangle(pdf);
            Rect rc = PdfUtils.RenderParagraph(pdf, pdf.DocumentInfo.Title, titleFont, rcPage, rcPage, false);
            rc.Y += 12;

            // create nonsense document
            var bkmk = new List<string[]>();
            Font headerFont = new Font("Arial", 14, PdfFontStyle.Bold);
            Font bodyFont = new Font("Times New Roman", 11);
            for (int i = 0; i < 30; i++)
            {
                // create ith header (as a link target and outline entry)
                string header = string.Format("{0}. {1}", i + 1, BuildRandomTitle());
                rc = PdfUtils.RenderParagraph(pdf, header, headerFont, rcPage, rc, true, true);

                // save bookmark to build TOC later
                int pageNumber = pdf.CurrentPage + 1;
                bkmk.Add(new string[] { pageNumber.ToString(), header });

                // create some text
                rc.X += 36;
                rc.Width -= 36;
                for (int j = 0; j < 3 + _rnd.Next(20); j++)
                {
                    string text = BuildRandomParagraph();
                    rc = PdfUtils.RenderParagraph(pdf, text, bodyFont, rcPage, rc);
                    rc.Y += 6;
                }
                rc.X -= 36;
                rc.Width += 36;
                rc.Y += 20;
            }

            // start Table of Contents
            pdf.NewPage();					// start TOC on a new page
            int tocPage = pdf.CurrentPage;	// save page index (to move TOC later)
            rc = PdfUtils.RenderParagraph(pdf, "Table of Contents", titleFont, rcPage, rcPage, true);
            rc.Y += 12;
            rc.X += 30;
            rc.Width -= 40;

            // render Table of Contents
            Pen dottedPen = new Pen(Colors.Gray, 1.5f);
            dottedPen.DashStyle = DashStyle.Dot;
            StringFormat sfRight = new StringFormat();
            sfRight.Alignment = HorizontalAlignment.Right;
            rc.Height = bodyFont.Size * 1.2;
            foreach (string[] entry in bkmk)
            {
                // get bookmark info
                string page = entry[0];
                string header = entry[1];

                // render header name and page number
                pdf.DrawString(header, bodyFont, Colors.Black, rc);
                pdf.DrawString(page, bodyFont, Colors.Black, rc, sfRight);

#if true
                // connect the two with some dots (looks better than a dotted line)
                string dots = ". ";
                var wid = pdf.MeasureString(dots, bodyFont).Width;
                var x1 = rc.X + pdf.MeasureString(header, bodyFont).Width + 8;
                var x2 = rc.Right - pdf.MeasureString(page, bodyFont).Width - 8;
                var x = rc.X;
                for (rc.X = x1; rc.X < x2; rc.X += wid)
                {
                    pdf.DrawString(dots, bodyFont, Colors.Gray, rc);
                }
                rc.X = x;
#else 
				// connect with a dotted line (another option)
				var x1 = rc.X + pdf.MeasureString(header, bodyFont).Width + 5;
				var x2 = rc.Right - pdf.MeasureString(page, bodyFont).Width  - 5;
				var y  = rc.Top + bodyFont.Size;
				pdf.DrawLine(dottedPen, x1, y, x2, y);
#endif
                // add local hyperlink to entry
                pdf.AddLink("#" + header, rc);

                // move on to next entry
                rc = PdfUtils.Offset(rc, 0, rc.Height);
                if (rc.Bottom > rcPage.Bottom)
                {
                    pdf.NewPage();
                    rc.Y = rcPage.Y;
                }
            }

            // move table of contents to start of document
            PdfPage[] arr = new PdfPage[pdf.Pages.Count - tocPage];
            pdf.Pages.CopyTo(tocPage, arr, 0, arr.Length);
            pdf.Pages.RemoveRange(tocPage, arr.Length);
            pdf.Pages.InsertRange(0, arr);
        }

        static string BuildRandomTitle()
        {
            string[] a1 = "Learning|Explaining|Mastering|Forgetting|Examining|Understanding|Applying|Using|Destroying".Split('|');
            string[] a2 = "Music|Tennis|Golf|Zen|Diving|Modern Art|Gardening|Architecture|Mathematics|Investments|.NET|Java".Split('|');
            string[] a3 = "Quickly|Painlessly|The Hard Way|Slowly|Painfully|With Panache".Split('|');
            return string.Format("{0} {1} {2}", a1[_rnd.Next(a1.Length - 1)], a2[_rnd.Next(a2.Length - 1)], a3[_rnd.Next(a3.Length - 1)]);
        }

        static string BuildRandomParagraph()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 5 + _rnd.Next(10); i++)
            {
                sb.AppendFormat(BuildRandomSentence());
            }
            return sb.ToString();
        }
        static string BuildRandomSentence()
        {
            string[] a1 = "Artists|Movie stars|Musicians|Politicians|Computer programmers|Modern thinkers|Gardeners|Experts|Some people|Hockey players".Split('|');
            string[] a2 = "know|seem to think about|care about|often discuss|dream about|hate|love|despise|respect|long for|pay attention to|embrace".Split('|');
            string[] a3 = "the movies|chicken soup|tea|many things|sushi|my car|deep thoughs|tasteless jokes|vaporware|cell phones|hot dogs|ballgames".Split('|');
            string[] a4 = "incessantly|too much|easily|without reason|rapidly|sadly|randomly|vigorously|more than usual|with enthusiasm|shamelessly|on Tuesdays".Split('|');
            return string.Format("{0} {1} {2} {3}. ", a1[_rnd.Next(a1.Length - 1)], a2[_rnd.Next(a2.Length - 1)], a3[_rnd.Next(a3.Length - 1)], a4[_rnd.Next(a4.Length - 1)]);
        }
        static Random _rnd = new Random();

        #endregion

        //---------------------------------------------------------------------------------
        #region ** text flow

        static void CreateDocumentTextFlow(C1PdfDocument pdf)
        {
            // load long string from resource file
            string text = "Resource not found...";
            using (var sr = new StreamReader(DataAccess.GetStream("flow.txt")))
            {
                text = sr.ReadToEnd();
            }
            text = text.Replace("\t", "   ");
            text = string.Format("{0}\r\n\r\n---oOoOoOo---\r\n\r\n{0}", text);

            // create pdf document
            pdf.DocumentInfo.Title = "Text Flow";

            // add title
            Font titleFont = new Font("Tahoma", 24, PdfFontStyle.Bold);
            Font bodyFont = new Font("Tahoma", 9);
            Rect rcPage = PdfUtils.PageRectangle(pdf);
            Rect rc = PdfUtils.RenderParagraph(pdf, pdf.DocumentInfo.Title, titleFont, rcPage, rcPage, false);
            rc.Y += titleFont.Size + 6;
            rc.Height = rcPage.Height - rc.Y;

            // create two columns for the text
            Rect rcLeft = rc;
            rcLeft.Width = rcPage.Width / 2 - 12;
            rcLeft.Height = 300;
            rcLeft.Y = (rcPage.Y + rcPage.Height - rcLeft.Height) / 2;
            Rect rcRight = rcLeft;
            rcRight.X = rcPage.Right - rcRight.Width;

            // start with left column
            rc = rcLeft;

            // render string spanning columns and pages
            for (; ; )
            {
                // render as much as will fit into the rectangle
                rc = PdfUtils.Inflate(rc, -3, -3);
                int nextChar = pdf.DrawString(text, bodyFont, Colors.Black, rc);
                rc = PdfUtils.Inflate(rc, +3, +3);
                pdf.DrawRectangle(Colors.LightGray, rc);

                // break when done
                if (nextChar >= text.Length)
                {
                    break;
                }

                // get rid of the part that was rendered
                text = text.Substring(nextChar);

                // switch to right-side rectangle
                if (rc.Left == rcLeft.Left)
                {
                    rc = rcRight;
                }
                else // switch to left-side rectangle on the next page
                {
                    pdf.NewPage();
                    rc = rcLeft;
                }
            }
        }

        #endregion

        //---------------------------------------------------------------------------------
        #region ** text

        static void CreateDocumentText(C1PdfDocument pdf)
        {
            // use landscape for more impact
            pdf.Landscape = true;

            // measure and show some text 
            var text = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            var font = new Font("Times New Roman", 9, PdfFontStyle.Italic);

            // create StringFormat used to set text alignment and line spacing
            var fmt = new StringFormat();
            fmt.LineSpacing = -1.5; // 1.5 char height
            fmt.Alignment = HorizontalAlignment.Center;

            // measure it
            var sz = pdf.MeasureString(text, font, 72 * 3, fmt);
            var rc = new Rect(pdf.PageRectangle.Width / 2, 72, sz.Width, sz.Height);
            rc = PdfUtils.Offset(rc, 72, 0);

            // draw a rounded frame
            rc = PdfUtils.Inflate(rc, 10, 10);
            pdf.FillRectangle(Colors.Black, rc, new Size(30, 30));
            pdf.DrawRectangle(new Pen(Colors.Red, 4), rc, new Size(30, 30));
            rc = PdfUtils.Inflate(rc, -10, -10);

            // draw the text
            pdf.DrawString(text, font, Colors.White, rc, fmt);

            // point in center for rotate the text
            rc = pdf.PageRectangle;
            var pt = new Point(rc.Location.X + rc.Width / 2, rc.Location.Y + rc.Height / 2);

            // rotate the string in small increments
            var step = 6;
            text = "PDF works in WPF!";
            for (int i = 0; i <= 360; i += step)
            {
                pdf.RotateAngle = i;
                font = new Font("Courier New", 8 + i / 30.0, PdfFontStyle.Bold);
                byte b = (byte)(255 * (1 - i / 360.0));
                pdf.DrawString(text, font, Color.FromArgb(0xff, b, b, b), pt);
            }
        }

        #endregion

        //---------------------------------------------------------------------------------
        #region ** graphics

        static void CreateDocumentGraphics(C1PdfDocument pdf)
        {
            // set up to draw
            Rect rc = new Rect(0, 0, 300, 200);
            string text = "Hello world of .NET Graphics and PDF.\r\nNice to meet you.";
            Font font = new Font("Times New Roman", 12, PdfFontStyle.Italic | PdfFontStyle.Underline);

            // draw to pdf document
            int penWidth = 0;
            byte penRGB = 0;
            pdf.FillPie(Colors.Red, rc, 0, 20f);
            pdf.FillPie(Colors.Green, rc, 20f, 30f);
            pdf.FillPie(Colors.Blue, rc, 60f, 12f);
            pdf.FillPie(Colors.Orange, rc, -80f, -20f);
            for (float startAngle = 0; startAngle < 360; startAngle += 40)
            {
                Color penColor = Color.FromArgb(0xff, penRGB, penRGB, penRGB);
                Pen pen = new Pen(penColor, penWidth++);
                penRGB = (byte)(penRGB + 20);
                pdf.DrawArc(pen, rc, startAngle, 40f);
            }
            pdf.DrawRectangle(Colors.Red, rc);
            pdf.DrawString(text, font, Colors.Black, rc);

            // show a Bezier curve
            var pts = new Point[]
			{
				new Point(400, 100), new Point(420,  30),
				new Point(500, 140), new Point(530,  20),
			};

            // draw Bezier 
            pdf.DrawBezier(new Pen(Colors.Blue, 4), pts[0], pts[1], pts[2], pts[3]);

            // show Bezier control points
            pdf.DrawLines(Colors.Gray, pts);
            foreach (Point pt in pts)
            {
                pdf.FillRectangle(Colors.Red, pt.X - 2, pt.Y - 2, 4, 4);
            }

            // title
            pdf.DrawString("Simple Bezier", font, Colors.Black, new Rect(500, 150, 100, 100));
        }

        #endregion

    }
}
