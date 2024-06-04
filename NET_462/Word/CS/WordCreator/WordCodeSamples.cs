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

using Microsoft.Win32;

namespace Word.Creator
{
    using C1.Util;
    using C1.WPF.RichTextBox;
    using C1.WPF.RichTextBox.Documents;
    using C1.WPF.Word;
    using C1.WPF.Word.Objects;

    public static class WordCodeSamples
    {
        static string _extension = ".rtf";
        //static string _extension = ".docx";

        public static void HandleButtonClick(object sender, RoutedEventArgs e)
        {
            // get stream to save to
            var dlg = new SaveFileDialog();
            dlg.FileName = "document";
            dlg.DefaultExt = ".docx";
            dlg.Filter = WordUtils.GetFileFilter(_extension);
            var dr = dlg.ShowDialog();
            if (!dr.HasValue || !dr.Value)
            {
                return;
            }

            // get sender button
            var btn = sender as Button;

            // create document
            var word = new C1WordDocument();
            word.Clear();

            // set document info
            var di = word.Info;
            di.Author = "ComponentOne";
            di.Subject = "C1.WPF.Word demo.";
            di.Title = (string)btn.Content;

            // create document
            switch (di.Title)
            {
                case "Quotes":
                    CreateDocumentQuotes(word);
                    break;
                case "Tables":
                    CreateDocumentTables(word);
                    break;
                case "Images":
                    CreateDocumentImages(word);
                    break;
                case "Paper Sizes":
                    CreateDocumentPaperSizes(word);
                    break;
                case "Table of Contents":
                    CreateDocumentTOC(word);
                    break;
                case "Text Flow":
                    CreateDocumentTextFlow(word);
                    break;
                case "Text":
                    CreateDocumentText(word);
                    break;
                case "Graphics":
                    CreateDocumentGraphics(word);
                    break;
                case "Rich Text":
                    CreateDocumentFromRtf(word);
                    break;
            }

            // render footers

            // standard footer
            var text = string.Format("C1.WPF.Word: {0}, page {1} of {2}", di.Title, "|", "|");
            var paragraph = new RtfParagraph(word.CurrentSection.Footer);
            paragraph.Alignment = RtfHorizontalAlignment.Right;
            int count = 0;
            foreach (var part in text.Split('|'))
            {
                if (!string.IsNullOrEmpty(part))
                {
                    paragraph.Add(new RtfString(part));
                }
                switch (count)
                {
                    case 0:
                        paragraph.Add(new RtfPageField());
                        break;
                    case 1:
                        paragraph.Add(new RtfNumPagesField());
                        break;
                }
                count++;
            }
            word.CurrentSection.Footer.Add(paragraph);

            // this reopens each page and adds content to them (now we know the page count).
            //var font = new Font("Arial", 8, RtfFontStyle.Bold);
            //var fmt = new StringFormat();
            //fmt.Alignment = HorizontalAlignment.Right;
            //fmt.LineAlignment = VerticalAlignment.Bottom;
            //for (int page = 0; page < rtf.Pages.Count; page++)
            //{
            //    rtf.CurrentPage = page;
            //    var text = string.Format("C1.WPF.Word: {0}, page {1} of {2}",
            //        di.Title,
            //        page + 1,
            //        rtf.Pages.Count);
            //    rtf.DrawString(
            //        text,
            //        font,
            //        Colors.DarkGray,
            //        WordUtils.Inflate(rtf.PageRectangle, -72, -36),
            //        fmt);
            //}

            // save document
            using (var stream = dlg.OpenFile())
            {
                word.Save(stream, dlg.FileName.ToLower().EndsWith(".docx") ? FileFormat.OpenXml : FileFormat.Rtf);
            }
            MessageBox.Show("Word Document saved to " + dlg.SafeFileName);
        }

        //---------------------------------------------------------------------------------
        #region ** quotes

