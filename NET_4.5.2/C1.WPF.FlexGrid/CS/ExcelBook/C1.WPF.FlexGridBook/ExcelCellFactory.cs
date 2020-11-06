using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;

namespace C1.WPF.FlexGridBook
{
    using C1.WPF.Excel;
    using C1.WPF.FlexGrid;

    /// <summary>
    /// Custom cell factory that displays Excel-style row and column headers.
    /// </summary>
    public class ExcelCellFactory : CellFactory
    {
        // fields
        C1FlexGrid _flex;

        // static fields
        static Thickness _thickness = new Thickness(4, 0, 4, 0);
        static Thickness _thicknessEmpty = new Thickness(0);
        static BitmapImage _bmpCollapsed, _bmpExpandedAbove, _bmpExpandedBelow, _bmpConnector, _bmpTerminalAbove, _bmpTerminalBelow, _bmpNoChildren;
        static Brush _brBlack = new SolidColorBrush(Colors.Black);
        static CellStyle _styleRight;

        // images that may be present in each grid node
        enum NodeImage
        {
            None,
            Collapsed,
            Expanded,
            Connector,
            Terminal
        }

        /// <summary>
        /// Initializes a new instance of an <see cref="ExcelCellFactory"/>.
        /// </summary>
        /// <param name="flex"><see cref="C1FlexGrid"/> that owns this cell factory.</param>
        public ExcelCellFactory(C1FlexGrid flex)
        {
            _flex = flex;
            if (_styleRight == null)
            {
                _styleRight = new CellStyle();
                _styleRight.HorizontalAlignment = HorizontalAlignment.Right;
            }
        }
        /// <summary>
        /// Provides content for the cells in the top left panel of the grid.
        /// </summary>
        public override void CreateTopLeftContent(C1FlexGrid grid, Border bdr, CellRange range)
        {
            if (range.Row == 0 && range.Column == 0)
            {
                var poly = CreatePolygon(0, 10, 10, 10, 10, 0);
                poly.Fill = new SolidColorBrush(Color.FromArgb(0xe0, 0xf5, 0xf5, 0xf5));
                poly.VerticalAlignment = VerticalAlignment.Center;
                poly.HorizontalAlignment = HorizontalAlignment.Right;
                poly.Margin = new Thickness(2);
                bdr.Child = poly;
            }
        }
        /// <summary>
        /// Provides content for the row header cells.
        /// </summary>
        public override void CreateRowHeaderContent(C1FlexGrid grid, Border bdr, CellRange range)
        {
            // first column contains letters
            if (range.Column == 0)
            {
                var tb = new TextBlock();
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.Text = string.Format("{0}", range.Row + 1);
                bdr.Padding = _thicknessEmpty;
                bdr.Child = tb;
            }

            // apply style to show errors
            if (grid.ShowErrors && grid.ErrorStyle != null)
            {
                var r = grid.Rows[range.Row];
                var errors = r.GetErrors(null);
                if (!string.IsNullOrEmpty(errors))
                {
                    grid.ErrorStyle.Apply(bdr, SelectedState.None);
                    ToolTipService.SetToolTip(bdr, errors);
                }
            }
        }
        /// <summary>
        /// Provides content for the column header cells.
        /// </summary>
        public override void CreateColumnHeaderContent(C1FlexGrid grid, Border bdr, CellRange range)
        {
            base.CreateColumnHeaderContent(grid, bdr, range);
            if (range.Row == 0)
            {
                // create alphabetical header content
                var tb = new TextBlock();
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.Text = C1FlexGridBook.GetAlphaColumnHeader(range.Column);
                bdr.Padding = _thickness;

                // preserve outline bar if it's there
                if (grid.ShowOutlineBar && range.Column == 0)
                {
                    var g = bdr.Child as Grid;
                    if (g != null && g.Children.Count > 0)
                    {
                        var index = g.Children.Count - 1;
                        tb.SetValue(Grid.ColumnProperty, index);
                        g.Children[index] = tb;
                        return;
                    }
                }

                // or simply set the content to the new textblock
                bdr.Child = tb;
            }
        }
        /// <summary>
        /// Provides content for the grid cells.
        /// </summary>
        public override void CreateCellContent(C1FlexGrid grid, Border bdr, CellRange range)
        {
            // check whether the grid contains a FrameworkElement (e.g. hyperlink)
            var cellElement = grid[range.Row, range.Column] as FrameworkElement;
            if (cellElement != null)
            {
                // show element in the cell
                bdr.Child = cellElement;
            }
            else
            {
                // allow default handling
                base.CreateCellContent(grid, bdr, range);
            }

            // honor gridline visibility 
            // (group rows always show horizontal lines by default)
            if (grid.GridLinesVisibility == GridLinesVisibility.None)
            {
                bdr.BorderThickness = _thicknessEmpty;
            }

            // create group and detail cells on the tree column
            var treeCol = GetOutlineColumn(grid);
            if (range.Column <= treeCol)
            {
                CreateOutlineCellContent(grid, bdr, range);
            }
        }
        /// <summary>
        /// Provides editable content for the grid cells.
        /// </summary>
        public override FrameworkElement CreateCellEditor(C1FlexGrid grid, CellType cellType, CellRange range)
        {
            // allow base class
            var bdr = base.CreateCellEditor(grid, cellType, range) as Border;

            // replace editor content with formula text
            var row = grid.Rows[range.Row] as ExcelRow;
            var tb = Util.Util.GetFirstChildOfType<TextBox>(bdr);
            if (tb != null && row != null)
            {
                var col = grid.Columns[range.Column];
                tb.Text = row.GetDataEditor(col);
            }

            // create group and detail cells on the tree column
            int treeCol = GetOutlineColumn(grid);
            if (range.Column <= treeCol)
            {
                CreateOutlineCellContent(grid, bdr, range);
            }

            // done
            return bdr;
        }
        /// <summary>
        /// Applies custom styles to the outline row.
        /// </summary>
        public override void ApplyCellStyles(C1FlexGrid grid, CellType cellType, CellRange range, Border bdr)
        {
            if (cellType == CellType.Cell)
            {
                bool hasAlignment = false;

                // get selection state for the range
                var selState = grid.GetSelectedState(range);

                // apply row style
                var row = grid.Rows[range.Row];
                if (row.CellStyle != null)
                {
                    row.CellStyle.Apply(bdr, selState);
                    hasAlignment |= row.CellStyle.HorizontalAlignment.HasValue;
                }

                // apply column style
                var col = grid.Columns[range.Column];
                if (col.CellStyle != null)
                {
                    col.CellStyle.Apply(bdr, selState);
                    hasAlignment |= col.CellStyle.HorizontalAlignment.HasValue;
                }

                // apply cell style
                var xlr = row as ExcelRow;
                if (xlr != null)
                {
                    var s = xlr.GetCellStyle(col);
                    if (s != null)
                    {
                        // leave hyperlink foreground alone...
                        if (bdr.Child is HyperlinkButton)
                        {
                            s.Foreground = null;
                        }

                        // apply standard CellStyle stuff
                        s.Apply(bdr, selState);
                        hasAlignment |= s.HorizontalAlignment.HasValue;

                        // apply general alignment
                        if (!hasAlignment)
                        {
                            var content = grid[range.Row, range.Column];
                            if (IsNumeric(content))
                            {
                                _styleRight.Apply(bdr, selState);
                            }
                        }
                    }
                }
            }
        }
        bool IsNumeric(object content)
        {
            // no content, easy
            if (content == null)
            {
                return false;
            }

            // check variable type
            var code = Type.GetTypeCode(content.GetType());
            if (code > TypeCode.Char && code <= TypeCode.UInt64)
            {
                return true;
            }
            if (code >= TypeCode.Single && code <= TypeCode.Decimal)
            {
                return true;
            }

            // try parsing
            var text = content.ToString().Trim('%');
            double d;
            return double.TryParse(text, NumberStyles.Any, CultureInfo.CurrentUICulture, out d);
        }

