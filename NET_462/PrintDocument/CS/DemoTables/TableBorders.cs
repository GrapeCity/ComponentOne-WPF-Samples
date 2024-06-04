using System.Windows.Media;
using C1.C1Preview;

namespace DemoTables
{
    static partial class MakeDocHelper
    {
        static public C1PrintDocument TableBorders()
        {
            C1PrintDocument doc = new C1PrintDocument();

            RenderText rtxt = new RenderText(doc);
            rtxt.Text = "This test shows a non-standard way to draw table borders.\n\n";
            doc.Body.Children.Add(rtxt);

            doc.Style.FontName = "Arial";
            doc.Style.FontSize = 14;

            const int ROWS = 42;
            const int COLS = 4;// 16;

            RenderTable rt = new RenderTable(doc);
            // rt.Width = "16in";
            rt.Width = "auto";
            rt.ColumnSizingMode = TableSizingModeEnum.Auto;
            rt.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
            for (int row = 0; row < ROWS; ++row)
            {
                for (int col = 0; col < COLS; ++col)
                {
                    //if (row == col)
                        //continue;
                    RenderText celltext = new RenderText(doc);
                    celltext.Text = string.Format("Cell ({0},{1})", row, col);
                    rt.Cells[row, col].RenderObject = celltext;
                }
            }
            // add the table to the document
            doc.Body.Children.Add(rt);

            // set up table style
            rt.Style.GridLines.All = new LineDef("1pt", Colors.Black);
            rt.Style.GridLines.Horz = LineDef.Empty;
            rt.Style.GridLines.Vert = LineDef.Empty;
            rt.CellStyle.Borders.All = new LineDef("1pt", Colors.DarkOrange);
            rt.CellStyle.Spacing.All = new Unit("0.5mm");
            rt.Style.TextAlignVert = AlignVertEnum.Center;

            return doc;
        }
    }
}