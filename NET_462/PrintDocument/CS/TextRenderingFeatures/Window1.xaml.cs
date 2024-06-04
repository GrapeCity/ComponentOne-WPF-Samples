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

namespace TextRenderingFeatures
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
            c1DocumentViewer1.Document = MakeDocument().FixedDocumentSequence;
        }

        private C1PrintDocument MakeDocument()
        {
            C1PrintDocument doc = new C1PrintDocument();
            RenderText rt;
            doc.UseGdiPlusTextRendering = false; // char spacing features do not work with GDI+ text rendering.

            doc.Style.FontName = "Verdana";
            doc.Style.FontSize = 14;

            // demo CharSpacing:
            rt = MakeDescription("CharSpacing");
            doc.Body.Children.Add(rt);

            Unit cs = "2mm";
            doc.Body.Children.Add(new RenderText("CharSpacing: " + cs.ToString()));
            rt = MakeSampleRenderText();
            rt.Style.CharSpacing = cs;
            doc.Body.Children.Add(rt);

            cs = "-2pt";
            doc.Body.Children.Add(new RenderText("CharSpacing: " + cs.ToString()));
            rt = MakeSampleRenderText();
            rt.Style.CharSpacing = cs;
            doc.Body.Children.Add(rt);
            rt.LayoutChangeAfter = new LayoutChangeNewPage(); // page break

            // demo CharWidth:
            rt = MakeDescription("CharWidth");
            doc.Body.Children.Add(rt);

            float cw = 200;
            doc.Body.Children.Add(new RenderText("CharWidth: " + cw.ToString()));
            rt = MakeSampleRenderText();
            rt.Style.CharWidth = cw;
            doc.Body.Children.Add(rt);

            cw = 50;
            doc.Body.Children.Add(new RenderText("CharWidth: " + cw.ToString()));
            rt = MakeSampleRenderText();
            rt.Style.CharWidth = cw;
            doc.Body.Children.Add(rt);
            rt.LayoutChangeAfter = new LayoutChangeNewPage(); // page break

            // demo JustifyChars:
            rt = MakeDescription("JustifyChars");
            doc.Body.Children.Add(rt);

            doc.Body.Children.Add(new RenderText("AlignHorzEnum.JustifyChars:"));
            rt = MakeSampleRenderText();
            rt.Style.TextAlignHorz = AlignHorzEnum.JustifyChars;
            doc.Body.Children.Add(rt);

            doc.Body.Children.Add(new RenderText("AlignHorzEnum.Justify:"));
            rt = MakeSampleRenderText();
            rt.Style.TextAlignHorz = AlignHorzEnum.Justify;
            doc.Body.Children.Add(rt);

            return doc;
        }

        private RenderText MakeSampleRenderText()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 6; ++i)
                sb.Append("The quick brown fox jumps over the lazy dog. ");
            sb.Remove(sb.Length - 1, 1);
            RenderText rt = new RenderText(sb.ToString());
            rt.Style.Borders.All = LineDef.Default;
            rt.Style.BackColor = Colors.LawnGreen;
            return rt;
        }

        private RenderText MakeDescription(string propName)
        {
            StringBuilder sb = new StringBuilder();
            switch (propName)
            {
                case "CharSpacing":
                    sb.Append("Style property: Unit Style.CharSpacing { get; set; }\r\r");
                    sb.Append("Summary:\r");
                    sb.Append("Gets or sets the spacing between characters in a text.\r\r");
                    sb.Append("Remarks:\r");
                    sb.Append("This property is ambient (inherited from the style of the object containing ");
                    sb.Append("the current style's owner if not explicitly set).\r");
                    sb.Append("The default is 0 (normal spacing).");
                    break;
                case "CharWidth":
                    sb.Append("Style property: float Style.CharWidth { get; set; }\r\r");
                    sb.Append("Summary:\r");
                    sb.Append("Gets or sets the amount (in percent) by which to increase or decrease the ");
                    sb.Append("widths of characters in a text.\r\r");
                    sb.Append("Remarks:\r");
                    sb.Append("This property is ambient (inherited from the style of the object containing ");
                    sb.Append("the current style's owner if not explicitly set).\r");
                    sb.Append("The default is 100 (normal width).");
                    break;
                case "JustifyChars":
                    sb.Append("Special mode of horizontal text alignment: AlignHorzEnum.JustifyChars.\r\r");
                    sb.Append("Summary:\r");
                    sb.Append("The text is justified horizontally by adding white spaces between all characters ");
                    sb.Append("in the text. (Note that if C1.C1Preview.C1PrintDocument.UseGdiPlusTextRendering ");
                    sb.Append("is false, this mode is not supported, and C1.C1Preview.AlignHorzEnum.Justify ");
                    sb.Append("is used instead.) ");
                    break;
            }
            RenderText rt = new RenderText(sb.ToString());
            rt.Style.Borders.All = new LineDef("1mm", Colors.Red);
            rt.Style.Spacing.Bottom = "10mm";
            return rt;
        }
    }
}