        // creates an outline cell (with collapse/expand/branch graphics)
        void CreateOutlineCellContent(C1FlexGrid grid, Border bdr, CellRange range)
        {
            var row = grid.Rows[range.Row];
            var gr = row as GroupRow;
            if (gr != null)
            {
                // build grid for tree cell
                var img = gr.IsCollapsed ? NodeImage.Collapsed : NodeImage.Expanded;
                var g = CreateCellGrid(row, gr.Level, grid.TreeIndent, img);

                // copy data from original cell
                var p = bdr.Child as Panel;
                if (p != null && p.Children.Count > 1)
                {
                    var tb = p.Children[1] as FrameworkElement;
                    p.Children.Clear();
                    tb.VerticalAlignment = VerticalAlignment.Center;
                    tb.Margin = new Thickness(4, 0, 0, 0);
                    tb.SetValue(Grid.ColumnProperty, gr.Level + 1);
                    g.Children.Add(tb);

                    // assign grid to cell
                    bdr.Padding = _thickness;
                    bdr.Margin = _thicknessEmpty;
                    bdr.Child = g;
                }
            }
            else
            {
                // add connecting lines to detail cells
                var levels = grid.Rows.MaxGroupLevel;
                var g = CreateCellGrid(row, levels - 1, grid.TreeIndent, NodeImage.Connector);

                // copy data from original cell
                var tb = bdr.Child as FrameworkElement;
                bdr.Child = null;
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.SetValue(Grid.ColumnProperty, g.ColumnDefinitions.Count - 1);
                g.Children.Add(tb);

                // assign grid to cell
                bdr.Padding = _thickness;
                bdr.Margin = _thicknessEmpty;
                bdr.Child = g;
            }
        }

