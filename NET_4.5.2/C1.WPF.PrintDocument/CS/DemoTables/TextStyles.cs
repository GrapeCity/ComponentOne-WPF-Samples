using System.Windows.Media;
using System.Reflection;
using C1.C1Preview;

namespace DemoTables
{
    static partial class MakeDocHelper
    {
        static public C1PrintDocument TextStyles()
        {
            C1.C1Preview.C1PrintDocument doc = new C1.C1Preview.C1PrintDocument();

            RenderText rtxt = new RenderText(doc);
            rtxt.Text = "Text styles in C1PrintDocument";
            rtxt.Style.FontName = "Arial";
            rtxt.Style.FontSize = 18;
            rtxt.Style.FontBold = true;
            rtxt.Style.TextAlignHorz = AlignHorzEnum.Center;
            rtxt.Style.Padding.All = new Unit("5mm");
            doc.Body.Children.Add(rtxt);

            // setup some text styles
            Style s1 = doc.Style.Children.Add();
            s1.TextColor = Colors.Blue;
            s1.FontBold = true;

            Style s2 = doc.Style.Children.Add();
            s2.BackColor = Colors.Chartreuse;

            Style s3 = doc.Style.Children.Add();
            s3.TextColor = Colors.Red;

            Style s4 = doc.Style.Children.Add();
            s4.FontName = "Arial";
            s4.FontSize = 14;
            s4.FontBold = true;
            s4.FontItalic = true;

            Style s5 = doc.Style.Children.Add();
            s5.BackColor = s2.BackColor;
            s5.TextColor = s3.TextColor;
            s5.Font = s4.Font;

            RenderParagraph rp = new RenderParagraph(doc);

            ParagraphObject po = new ParagraphText("In ");
            rp.Content.Add(po);
            po = new ParagraphText("C1PrintDocument");
            po.Style.AssignNonInheritedFrom(s1);
            rp.Content.Add(po);
            rp.Content.Add(new ParagraphText(", multi-style text is fully supported via "));
            rp.Content.Add(new ParagraphText("RenderParagraph", Colors.Blue));
            rp.Content.Add(new ParagraphText(" render objects."));
            
            rp.Content.Add(new ParagraphText("\rWithin a single paragraph, you can change "));
            
            po = new ParagraphText("background color,");
            po.Style.Parent = s2;
            rp.Content.Add(po);

            po = new ParagraphText(" text color,");
            po.Style.AssignNonInheritedFrom(s3);
            rp.Content.Add(po);

            po = new ParagraphText(" font,");
            po.Style.AssignNonInheritedFrom(s4);
            rp.Content.Add(po);

            po = new ParagraphText(" or all together.");
            po.Style.AssignNonInheritedFrom(s5);
            rp.Content.Add(po);

            rp.Content.Add(new ParagraphText("\rFont sub-properties such as "));
            rp.Content.Add(new ParagraphText("bold, "));
            rp.Content[rp.Content.Count - 1].Style.FontBold = true;
            rp.Content.Add(new ParagraphText("italic "));
            rp.Content[rp.Content.Count - 1].Style.FontItalic = true;
            rp.Content.Add(new ParagraphText("or "));
            rp.Content.Add(new ParagraphText("underline"));
            rp.Content[rp.Content.Count - 1].Style.FontUnderline = true;
            rp.Content.Add(new ParagraphText(" can be individually adjusted."));

            rp.Content.Add(new ParagraphText("\rText positions such as "));
            rp.Content.Add(new ParagraphText("superscript", TextPositionEnum.Superscript));
            rp.Content.Add(new ParagraphText(" and "));
            rp.Content.Add(new ParagraphText("subscript", TextPositionEnum.Subscript));
            rp.Content.Add(new ParagraphText(" are supported."));

            po = new ParagraphText("\rInline images ");
            rp.Content.Add(po);
            System.Drawing.Image inlineImage = System.Drawing.Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("DemoTables.check.png"));
            po = new ParagraphImage(inlineImage);
            rp.Content.Add(po);
            po = new ParagraphText(" can be embedded in the text as well.");
            rp.Content.Add(po);

            doc.Body.Children.Add(rp);

            return doc;
        }
    }
}