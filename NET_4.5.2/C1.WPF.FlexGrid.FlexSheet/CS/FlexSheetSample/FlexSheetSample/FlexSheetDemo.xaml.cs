using C1.WPF;
using C1.WPF.Binding;
using C1.WPF.C1Chart;
using C1.WPF.FlexGrid;
using FlexSheetSample.CustomUndoActions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Printing;
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

namespace FlexSheetSample
{
    /// <summary>
    /// Interaction logic for FlexSheetDemo.xaml
    /// </summary>
    public partial class FlexSheetDemo : UserControl
    {
        //-----------------------------------------------------------------------------
        #region ** fields

        enum CustomContextCommands
        {
            UpperCase,
            LowerCase,
            MakeOdd,
            MakeEven,
            MakeZero,
            True,
            False,
            Toggle
        }

        private Window _findReplaceWindow;
        private Window _insertSparkLineWindow;
        private double _currentIndent = 0;
        private bool _selectionBeingChanged = false;

        #endregion

        //-----------------------------------------------------------------------------
        #region ** ctor

        public FlexSheetDemo()
        {
            InitializeComponent();

            // set language to match the system settings
            this.Language = System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);

            // connect function menu to _tbFx TextBlock
            var fnMenu = new ExcelCalcFunctionMenu();

#if false
            fnMenu.ItemClick += (s, e) =>
                {
                    var item = e.Source as C1MenuItem;
                    var text = (string)item.Header;
                    var selStart = _txtFormula.SelectionStart;
                    _txtFormula.SelectedText = text + "()";
                    _txtFormula.SelectionStart = selStart + text.Length + 1;
                    _txtFormula.Focus();
                };
#endif

            // add some sheets
            _flex.AddSheet("Sheet1", 50, 10);
            _flex.AddSheet("Sheet2", 50, 10);
            _flex.AddSheet("Sheet3", 50, 10);
            _flex.AddSheet("New formulas", 50, 10);
            _flex.Sheets.SelectedIndex = 0;

            var tabContorl = _flex.Sheets.SelectedSheet.Parent as C1TabControl;
            if (tabContorl != null)
            {
                tabContorl.SelectionChanged += (s, e) =>
                {
                    _ribbon.DataContext = _flex.Sheets.SelectedSheet;
                };
            }

            #region Populate Sheet1
            // populate the grid with some formulas (multiplication table)
            for (int r = 0; r < _flex.Rows.Count - 2; r++)
            {
                List<double> datas = new List<double>();
                for (int c = 0; c < _flex.Columns.Count; c++)
                {
                    _flex[r, c] = string.Format("={0}*{1}", r + 1, c + 1);
                    double value = (double)_flex[r, c];
                    datas.Add(value);
                }
            }

            // add a totals row to illustrate
            AddTotalRow();
            
            #endregion

            #region Populate Sheet2
            var protectedSheet = _flex.Sheets["Sheet2"];

            if (protectedSheet != null)
            {
                for (int r = 0; r < 2; r++)
                {
                    for (int c = 0; c < 2; c++)
                    {
                        protectedSheet.Grid[r, c] = "Locked Cell";
                    }
                }

                protectedSheet.IsProtected = true;
                protectedSheet.AddLockedCell(0, 0, 1, 1);
            }
            #endregion

            #region Pupulate Sheet3
            var sparklineSheet = _flex.Sheets["Sheet3"];

            if (sparklineSheet != null)
            {
                for (int r = 0; r < 5; r++)
                {
                    List<double> datas = new List<double>();
                    for (int c = 0; c < 6; c++)
                    {
                        if (c < 5)
                        {
                            Random rnd = new Random(new object().GetHashCode());
                            double num = rnd.Next(10);
                            sparklineSheet.Grid[r, c] = num;
                            datas.Add(num);
                        }
                        else
                        {
                            _flex.InsertSparkLine(SparkLineType.Line, datas, sparklineSheet, new CellRange(r, 5));
                        }
                    }
                }
            }

            #endregion

            #region Populate "New formulas" sheet

            var formulaSheet = _flex.Sheets["New formulas"];

            formulaSheet.Grid[0, 0] = "John";
            formulaSheet.Grid[1, 0] = "Kelvin";
            formulaSheet.Grid[2, 0] = "Micheal";
            formulaSheet.Grid[3, 0] = "Peter";

            formulaSheet.Grid[0, 2] = "Peter"; // VLookup value

            formulaSheet.Grid[0, 1] = 1d;
            formulaSheet.Grid[1, 1] = 2d;
            formulaSheet.Grid[2, 1] = 3d;
            formulaSheet.Grid[3, 1] = 4d;

