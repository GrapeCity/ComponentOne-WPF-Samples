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
using C1.C1Preview;

namespace CoordinatesOfCharsInText
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            C1PrintDocument doc = MakeDoc1();
            this.c1DocumentViewer1.Document = doc.FixedDocumentSequence;
        }


        private C1PrintDocument MakeDoc1()
        {
            C1PrintDocument doc = new C1PrintDocument();
            // doc.UseGdiPlusTextRendering = true;

            // create the document title:
            RenderParagraph rp = new RenderParagraph();
            rp.Content.AddText("You can get coordinates and sizes of individual characters in a text block using the ");
            rp.Content.AddText("GetCharRect()", Colors.Blue);
            rp.Content.AddText(" method of RenderText and RenderParagraph classes.\r\n");
            rp.Content.AddText("In the following example each character has a red rectangle drawn around it.");
            rp.Style.FontName = "Verdana";
            rp.Style.FontSize = 15;
            rp.Style.Spacing.Bottom = "3mm";
            rp.Style.Borders.Bottom = LineDef.DefaultBold;
            rp.Style.TextAlignHorz = AlignHorzEnum.Justify;
            doc.Body.Children.Add(rp);

            // add some sample texts:
            rp = new RenderParagraph();
            rp.Style.FontName = "Arial";
            rp.Style.FontSize = 36;
            rp.Content.Add(new ParagraphText("Normal text", TextPositionEnum.Normal));
            rp.Content.Add(new ParagraphText("Super script text\r\n", TextPositionEnum.Superscript));
            rp.Content.Add(new ParagraphText("Sub script text\r\n", TextPositionEnum.Subscript));
            rp.Content.Add(new ParagraphText("Normal text. ", TextPositionEnum.Normal));
            rp.Content.Add(new ParagraphText("Sub script. ", TextPositionEnum.Subscript));
            rp.Content.Add(new ParagraphText("Super script. ", TextPositionEnum.Superscript));
            rp.Content.Add(new ParagraphText("Normal text.\r\n"));
            rp.Content.Add(new ParagraphText("Normal text Normal text Normal text Normal text Normal text Normal text Normal text Normal text."));
            doc.Body.Children.Add(rp);

            // To use the GetCharRect method, we must first generate
            // the document so that character positions are calculated.
            // The generation will be later repeated with drawing
            // red rectangles around the individual characters.
            doc.Generate();

            int textLength = rp.TextLength;
            // Fragments contain info about the rendered objects:
            // get the first fragment of rp object
            RenderParagraphFragment rpf = (RenderParagraphFragment)rp.Fragments[0];
            // go over all characters in the text
            for (int i = 0; i < textLength; ++i)
            {
                // get the coordinates of character,
                // they will be returned in C1PrintDocument.ResolvedUnit units
                RectangleD charRect = rpf.GetCharRect(i);

                // make a rectangle around the char
                RenderRectangle r = new RenderRectangle();
                // specify all coordinates of rectangle
                r.X = new Unit(rpf.Bounds.Left + charRect.Left, doc.ResolvedUnit);
                r.Y = new Unit(rpf.Bounds.Top + charRect.Top, doc.ResolvedUnit);
                r.Width = new Unit(charRect.Width, doc.ResolvedUnit);
                r.Height = new Unit(charRect.Height, doc.ResolvedUnit);
                // set shape (rectangle) coordinates, they are specified
                // relative to object
                r.Rectangle.X = 0;
                r.Rectangle.Y = 0;
                r.Rectangle.Width = r.Width;
                r.Rectangle.Height = r.Height;
                r.Style.ShapeLine = new LineDef("1pt", Colors.Red);
                // add the rectangle to the object
                doc.Body.Children.Add(r);
            }

            // re-generate the document to show char rectangles:
            doc.Generate();

            // done:
            return doc;
        }
    }
}