        // gets the index of the column that contains the outline
        int GetOutlineColumn(C1FlexGrid grid)
        {
            int col = -1;
            for (int c = 0; c < grid.Columns.Count; c++)
            {
                if (grid.Columns[c].IsVisible)
                {
                    col = c;
                    break;
                }
            }
            return col;
        }

        // creates a grid element with images to represent a grid node
        Grid CreateCellGrid(Row r, int level, double indent, NodeImage nodeImage)
        {
            // create grid
            var g = new Grid();

            // add columns for images and content
            bool above = r.Grid.GroupRowPosition == GroupRowPosition.AboveData;

            // create elements for each level
            for (int i = 0; i < level + 1; i++)
            {
                // add images to the grid
                if (i <= level)
                {
                    // select image to display in this part of the cell
                    var ni = i == level ? nodeImage : NodeImage.Connector;
                    if (ni == NodeImage.Connector)
                    {
                        var index = r.Index;
                        var rows = r.Grid.Rows;

                        // find next group row to determine the image to display
                        int next = index;
                        if (above)
                        {
                            // look for next group below this one
                            for (next = index + 1; next < rows.Count; next++)
                            {
                                if (rows[next].IsVisible)
                                {
                                    var gr = rows[next] as GroupRow;
                                    if (gr != null && gr.Level <= i)
                                    {
                                        ni = NodeImage.Terminal;
                                    }
                                    break;
                                }
                            }
                            if (next == rows.Count)
                            {
                                ni = NodeImage.Terminal;
                            }
                        }
                        else
                        {
                            // look for next group above this one
                            for (next = index - 1; next > -1; next--)
                            {
                                if (rows[next].IsVisible)
                                {
                                    var gr = rows[next] as GroupRow;
                                    if (gr != null && gr.Level <= i)
                                    {
                                        ni = NodeImage.Terminal;
                                    }
                                    break;
                                }
                            }
                            if (next <= 0)
                            {
                                ni = NodeImage.Terminal;
                            }
                        }
                    }

                    // add image
                    var img = GetNodeImage(r, ni);
                    if (img != null)
                    {
                        var cd = new ColumnDefinition();
                        cd.Width = new GridLength(indent);
                        g.ColumnDefinitions.Add(cd);
                        img.SetValue(Grid.ColumnProperty, i);
                        g.Children.Add(img);
                    }
                }
            }

            // and add a cell for the content
            g.ColumnDefinitions.Add(new ColumnDefinition());

            // done
            return g;
        }

