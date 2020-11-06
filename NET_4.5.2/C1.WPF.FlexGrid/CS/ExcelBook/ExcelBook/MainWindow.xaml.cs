using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
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
using C1.WPF;
using C1.WPF.FlexGrid;
using C1.WPF.FlexGridBook;
using C1.WPF.Pdf;

namespace ExcelBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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

        #endregion

        //-----------------------------------------------------------------------------
        #region ** ctor

        public MainWindow()
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
            _tbFx.MouseLeftButtonDown += (s, e) =>
                {
                    fnMenu.IsOpen = true;
                    //fnMenu.Show(_tbFx, new Point(_tbFx.RenderSize.Width - 4, _tbFx.RenderSize.Height - 4));
                };

            // add some sheets
            _flex.AddSheet("Sheet1", 50, 10);
            _flex.AddSheet("Sheet2", 50, 10);
            _flex.AddSheet("Sheet3", 50, 10);
            _flex.Sheets.SelectedIndex = 0;

            // populate the grid with some formulas (multiplication table)
            for (int r = 0; r < _flex.Rows.Count - 2; r++)
            {
                for (int c = 0; c < _flex.Columns.Count; c++)
                {
                    _flex[r, c] = string.Format("={0}*{1}", r + 1, c + 1);
                }
            }

            // add a totals row to illustrate
            var totRow = _flex.Rows.Count - 1;
            for (int c = 0; c < _flex.Columns.Count; c++)
            {
                _flex[totRow, c] = string.Format("=sum({0}1:{0}{1})",
                    (char)('A' + c), _flex.Rows.Count - 3);
            }
            _flex.Rows[totRow].FontWeight = FontWeights.Bold;

            // set up event handler to update the selection status
            _flex.SelectionChanged += (s, e) =>
            {
                var sel = _flex.Selection;

                // update address label
                var text = string.Empty;
                if (sel.IsValid)
                {
                    text = _flex.GetAddress(sel, false);
                }
                _tbAddress.Text = text;

                // update formula textbox
                text = string.Empty;
                if (sel.IsValid)
                {
                    var row = _flex.Rows[sel.Row] as ExcelRow;
                    if (row != null)
                    {
                        var col = _flex.Columns[sel.Column];
                        text = row.GetDataEditor(col);
                    }
                }
                _txtFormula.Text = text;
                _txtFormula.Tag = text;

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
            };

            // update grid when user types into formula bar
            _txtFormula.LostFocus += (s, e) =>
                {
                    var sel = _flex.Selection;
                    if (sel.IsValid)
                    {
                        _txtFormula.Tag = _txtFormula.Text;
                        _flex[sel.Row, sel.Column] = _txtFormula.Text;
                    }
                };
            _txtFormula.KeyDown += (s, e) =>
                {
                    switch (e.Key)
                    {
                        // apply formula when user hits enter
                        case Key.Enter:
                            _flex.Focus();
                            break;

                        // restore original value when user hits escape
                        case Key.Escape:
                            _txtFormula.Text = _txtFormula.Tag as string;
                            _txtFormula.SelectAll();
                            break;
                    }
                };

            // give grid the focus when the app loads
            Loaded += (s, e) =>
            {
                _flex.Focus();
            };

#if false
            // demonstrate Excel notation
            var val = _flex["B4"];
            _flex["B4"] = "Orange";
            System.Diagnostics.Debug.WriteLine("cell 'b4' was '{0}' and is now '{1}'", val, _flex["b4"]);
