using System.Drawing;
using System.Windows.Media;
using C1.C1Preview;

namespace DemoTables
{
    static partial class MakeDocHelper
    {
        static public C1PrintDocument LargeTable(int nrows, int ncols)
        {
            C1.C1Preview.C1PrintDocument doc = new C1.C1Preview.C1PrintDocument();

            RenderText rtxt = new RenderText(doc);
            rtxt.Text = "This test demonstrates the efficiency of large tables.\n\n";
            doc.Body.Children.Add(rtxt);

            doc.Style.Font = new Font("Arial", 12, FontStyle.Regular);

            int ROWS = nrows;
            int COLS = ncols;

            RenderTable rt = new RenderTable(doc);
            // allocate 3cm per column:
            rt.Width = new Unit(ncols * 30, UnitTypeEnum.Mm);
            rt.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
            for (int row = 0; row < ROWS; ++row)
            {
                for (int col = 0; col < COLS; ++col)
                {
                    RenderText celltext = new RenderText(doc);
                    celltext.Text = string.Format("Cell ({0},{1})", row, col);
                    rt.Cells[row, col].RenderObject = celltext;
                }
            }
            // add the table to the document
            doc.Body.Children.Add(rt);

            // set up table style
            rt.Style.GridLines.All = new LineDef("2pt", Colors.Black);
            rt.Style.GridLines.Horz = new LineDef("1pt", Colors.Gray);
            rt.Style.GridLines.Vert = new LineDef("1pt", Colors.Gray);

            return doc;
        }
    }
}