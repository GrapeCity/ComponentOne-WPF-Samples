using System.Drawing;
using System.Windows.Media;
using System.Reflection;
using C1.C1Preview;

namespace DemoTables
{
    static partial class MakeDocHelper
    {
        static public C1PrintDocument BasicTable()
        {
            C1.C1Preview.C1PrintDocument doc = new C1.C1Preview.C1PrintDocument();

            RenderText rtxt1 = new RenderText(doc);
            rtxt1.Text = "This test shows the basic features of tables in C1PrintDocument:\n"
                + "\t- table borders (the GridLines style property, which allows to specify 4 outer and 2 inner lines);\n"
                + "\t- borders around individual cells and groups of cells;\n"
                + "\t- style attributes (including borders) for groups of disconnected cells;\n"
                + "\t- cells spanning rows and columns;\n"
                + "\t- content alignment within the cells (spanned or otherwise);\n"
                + "\t- table headers and footers;\n"
                + "\t- tags (such as page number/total page count) anywhere in the document (see the table footer);\n"
                + "\t- different style attributes including borders, font and background images.\n"
                + "\t  \n"
            ;
            rtxt1.Style.Font = new Font(rtxt1.Style.Font.FontFamily, 14);
            rtxt1.Style.Padding.Bottom = new Unit("5mm");
            doc.Body.Children.Add(rtxt1);

            //
            // make a table and fill all its cells with some demo data
            RenderTable rt1 = new RenderTable(doc);
            const int ROWS = 100;
            const int COLS = 4;
            for (int row = 0; row < ROWS; ++row)
            {
                for (int col = 0; col < COLS; ++col)
                {
                    RenderText celltext = new RenderText(doc);
                    celltext.Text = string.Format("Cell ({0},{1})", row, col);
                    // Note that rt1.Cells[row, col] will create cells on demand -
                    // no need to specify the number of rows/cols initially.
                    rt1.Cells[row, col].RenderObject = celltext;
                }
            }
            // add the table to the document
            doc.Body.Children.Add(rt1);

            //
            // unlike the old print-doc, in the new one changes can be made at any
            // point in the program, and they will be reflected in the document when
            // it is rendered. Add some customizations to the table:

            //
            // by default, tables have no borders. add a simple border:
            rt1.Style.GridLines.All = new LineDef("2pt", Colors.DarkGray);
            rt1.Style.GridLines.Horz = new LineDef("1pt", Colors.Blue);
            rt1.Style.GridLines.Vert = new LineDef("1pt", Colors.Brown);

            //
            // table headers and footers

            // add a table header:
            // setup the header as a whole
            rt1.RowGroups[0, 2].PageHeader = true;
            System.Drawing.Image background = System.Drawing.Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("DemoTables.orange.jpg"));
            rt1.RowGroups[0, 2].Style.BackgroundImage = background;
            rt1.RowGroups[0, 2].Style.BackgroundImageAlign.StretchHorz = true;
            rt1.RowGroups[0, 2].Style.BackgroundImageAlign.StretchVert = true;
            rt1.RowGroups[0, 2].Style.BackgroundImageAlign.KeepAspectRatio = false;
            // multiple inheritance supported in styles: the text color from the
            // group's style will merge with the font from the cell's own style:
            rt1.RowGroups[0, 2].Style.TextColor = Colors.LightGreen;
            rt1.RowGroups[0, 2].Style.GridLines.All = new LineDef("2pt", Colors.DarkCyan);
            rt1.RowGroups[0, 2].Style.GridLines.Horz = LineDef.Empty;
            rt1.RowGroups[0, 2].Style.GridLines.Vert = LineDef.Empty;
            // setup specific cells in the header:
            ((RenderText)rt1.Cells[0, 0].RenderObject).Text = "Header row 0";
            ((RenderText)rt1.Cells[1, 0].RenderObject).Text = "Header row 1";
            rt1.Cells[0, 1].SpanCols = 2;
            rt1.Cells[0, 1].SpanRows = 2;
            rt1.Cells[0, 1].RenderObject = new RenderText(doc);
            ((RenderText)rt1.Cells[0, 1].RenderObject).Text = "Multi-row table headers and footers are supported";
            rt1.Cells[0, 1].Style.TextAlignHorz = AlignHorzEnum.Center;
            rt1.Cells[0, 1].Style.Font = new Font("Arial", 14, FontStyle.Bold);