            formulaSheet.Grid[6, 1] = "Kelvin";
            formulaSheet.Grid[7, 0] = "LOOKUP => ";
            formulaSheet.Grid[7, 1] = "=LOOKUP(B7, $A$1:$A$4, $B$1:$B$4)";
            formulaSheet.Grid[10, 0] = "VLOOKUP => ";
            formulaSheet.Grid[10, 1] = "=VLOOKUP(C1, $A$1:$B$4, 2)";

            formulaSheet.Grid[0, 5] = "John";
            formulaSheet.Grid[0, 6] = "Kelvin";
            formulaSheet.Grid[0, 7] = "Micheal";
            formulaSheet.Grid[0, 8] = "Peter";
            formulaSheet.Grid[1, 5] = 3d;
            formulaSheet.Grid[1, 6] = 4d;
            formulaSheet.Grid[1, 7] = 5d;
            formulaSheet.Grid[1, 8] = 6d;

            formulaSheet.Grid[10, 5] = "HLOOKUP => ";
            formulaSheet.Grid[10, 6] = "=HLOOKUP(\"Kelvin\", F1:I2, 2)";

            formulaSheet.Grid[12, 0] = "NOW => ";
            formulaSheet.Grid[12, 1] = "=NOW()";
            ((ExcelRow)formulaSheet.Grid.Rows[12]).SetCellStyle(formulaSheet.Grid.Columns[1], new ExcelCellStyle() { Format = "f" });
            formulaSheet.Grid[13, 0] = "TODAY => ";
            formulaSheet.Grid[13, 1] = "=TODAY()";
            ((ExcelRow)formulaSheet.Grid.Rows[13]).SetCellStyle(formulaSheet.Grid.Columns[1], new ExcelCellStyle() { Format = "d" });

            // A custom function to find the greatest number in a cell range.
            _flex.CalcEngine.RegisterFunction("GREATEST", 1, (expressions) =>
            {
                CellRangeReference rangeReference = (expressions[0] as XObjectExpression).Value as CellRangeReference;
                if (rangeReference != null)
                {
                    var enumerator = rangeReference.GetEnumerator();
                    double greatest = double.MinValue;
                    while (enumerator.MoveNext())
                    {
                        double? v = (enumerator.Current as double?);
                        if (v.HasValue)
                            greatest = Math.Max(greatest, v.Value);
                    }
                    return greatest;
                }
                return null;
            });

            formulaSheet.Grid[15, 0] = "Custom function";
            formulaSheet.Grid[15, 1] = "GREATEST =>";
            formulaSheet.Grid[15, 2] = "=GREATEST(B1:B4)";

            formulaSheet.Grid.Columns[0].Width = new GridLength(100);

            #endregion

            // set up event handler to update the selection status
            _flex.SelectionChanged += (s, e) =>
            {
                _selectionBeingChanged = true;
                var sel = _flex.Selection;
                if (sel.Row >= _flex.Rows.Count || sel.Column >= _flex.Columns.Count)
                {
                    if (sel.Row >= _flex.Rows.Count)
                    {
                        _flex.Selection = new CellRange(-1, sel.Column, -1, sel.Column2);
                    }
                    else
                    {
                        _flex.Selection = new CellRange(sel.Row, -1, sel.Row2, -1);
                    }
                    
                    sel = _flex.Selection;
                }

                // update address label
                var text = string.Empty;
                if (sel.IsValid)
                {
                    text = _flex.GetAddress(sel, false);

                    //update angle combobox
                    var row = _flex.Rows[sel.Row] as ExcelRow;
                    if (row != null)
                    {
                        double angle = row.GetCellAngle(_flex.Columns[sel.Column]);
                        int selectedIndex = 0;
                        if (angle == 90)
                            selectedIndex = 0;
                        else if (angle == 45)
                            selectedIndex = 1;
                        else if (angle == 0)
                            selectedIndex = 2;
                        else if (angle == -45)
                            selectedIndex = 3;
                        else if (angle == -90)
                            selectedIndex = 4;
                        else
                            selectedIndex = 2;
                        _cmbRotate.SelectedIndex = selectedIndex;
                    }
                }

                // update status bar
                text = "Ready";
                if (sel.IsValid && !sel.IsSingleCell &&
                    sel.BottomRow < _flex.Rows.Count && sel.RightColumn < _flex.Columns.Count)
                {
                    try
                    {
                        var address = _flex.GetAddress(sel, true);
                        var avg = _flex.Evaluate(string.Format("Average({0})", address));
                        var cnt = _flex.Evaluate(string.Format("Count({0})", address));
                        var sum = _flex.Evaluate(string.Format("Sum({0})", address));
                        if ((double)cnt > 0)
                        {
                            text = string.Format("Average: {0:#,##0.##} Count: {1:n0} Sum: {2:#,##0.##}",
                                avg, cnt, sum);
                        }
                    }
                    catch
                    {
                        // circular reference/bad expression?
                    }
                }
                _tbStatus.Text = text;
                _selectionBeingChanged = false;
            };

