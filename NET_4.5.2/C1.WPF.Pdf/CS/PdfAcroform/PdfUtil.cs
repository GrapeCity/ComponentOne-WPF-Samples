using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PdfAcroform
{
    using C1.WPF.Pdf;

    public static class PdfUtil
    {
        static int _textBoxCount = 0;
        static int _checkBoxCount = 0;
        static int _radioButtonCount = 0;
        static int _comboBoxCount = 0;
        static int _listBoxCount = 0;
        static int _pushButtonCount = 0;

        public static void CreateDocument(C1PdfDocument pdf)
        {
            // create pdf document
            pdf.Clear();
            //pdf.Compression = CompressionLevel.NoCompression;
            pdf.ConformanceLevel = PdfAConformanceLevel.PdfA2b;
            pdf.DocumentInfo.Title = "PDF Acroform";

            // calculate page rect (discounting margins)
            var rcPage = GetPageRect(pdf);
            var rc = rcPage;

            // add title
            Font titleFont = new Font("Tahoma", 24, PdfFontStyle.Bold);
            rc = RenderParagraph(pdf, pdf.DocumentInfo.Title, titleFont, rcPage, rc, false);

            // render acroforms
            rc = rcPage;
            Font fieldFont = new Font("Arial", 14, PdfFontStyle.Regular);

            // text box field
            rc = new Rect(rc.X, rc.Y + rc.Height / 10, rc.Width / 3, rc.Height / 30);
            PdfTextBox textBox1 = RenderTextBox(pdf, "TextBox Sample", fieldFont, rc);
            textBox1.BorderWidth = FieldBorderWidth.Thick;
            textBox1.BorderStyle = FieldBorderStyle.Inset;
            textBox1.BorderColor = Colors.Green;
            //textBox1.BackColor = Colors.Yellow;

            // first check box field
            rc = new Rect(rc.X, rc.Y + 2 * rc.Height, rc.Width, rc.Height);
            RenderCheckBox(pdf, true, "CheckBox 1 Sample", fieldFont, rc);

            // first radio button group
            rc = new Rect(rc.X, rc.Y + 2 * rc.Height, rc.Width, rc.Height);
            RenderRadioButton(pdf, false, "RadioGroup1", "RadioButton 1 Sample Group 1", fieldFont, rc);
            rc = new Rect(rc.X, rc.Y + 2 * rc.Height, rc.Width, rc.Height);
            RenderRadioButton(pdf, true, "RadioGroup1", "RadioButton 2 Sample Group 1", fieldFont, rc);
            rc = new Rect(rc.X, rc.Y + 2 * rc.Height, rc.Width, rc.Height);
            RenderRadioButton(pdf, false, "RadioGroup1", "RadioButton 3 Sample Group 1", fieldFont, rc);

            // second check box field
            rc = new Rect(rc.X, rc.Y + 2 * rc.Height, rc.Width, rc.Height);
            RenderCheckBox(pdf, false, "CheckBox 2 Sample", fieldFont, rc);

            // second radio button group
            rc = new Rect(rc.X, rc.Y + 2 * rc.Height, rc.Width, rc.Height);
            RenderRadioButton(pdf, true, "RadioGroup2", "RadioButton 1 Sample Group 2", fieldFont, rc);
            rc = new Rect(rc.X, rc.Y + 2 * rc.Height, rc.Width, rc.Height);
            RenderRadioButton(pdf, false, "RadioGroup2", "RadioButton 2 Sample Group 2", fieldFont, rc);

            // first combo box field
            rc = new Rect(rc.X, rc.Y + 2 * rc.Height, rc.Width, rc.Height);
            PdfComboBox comboBox1 = RenderComboBox(pdf, new string[] { "First", "Second", "Third" }, 2, fieldFont, rc);

            // first list box field
            var rclb = new Rect(rc.X, rc.Y + 2 * rc.Height, rc.Width, 3 * rc.Height);
            RenderListBox(pdf, new string[] { "First", "Second", "Third", "Fourth", "Fifth" }, 5, fieldFont, rclb);

            // load first icon
            Image icon = null;
            //using (Stream stream = GetManifestResource("phoenix.png"))
            //{
            //    icon = Image.FromStream(stream);
            //}

            // first push putton field
            rc = new Rect(rc.X, rc.Y + 6 * rc.Height, rc.Width, rc.Height);
            PdfPushButton button1 = RenderPushButton(pdf, "Submit", fieldFont, rc, icon, ButtonLayout.ImageLeftTextRight);
            button1.Actions.Released.Add(new PdfPushButton.Action(ButtonAction.CallMenu, "FullScreen"));
            button1.Actions.GotFocus.Add(new PdfPushButton.Action(ButtonAction.OpenFile, @"..\..\Program.cs"));
            button1.Actions.LostFocus.Add(new PdfPushButton.Action(ButtonAction.GotoPage, "2"));
            button1.Actions.Released.Add(new PdfPushButton.Action(ButtonAction.OpenUrl, "https://www.grapecity.com/en/componentone"));

            //// load second icon
            //using (Stream stream = GetManifestResource("download.png"))
            //{
            //    icon = Image.FromStream(stream);
            //}

            // second push putton field
            rc = new Rect(rc.X, rc.Y + 2 * rc.Height, rc.Width, 2 * rc.Height);
            PdfPushButton button2 = RenderPushButton(pdf, "Cancel", fieldFont, rc, icon, ButtonLayout.TextTopImageBottom);
            button2.Actions.Pressed.Add(new PdfPushButton.Action(ButtonAction.ClearFields));
            button2.Actions.Released.Add(new PdfPushButton.Action(ButtonAction.CallMenu, "Quit"));

            //// load second icon
            //using (Stream stream = GetManifestResource("top100.png"))
            //{
            //    icon = Image.FromStream(stream);
            //}

            // push putton only icon field
            rc = new Rect(rc.X + 1.5f * rc.Width, rc.Y, rc.Width / 2, rc.Height);
            PdfPushButton button3 = RenderPushButton(pdf, "", fieldFont, rc, icon, ButtonLayout.ImageOnly);
            button3.Actions.MouseEnter.Add(new PdfPushButton.Action(ButtonAction.HideField, button1.Name));
            button3.Actions.MouseLeave.Add(new PdfPushButton.Action(ButtonAction.ShowField, button1.Name));
            button3.Actions.Released.Add(new PdfPushButton.Action(ButtonAction.CallMenu, "ShowGrid"));
            button3.BorderWidth = FieldBorderWidth.Medium;
            button3.BorderStyle = FieldBorderStyle.Beveled;
            button3.BorderColor = Colors.Gray;

            // next page
            pdf.NewPage();

            // text for next page
            rc = rcPage;
            RenderParagraph(pdf, "Second page as bookmark", titleFont, rcPage, rc, false);

            // text box field
            //rc = rcPage;
            //rc = new Rect(rc.X, rc.Y + rc.Height / 10, rc.Width / 3, rc.Height / 30);
            //PdfTextBox textBox2 = RenderTextBox("TextSample 2", fieldFont, rc, Color.Yellow, "In 2 page");

            // second pass to number pages
            AddFooters(pdf);
        }


        // get the current page rectangle (depends on paper size)
        // and apply a 1" margin all around it.
        public static Rect GetPageRect(C1PdfDocument pdf)
        {
            var rcPage = pdf.PageRectangle;
            rcPage = new Rect(rcPage.Left + 72, rcPage.Top + 72, rcPage.Width - 144, rcPage.Height - 144);
            //rcPage.Inflate(-72, -72);
            return rcPage;
        }

        // add text box field for fields of the PDF document
        // with common parameters and default names.
        // 
        static PdfTextBox RenderTextBox(C1PdfDocument pdf, string text, Font font, Rect rc, Color back, string toolTip)
        {
            // create
            string name = string.Format("ACFTB{0}", _textBoxCount + 1);
            PdfTextBox textBox = new PdfTextBox();

            // default border
            //textBox.BorderWidth = 3f / 4;
            textBox.BorderStyle = FieldBorderStyle.Solid;
            textBox.BorderColor = Colors.DarkGray;

            // parameters
            textBox.Font = font;
            textBox.Name = name;
            textBox.DefaultText = text;
            textBox.Text = text;
            textBox.ToolTip = string.IsNullOrEmpty(toolTip) ? string.Format("{0} ({1})", text, name) : toolTip;
            if (back != Colors.Transparent)
            {
                textBox.BackColor = back;
            }

            // add
            pdf.AddField(textBox, rc);
            _textBoxCount++;

            // done
            return textBox;
        }
        public static PdfTextBox RenderTextBox(C1PdfDocument pdf, string text, Font font, Rect rc, Color back)
        {
            return RenderTextBox(pdf, text, font, rc, back, null);
        }
        static PdfTextBox RenderTextBox(C1PdfDocument pdf, string text, Font font, Rect rc)
        {
            return RenderTextBox(pdf, text, font, rc, Colors.Transparent, null);
        }

        // add check box field for fields of the PDF document
        // with common parameters and default names.
        // 
        static PdfCheckBox RenderCheckBox(C1PdfDocument pdf, bool value, string text, Font font, Rect rc, Color back, string toolTip)
        {
            // create
            string name = string.Format("ACFCB{0}", _checkBoxCount + 1);
            PdfCheckBox checkBox = new PdfCheckBox();

            // default border
            checkBox.BorderWidth = FieldBorderWidth.Thin;
            checkBox.BorderStyle = FieldBorderStyle.Solid;
            checkBox.BorderColor = Colors.DarkGray;

            // parameters
            checkBox.Name = name;
            checkBox.DefaultValue = value;
            checkBox.Value = value;
            checkBox.ToolTip = string.IsNullOrEmpty(toolTip) ? string.Format("{0} ({1})", text, name) : toolTip;
            if (back != Colors.Transparent)
            {
                checkBox.BackColor = back;
            }

            // add
            var checkBoxSize = font.Size;
            var checkBoxTop = rc.Top + (rc.Height - checkBoxSize) / 2;
            pdf.AddField(checkBox, new Rect(rc.Left, checkBoxTop, checkBoxSize, checkBoxSize));
            _checkBoxCount++;

            // text for check box field
            var x = rc.Left + checkBoxSize + 1.0f;
            var y = rc.Top + (rc.Height - checkBoxSize - 1.0f) / 2;
            pdf.DrawString(text, new Font(font.Name, checkBoxSize, font.Style), Colors.Black, new Point(x, y));

            // done
            return checkBox;
        }
        public static PdfCheckBox RenderCheckBox(C1PdfDocument pdf, bool value, string text, Font font, Rect rc, Color back)
        {
            return RenderCheckBox(pdf, value, text, font, rc, back, null);
        }
        public static PdfCheckBox RenderCheckBox(C1PdfDocument pdf, bool value, string text, Font font, Rect rc)
        {
            return RenderCheckBox(pdf, value, text, font, rc, Colors.Transparent, null);
        }

        // add radio button box field for fields of the PDF document
        // with common parameters and default names.
        // 
        public static PdfRadioButton RenderRadioButton(C1PdfDocument pdf, bool value, string group, string text, Font font, Rect rc, Color back, string toolTip)
        {
            // create
            string name = string.IsNullOrEmpty(group) ? "ACFRGR" : group;
            PdfRadioButton radioButton = new PdfRadioButton();

            // parameters
            radioButton.Name = name;
            radioButton.DefaultValue = value;
            radioButton.Value = value;
            radioButton.ToolTip = string.IsNullOrEmpty(toolTip) ? string.Format("{0} ({1})", text, name) : toolTip;
            if (back != Colors.Transparent)
            {
                radioButton.BackColor = back;
            }

            // add
            var radioSize = font.Size;
            var radioTop = rc.Top + (rc.Height - radioSize) / 2;
            pdf.AddField(radioButton, new Rect(rc.Left, radioTop, radioSize, radioSize));
            _radioButtonCount++;

            // text for radio button field
            var x = rc.Left + radioSize + 1.0f;
            var y = rc.Top + (rc.Height - radioSize - 1.0f) / 2;
            pdf.DrawString(text, new Font(font.Name, radioSize, font.Style), Colors.Black, new Point(x, y));

            // done
            return radioButton;
        }
        public static PdfRadioButton RenderRadioButton(C1PdfDocument pdf, bool value, string group, string text, Font font, Rect rc, Color back)
        {
            return RenderRadioButton(pdf, value, group, text, font, rc, back, null);
        }
        public static PdfRadioButton RenderRadioButton(C1PdfDocument pdf, bool value, string group, string text, Font font, Rect rc)
        {
            return RenderRadioButton(pdf, value, group, text, font, rc, Colors.Transparent, null);
        }

        // add combo box field for fields of the PDF document
        // with common parameters and default names.
        // 
        public static PdfComboBox RenderComboBox(C1PdfDocument pdf, string[] list, int activeIndex, Font font, Rect rc, Color back, string toolTip)
        {
            // create
            string name = string.Format("ACFCLB{0}", _comboBoxCount + 1);
            PdfComboBox comboBox = new PdfComboBox();

            // default border
            comboBox.BorderWidth = FieldBorderWidth.Thin;
            comboBox.BorderStyle = FieldBorderStyle.Solid;
            comboBox.BorderColor = Colors.DarkGray;

            // array
            foreach (string text in list)
            {
                comboBox.Items.Add(text);
            }

            // parameters
            comboBox.Font = font;
            comboBox.Name = name;
            comboBox.DefaultValue = activeIndex;
            comboBox.Value = activeIndex;
            comboBox.ToolTip = string.IsNullOrEmpty(toolTip) ? string.Format("{0} ({1})", string.Format("Count = {0}", comboBox.Items.Count), name) : toolTip;
            if (back != Colors.Transparent)
            {
                comboBox.BackColor = back;
            }

            // add
            pdf.AddField(comboBox, rc);
            _comboBoxCount++;

            // done
            return comboBox;
        }
        public static PdfComboBox RenderComboBox(C1PdfDocument pdf, string[] list, int activeIndex, Font font, Rect rc, Color back)
        {
            return RenderComboBox(pdf, list, activeIndex, font, rc, back, null);
        }
        public static PdfComboBox RenderComboBox(C1PdfDocument pdf, string[] list, int activeIndex, Font font, Rect rc)
        {
            return RenderComboBox(pdf, list, activeIndex, font, rc, Colors.Transparent, null);
        }

        // add list box field for fields of the PDF document
        // with common parameters and default names.
        // 
        static PdfListBox RenderListBox(C1PdfDocument pdf, string[] list, int activeIndex, Font font, Rect rc, Color back, string toolTip)
        {
            // create
            string name = string.Format("ACFLB{0}", _listBoxCount + 1);
            PdfListBox listBox = new PdfListBox();

            // default border
            listBox.BorderWidth = FieldBorderWidth.Thin;
            listBox.BorderStyle = FieldBorderStyle.Solid;
            listBox.BorderColor = Colors.DarkGray;

            // array
            foreach (string text in list)
            {
                listBox.Items.Add(text);
            }

            // parameters
            listBox.Font = font;
            listBox.Name = name;
            listBox.DefaultValue = activeIndex;
            listBox.Value = activeIndex;
            listBox.ToolTip = string.IsNullOrEmpty(toolTip) ? string.Format("{0} ({1})", string.Format("Count = {0}", listBox.Items.Count), name) : toolTip;
            if (back != Colors.Transparent)
            {
                listBox.BackColor = back;
            }

            // add
            pdf.AddField(listBox, rc);
            _listBoxCount++;

            // done
            return listBox;
        }
        public static PdfListBox RenderListBox(C1PdfDocument pdf, string[] list, int activeIndex, Font font, Rect rc, Color back)
        {
            return RenderListBox(pdf, list, activeIndex, font, rc, back, null);
        }
        public static PdfListBox RenderListBox(C1PdfDocument pdf, string[] list, int activeIndex, Font font, Rect rc)
        {
            return RenderListBox(pdf, list, activeIndex, font, rc, Colors.Transparent, null);
        }

        // add push button box field for fields of the PDF document
        // with common parameters and default names.
        // 
        public static PdfPushButton RenderPushButton(C1PdfDocument pdf, string text, Font font, Rect rc, Color back, Color fore, string toolTip, Image image, ButtonLayout layout)
        {
            // create
            string name = string.Format("ACFPB{0}", _pushButtonCount + 1);
            PdfPushButton pushButton = new PdfPushButton();

            // parameters
            pushButton.Name = name;
            pushButton.DefaultValue = text;
            pushButton.Value = text;
            pushButton.Font = font;
            pushButton.ToolTip = string.IsNullOrEmpty(toolTip) ? string.Format("{0} ({1})", text, name) : toolTip;
            if (back != Colors.Transparent)
            {
                pushButton.BackColor = back;
            }
            if (fore != Colors.Transparent)
            {
                pushButton.ForeColor = fore;
            }

            // icon
            if (image != null)
            {
                //    pushButton.Image = image;
                //    pushButton.Layout = layout;
            }

            // add
            pdf.AddField(pushButton, rc);
            _pushButtonCount++;

            // done
            return pushButton;
        }
        public static PdfPushButton RenderPushButton(C1PdfDocument pdf, string text, Font font, Rect rc, Color back)
        {
            return RenderPushButton(pdf, text, font, rc, back, Colors.Transparent, null, null, ButtonLayout.TextOnly);
        }
        public static PdfPushButton RenderPushButton(C1PdfDocument pdf, string text, Font font, Rect rc)
        {
            return RenderPushButton(pdf, text, font, rc, Colors.Transparent, Colors.Transparent, null, null, ButtonLayout.TextOnly);
        }
        public static PdfPushButton RenderPushButton(C1PdfDocument pdf, string text, Font font, Rect rc, Image icon, ButtonLayout layout)
        {
            return RenderPushButton(pdf, text, font, rc, Colors.Transparent, Colors.Transparent, null, icon, layout);
        }
        public static PdfPushButton RenderPushButton(C1PdfDocument pdf, Font font, Rect rc, Color back, Image image)
        {
            return RenderPushButton(pdf, string.Empty, font, rc, back, Colors.Transparent, null, image, ButtonLayout.ImageOnly);
        }

        // measure a paragraph, skip a page if it won't fit, render it into a rectangle,
        // and update the rectangle for the next paragraph.
        // 
        // optionally mark the paragraph as an outline entry and as a link target.
        //
        // this routine will not break a paragraph across pages. for that, see the Text Flow sample.
        //
        public static Rect RenderParagraph(C1PdfDocument pdf, string text, Font font, Rect rcPage, Rect rc, bool outline, bool linkTarget)
        {
            // if it won't fit this page, do a page break
            rc.Height = pdf.MeasureString(text, font, rc.Width).Height;
            if (rc.Bottom > rcPage.Bottom)
            {
                pdf.NewPage();
                rc.Y = rcPage.Top;
            }

            // draw the string
            pdf.DrawString(text, font, Colors.Black, rc);

            // show bounds (mainly to check word wrapping)
            //pdf.DrawRectangle(Pens.Sienna, rc);

            // add headings to outline
            if (outline)
            {
                pdf.DrawLine(new Pen(Colors.Black), rc.X, rc.Y, rc.Right, rc.Y);
                pdf.AddBookmark(text, 0, rc.Y);
            }

            // add link target
            if (linkTarget)
            {
                pdf.AddTarget(text, rc);
            }

            // update rectangle for next time
            rc.Y += rc.Height;
            //rc.Offset(0, rc.Height);
            return rc;
        }
        public static Rect RenderParagraph(C1PdfDocument pdf, string text, Font font, Rect rcPage, Rect rc, bool outline)
        {
            return RenderParagraph(pdf, text, font, rcPage, rc, outline, false);
        }
        public static Rect RenderParagraph(C1PdfDocument pdf, string text, Font font, Rect rcPage, Rect rc)
        {
            return RenderParagraph(pdf, text, font, rcPage, rc, false, false);
        }

        //================================================================================
        // add page footers to a document
        //
        // this method is called by all samples in this project. it scans the document
        // and adds a 'page n of m' footer to each page. the footers are rendered as 
        // vertical text along the right edge of the document.
        //
        // adding content to an existing page is easy: just set the CurrentPage property
        // to point to an existing page and write into it as usual.
        //
        public static void AddFooters(C1PdfDocument pdf)
        {
            Font fontHorz = new Font("Tahoma", 7, PdfFontStyle.Bold);
            Font fontVert = new Font("Viner Hand ITC", 14, PdfFontStyle.Bold);

            StringFormat sfRight = new StringFormat();
            sfRight.Alignment = HorizontalAlignment.Right;

            StringFormat sfVert = new StringFormat();
            sfVert.FormatFlags |= StringFormatFlags.DirectionVertical;
            sfVert.Alignment = HorizontalAlignment.Center;

            for (int page = 0; page < pdf.Pages.Count; page++)
            {
                // select page we want (could change PageSize)
                pdf.CurrentPage = page;

                // build rectangles for rendering text
                var rcPage = GetPageRect(pdf);
                var rcFooter = rcPage;
                rcFooter.Y = rcFooter.Bottom + 6;
                rcFooter.Height = 12;
                var rcVert = rcPage;
                rcVert.X = rcPage.Right + 6;

                // add left-aligned footer
                string text = pdf.DocumentInfo.Title;
                pdf.DrawString(text, fontHorz, Colors.Gray, rcFooter);

                // add right-aligned footer
                text = string.Format("Page {0} of {1}", page + 1, pdf.Pages.Count);
                pdf.DrawString(text, fontHorz, Colors.Gray, rcFooter, sfRight);

                // add vertical text
                text = pdf.DocumentInfo.Title + " (document created using the C1Pdf component)";
                pdf.DrawString(text, fontVert, Colors.LightGray, rcVert, sfVert);

                // draw lines on bottom and right of the page
                var pen = new Pen(Colors.Gray, 1.0);
                pdf.DrawLine(pen, rcPage.Left, rcPage.Bottom, rcPage.Right, rcPage.Bottom);
                pdf.DrawLine(pen, rcPage.Right, rcPage.Top, rcPage.Right, rcPage.Bottom);
            }
        }

        public static MemoryStream SaveToStream(this C1PdfDocument pdf)
        {
            MemoryStream ms = new MemoryStream();
            pdf.Save(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }
}