            // setup a table footer
            rt1.RowGroups[rt1.Rows.Count - 2, 2].PageFooter = true;
            rt1.RowGroups[rt1.Rows.Count - 2, 2].Style.BackColor = Colors.LemonChiffon;
            rt1.Cells[rt1.Rows.Count - 2, 0].SpanRows = 2;
            rt1.Cells[rt1.Rows.Count - 2, 0].SpanCols = rt1.Cols.Count - 1;
            rt1.Cells[rt1.Rows.Count - 2, 0].Style.TextAlignHorz = AlignHorzEnum.Center;
            rt1.Cells[rt1.Rows.Count - 2, 0].Style.TextAlignVert = AlignVertEnum.Center;
            ((RenderText)rt1.Cells[rt1.Rows.Count - 2, 0].RenderObject).Text = "This is a table footer.";
            rt1.Cells[rt1.Rows.Count - 2, 3].SpanRows = 2;
            rt1.Cells[rt1.Rows.Count - 2, 3].Style.TextAlignHorz = AlignHorzEnum.Right;
            // tags (such as page no/page count) can be inserted anywhere in the document
            ((RenderText)rt1.Cells[rt1.Rows.Count - 2, 3].RenderObject).Text = "Page [PageNo] of [PageCount]";

            //
            // in tables, Style.Borders merges seamlessly into the table grid lines:

            // it is easy to put a border around specific cells:
            rt1.Cells[8, 3].Style.Borders.All = new LineDef("3pt", Colors.OrangeRed);
            ((RenderText)rt1.Cells[8, 3].RenderObject).Text =
                "It is easy to put a border around a single cell using cell.Style.Borders";

            //
            // cells can be combined into groups, and their styles can be manipulated as
            // a single entity:

            // define a group of cells by specifying the rectangles bounding the cells:
            ((RenderText)rt1.Cells[3, 2].RenderObject).Text =
                "Cells can be combined into groups to be manipulated as a single entity " +
                "(such as all cells with the pale green background in this table).";
            rt1.Cells[3, 2].SpanCols = 2;
            rt1.Cells[3, 2].SpanRows = 3;
            Rectangle[] cells1 = new Rectangle[] {
                new Rectangle(2, 3, 2, 3),
                new Rectangle(0, 10, 3, 2),
                new Rectangle(1, 23, 2, 4),
                new Rectangle(1, 36, 1, 24),
                new Rectangle(0, 72, 3, 6),
                };
            UserCellGroup grp1 = new UserCellGroup(cells1);
            grp1.Style.BackColor = Colors.PaleGreen;
            grp1.Style.Font = new Font("Arial", 12, FontStyle.Bold);
            grp1.Style.Borders.All = new LineDef("2pt", Colors.DarkGreen);
            rt1.UserCellGroups.Add(grp1);


            // row/col span
            ((RenderText)rt1.Cells[14, 1].RenderObject).Text =
                "Column and row spans are fully supported, as well as alignment within the (spanned or not) cells.";
            rt1.Cells[14, 1].SpanCols = 3;
            rt1.Cells[14, 1].SpanRows = 5;
            rt1.Cells[14, 1].Style.Font = new Font("Arial", 12, FontStyle.Bold | FontStyle.Italic);
            rt1.Cells[14, 1].Style.Borders.All = new LineDef("2pt", Colors.DarkOrange);
            rt1.Cells[14, 1].Style.TextAlignHorz = AlignHorzEnum.Center;
            rt1.Cells[14, 1].Style.TextAlignVert = AlignVertEnum.Center;
            rt1.RowGroups[14, 5].SplitBehavior = SplitBehaviorEnum.SplitIfLarge;

            return doc;
        }
    }
}
