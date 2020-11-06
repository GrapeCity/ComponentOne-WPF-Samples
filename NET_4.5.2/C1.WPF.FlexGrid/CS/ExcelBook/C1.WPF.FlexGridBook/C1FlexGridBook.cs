using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace C1.WPF.FlexGridBook
{
    using C1.WPF;
    using C1.WPF.Excel;
    using C1.WPF.FlexGrid;

    /// <summary>
    /// Class that extends the C1FlexGrid control to provide Excel-like look and 
    /// feel including a multi-sheet tabbed interface, support for cell formatting, 
    /// outlining, and XLSX file import and export.
    /// </summary>
    public class C1FlexGridBook : C1FlexGrid
    {
        //--------------------------------------------------------------------
        #region ** fields

        ExcelContextMenu _menu;             // context menu
        ExcelCalcEngine _ce;                // calc engine
        ExcelUndoStack _undo;               // undo/redo stack
        PasteAction _pasteAction;           // for recording undoable paste actions
        HitTestInfo _htDown;                // for selecting columns
        bool _selecting;                    // state for selecting columns
        List<Column> _selColumns;           // selected columns
        C1FlexGridFilter _filter;           // Excel-style column filter

        Grid _tabHolder;                    // bottom panel with tabs and scrollbar
        GridSplitter _splitter;             // between tabs and scrollbar
        C1TabControl _tabs;                 // sheet tabs
        ScrollBar _sbHorz;                  // horizontal scrollbar
        Button _btnFirst, _btnPrev, _btnNext, _btnLast; // sheet selection
        SheetCollection _sheets;            // sheet content

        bool _markingRange;                 // marking a range to insert into formula
        CellRange _fRange;                  // range being marked to insert into formula
        Rectangle _rcRange;                 // marquee around formula range
        static Regex _rxCellRef = new Regex("[A-Z]+[0-9]+(:[A-Z]+[0-9]+)?$", RegexOptions.Compiled);

        // default color scheme for the grid
        const ColorScheme _defaultColorScheme = ColorScheme.Blue;

        #endregion

        //--------------------------------------------------------------------
        #region ** ctor

        /// <summary>
        /// Initializes a new instance of a C1FlexGridBook.
        /// </summary>
        public C1FlexGridBook()
        {
            this.DefaultStyleKey = typeof(C1FlexGridBook);

            // create context menu
            _menu = new ExcelContextMenu(this);

            // create calc engine
            _ce = new ExcelCalcEngine(this);
#if DEBUG
            _ce.Test();
#endif
            // add some rows/columns by default
            for (int r = 0; r < 50; r++)
            {
                Rows.Add(new ExcelRow());
            }
            for (int c = 0; c < 10; c++)
            {
                Columns.Add(new Column());
            }
        }
        public override void OnApplyTemplate()
        {
            // allow base
            base.OnApplyTemplate();

            // get our parts
            _tabHolder = GetTemplateChild("_tabHolder") as Grid;
            _splitter = GetTemplateChild("_splitter") as GridSplitter;
            _sbHorz = GetTemplateChild("_sbH") as ScrollBar;
            _tabs = GetTemplateChild("_tabs") as C1TabControl;
            _sheets = new SheetCollection(this);

            _btnFirst = GetTemplateChild("_btnFirstSheet") as Button;
            _btnPrev = GetTemplateChild("_btnPreviousSheet") as Button;
            _btnNext = GetTemplateChild("_btnNextSheet") as Button;
            _btnLast = GetTemplateChild("_btnLastSheet") as Button;

            // add images to tab browsing buttons
            _btnFirst.Content = GetImage("FirstSheet.png");
            _btnPrev.Content = GetImage("PreviousSheet.png");
            _btnNext.Content = GetImage("NextSheet.png");
            _btnLast.Content = GetImage("LastSheet.png");

            // configure grid
            this.MergeManager = new ExcelMergeManager();
            this.CellFactory = new ExcelCellFactory(this);

            // apply default color scheme
            ColorSchemeManager.ApplyColorScheme(this, _defaultColorScheme);

            // hook up sheet navigation buttons
            _btnFirst.Click += (s, e) => { Sheets.SelectFirst(); };
            _btnPrev.Click += (s, e) => { Sheets.SelectPrev(); };
            _btnNext.Click += (s, e) => { Sheets.SelectNext(); };
            _btnLast.Click += (s, e) => { Sheets.SelectLast(); };

            // create undo/redo stack
            _undo = new ExcelUndoStack(this);
            _undo.StateChanged += (s, e) =>
            {
                SetValue(CanUndoProperty, _undo.CanUndo);
                SetValue(CanRedoProperty, _undo.CanRedo);
            };

            // enable column filtering
            _filter = new C1FlexGridFilter(this);
            _filter.UseCollectionView = true;
            _filter.Editor = new ExcelFilterEditor();

            // connect handlers to select columns and to insert references into formulas
            PreviewMouseLeftButtonDown += mouseLeftButtonDown;
            MouseLeftButtonUp += mouseLeftButtonUp;
            MouseMove += mouseMove;
            SelectionChanged += selectionChanged;
            LostMouseCapture += lostMouseCapture;
            CellEditEnded += cellEditEnded;

            // connect handler to resize selected columns
            ResizedColumn += resizedColumn;
        }

        #endregion

        //--------------------------------------------------------------------
        #region ** object model

        /// <summary>
        /// Invalidate when the DataContext changes (since we don't get notifications in SL4).
        /// </summary>
        new public object DataContext
        {
            get { return base.DataContext; }
            set
            {
                base.DataContext = value;
                Invalidate();
            }
        }
        /// <summary>
        /// Gets the collection of sheets on this book.
        /// </summary>
        public SheetCollection Sheets
        {
            get { return _sheets; }
        }
        /// <summary>
        /// Adds a new sheet to this book.
        /// </summary>
        public bool AddSheet(string sheetName, int rows, int cols)
        {
            // make sure we have sheets
            ApplyTemplate();
            if (Sheets == null)
            {
                return false;
            }

            // create a new sheet
            var newSheet = new Sheet();

            newSheet.Name = sheetName;

            // add rows and columns to the sheet
            for (int r = 0; r < rows; r++)
            {
                newSheet.Grid.Rows.Add(new ExcelRow());
            }
            for (int c = 0; c < cols; c++)
            {
                newSheet.Grid.Columns.Add(new Column());
            }

            // add sheet to grid, select it
            Sheets.Add(newSheet);
            Sheets.SelectedSheet = newSheet;

            // done
            return true;
        }
        #region Import

        /// <summary>
        /// Load txt file to ExcelBook
        /// </summary>
        /// <param name="stream">txt stream</param>
        /// <param name="sheetName">The sheet name of ExcelBook</param>
        public void LoadTxt(Stream stream, string sheetName)
        {
            // ensure we have _tabs
            this.ApplyTemplate();

            // de-select and clear tabs
            Sheets.Clear();
            this.ItemsSource = null; 
            string txt = string.Empty;
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream))
            {
                txt = reader.ReadToEnd();
            }

            if (txt.Length > 0)
            {
                Rows.Clear();
                Columns.Clear();
                string newText = txt.Replace("\r\n", "\r");

                // remove trailing break if any (Excel always adds one)
                if (newText[newText.Length - 1] == '\r')
                {
                    newText = newText.Substring(0, newText.Length - 1);
                }

                var lines = newText.Split('\r', '\n');
                int rowCount = lines.Length;
                if (rowCount <= 0)
                {
                    return;
                }
                int colCount = lines[0].Split('\t').Length;

                this.AddSheet("temp", rowCount, colCount);
                Sheets.SelectedSheet.SheetName = sheetName;

                CellRange rang = new CellRange(0, 0, rowCount - 1, colCount - 1);
                SetClipString(txt, rang, ClipboardCopyMode.None);

            }
        }

        /// <summary>
        /// Load simple CSV file to ExcelBook
        /// </summary>
        /// <param name="stream">CSV stream</param>
        /// <param name="sheetName">The sheet name of ExcelBook</param>
        public void LoadCsv(Stream stream, string sheetName, string separator = null)
        {
            // ensure we have _tabs
            this.ApplyTemplate();

            // de-select and clear tabs
            Sheets.Clear();
            this.ItemsSource = null;
            C1XLBook l = new C1XLBook();
            l.ListSeparator = separator;
            l.Load(stream, C1.WPF.Excel.FileFormat.Csv, false);
            if (l.Sheets.Count>0)
            {
                var sheet = l.Sheets[0];
                this.AddSheet("temp", sheet.Rows.Count, sheet.Columns.Count);
                Sheets.SelectedSheet.SheetName = sheetName;

                int rowCount = sheet.Rows.Count;
                int columnCount = sheet.Columns.Count;

                for (int r = 0; r < rowCount; r++)
                {
                    for (int c = 0; c < Columns.Count; c++)
                    {
                        var cell = sheet[r, c];
                        if (cell != null)
                        {
                            if (cell.Value != null)
                            {
                                // save value
                                this[r, c] = cell.Value;
                            }

                        }
                    }
                }
            }
            
        }

        /// <summary>
        /// Load excel file to ExcelBook
        /// </summary>
        /// <param name="stream">Excel stream</param>
        /// <param name="fileFormat"> fileformat of excel, support .xlsx and .xls</param>
        public void LoadExcel(Stream stream, Excel.FileFormat fileFormat)
        {
            // ensure we have _tabs
            this.ApplyTemplate();

            // de-select and clear tabs
            Sheets.Clear();
            this.ItemsSource = null;
            // load the stream
            var book = new C1XLBook();
            book.Load(stream, fileFormat);

            // load each sheet
            foreach (XLSheet xlSheet in book.Sheets)
            {
                Sheets.Add(new Sheet(xlSheet));
            }

            // select the first tab
            if (Sheets.Count > 0)
            {
                Sheets.SelectedIndex = 0;
            }
        }

        #endregion

        /// <summary>
        /// Saves the current book into an XLSX stream.
        /// </summary>
        /// <param name="stream"></param>
        public void Save(Stream stream)
        {
            // create the book to save
            var book = new C1XLBook();
            book.Sheets.Clear();

            // no sheets
            if (Sheets.Count == 0)
            {
                var xlSheet = book.Sheets.Add("Sheet1");
                ExcelFilter.Save(this, xlSheet);
            }
            else
            {
                // save each sheet
                foreach (var sheet in Sheets)
                {
                    var xlSheet = book.Sheets.Add(sheet.SheetName);
                    ExcelFilter.Save(sheet.Grid, xlSheet);
                }
            }

            // save the book
            book.Save(stream, C1.WPF.Excel.FileFormat.OpenXml);
        }
        /// <summary>
        /// Gets or sets the value stored in a grid cell using Excel range notation.
        /// </summary>
        /// <param name="cellReference">Cell reference using Excel notation (e.g. "A1", "AB32").</param>
        /// <returns>The value stored in the cell.</returns>
        public object this[string cellReference]
        {
            get
            {
                var rng = GetCellRange(cellReference);
                return this[rng.Row, rng.Column];
            }
            set
            {
                var rng = GetCellRange(cellReference);
                this[rng.Row, rng.Column] = value;
            }
        }
        /// <summary>
        /// Gets the address of a range using Excel notation (e.g. A1).
        /// </summary>
        /// <param name="rng"><see cref="CellRange"/> that defines the range.</param>
        /// <returns>The address of the range using Excel notation (e.g. A1).</returns>
        public string GetAddress(CellRange rng, bool fullRange)
        {
            var address = string.Format("{0}{1}", GetAlphaColumnHeader(rng.Column), rng.Row + 1);
            if (fullRange && !rng.IsSingleCell)
            {
                address += string.Format(":{0}{1}", GetAlphaColumnHeader(rng.Column2), rng.Row2 + 1);
            }
            return address;

        }
        /// <summary>
        /// Evaluates an expression and returns the result.
        /// </summary>
        /// <param name="expression">String containing the expression to evaluate.</param>
        /// <returns>The result of the expression.</returns>
        public object Evaluate(string expression)
        {
            return _ce.Evaluate(expression);
        }
        /// <summary>
        /// Gets the <see cref="ContextMenu"/> associated with this <see cref="C1FlexGridBook"/>.
        /// </summary>
        /// <remarks>
        /// Callers may customize the context menu by modifying the content of the Items property.
        /// </remarks>
        public ExcelContextMenu ContextMenu
        {
            get { return _menu; }
        }
        /// <summary>
        /// Gets the <see cref="UndoStack"/> associated with this <see cref="C1FlexGridBook"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="IUndoableAction"/> elements added to the UndoStack can be undone/re-done later.
        /// </remarks>
        public ExcelUndoStack UndoStack
        {
            get { return _undo; }
        }
        /// <summary>
        /// Saves the grid to a Xlsx file.
        /// </summary>
        /// <param name="stream">Stream where the file is to be saved.</param>
        public void SaveXlsx(Stream stream)
        {
            Save(stream);
        }