        // gets an image to represents a node
        Image GetNodeImage(Row r, NodeImage nodeImage)
        {
            // load bitmaps (once)
            if (_bmpCollapsed == null)
            {
                _bmpCollapsed = LoadBitmap("Collapsed.png");
                _bmpExpandedAbove = LoadBitmap("ExpandedAbove.png");
                _bmpExpandedBelow = LoadBitmap("ExpandedBelow.png");
                _bmpConnector = LoadBitmap("Connector.png");
                _bmpTerminalAbove = LoadBitmap("TerminalAbove.png");
                _bmpTerminalBelow = LoadBitmap("TerminalBelow.png");
                _bmpNoChildren = LoadBitmap("NoChildren.png");
            }

            // get image source
            ImageSource src = null;
            var above = r.Grid.GroupRowPosition == GroupRowPosition.AboveData;
            switch (nodeImage)
            {
                case NodeImage.Collapsed:
                    if (HasVisibleChildren(r))
                    {
                        src = _bmpCollapsed;
                    }
                    break;
                case NodeImage.Expanded:
                    if (HasVisibleChildren(r))
                    {
                        src = above ? _bmpExpandedAbove : _bmpExpandedBelow;
                    }
                    break;
                case NodeImage.Connector:
                    src = _bmpConnector;
                    break;
                case NodeImage.Terminal:
                    src = above ? _bmpTerminalAbove : _bmpTerminalBelow;
                    break;
            }

            // no source? we're done
            if (src == null)
            {
                return null;
            }

            // create image element
            var img = new Image();
            img.Source = src;
            img.Stretch = Stretch.None;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.HorizontalAlignment = HorizontalAlignment.Center;
            img.Tag = r;

            // assign source
            switch (nodeImage)
            {
                case NodeImage.Collapsed:
                case NodeImage.Expanded:
                    img.MouseLeftButtonDown += img_MouseLeftButtonDown;
                    break;
            }

            // done
            return img;
        }
        bool HasVisibleChildren(Row r)
        {
            var gr = r as GroupRow;
            if (gr != null && r.Grid != null)
            {
                var rg = gr.GetCellRange();
                var rows = r.Grid.Rows;
                for (int i = rg.TopRow; i <= rg.BottomRow; i++)
                {
                    var row = rows[i];
                    if (row != gr && row.Visible)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // expands/collapses group rows when user clicks the node icon
        void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var img = sender as Image;
            var groupRow = img.Tag as GroupRow;
            groupRow.IsCollapsed = !groupRow.IsCollapsed;
        }

        // loads a bitmap from an embedded resource
        BitmapImage LoadBitmap(string bmpName)
        {
            var bmp = new BitmapImage();
            bmp.StreamSource = GetResourceStream(bmpName);
            return bmp;
        }

        // gets an embedded resource stream
        Stream GetResourceStream(string resName)
        {
            var asm = this.GetType().Assembly;
            foreach (var rn in asm.GetManifestResourceNames())
            {
                if (rn.EndsWith(resName))
                {
                    return asm.GetManifestResourceStream(rn);
                }
            }
            return null;
        }

        // creates a polygon based on a list of points
        static Polygon CreatePolygon(params double[] values)
        {
            var p = new Polygon();
            p.Points = new PointCollection();
            for (int i = 0; i < values.Length - 1; i += 2)
            {
                p.Points.Add(new Point(values[i], values[i + 1]));
            }
            return p;
        }
    }
}