#endif
            // create custom menu items
            CustomizeContextMenu();

            // start with blue color scheme
            SetColorScheme(ColorScheme.Blue);
        }

        #endregion

        //-----------------------------------------------------------------------------
        #region ** file

        void _btnOpen_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter =  "Excel Workbook (*.xlsx)|*.xlsx|"
              + "Excel 97-2003 Workbook (*.xls)|*.xls|"
              + "Comma Separated Values (*.csv)|*.csv|" +
                "Text File (*.txt)|*.txt";
            if (dlg.ShowDialog().Value)
            {
                try
                {
                    using (var s = dlg.OpenFile())
                    {
                        var ext = System.IO.Path.GetExtension(dlg.SafeFileName).ToLower();
                      
                        switch (ext)
                        {
                            case ".csv":
                                _flex.LoadCsv(s, dlg.SafeFileName.Split('.')[0],",");
                                break;
                            case ".txt":
                                //Separator must be '\t'
                                _flex.LoadTxt(s, dlg.SafeFileName.Split('.')[0]);
                                break;
                            case ".xlsx":
                                _flex.LoadExcel(s, C1.WPF.Excel.FileFormat.OpenXml);
                                break;
                            case ".xls":
                                _flex.LoadExcel(s, C1.WPF.Excel.FileFormat.Biff8);
                                break;
                            default:

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
                            SavePdf(s);
                            break;
                        case ".xlsx":
                        default:
                            _flex.SaveXlsx(s);
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
        #region ** undo/redo

        void _btnUndo_Click(object sender, RoutedEventArgs e)
        {
            _flex.Undo();
        }
        void _btnRedo_Click(object sender, RoutedEventArgs e)
        {
            _flex.Redo();
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
            Type type = sel.IsValid && _flex[sel.Row, sel.Column] != null
                ? _flex[sel.Row, sel.Column].GetType()
                : null;

            // show/hide commands depending on selection type
            foreach (var item in _flex.ContextMenu.Items)
            {
                MenuItem mi = item as MenuItem;
                if (mi != null && mi.Tag is CustomContextCommands)
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
        void _btnPrint_Click(object sender, RoutedEventArgs e)
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
            _flex.Print("C1FlexGrid", scaleMode, new Thickness(96), 20);
        }

        // export the grid to a PDF file
        void SavePdf(Stream s)
        {
            // get root element to lay out the PDF pages
            Panel root = null;
            for (var parent = _flex.Parent as FrameworkElement; parent != null; parent = parent.Parent as FrameworkElement)
            {
                if (parent is Panel)
                {
                    root = parent as Panel;
                }
            }

            // create pdf document
            var pdf = new C1PdfDocument(PaperKind.Letter, false);

            // get page size
            var rc = pdf.PageRectangle;
            var m = new Thickness(96, 96, 96 / 2, 96 / 2);
            var scaleMode = ScaleMode.ActualSize;

            // create panel to hold elements while they render
            var pageTemplate = new PageTemplate();
            pageTemplate.Width = rc.Width;
            pageTemplate.Height = rc.Height;
            pageTemplate.SetPageMargin(m);
            root.Children.Add(pageTemplate);

            // render grid into PDF document
            var sz = new Size(rc.Width - m.Left - m.Right, rc.Height - m.Top - m.Bottom);
            var pages = _flex.GetPageImages(scaleMode, sz, 100);
            for (int i = 0; i < pages.Count; i++)
            {
                // skip a page when necessary
                if (i > 0)
                {
                    pdf.NewPage();
                }

                // set content
                pageTemplate.PageContent.Child = pages[i];
                pageTemplate.PageContent.Stretch = System.Windows.Media.Stretch.Uniform;

                // set footer text
                pageTemplate.FooterRight.Text = string.Format("Page {0} of {1}",
                    i + 1, pages.Count);

                // measure page element
                pageTemplate.Measure(new Size(rc.Width, rc.Height));
                pageTemplate.UpdateLayout();

                // add page element to PDF
                pdf.DrawElement(pageTemplate, rc);
            }

            // done with template
            root.Children.Remove(pageTemplate);

            // save the PDF document
            pdf.Save(s);
            s.Close();

        }

        #endregion

        //-----------------------------------------------------------------------------
        #region ** view

        // change color scheme (blue, silver, black)
        void ComboBox_ColorSchemeChanged(object sender, SelectionChangedEventArgs e)
        {
            var cs = (ColorScheme)((ComboBox)sender).SelectedIndex;
            SetColorScheme(cs);
        }
        void SetColorScheme(ColorScheme cs)
        {
            if (_flex != null)
            {
                _flex.ColorScheme = cs;
                _flex.Select(0, 0);
                _flex.Focus();
                _ribbon.Background = _flex.ColumnHeaderBackground;
                _tbFx.Background = _flex.RowHeaderBackground;
                this.Background = _flex.RowHeaderBackground;
            }
        }

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

        #endregion

        //-----------------------------------------------------------------------------
        #region ** merging

        void _btnMerge_Click(object sender, RoutedEventArgs e)
        {
            // get current selection, ensure there's more than one cell in it
            var sel = _flex.Selection;
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

        }
        void _btnUngroup_Click(object sender, RoutedEventArgs e)
        {

        }
        void _btnShowDetail_Click(object sender, RoutedEventArgs e)
        {

        }
        void _btnHideDetail_Click(object sender, RoutedEventArgs e)
        {

        }
        void _chkTotalsBelow_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        //-----------------------------------------------------------------------------
        #region ** formatting (TODO)

        void _btnBold_Click(object sender, RoutedEventArgs e)
        {
            FontWeight? fw = null;
            var cs = GetFirstSelectedStyle();
            if (cs == null || cs.FontWeight == null)
            {
                fw = FontWeights.Bold;
            }
            SetSelectionStyle("FontWeight", fw);
        }
        void _btnItalic_Click(object sender, RoutedEventArgs e)
        {
            FontStyle? fs = null;
            var cs = GetFirstSelectedStyle();
            if (cs == null || cs.FontStyle == null || cs.FontStyle == FontStyles.Normal)
            {
                fs = FontStyles.Italic;
            }
            SetSelectionStyle("FontStyle", fs);
        }
        void _btnUnderline_Click(object sender, RoutedEventArgs e)
        {
            TextDecorationCollection td = null;
            var cs = GetFirstSelectedStyle();
            if (cs == null || cs.TextDecorations == null)
            {
                td = TextDecorations.Underline;
            }
            SetSelectionStyle("TextDecorations", td);
        }
        void _btnLeft_Click(object sender, RoutedEventArgs e)
        {
            SetSelectionStyle("HorizontalAlignment", HorizontalAlignment.Left);
        }
        void _btnCenter_Click(object sender, RoutedEventArgs e)
        {
            SetSelectionStyle("HorizontalAlignment", HorizontalAlignment.Center);
        }
        void _btnRight_Click(object sender, RoutedEventArgs e)
        {
            SetSelectionStyle("HorizontalAlignment", HorizontalAlignment.Right);
        }
        void _btnTop_Click(object sender, RoutedEventArgs e)
        {
            SetSelectionStyle("VerticalAlignment", VerticalAlignment.Top);
        }
        void _btnMiddle_Click(object sender, RoutedEventArgs e)
        {
            SetSelectionStyle("VerticalAlignment", VerticalAlignment.Center);
        }
        void _btnBottom_Click(object sender, RoutedEventArgs e)
        {
            SetSelectionStyle("VerticalAlignment", VerticalAlignment.Bottom);
        }
        void _btnBlue_Click(object sender, RoutedEventArgs e)
        {
            SetSelectionStyle("Background", new SolidColorBrush(Colors.Blue));
        }
        void _btnYellow_Click(object sender, RoutedEventArgs e)
        {
            SetSelectionStyle("Background", new SolidColorBrush(Colors.Yellow));
        }
        void _btnOrange_Click(object sender, RoutedEventArgs e)
        {
            SetSelectionStyle("Background", new SolidColorBrush(Colors.Orange));
        }
        void _btnRed_Click(object sender, RoutedEventArgs e)
        {
            SetSelectionStyle("Background", new SolidColorBrush(Colors.Red));
        }
        void _btnGreen_Click(object sender, RoutedEventArgs e)
        {
            SetSelectionStyle("Background", new SolidColorBrush(Colors.Green));
        }
        void _btnNoColor_Click(object sender, RoutedEventArgs e)
        {
            SetSelectionStyle("Background", null);
        }
        void SetSelectionStyle(string propName, object value)
        {
            // get the property we're about to change
            var pi = typeof(CellStyle).GetProperty(propName);

            // create undo action
            var action = new CellStyleChangeAction(_flex);

            // apply the change to the current selection
            foreach (var rng in _flex.Selection.Cells)
            {
                // get cell style
                var row = _flex.Rows[rng.Row] as ExcelRow;
                var col = _flex.Columns[rng.Column];
                var cs = row.GetCellStyle(col);

                // clone it/create a new one
                cs = cs == null
                    ? new ExcelCellStyle()
                    : (ExcelCellStyle)cs.Clone();

                // apply the new setting
                pi.SetValue(cs, value, null);
                row.SetCellStyle(col, cs);
            }

            // save undo action
            action.SaveNewState();
            _flex.UndoStack.AddAction(action);
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
    }
}