#if false // TODO: PDF
        /// <summary>
        /// Saves the grid to a PDF stream.
        /// </summary>
        /// <param name="stream">Stream where the file is to be saved.</param>
        public void SavePdf(Stream stream)
        {
            GridExport.SavePdf(this, stream);
        }
        /// <summary>
        /// Saves the grid to a PDF stream.
        /// </summary>
        /// <param name="stream">Stream where the file is to be saved.</param>
        /// <param name="options"><see cref="PdfExportOptions"/> class with parameters 
        /// that customize the PDF output.</param>
        public void SavePdf(Stream stream, PdfExportOptions options)
        {
            GridExport.SavePdf(this, stream, options);
        }
#endif
        /// <summary>
        /// Gets or sets the color scheme used to render the grid.
        /// </summary>
        public ColorScheme ColorScheme
        {
            get { return (ColorScheme)GetValue(ColorSchemeProperty); }
            set { SetValue(ColorSchemeProperty, value); }
        }
        /// <summary>
        /// Identifies the <see cref="ColorScheme"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorSchemeProperty =
            DependencyProperty.Register(
                "ColorScheme",
                typeof(ColorScheme),
                typeof(C1FlexGridBook),
                new PropertyMetadata(_defaultColorScheme, C1FlexGridBook.OnColorSchemeChanged));
        // apply the new color scheme to the grid
        static void OnColorSchemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = (C1FlexGridBook)d;
            ColorSchemeManager.ApplyColorScheme(grid, grid.ColorScheme);
        }
        /// <summary>
        /// Undoes the last action (valid only when <see cref="CanUndo"/> returns true).
        /// </summary>
        public void Undo()
        {
            _undo.Undo();
        }
        /// <summary>
        /// Gets a value that determines whether the undo stack contains any actions.
        /// </summary>
        public bool CanUndo
        {
            get { return (bool)GetValue(CanUndoProperty); }
        }
        /// <summary>
        /// Identifies the <see cref="CanUndo"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanUndoProperty =
            DependencyProperty.Register(
                "CanUndo",
                typeof(bool),
                typeof(C1FlexGridBook),
                new PropertyMetadata(false));
        /// <summary>
        /// Repeats the last action (valid only when <see cref="CanRedo"/> returns true).
        /// </summary>
        public void Redo()
        {
            _undo.Redo();
        }
        /// <summary>
        /// Gets a value that determines whether the redo stack contains any actions.
        /// </summary>
        public bool CanRedo
        {
            get { return (bool)GetValue(CanRedoProperty); }
        }
        /// <summary>
        /// Identifies the <see cref="CanUndo"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanRedoProperty =
            DependencyProperty.Register(
                "CanRedo",
                typeof(bool),
                typeof(C1FlexGridBook),
                new PropertyMetadata(false));
        /// <summary>
        /// Gets or sets whether the grid should provide column filters.
        /// </summary>
        public bool EnableColumnFilters
        {
            get { return (bool)GetValue(EnableColumnFiltersProperty); }
            set { SetValue(EnableColumnFiltersProperty, value); }
        }
        /// <summary>
        /// Identifies the <see cref="EnableColumnFiltersProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EnableColumnFiltersProperty =
            DependencyProperty.Register(
                "EnableColumnFilters",
                typeof(bool),
                typeof(C1FlexGridBook),
                new PropertyMetadata(true, C1FlexGridBook.OnEnableColumnFiltersChanged));
        static void OnEnableColumnFiltersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = (C1FlexGridBook)d;
            if ((bool)e.NewValue)
            {
                grid._filter.Owner = grid;
            }
            else
            {
                grid._filter.Owner = null;
            }
        }
        /// <summary>
        /// Show the filter editor for the currently selected column.
        /// </summary>
        public void ShowFilterEditor()
        {
            var index = Selection.Column;
            if (index > -1)
            {
                _filter.ShowFilterEditor(Columns[index]);
            }
        }
        /// <summary>
        /// Provide internal access to tabs.
        /// </summary>
        internal C1TabControl Tabs
        {
            get { return _tabs; }
        }
        /// <summary>
        /// Gets a reference to the grid's calc engine (used by the rows).
        /// </summary>
        internal ExcelCalcEngine CalcEngine
        {
            get { return _ce; }
        }

        #endregion

        //---------------------------------------------------------------------------
        #region ** overrides

        /// <summary>
        /// Overridden to handle keyboard shortcuts.
        /// </summary>
        /// <param name="e"><see cref="KeyEventArgs"/> that contains the event data.</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            // default processing
            base.OnPreviewKeyDown(e);
            if (!e.Handled)
            {
                // additional key handling
                var ctl = (Keyboard.Modifiers & ModifierKeys.Control) != 0;
                var shft = (Keyboard.Modifiers & ModifierKeys.Shift) != 0;

                // honor undo/redo shortcuts (CTRL-Z, CTRL-Y)
                if (ctl)
                {
                    switch (e.Key)
                    {
                        case Key.Z:
                            e.Handled = true;
                            this.Undo();
                            break;
                        case Key.Y:
                            e.Handled = true;
                            this.Redo();
                            break;
                    }
                }

                // delete selection content on Delete key
                if (e.Key == Key.Delete && !shft)
                {
                    e.Handled = true;
                    ClearSelection();
                }

                // delete current cell content and start editing on BackSpace key
                // (like Excel)
                if (e.Key == Key.Back)
                {
                    var sel = this.Selection;
                    if (sel.IsValid)
                    {
                        // clear cursor cell and start editing
                        ClearRange(new CellRange(sel.Row, sel.Column));
                        StartEditing(true);
                    }
                }
            }
        }
        /// <summary>
        /// Clears all nullable cells in the selection.
        /// </summary>
        public void ClearSelection()
        {
            ClearRange(Selection);
        }
        /// <summary>
        /// Clears all nullable/editable cells in a given range.
        /// </summary>
        public void ClearRange(CellRange range)
        {
            if (!IsReadOnly)
            {
                // create undoable paste action to represent this clear operation
                _pasteAction = new PasteAction(this);

                // clear selected cells
                var invalidate = false;
                foreach (var cell in range.Cells)
                {
                    if (AllowEditing(cell.Row, cell.Column))
                    {
                        this[cell.Row, cell.Column] = null;
                        invalidate |= this.Columns[cell.Column].GroupAggregate != Aggregate.None;
                    }
                }

                // if the range that was cleared contains aggregates, 
                // invalidate grid to force an update
                if (invalidate)
                {
                    Invalidate();
                }

                // close the paste action and add to the undo stack
                if (_pasteAction.SaveNewState())
                {
                    UndoStack.AddAction(_pasteAction);
                }
                _pasteAction = null;
            }
        }
        /// <summary>
        /// Overridden to enable undoing paste actions.
        /// </summary>
        public override void Paste()
        {
            // paste into active editor, if any
            var tb = GetActiveTextBox();
            if (tb != null)
            {
                // note: setting SelectedText doesn't work with binding
                // looks like a bug in the TextBox control.
                var clip = System.Windows.Clipboard.GetText();
                var txt = tb.Text;
                var ss = tb.SelectionStart;
                var sl = tb.SelectionLength;
                tb.Text = txt.Substring(0, ss) + clip + txt.Substring(ss + sl);
                tb.Select(ss, clip.Length);
                tb.Focus();
                return;
            }

            // create undoable paste action to represent this paste operation
            _pasteAction = new PasteAction(this);

            // perform the paste operation 
            // (all flex[r,c] will be recorded while _pasteAction != null)
            base.Paste();

            // close the paste action and add to the undo stack
            if (_pasteAction.SaveNewState())
            {
                _undo.AddAction(_pasteAction);
            }
            _pasteAction = null;
        }
        /// <summary>
        /// Overridden to copy editor content if the editor is active.
        /// </summary>
        public override void Copy()
        {
            // copy from active editor, if any
            var tb = GetActiveTextBox();
            if (tb != null && tb.SelectionLength > 0)
            {
                System.Windows.Clipboard.SetText(tb.SelectedText);
                tb.Focus();
                return;
            }

            // allow default behavior
            base.Copy();
        }
        /// <summary>
        /// Overridden to record individual cell assignments during a paste operation.
        /// </summary>
        public override object this[int row, Column col]
        {
            get { return base[row, col]; }
            set
            {
                // prevent pasting if the cell is read-only
                if (!AllowEditing(row, col.Index))
                {
                    return;
                }

                // if recording the paste operation, add this assignment to the list
                if (_pasteAction != null)
                {
                    _pasteAction.PasteCell(row, col.Index, base[row, col], value);
                }

                try
                {
                    // delegate to base class
                    base[row, col] = value;
                }
                catch
                {
                    // cannot paste this value (wrong type, etc)
                }
            }
        }
        /// <summary>
        /// Overridden to update the layout of the panel with sheet tabs and horizontal scrollbar.
        /// </summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var sz = base.ArrangeOverride(finalSize);
            UpdateTabVisibility();
            return sz;
        }
        /// <summary>
        /// Invalidate all formula cells after any edits.
        /// </summary>
        /// <param name="e"><see cref="CellEditEventArgs"/> that contains the event data.</param>
        protected override void OnCellEditEnded(CellEditEventArgs e)
        {
            foreach (var cell in ViewRange.Cells)
            {
                var row = Rows[cell.Row] as ExcelRow;
                if (row != null)
                {
                    var col = Columns[cell.Column];
                    if (row.GetFormula(col) != null)
                    {
                        Invalidate(cell);
                    }
                }
            }
            base.OnCellEditEnded(e);
        }

        #endregion

        //--------------------------------------------------------------------
        #region ** event handlers

        // handle clicks on column headers to select them
        void mouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // ignore if the event has already been handled
            if (e.Handled)
            {
                return;
            }

            // get the grid element that was clicked
            _htDown = HitTest(e);

            // insert reference to clicked cell into editor formula
            if (_htDown.CellType == CellType.Cell && this.Selection.IsValid)
            {
                // get active editor
                var tb = GetActiveTextBox();
                if (tb != null)
                {
                    // check whether we want to do range editing
                    var text = tb.Text.Trim();
                    if (_markingRange || (text.StartsWith("=") && !char.IsLetterOrDigit(text[text.Length - 1])))
                    {
                        // this event is ours
                        e.Handled = true;

                        // extend range on shift-click
                        var rng = _htDown.CellRange;
                        if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0 && FormulaRange.IsValid)
                        {
                            rng.Row = FormulaRange.Row;
                            rng.Column = FormulaRange.Column;
                        }

                        // start building cell reference into current formula
                        _markingRange = true;
                        FormulaRange = rng;
                    }
                }
            }

            // select columns when clicking on column headers
            if (_htDown.CellType == CellType.ColumnHeader)
            {
                // make sure the click was not in the resizing area
                const int RESIZE_THRESHOLD = 3;
                if (Math.Abs(_htDown.Point.X - _htDown.Rect.X) <= RESIZE_THRESHOLD ||
                    Math.Abs(_htDown.Point.X - _htDown.Rect.Right) <= RESIZE_THRESHOLD)
                {
                    _htDown = null;
                }

                // select the column
                if (_htDown != null)
                {
                    // get column that was clicked
                    var col = Columns[_htDown.Column];

                    // reset selection if control key is not pressed
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == 0)
                    {
                        foreach (var c in Columns)
                        {
                            c.Selected = c == col;
                        }
                    }

                    // save selected columns to handle mouse move
                    _selColumns = Columns.Selected;

                    // select column that was clicked
                    _selecting = true;
                    Select(0, _htDown.Column, Rows.Count - 1, _htDown.Column, false);
                    col.Selected = true;
                    _selecting = false;
                }
            }
        }
        void mouseLeftButtonUp(object sender, EventArgs e)
        {
            // reset mouse state
            _htDown = null;
        }
        void lostMouseCapture(object sender, EventArgs e)
        {
            mouseLeftButtonUp(sender, e);
        }

        // update formula as the user selects cells with the mouse while editing a cell
        // select columns as the user drags the mouse
        void mouseMove(object sender, MouseEventArgs e)
        {
            if (_markingRange && e.LeftButton == MouseButtonState.Pressed)
            {
                var ht = HitTest(e);
                if (ht.CellType == CellType.Cell)
                {
                    var rng = FormulaRange;
                    rng.Column2 = ht.CellRange.Column;
                    rng.Row2 = ht.CellRange.Row;
                    FormulaRange = rng;
                }
            }
            if (_htDown != null && _htDown.CellType == CellType.ColumnHeader)
            {
                var ht = HitTest(e);
                if (ht.CellType == CellType.ColumnHeader)
                {
                    // get selection range
                    var c1 = Math.Min(_htDown.Column, ht.Column);
                    var c2 = Math.Max(_htDown.Column, ht.Column);

                    // select columns in range or in the original selection
                    for (int c = 0; c < Columns.Count; c++)
                    {
                        var col = Columns[c];
                        col.Selected =
                            (c >= c1 && c <= c2) ||
                            (_selColumns != null && _selColumns.Contains(col));
                    }
                }
            }
        }

        // finish editing formula range
        void cellEditEnded(object sender, CellEditEventArgs e)
        {
            _markingRange = false;
            FormulaRange = CellRange.Empty;
        }

        // reset column selection when the regular selection changes
        void selectionChanged(object sender, CellRangeEventArgs e)
        {
            if (!_selecting)
            {
                foreach (var c in Columns)
                {
                    c.Selected = false;
                }
            }
        }

        // resize all selected columns at once
        void resizedColumn(object sender, CellRangeEventArgs e)
        {
            var col = Columns[e.Column];
            if (col.Selected)
            {
                foreach (var c in Columns.Selected)
                {
                    c.Width = col.Width;
                }
            }
        }

        #endregion

        //--------------------------------------------------------------------
        #region ** implementation

        // update the range to be inserted into the formula
        CellRange FormulaRange
        {
            get { return _fRange; }
            set
            {
                if (value != _fRange)
                {
                    // update formula range
                    _fRange = value;

                    // update textbox content (as the user moves the mouse)
                    var tb = GetActiveTextBox();
                    if (tb != null && _fRange.IsValid)
                    {
                        // remove last cell reference from textbox
                        var m = _rxCellRef.Match(tb.Text);
                        if (m.Success)
                        {
                            tb.Text = tb.Text.Substring(0, m.Index);
                        }

                        // append current address to textbox
                        var address = GetAddress(FormulaRange, true);
                        tb.Text += address;
                        tb.Select(tb.Text.Length, 0);
                    }

                    // initialize marquee
                    var canvas = Marquee.Parent as Canvas;
                    if (_rcRange == null)
                    {
                        _rcRange = new Rectangle();
                        _rcRange.Stroke = Marquee.Stroke;
                        _rcRange.StrokeThickness = 2;
                        _rcRange.StrokeDashArray = new DoubleCollection(new double[] { 2, 2 }); ;
                        canvas.Children.Add(_rcRange);
                    }

                    // update marquee
                    var rc = Cells.GetCellRect(_fRange);
                    var szCanvas = canvas.RenderSize;
                    if (_fRange.IsValid &&
                        rc.Width > 0 && rc.Height > 0 &&
                        szCanvas.Width > 0 && szCanvas.Height > 0)
                    {
                        var hdrX = RowHeaders.Visibility == Visibility.Visible ? RowHeaders.ActualWidth : 0;
                        var hdrY = ColumnHeaders.Visibility == Visibility.Visible ? ColumnHeaders.ActualHeight : 0;
                        var w = _rcRange.StrokeThickness;
                        rc = new Rect(hdrX + rc.X - w / 2 - 0.5, hdrY + rc.Y - w / 2 - 0.5, rc.Width + w, rc.Height + w);
                        _rcRange.SetValue(Canvas.LeftProperty, rc.X);
                        _rcRange.SetValue(Canvas.TopProperty, rc.Y);
                        _rcRange.Width = Math.Min(rc.Width, Math.Max(0, szCanvas.Width - rc.X));
                        _rcRange.Height = Math.Min(rc.Height, Math.Max(0, szCanvas.Height - rc.Y));
                        _rcRange.Visibility = Visibility.Visible;
                        _rcRange.StrokeDashOffset++;
                    }
                    else
                    {
                        _rcRange.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        // show/hide tabs/scrollbar/splitter
        void UpdateTabVisibility()
        {
            // check which elements should be visible
            var tabVisible = _tabs.Items.Count > 0;
            var sbVisible = _sbHorz.Visibility == Visibility.Visible;

            if (!tabVisible && !sbVisible)
            {
                // none visible, hide the tab holder
                _tabHolder.Visibility = Visibility.Collapsed;
            }
            else
            {
                // at least one is visible, show the tab holder
                _tabHolder.Visibility = Visibility.Visible;

                // show splitter if tabs and scrollbar are visible
                _splitter.Visibility = tabVisible && sbVisible ? Visibility.Visible : Visibility.Collapsed;

                // show/hide tabs/scrollbar
                ShowGridColumn(0, 2, tabVisible);
                ShowGridColumn(2, 0, sbVisible);
            }
        }
        void ShowGridColumn(int column, int otherColumn, bool show)
        {
            var cd = _tabHolder.ColumnDefinitions[column];
            var cdo = _tabHolder.ColumnDefinitions[otherColumn];
            if (show && cd.ActualWidth == 0)
            {
                cd.Width = cdo.Width = new GridLength(1, GridUnitType.Star);
            }
            if (!show && cd.ActualWidth > 0)
            {
                cd.Width = new GridLength(0);
            }
        }

        // gets the active TextBox editor
        TextBox GetActiveTextBox()
        {
            var bdr = ActiveEditor;
            return Util.Util.GetFirstChildOfType<TextBox>(bdr);
        }

        // translates an excel cell reference (e.g. "A1" into a CellRange)
        CellRange GetCellRange(string xlRef)
        {
            // parse reference
            var rx = new System.Text.RegularExpressions.Regex("([A-Z]+)([1-9]+)");
            var m = rx.Match(xlRef.ToUpper());
            if (!m.Success)
            {
                throw new Exception("Invalid cell reference.");
            }

            // parse row
            var row = int.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture);

            // parse column
            var col = 0;
            var colRef = m.Groups[1].Value;
            for (int i = 0; i < colRef.Length; i++)
            {
                col = 26 * col + (colRef[i] - 'A' + 1);
            }

            // return cell range
            return new CellRange(row - 1, col - 1);
        }

        // creates Excel style column headers ("A", "B", ... "AB", etc)
        internal static string GetAlphaColumnHeader(int i)
        {
            return GetAlphaColumnHeader(i, string.Empty);
        }
        static string GetAlphaColumnHeader(int i, string s)
        {
            var rem = i % 26;
            s = (char)((int)'A' + rem) + s;
            i = i / 26 - 1;
            return i < 0 ? s : GetAlphaColumnHeader(i, s);
        }

        // check whether a cell is Read-Only (many possibilities)
        bool AllowEditing(int row, int col)
        {
            // check grid for read-only status
            if (row < 0 || col < 0 || IsReadOnly)
            {
                return false;
            }

            // check row, column for read-only status
            var c = Columns[col];
            var r = Rows[row];
            if (c.IsReadOnly || r.IsReadOnly)
            {
                return false;
            }

            // check binding
            var b = c.Binding;
            if (b != null && (b.Mode == BindingMode.OneWay || b.Mode == BindingMode.OneTime))
            {
                return false;
            }

            // check binding in cell editing template 
            // NOTE: this is not 100% safe, it only works if the editor is a TextBox
            // (will always allow editing CheckBoxes, ComboBoxes, NumericTextBoxes, DatePickers, etc)
            if (c.CellEditingTemplate != null)
            {
                var editor = c.CellEditingTemplate.LoadContent();
                foreach (TextBox tb in GetChildrenOfType<TextBox>(editor))
                {
                    tb.DataContext = r.DataItem;
                    if (tb.IsReadOnly || !tb.IsEnabled)
                    {
                        return false;
                    }
                    var bx = tb.GetBindingExpression(TextBox.TextProperty);
                    if (bx != null && bx.ParentBinding.Mode != BindingMode.TwoWay)
                    {
                        return false;
                    }
                }
                // could check for other control types here...
            }

            // all seems OK
            return true;
        }

        // get all the children of a specific type
        static IEnumerable<T> GetChildrenOfType<T>(DependencyObject e) where T : DependencyObject
        {
            if (e != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(e); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(e, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }
                    foreach (T childOfChild in GetChildrenOfType<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        // get images from resources
        public static Image GetImage(string bmpName)
        {
            var img = new Image();
            img.Source = LoadBitmap(bmpName);
            return img;
        }
        public static BitmapImage LoadBitmap(string bmpName)
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = GetResourceStream(bmpName);
            bmp.EndInit();
            return bmp;
        }
        static Stream GetResourceStream(string resName)
        {
            var asm = typeof(C1FlexGridBook).Assembly;
            foreach (var rn in asm.GetManifestResourceNames())
            {
                if (rn.EndsWith(resName))
                {
                    return asm.GetManifestResourceStream(rn);
                }
            }
            return null;
        }

        #endregion
    }

    /// <summary>
    /// Merge manager based on range list
    /// </summary>
    public class ExcelMergeManager : IMergeManager
    {
        List<CellRange> _ranges;

        // ** ctor
        public ExcelMergeManager()
        {
            _ranges = new List<CellRange>();
        }

        // ** IMergeManager
        public CellRange GetMergedRange(C1FlexGrid grid, CellType cellType, CellRange range)
        {
            if (cellType == CellType.Cell)
            {
                foreach (var mergedRange in _ranges)
                {
                    if (mergedRange.Contains(range))
                        return mergedRange;
                }
            }
            return range;
        }

        // get merged ranges from an Excel sheet
        public void GetMergedRanges(XLSheet sheet)
        {
            _ranges.Clear();
            if (sheet != null && sheet.MergedCells != null)
            {
                foreach (XLCellRange xRng in sheet.MergedCells)
                {
                    var rng = new CellRange(xRng.RowFrom, xRng.ColumnFrom, xRng.RowTo, xRng.ColumnTo);
                    _ranges.Add(rng);
                }
            }
        }

        // save merged ranges to an Excel sheet
        public void SetMergedRanges(XLSheet sheet)
        {
            var mc = sheet.MergedCells;
            mc.Clear();
            foreach (var rng in _ranges)
            {
                mc.Add(rng.TopRow, rng.LeftColumn, rng.RowSpan, rng.ColumnSpan);
            }
        }

        // remove merges that contain a given range
        public void RemoveRange(CellRange sel)
        {
            for (var i = 0; i < _ranges.Count; i++)
            {
                if (sel.Intersects(_ranges[i]))
                {
                    _ranges.RemoveAt(i);
                    i--;
                }
            }
        }

        // merge the given range
        public void AddRange(CellRange sel)
        {
            RemoveRange(sel);
            _ranges.Add(sel);
        }
    }
}