        static void CreateDocumentQuotes(C1WordDocument word)
        {
            // calculate page rect (discounting margins)
            Rect rcPage = WordUtils.PageRectangle(word);
            Rect rc = rcPage;

            // initialize output parameters
            Font hdrFont = new Font("Arial", 14, RtfFontStyle.Bold);
            Font titleFont = new Font("Arial", 24, RtfFontStyle.Bold);
            Font txtFont = new Font("Times New Roman", 10, RtfFontStyle.Italic);

            // add title
            var rcTop = WordUtils.RenderParagraph(word, word.Info.Title, titleFont, rcPage, rc);
            rc = rcTop;

            // build document
            foreach (string s in GetQuotes())
            {
                string[] authorQuote = s.Split('\t');

                // render header (author)
                var author = authorQuote[0];
                rc.Y += 25;
                rc = WordUtils.RenderParagraph(word, author, hdrFont, rcPage, rc, true);

                // render body text (quote)
                string text = authorQuote[1];
                rc.X = rcPage.X + 36; // << indent body text by 1/2 inch
                rc.Width = rcPage.Width - 40;
                rc = WordUtils.RenderParagraph(word, text, txtFont, rcPage, rc);
                rc.X = rcPage.X; // << restore indent
                rc.Width = rcPage.Width;
                rc.Y += 12; // << add 12pt spacing after each quote
                if (rc.Y > rcPage.Height)
                {
                    word.PageBreak();
                    rc = rcTop;
                }
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

        static void CreateDocumentTables(C1WordDocument word)
        {
            // get the data
            var ds = DataAccess.GetDataSet();

            // calculate page rect (discounting margins)
            Rect rcPage = WordUtils.PageRectangle(word);
            Rect rc = rcPage;

            // add title
            Font titleFont = new Font("Tahoma", 24, RtfFontStyle.Bold);
            rc = WordUtils.RenderParagraph(word, word.Info.Title, titleFont, rcPage, rc, false);

            // render some tables
            RenderTable(word, rc, rcPage, ds.Tables["Customers"], new string[] { "CompanyName", "ContactName", "Country", "Address", "Phone" });
            word.PageBreak();
            rc = rcPage;
            RenderTable(word, rc, rcPage, ds.Tables["Products"], new string[] { "ProductName", "QuantityPerUnit", "UnitPrice", "UnitsInStock", "UnitsOnOrder" });
            word.PageBreak();
            rc = rcPage;
            RenderTable(word, rc, rcPage, ds.Tables["Employees"], new string[] { "FirstName", "LastName", "Country", "Notes" });
        }

        static Rect RenderTable(C1WordDocument word, Rect rc, Rect rcPage, DataTable table, string[] fields)
        {
            // select fonts
            Font hdrFont = new Font("Tahoma", 10, RtfFontStyle.Bold);
            Font txtFont = new Font("Tahoma", 8);

            // build table
            //word.AddBookmark(table.TableName, 0, rc.Y);
            rc = WordUtils.RenderParagraph(word, "NorthWind " + table.TableName, hdrFont, rcPage, rc, false);

            // build table
            rc = RenderTableHeader(word, hdrFont, rc, fields);
            foreach (DataRow dr in table.Rows)
            {
                rc = RenderTableRow(word, txtFont, hdrFont, rcPage, rc, fields, dr);
            }

            // done
            return rc;
        }

        static Rect RenderTableHeader(C1WordDocument word, Font font, Rect rc, string[] fields)
        {
            // calculate cell width (same for all columns)
            Rect rcCell = rc;
            rcCell.Width = rc.Width / fields.Length;
            rcCell.Height = 0;

            // calculate cell height (max of all columns)
            foreach (string field in fields)
            {
                var height = C1WordDocument.MeasureString(field, font, rcCell.Width).Height;
                rcCell.Height = Math.Max(rcCell.Height, height);
            }
            rcCell.Height += 6; // add 6 point margin

            // render header cells
            var fmt = new StringFormat();
            fmt.LineAlignment = VerticalAlignment.Center;
            foreach (string field in fields)
            {
                word.FillRectangle(Colors.Black, rcCell);
                word.DrawString(field, font, Colors.White, rcCell, fmt);
                rcCell = WordUtils.Offset(rcCell, rcCell.Width, 0);
            }

            // update rectangle and return it
            return WordUtils.Offset(rc, 0, rcCell.Height);
        }

        static Rect RenderTableRow(C1WordDocument word, Font font, Font hdrFont, Rect rcPage, Rect rc, string[] fields, DataRow dr)
        {
            // calculate cell width (same for all columns)
            Rect rcCell = rc;
            rcCell.Width = rc.Width / fields.Length;
            rcCell.Height = 0;

            // calculate cell height (max of all columns)
            rcCell = WordUtils.Inflate(rcCell, -4, 0);
            foreach (string field in fields)
            {
                string text = dr[field].ToString();
                var height = C1WordDocument.MeasureString(text, font, rcCell.Width).Height;
                rcCell.Height = Math.Max(rcCell.Height, height);
            }
            rcCell = WordUtils.Inflate(rcCell, 4, 0); // add 4 point margin
            rcCell.Height += 2;

            // break page if we have to
            if (rcCell.Bottom > rcPage.Bottom)
            {
                word.PageBreak();
                rc = RenderTableHeader(word, hdrFont, rcPage, fields);
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
                word.DrawRectangle(Colors.LightGray, rcCell);
                rcCell = WordUtils.Inflate(rcCell, -4, 0);
                word.DrawString(text, font, Colors.Black, rcCell, fmt);
                rcCell = WordUtils.Inflate(rcCell, 4, 0);
                rcCell = WordUtils.Offset(rcCell, rcCell.Width, 0);
            }

            // update rectangle and return it
            return WordUtils.Offset(rc, 0, rcCell.Height);
        }

        #endregion

        //---------------------------------------------------------------------------------
        #region ** images

        static void CreateDocumentImages(C1WordDocument word)
        {
            // calculate page rect (discounting margins)
            Rect rcPage = WordUtils.PageRectangle(word);

            // load image into writeable bitmap
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = DataAccess.GetStream("borabora.jpg");
            bi.EndInit();
            var wb = new WriteableBitmap(bi);

            // center image on page preserving aspect ratio
            //word.DrawImage(wb, rcPage, ContentAlignment.MiddleCenter, Stretch.Uniform);
            word.DrawImage(wb, rcPage);
        }

        #endregion

        //---------------------------------------------------------------------------------
        #region ** paper sizes

        static void CreateDocumentPaperSizes(C1WordDocument word)
        {
            // landscape for first page
            bool landscape = true;
            word.Landscape = landscape;

            // add title
            Font titleFont = new Font("Tahoma", 24, RtfFontStyle.Bold);
            Rect rc = WordUtils.PageRectangle(word);
            word.AddParagraph(word.Info.Title, titleFont);
            var paragraph = (RtfParagraph)word.Current;
            paragraph.BottomBorderColor = Colors.Purple;
            paragraph.BottomBorderStyle = RtfBorderStyle.Dotted;
            paragraph.BottomBorderWidth = 3.0f;

            // create constant font and StringFormat objects
            Font font = new Font("Tahoma", 18);
            StringFormat sf = new StringFormat();
            sf.Alignment = HorizontalAlignment.Center;
            sf.LineAlignment = VerticalAlignment.Center;
            word.AddParagraph("By default used Landscape A4", font);
            word.AddParagraph(string.Empty, font);

            // create one page with each paper size
            foreach (var fi in typeof(PaperKind).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                // Silverlight/Phone doesn't have Enum.GetValues
                PaperKind pk = (PaperKind)fi.GetValue(null);

                // skip custom size
                if (pk == PaperKind.Custom) continue;

                // add new page for every page after the first one
                word.PageBreak();

                // set paper kind and orientation
                var section = new RtfSection(pk);
                word.Add(section);
                landscape = !landscape;
                word.Landscape = landscape;

                // add some content on the page
                string text = string.Format("PaperKind: [{0}];\r\nLandscape: [{1}];\r\nFont: [Tahoma 18pt]", pk, word.Landscape);
                word.AddParagraph(text);
                paragraph = (RtfParagraph)word.Current;
                paragraph.SetRectBorder(RtfBorderStyle.DashSmall, Colors.Aqua, 2.0f);
                word.AddParagraph(string.Empty);
            }
        }

        #endregion

        //---------------------------------------------------------------------------------
        #region ** table of contents

        static void CreateDocumentTOC(C1WordDocument word)
        {
            // create pdf document
            word.Info.Title = "Document with Table of Contents";

            // add title
            Font titleFont = new Font("Tahoma", 24, RtfFontStyle.Bold);
            Rect rcPage = WordUtils.PageRectangle(word);
            Rect rc = WordUtils.RenderParagraph(word, word.Info.Title, titleFont, rcPage, rcPage, false);
            rc.Y += 12;

            // create nonsense document
            var bkmk = new List<string[]>();
            Font headerFont = new Font("Arial", 14, RtfFontStyle.Bold);
            Font bodyFont = new Font("Times New Roman", 11);
            for (int i = 0; i < 30; i++)
            {
                // create ith header (as a link target and outline entry)
                string header = string.Format("{0}. {1}", i + 1, BuildRandomTitle());
                rc = WordUtils.RenderParagraph(word, header, headerFont, rcPage, rc, true, true);

                // save bookmark to build TOC later
                int pageNumber = 1;
                bkmk.Add(new string[] { pageNumber.ToString(), header });

                // create some text
                rc.X += 36;
                rc.Width -= 36;
                for (int j = 0; j < 3 + _rnd.Next(20); j++)
                {
                    string text = BuildRandomParagraph();
                    rc = WordUtils.RenderParagraph(word, text, bodyFont, rcPage, rc);
                    rc.Y += 6;
                }
                rc.X -= 36;
                rc.Width += 36;
                rc.Y += 20;
            }

            // start Table of Contents
            word.PageBreak();                   // start TOC on a new page
            //int tocPage = word.CurrentPage;	// save page index (to move TOC later)
            //int tocPage = 1;	// save page index (to move TOC later)
            rc = WordUtils.RenderParagraph(word, "Table of Contents", titleFont, rcPage, rcPage, true);
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
                word.DrawString(header, bodyFont, Colors.Black, rc);
                word.DrawString(page, bodyFont, Colors.Black, rc, sfRight);

#if true
                // connect the two with some dots (looks better than a dotted line)
                string dots = ". ";
                var wid = C1WordDocument.MeasureString(dots, bodyFont).Width;
                var x1 = rc.X + C1WordDocument.MeasureString(header, bodyFont).Width + 8;
                var x2 = rc.Right - C1WordDocument.MeasureString(page, bodyFont).Width - 8;
                var x = rc.X;
                for (rc.X = x1; rc.X < x2; rc.X += wid)
                {
                    word.DrawString(dots, bodyFont, Colors.Gray, rc);
                }
                rc.X = x;
#else 
				// connect with a dotted line (another option)
				var x1 = rc.X + word.MeasureString(header, bodyFont).Width + 5;
				var x2 = rc.Right - word.MeasureString(page, bodyFont).Width  - 5;
				var y  = rc.Top + bodyFont.Size;
				word.DrawLine(dottedPen, x1, y, x2, y);
#endif
                // add local hyperlink to entry
                //rtf.AddLink("#" + header, rc);

                // move on to next entry
                rc = WordUtils.Offset(rc, 0, rc.Height);
                if (rc.Bottom > rcPage.Bottom)
                {
                    word.PageBreak();
                    rc.Y = rcPage.Y;
                }
            }

            // move table of contents to start of document
            //var arr = new WordPage[rtf.Pages.Count - tocPage];
            //rtf.Pages.CopyTo(tocPage, arr, 0, arr.Length);
            //rtf.Pages.RemoveRange(tocPage, arr.Length);
            //rtf.Pages.InsertRange(0, arr);
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

        static void CreateDocumentTextFlow(C1WordDocument word)
        {
            // load long string from resource file
            string text = "Resource not found...";
            using (var sr = new StreamReader(DataAccess.GetStream("flow.txt")))
            {
                text = sr.ReadToEnd();
            }

            // content rectangle
            Rect rcPage = WordUtils.PageRectangle(word);

            // create two columns for the text
            var columnWidth = (float)(rcPage.Width - 30) / 2;
            word.MainSection.Columns.Clear();
            word.MainSection.Columns.Add(new RtfColumn(columnWidth));
            word.MainSection.Columns.Add(new RtfColumn(columnWidth));

            // add title
            Font titleFont = new Font("Tahoma", 14, RtfFontStyle.Bold);
            Font bodyFont = new Font("Tahoma", 11);
            word.AddParagraph("Text Flow", titleFont, Colors.DarkOliveGreen, RtfHorizontalAlignment.Center);
            word.AddParagraph(string.Empty);

            // render string spanning columns and pages
            foreach (var part in text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                word.AddParagraph(part, bodyFont, Colors.Black, RtfHorizontalAlignment.Justify);
            }
        }

        #endregion

        //---------------------------------------------------------------------------------
        #region ** text

        static void CreateDocumentText(C1WordDocument word)
        {
            // use landscape for more impact
            word.Landscape = true;

            // measure and show some text 
            var text = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            var font = new Font("Segoe UI Light", 14, RtfFontStyle.Italic);

            // add paragraph
            word.AddParagraph(text, font, Colors.BlueViolet, RtfHorizontalAlignment.Justify);
            var paragraph = (RtfParagraph)word.Current;
            paragraph.LeftBorderColor = Colors.Blue;
            paragraph.LeftBorderStyle = RtfBorderStyle.Emboss;
            paragraph.LeftBorderWidth = 1f;
            paragraph.TopBorderColor = Colors.Blue;
            paragraph.TopBorderStyle = RtfBorderStyle.Emboss;
            paragraph.TopBorderWidth = 2f;
            paragraph.RightBorderColor = Colors.Purple;
            paragraph.RightBorderStyle = RtfBorderStyle.DotDash;
            paragraph.RightBorderWidth = 5f;
            paragraph.BottomBorderColor = Colors.Red;
            paragraph.BottomBorderStyle = RtfBorderStyle.Dotted;
            paragraph.BottomBorderWidth = 3f;

            // add table
            word.LineBreak();
            int rows = 4;
            int cols = 2;
            RtfTable table = new RtfTable(rows, cols);
            word.Add(table);
            table.Rows[0].Cells[0].SetMerged(1, 2);

            for (int row = 0; row < rows; row++)
            {
                if (row == 0)
                {
                    table.Rows[row].Height = 50;
                }
                for (int col = 0; col < cols; col++)
                {
                    if (row == 0 && col == 0)
                    {
                        text = "Word/RTF works in WPF!";
                        table.Rows[row].Cells[col].Alignment = ContentAlignment.MiddleCenter;
                        table.Rows[row].Cells[col].BackFilling = Colors.LightPink;
                    }
                    else
                    {
                        text = string.Format("table cell {0}:{1}.", row, col);
                        table.Rows[row].Cells[col].BackFilling = Colors.LightYellow;
                    }
                    table.Rows[row].Cells[col].Content.Add(new RtfString(text));
                    table.Rows[row].Cells[col].BottomBorderWidth = 2;
                    table.Rows[row].Cells[col].TopBorderWidth = 2;
                    table.Rows[row].Cells[col].LeftBorderWidth = 2;
                    table.Rows[row].Cells[col].RightBorderWidth = 2;
                    if (col == cols - 1)
                    {
                        table.Rows[row].Cells[col].Alignment = ContentAlignment.BottomRight;
                    }
                }
            }
        }

        #endregion

        //---------------------------------------------------------------------------------
        #region ** graphics

        static void CreateDocumentGraphics(C1WordDocument rtf)
        {
            // set up to draw
            Rect rc = new Rect(100, 100, 300, 200);
            string text = "Hello world of .NET Graphics and Word/RTF.\r\nNice to meet you.";
            Font font = new Font("Times New Roman", 12, RtfFontStyle.Italic | RtfFontStyle.Underline);

            // draw to pdf document
            int penWidth = 0;
            byte penRGB = 0;
            rtf.FillPie(Colors.Red, rc, 0, 20f);
            rtf.FillPie(Colors.Green, rc, 20f, 30f);
            rtf.FillPie(Colors.Blue, rc, 60f, 12f);
            rtf.FillPie(Colors.Orange, rc, -80f, -20f);
            for (float startAngle = 0; startAngle < 360; startAngle += 40)
            {
                Color penColor = Color.FromArgb(0xff, penRGB, penRGB, penRGB);
                Pen pen = new Pen(penColor, penWidth++);
                penRGB = (byte)(penRGB + 20);
                rtf.DrawArc(pen, rc, startAngle, 40f);
            }
            rtf.DrawRectangle(Colors.Red, rc);
            rtf.DrawString(text, font, Colors.Black, rc);

            // show a Bezier curve
            var pts = new Point[]
			{
				new Point(400, 200), new Point(420, 130),
				new Point(500, 240), new Point(530, 120),
			};

            // draw Bezier 
            rtf.DrawBeziers(new Pen(Colors.Blue, 4), pts);

            // show Bezier control points
            rtf.DrawPolyline(Colors.Gray, pts);
            foreach (Point pt in pts)
            {
                rtf.FillRectangle(Colors.Red, pt.X - 2, pt.Y - 2, 4, 4);
            }

            // title
            rtf.DrawString("Simple Bezier", font, Colors.Black, new Rect(500, 150, 100, 100));
        }

        #endregion

        #region ** richtext
        private static string _asmName = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name;
        static void CreateDocumentFromRtf(C1WordDocument doc)
        {            
            var stream = Application.GetResourceStream(new Uri("/" + _asmName + ";component/Resources/dickens.htm", UriKind.Relative)).Stream;
            var html = new StreamReader(stream).ReadToEnd();
            var richTB = new C1RichTextBox();
            richTB.HtmlFilter.ConvertingHtmlNode += OnConvertingHtmlNode;
            richTB.Html = html;
            richTB.HtmlFilter.ConvertingHtmlNode -= OnConvertingHtmlNode;

            //This string rtfText can be used well in MS Word
            var rtfText = new RtfFilter().ConvertFromDocument(richTB.Document);
            doc.ParseRtfText(rtfText);
        }

        static void OnConvertingHtmlNode(object sender, ConvertingHtmlNodeEventArgs e)
        {
            var element = e.HtmlNode as C1HtmlElement;
            if (element != null && element.Name == "img")
            {
                string src;
                if (element.Attributes.TryGetValue("src", out src) && src.StartsWith("images"))
                {
                    element.Attributes["src"] = "/" + _asmName + ";component/Resources/" + src;
                }
            }
        }
        #endregion
    }
}
