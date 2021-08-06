using System.Windows.Media;
using C1.C1Preview;

namespace DemoTables
{
    static partial class MakeDocHelper
    {
        static public C1PrintDocument StylesInTables()
        {
            C1.C1Preview.C1PrintDocument doc = new C1.C1Preview.C1PrintDocument();

            RenderText rtxt = new RenderText(doc);
            rtxt.Text = "This test demonstrates multiple inheritance of styles in tables. " +
                "In the table below, the font is redefined for row 4. " +
                "Then, the text color is redefined for column 1. " +
                "Finally, a cell group is defined containing cells in rows 4 to 6, cols 1 & 2, " +
                "and background color is redefined for that cell group. " +
                "\nThe cell at the intersection of row 4 and col 1 combines all 3 styles.\n\n";
            doc.Body.Children.Add(rtxt);

            doc.Style.FontName = "Arial";
            doc.Style.FontSize = 12;

            const int ROWS = 12;
            const int COLS = 6;

            RenderTable rt = new RenderTable(doc);
            rt.Style.Padding.All = new Unit("4mm");
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

            // define a row style
            rt.Rows[4].Style.FontName = "Arial";
            rt.Rows[4].Style.FontSize = 12;
            rt.Rows[4].Style.FontBold = true;
            rt.Rows[4].Style.FontItalic = true;

            // define a column style
            rt.Cols[1].Style.TextColor = Colors.DarkOrange;

            // define a cell group with a background color
            rt.UserCellGroups.Add(new UserCellGroup(new System.Drawing.Rectangle(1, 4, 2, 3)));
            rt.UserCellGroups[0].Style.BackColor = Colors.PaleGreen;

            return doc;
        }
    }
}