using System.Drawing;
using System.Text;
using System.Windows.Media;
using C1.C1Preview;

namespace DemoTables
{
    static partial class MakeDocHelper
    {
        static public C1PrintDocument WideTable()
        {
            C1.C1Preview.C1PrintDocument doc = new C1.C1Preview.C1PrintDocument();

            RenderText rtxt = new RenderText();
            rtxt.Text = "This test demonstrates horizontal (extended) pages, which allow to " +
                "render for example tables with many columns on separate pages that can " +
                "be \"glued\" side to side.\n\n";
            doc.Body.Children.Add(rtxt);

            doc.Style.Font = new Font("Arial", 12, FontStyle.Regular);

            const int ROWS = 63;
            const int COLS = 16;

            RenderTable rt = new RenderTable();
            rt.Width = "16in";
            rt.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
            for (int row = 0; row < ROWS; ++row)
            {
                for (int col = 0; col < COLS; ++col)
                {
                    RenderText celltext = new RenderText();
                    celltext.Text = string.Format("Cell ({0},{1})", row, col);
                    rt.Cells[row, col].RenderObject = celltext;
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 10; i++)
                sb.Append(string.Format("Text fragment{0}", i));
            rt.Cells[1, 1].Text = sb.ToString();
            // add the table to the document
            doc.Body.Children.Add(rt);

            // set up table style
            rt.Style.GridLines.All = new LineDef("2pt", Colors.Black);
            rt.Style.GridLines.Horz = new LineDef("1pt", Colors.Gray);
            rt.Style.GridLines.Vert = new LineDef("1pt", Colors.Gray);

#if skip_this // does not work in WPF viewer yet
            // add some comments at the bottom
            rtxt = new RenderText();
            rtxt.Text = "\n\nNote also that the preview supports such documents by showing " +
                "the pages side by side rather than one below the other (this can be turned off " +
                "if desired). Also, the vertical margins can be hidden in the preview for better viewing.";
            doc.Body.Children.Add(rtxt);
#endif

            return doc;
        }
    }
}