            // give grid the focus when the app loads
            Loaded += (s, e) =>
            {
                _flex.Focus();
            };

            // close the find and replace window when app is closed.
            Unloaded += (s, e) =>
            {
                if (_findReplaceWindow != null)
                    _findReplaceWindow.Close();
            };

            // create custom menu items
            CustomizeContextMenu();

            _flex.Rows.CollectionChanged += (s, e) => AddTotalRow(false);
            _flex.Columns.CollectionChanged += (s, e) => AddTotalRow(false);
        }

        #endregion

        //-----------------------------------------------------------------------------
        #region ** file

        void _btnOpen_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Excel Workbook (*.xlsx)|*.xlsx|" +
                "Excel 97-2003 Workbook (*.xls)|*.xls";
            if (dlg.ShowDialog().Value)
            {
                try
                {
                    using (var s = dlg.OpenFile())
                    {
                        var ext = System.IO.Path.GetExtension(dlg.SafeFileName).ToLower();
                        switch (ext)
                        {
                            case ".xlsx":
                                _flex.Load(s, ImportFileFormat.XLSX);
                                break;
                            case ".xls":
                                _flex.Load(s, ImportFileFormat.XLS);
                                break;
                        }
                    }
                }
                catch (Exception x)
                {
                    var msg = "Error opening file: \r\n\r\n" + x.Message;
                    MessageBox.Show(msg, "Error", MessageBoxButton.OK);
                }
            }
        }
        void _btnSave_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = "xlsx";
            dlg.Filter =
                "Excel Workbook (*.xlsx)|*.xlsx|" +
                "Excel 97-2003 Workbook (*.xls)|*.xls|" +
                "HTML File (*.htm;*.html)|*.htm;*.html|" +
                "Comma Separated Values (*.csv)|*.csv|" +
                "Text File (*.txt)|*.txt|" +
                "PDF (*.pdf)|*.pdf";
            if (dlg.ShowDialog().Value)
            {
                using (var s = dlg.OpenFile())
                {
                    // dlg.FilterIndex is not working properly (SL bug) so we check
                    // the file extension to chose the proper save format.
                    var ext = System.IO.Path.GetExtension(dlg.SafeFileName).ToLower();
                    switch (ext)
                    {
                        case ".htm":
                        case ".html":
                            _flex.Save(s, FileFormat.Html, SaveOptions.Formatted);
                            break;
                        case ".csv":
                            _flex.Save(s, FileFormat.Csv, SaveOptions.Formatted);
                            break;
                        case ".txt":
                            _flex.Save(s, FileFormat.Text, SaveOptions.Formatted);
                            break;
                        case ".pdf":
                            SavePdf(s, "ComponentOne ExcelBook");
                            break;
                        case ".xlsx":
                            _flex.SaveXlsx(s);
                            break;
                        default:
                            _flex.SaveXls(s);
                            break;
                    }
                }
            }
        }

        #endregion

        //-----------------------------------------------------------------------------
        #region ** clipboard

        void _btnPaste_Click(object sender, RoutedEventArgs e)
        {
            _flex.Paste();
        }
        void _btnCut_Click(object sender, RoutedEventArgs e)
        {
            if (!_flex.Selection.IsValid)
            {
                return;
            }

            // create undoable cell range action and record the cell values
            // of the current selection before changing them
            var action = new CellRangeEditAction(_flex);

            _flex.Copy();
            foreach (var cell in _flex.Selection.Cells)
            {
                try
                {
                    _flex[cell.Row, cell.Column] = null;
                }
                catch { }
            }

            // record the cell values after the changes and add the
            // undoable action to the undo stack
            if (action.SaveNewState())
            {
                _flex.UndoStack.AddAction(action);
            }
        }
        void _btnCopy_Click(object sender, RoutedEventArgs e)
        {
            _flex.Copy();
        }
        CellRange GetClipboardRange()
        {
            // get current location
            int row = _flex.Selection.TopRow;
            int col = _flex.Selection.LeftColumn;
            CellRange range = new CellRange(row, col);

            // split text into lines at line breaks
            string text = System.Windows.Clipboard.GetText().Replace("\r\n", "\r").Trim();
            var lines = text.Split('\r', '\n');
            range.Row2 = lines.Length + row - 1;
            if (lines.Length > 0)
            {
                // split line to get column number
                var line = lines[0];
                var cells = line.Split('\t');
                range.Column2 = cells.Length + col - 1;
            }
            return range;
        }

        #endregion

        //-----------------------------------------------------------------------------
        #region ** custom context menu

        void CustomizeContextMenu()
        {
            // create custom items
            _flex.ContextMenu.Items.Add(new C1Separator());

            // populate
            AddMenuItem("MAKE UPPER CASE", CustomContextCommands.UpperCase);
            AddMenuItem("make lower case", CustomContextCommands.LowerCase);
            AddMenuItem("Make Odd", CustomContextCommands.MakeOdd);
            AddMenuItem("Make Even", CustomContextCommands.MakeEven);
            AddMenuItem("Make Zero", CustomContextCommands.MakeZero);
            AddMenuItem("Make True", CustomContextCommands.True);
            AddMenuItem("Make False", CustomContextCommands.False);
            AddMenuItem("Toggle Value", CustomContextCommands.Toggle);

            // select items to display when showing the menu
            _flex.ContextMenu.Opened += ContextMenu_Opened;

            // add a menu with sub-items to show it's possible
            var newItem = new MenuItem();
            newItem.Header = "Country";
            foreach (string country in new string[] { "Austria", "Belgium", "Croatia", "Denmark", "Others" })
            {
                var subItem = new MenuItem();
                subItem.Header = country;
                subItem.Click += subItem_Click;
                newItem.Items.Add(subItem);
            }

            // add new item to context menu 
            _flex.ContextMenu.Items.Insert(0, newItem);
            _flex.ContextMenu.Items.Insert(1, new C1Separator());

            //format cell dialoog
            var item = new MenuItem();
            item.Header = "Format Cells...";
            item.Click += formatCell_Click;
            _flex.ContextMenu.Items.Insert(0, item);
        }

        void formatCell_Click(object sender, RoutedEventArgs e)
        {
            var sel = _flex.Selection;
            if (!sel.IsValid)
                return;
            var formatDialog = new FormatCellDialog(_flex, sel.Cells);
            formatDialog.ShowDialog();
        }

        // sub-menu clicked:
        void subItem_Click(object sender, EventArgs e)
        {
            var mi = sender as MenuItem;
            var msg = string.Format("Thank you for selecting '{0}'.", mi.Header as string);
            MessageBox.Show(msg);
        }

        // select items to display when showing the menu
        void ContextMenu_Opened(object sender, EventArgs e)
        {
            // get current selection
            var sel = _flex.Selection;
            bool isInvalidSelection = !sel.IsValid || !sel.IsSingleCell ||
                sel.Row > _flex.Rows.Count || sel.Column > _flex.Columns.Count || _flex.Rows.Count == 0 || _flex.Columns.Count == 0;

            Type type = null;
            if (!isInvalidSelection)
            {
                type = _flex[sel.Row, sel.Column] != null ? _flex[sel.Row, sel.Column].GetType() : null;
            }

            // show/hide commands depending on selection type
            foreach (var item in _flex.ContextMenu.Items)
            {
                MenuItem mi = item as MenuItem;
                if (mi != null && mi.Tag is CustomContextCommands)
                {
                    if (isInvalidSelection)
                    {
                        mi.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        // hide options that don't apply
                        switch ((CustomContextCommands)mi.Tag)
                        {
                            case CustomContextCommands.UpperCase:
                            case CustomContextCommands.LowerCase:
                                mi.Visibility = type == typeof(string)
                                    ? Visibility.Visible
                                    : Visibility.Collapsed;
                                break;
                            case CustomContextCommands.MakeEven:
                            case CustomContextCommands.MakeOdd:
                            case CustomContextCommands.MakeZero:
                                mi.Visibility = IsNumeric(type)
                                    ? Visibility.Visible
                                    : Visibility.Collapsed;
                                break;
                            case CustomContextCommands.True:
                            case CustomContextCommands.False:
                            case CustomContextCommands.Toggle:
                                mi.Visibility = type == typeof(bool) || type == typeof(bool?)
                                    ? Visibility.Visible
                                    : Visibility.Collapsed;
                                break;
                        }
                    }
                }
            }
        }
        void AddMenuItem(string header, CustomContextCommands cmd)
        {
            var mi = new MenuItem();
            mi.Header = header;
            mi.Tag = cmd;
            mi.Click += mi_Click;
            _flex.ContextMenu.Items.Add(mi);
        }

        // execute context menu command
        void mi_Click(object sender, EventArgs e)
        {
            var mi = sender as MenuItem;
            if (mi != null && mi.Tag is CustomContextCommands)
            {
                // get current value in the cell
                var sel = _flex.Selection;
                if (sel.IsValid && sel.IsSingleCell && sel.Row2 < _flex.Rows.Count && sel.Column2 < _flex.Columns.Count)
                {
                    object value = _flex[sel.Row, sel.Column];

                    // perform command
                    switch ((CustomContextCommands)mi.Tag)
                    {
                        case CustomContextCommands.UpperCase:
                            if (value != null)
                            {
                                value = value.ToString().ToUpper();
                            }
                            break;
                        case CustomContextCommands.LowerCase:
                            if (value != null)
                            {
                                value = value.ToString().ToLower();
                            }
                            break;
                        case CustomContextCommands.MakeEven:
                            try
                            {
                                int i = (int)Convert.ChangeType(value, typeof(int), CultureInfo.CurrentCulture);
                                value = i % 2 == 0 ? i : i + 1;
                            }
                            catch { }
                            break;
                        case CustomContextCommands.MakeOdd:
                            try
                            {
                                int i = (int)Convert.ChangeType(value, typeof(int), CultureInfo.CurrentCulture);
                                value = i % 2 != 0 ? i : i + 1;
                            }
                            catch { }
                            break;
                        case CustomContextCommands.MakeZero:
                            value = 0;
                            break;
                        case CustomContextCommands.True:
                            value = true;
                            break;
                        case CustomContextCommands.False:
                            value = false;
                            break;
                        case CustomContextCommands.Toggle:
                            try
                            {
                                bool b = (bool)Convert.ChangeType(value, typeof(bool), CultureInfo.CurrentCulture);
                                value = !b;
                            }
                            catch { }
                            break;
                    }

                    // apply new value
                    _flex[sel.Row, sel.Column] = value;
                }
            }
        }
        static bool IsNumeric(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
            }
            return false;
        }

        #endregion

        //-----------------------------------------------------------------------------
        #region ** print / pdf export

        // print the grid
        void _btnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            var scaleMode = ScaleMode.PageWidth;
            if (sender.Equals(_btnPrintPageWidth))
            {
                scaleMode = ScaleMode.PageWidth;
            }
            else if (sender.Equals(_btnPrintSinglePage))
            {
                scaleMode = ScaleMode.SinglePage;
            }
            else if (sender.Equals(_btnPrintSelection))
            {
                scaleMode = ScaleMode.Selection;
            }
            
            _flex.PrintPreview(new PrintParameters()
            {
                DocumentName = "C1FlexSheet",
                ScaleMode = scaleMode,
                Margin = new Thickness(96),
                MaxPages = int.MaxValue,
                PrintTicket = new PrintTicket()
                {
                    PageOrientation = PageOrientation.Portrait
                }
            });
        }

        // export the grid to a PDF file
        void SavePdf(Stream s, string documentName)
        {
            PdfExportOptions options = new PdfExportOptions();
            options.Margin = new Thickness(96, 96, 96 / 2, 96 / 2);
            options.ScaleMode = ScaleMode.ActualSize;
            _flex.SavePdf(s, options);
            s.Close();
        }

        #endregion

        //-----------------------------------------------------------------------------
        #region ** view


        // freeze/unfreeze panes
        void _btnFreeze_Click(object sender, RoutedEventArgs e)
        {
            if (_flex.Rows.Frozen > 0 || _flex.Columns.Frozen > 0)
            {
                // unfreeze
                for (var i = 0; i < _flex.Rows.Frozen; i++)
                {
                    _flex.Rows[i].Visible = true;
                }
                for (var i = 0; i < _flex.Columns.Frozen; i++)
                {
                    _flex.Columns[i].Visible = true;
                }
                _flex.Rows.Frozen = 0;
                _flex.Columns.Frozen = 0;
            }
            else
            {
                // freeze
                var vr = _flex.ViewRange;
                for (var i = 0; i < vr.TopRow; i++)
                {
                    _flex.Rows[i].Visible = false;
                }
                for (var i = 0; i < vr.LeftColumn; i++)
                {
                    _flex.Columns[i].Visible = false;
                }
                _flex.Rows.Frozen = _flex.Selection.TopRow;
                _flex.Columns.Frozen = _flex.Selection.LeftColumn;
                _flex.ScrollIntoView(_flex.Rows.Frozen, _flex.Columns.Frozen);
            }
        }

        // toggle gridline visibility
        void _chkGridlines_Click(object sender, RoutedEventArgs e)
        {
            _flex.GridLinesVisibility = _chkGridlines.IsChecked.Value
                ? GridLinesVisibility.All
                : GridLinesVisibility.None;
        }

        // toggle heading visibility
        void _chkHeadings_Click(object sender, RoutedEventArgs e)
        {
            _flex.HeadersVisibility = _chkHeadings.IsChecked.Value
                ? HeadersVisibility.All
                : HeadersVisibility.None;
        }

        #endregion

        //-----------------------------------------------------------------------------
        #region ** editing

        // show sort dialog
        void _btnSort_Click(object sender, RoutedEventArgs e)
        {
            if (_flex.Columns.Count > 0)
            {
                var sortDialog = new SortDialog(_flex);
                sortDialog.Show();
            }
        }

        // show enhanced filter editor
        void _btnFilter_Click(object sender, RoutedEventArgs e)
        {
            if (_flex.Columns.Count > 0)
            {
                _flex.ShowFilterEditor();
            }
        }

        // show find and replace dialog
        void _btnFind_Click(object sender, RoutedEventArgs e)
        {
            if (_flex.Columns.Count > 0)
            {
                if (_findReplaceWindow == null)
                {
                    _findReplaceWindow = new FindReplaceWindow(_flex);
                    _findReplaceWindow.Show();
                    _findReplaceWindow.Closed += (s, e1) => { _findReplaceWindow = null; };
                }
                else
                {
                    _findReplaceWindow.Focus();
                }
            }
        }

        #endregion

        //-----------------------------------------------------------------------------
        #region ** merging

        void _btnMerge_Click(object sender, RoutedEventArgs e)
        {
            // get current selection, ensure there's more than one cell in it
            var sel = _flex.Selection;
            if (!sel.IsValid)
                return;
            var xmm = _flex.MergeManager as ExcelMergeManager;
            if (xmm != null)
            {
                // check if the selection contains any merged ranges
                var hasMerges = false;
                foreach (var cell in sel.Cells)
                {
                    if (!xmm.GetMergedRange(_flex, CellType.Cell, cell).IsSingleCell)
                    {
                        hasMerges = true;
                    }
                }

                // toggle merging for the selection
                if (hasMerges)
                {
                    // clear merged ranges
                    xmm.RemoveRange(sel);
                }
                else
                {
                    // merge selection
                    xmm.AddRange(sel);
                }

                // show changes
                _flex.Invalidate();
            }
        }

        #endregion

        //-----------------------------------------------------------------------------
        #region ** grouping (TODO)

        void _btnGroup_Click(object sender, RoutedEventArgs e)
        {
            _flex.GroupRows();
        }
        void _btnUngroup_Click(object sender, RoutedEventArgs e)
        {
            _flex.UnGroupRows();
        }
        void _chkTotalsBelow_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        //-----------------------------------------------------------------------------
        #region ** formatting

        void _btnBold_Click(object sender, RoutedEventArgs e)
        {
            FontWeight? fw = null;
            var cs = GetFirstSelectedStyle();
            if (cs == null || cs.FontWeight == null)
            {
                fw = FontWeights.Bold;
            }
            _flex.SetCellFormat(_flex.Selection.Cells, CellFormat.FontWeight, fw);
        }
        void _btnItalic_Click(object sender, RoutedEventArgs e)
        {
            FontStyle? fs = null;
            var cs = GetFirstSelectedStyle();
            if (cs == null || cs.FontStyle == null || cs.FontStyle == FontStyles.Normal)
            {
                fs = FontStyles.Italic;
            }
            _flex.SetCellFormat(_flex.Selection.Cells, CellFormat.FontStyle, fs);
        }
        void _btnUnderline_Click(object sender, RoutedEventArgs e)
        {
            TextDecorationCollection td = null;
            var cs = GetFirstSelectedStyle();
            if (cs == null || cs.TextDecorations == null)
            {
                td = TextDecorations.Underline;
            }
            _flex.SetCellFormat(_flex.Selection.Cells, CellFormat.TextDecorations, td);
        }
        void _btnLeft_Click(object sender, RoutedEventArgs e)
        {
            _flex.SetCellFormat(_flex.Selection.Cells, CellFormat.HorizontalAlignment, HorizontalAlignment.Left);
        }
        void _btnCenter_Click(object sender, RoutedEventArgs e)
        {
            _flex.SetCellFormat(_flex.Selection.Cells, CellFormat.HorizontalAlignment, HorizontalAlignment.Center);
        }
        void _btnRight_Click(object sender, RoutedEventArgs e)
        {
            _flex.SetCellFormat(_flex.Selection.Cells, CellFormat.HorizontalAlignment, HorizontalAlignment.Right);
        }
        void _btnDecreaseIndent_Click(object sender, RoutedEventArgs e)
        {
            if (_flex.Selection.IsValid)
            {
                _currentIndent -= 12;
                if (_currentIndent >= 0)
                    _flex.SetCellIndent(_flex.Selection.Cells, _currentIndent);
                else
                    _currentIndent = 0;
            }
        }
        void _btnIncreaseIndent_Click(object sender, RoutedEventArgs e)
        {
            if (_flex.Selection.IsValid)
            {
                _currentIndent += 12;
                _flex.SetCellIndent(_flex.Selection.Cells, _currentIndent);
            }
        }
        void _btnTop_Click(object sender, RoutedEventArgs e)
        {
            _flex.SetCellFormat(_flex.Selection.Cells, CellFormat.VerticalAlignment, VerticalAlignment.Top);
        }
        void _btnMiddle_Click(object sender, RoutedEventArgs e)
        {
            _flex.SetCellFormat(_flex.Selection.Cells, CellFormat.VerticalAlignment, VerticalAlignment.Center);
        }
        void _btnBottom_Click(object sender, RoutedEventArgs e)
        {
            _flex.SetCellFormat(_flex.Selection.Cells, CellFormat.VerticalAlignment, VerticalAlignment.Bottom);
        }
        void _btnBlue_Click(object sender, RoutedEventArgs e)
        {
            _flex.SetCellFormat(_flex.Selection.Cells, CellFormat.Background, new SolidColorBrush(Colors.Blue));
        }
        void _btnYellow_Click(object sender, RoutedEventArgs e)
        {
            _flex.SetCellFormat(_flex.Selection.Cells, CellFormat.Background, new SolidColorBrush(Colors.Yellow));
        }
        void _btnOrange_Click(object sender, RoutedEventArgs e)
        {
            _flex.SetCellFormat(_flex.Selection.Cells, CellFormat.Background, new SolidColorBrush(Colors.Orange));
        }
        void _btnRed_Click(object sender, RoutedEventArgs e)
        {
            _flex.SetCellFormat(_flex.Selection.Cells, CellFormat.Background, new SolidColorBrush(Colors.Red));
        }
        void _btnGreen_Click(object sender, RoutedEventArgs e)
        {
            _flex.SetCellFormat(_flex.Selection.Cells, CellFormat.Background, new SolidColorBrush(Colors.Green));
        }
        void _btnNoColor_Click(object sender, RoutedEventArgs e)
        {
            _flex.SetCellFormat(_flex.Selection.Cells, CellFormat.Background, null);
        }
        CellStyle GetFirstSelectedStyle()
        {
            var sel = _flex.Selection;
            if (sel.IsValid)
            {
                var row = _flex.Rows[sel.Row] as ExcelRow;
                var col = _flex.Columns[sel.Column];
                return row.GetCellStyle(col);
            }
            return null;
        }

        #endregion

        #region insert/remove

        private void _cmbRotate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selectionBeingChanged)
                return;

            var index = ((ComboBox)sender).SelectedIndex;
            double angle = 0;
            switch (index)
            {
                case 0:
                    angle = 90;
                    break;
                case 1:
                    angle = 45;
                    break;
                case 2:
                    angle = 0;
                    break;
                case 3:
                    angle = -45;
                    break;
                case 4:
                    angle = -90;
                    break;
                default:
                    angle = 0;
                    break;
            }
            if (_flex != null && _flex.Selection.IsValid)
                _flex.SetCellAngle(_flex.Selection.Cells, angle, 8);
        }

        private void _btnRemoveDul_Click(object sender, RoutedEventArgs e)
        {
            if (Math.Abs(_flex.Selection.RightColumn - _flex.Selection.LeftColumn) >= 0
                && Math.Abs(_flex.Selection.BottomRow - _flex.Selection.TopRow) >= 0
                && _flex.Selection.IsValid)
            {
                List<int> selectedColumns = new List<int>();
                for (int i = _flex.Selection.LeftColumn; i <= _flex.Selection.RightColumn; i++)
                {
                    selectedColumns.Add(i);
                }
                _flex.RemoveDuplicates(_flex.Selection, selectedColumns);
            }
        }

        private void _btnInsertPicture_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*";
            if (dlg.ShowDialog().Value)
            {
                try
                {
                    BitmapImage b = new BitmapImage();
                    b.BeginInit();
                    b.UriSource = new Uri(dlg.FileName);
                    b.EndInit();
                    _flex.InsertImage(b);
                }
                catch (Exception x)
                {
                    var msg = "Error opening file: \r\n\r\n" + x.Message;
                    MessageBox.Show(msg, "Error", MessageBoxButton.OK);
                }
            }
        }

        private void _btnInsertChart_Click(object sender, RoutedEventArgs e)
        {
            if (Math.Abs(_flex.Selection.RightColumn - _flex.Selection.LeftColumn) > 0
               && Math.Abs(_flex.Selection.BottomRow - _flex.Selection.TopRow) > 0
               && _flex.Selection.IsValid)
            {
                C1Chart c1Chart1 = new C1Chart();
                c1Chart1.Data.Children.Clear();
                for (int row = _flex.Selection.TopRow; row <= _flex.Selection.BottomRow; row++)
                {
                    List<double> datas = new List<double>();
                    for (int col = _flex.Selection.LeftColumn; col <= _flex.Selection.RightColumn; col++)
                    {
                        object value = _flex[row, col];
                        if (value != null && value.GetType().IsNumeric())
                        {
                            double cellValue = (double)value;
                            datas.Add(cellValue);
                        }
                    }
                    // create single series for product price
                    DataSeries ds = new DataSeries();
                    //set data
                    ds.ValuesSource = datas;
                    // add series to the chart
                    c1Chart1.Data.Children.Add(ds);
                }

                // add item names
                //c1Chart1.Data.ItemNames = ProductNames;
                // Set chart type
                c1Chart1.ChartType = ChartType.Bar;
                _flex.InsertChart(c1Chart1);
            }
            else
            {
                MessageBox.Show("Please select more data");
            }
        }

        private void _btnInsertComment_Click(object sender, RoutedEventArgs e)
        {
            if (_flex.Selection.IsValid && _flex.Selection.IsSingleCell)
            {
                _flex.InsertComment(_flex.Selection);
            }
        }

        private void _btnGetComment_Click(object sender, RoutedEventArgs e)
        {
            if (_flex.Selection.IsValid && _flex.Selection.IsSingleCell)
            {
                var c = _flex.GetComment(_flex.Selection.Row, _flex.Selection.Column);
                if (c != null)
                {
                    MessageBox.Show(c.Text);
                }
            }
        }

        private void _btnRemoveComment_Click(object sender, RoutedEventArgs e)
        {
            _flex.RemoveComment(_flex.Selection);
        }

        private void _btnInsertSL_Click(object sender, RoutedEventArgs e)
        {
            if (_insertSparkLineWindow == null)
            {
                _insertSparkLineWindow = new SelectedRangeWindow(_flex);
                _insertSparkLineWindow.Show();
                _insertSparkLineWindow.Closed += (s, e1) => { _insertSparkLineWindow = null; };
            }
            else
            {
                _insertSparkLineWindow.Focus();
            }
        }

        #endregion

        #region Undo/Redo

        private void _btnUndo_Click(object sender, RoutedEventArgs e)
        {
            _flex.Undo();
        }

        private void _btnRedo_Click(object sender, RoutedEventArgs e)
        {
            _flex.Redo();
        }


        #endregion

        private void _btnPrint_Click(object sender, RoutedEventArgs e)
        {
            _flex.Print(new PrintParameters()
            {
                DocumentName = "C1FlexSheet",
                ScaleMode = ScaleMode.PageWidth,
                ShowPrintDialog = true,
                //ShowPrintPreview = true
            });

        }
        
        private void AddTotalRow(bool init = true)
        {
            if (_flex.Rows.Count == 0)
                return;

            var totRow = _flex.Rows.Count - 1;

            if (init)
            {
                for (int c = 0; c < _flex.Columns.Count; c++)
                {
                    _flex[totRow, c] = string.Format("=sum({0}1:{0}{1})", (char)('A' + c), _flex.Rows.Count - 2);
                }

                _flex.Rows[totRow].FontWeight = FontWeights.Bold;
                _flex.Rows[totRow].Tag = "TotalRow";
            }
            else
            {
                var row = _flex.Rows[totRow];
                if(row.Tag + "" == "TotalRow")
                {
                    for (int c = 0; c < _flex.Columns.Count; c++)
                    {
                        var cellValue = _flex[totRow, c] + "";
                        //if (!cellValue.Contains("** ERR"))
                        {
                            _flex[totRow, c] = string.Format("=sum({0}1:{0}{1})", (char)('A' + c), _flex.Rows.Count - 2);
                        }
                    }
                }
            }
        }
    }

    public class ProtectedConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isProtected = (bool)value;
            bool isEnable = isProtected ? false : true;
            return isEnable;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
