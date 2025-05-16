using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Reflection;
using System.IO;
using C1.WPF.RichTextBox.Documents;
using C1.WPF.RichTextBox;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using docs = C1.WPF.RichTextBox.Documents;
using C1.WPF.Core;
using C1.WPF.Menu;
using MenuExplorer.Tools;

namespace MenuExplorer
{
    /// <summary>
    /// Interaction logic for CustomContextMenu.xaml
    /// </summary>
    public partial class CustomRadialContextMenu : UserControl
    {
        // keep different sets of selected colors depending on context (whether we select foreground or background)
        Dictionary<string, int> _selectedForegroundColors = new Dictionary<string, int>();
        Dictionary<string, int> _selectedBackroundColors = new Dictionary<string, int>();
        private string _asmName = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name;
        Window window;
        public CustomRadialContextMenu()
        {
            InitializeComponent();
            this.Tag = MenuExplorer.Properties.Resources.CustomRadialContextMenuDesc;
            LastLocation = new Point();

            var stream = Application.GetResourceStream(new Uri("/" + _asmName + ";component/Resources/simple.htm", UriKind.Relative)).Stream;
            var html = new StreamReader(stream).ReadToEnd();
            rtb.Html = html;
            rtb.DocumentHistory.HistoryChanged += DocumentHistory_HistoryChanged;
            rtb_SelectionChanged(rtb, null);

            // initialize default foreground indices
            _selectedForegroundColors.Add("Orange", 2);
            _selectedForegroundColors.Add("Red", 5);
            _selectedForegroundColors.Add("Green", 5);
            _selectedForegroundColors.Add("Blue", 0);
            _selectedForegroundColors.Add("Gray", 0);

            // initialize default background indices
            _selectedBackroundColors.Add("Orange", 0);
            _selectedBackroundColors.Add("Red", 6);
            _selectedBackroundColors.Add("Green", 2);
            _selectedBackroundColors.Add("Blue", 3);
            _selectedBackroundColors.Add("Gray", 7);

            fontColorItem.Tag = new SolidColorBrush(Colors.Red);
            textHighlightItem.Tag = new SolidColorBrush(Colors.Green);
        }

        private void rtb_RequestNavigate(object sender, C1.WPF.RichTextBox.RequestNavigateEventArgs e)
        {
            if (MessageBox.Show("The document is requesting to navigate to " + e.Hyperlink.NavigateUri, "Navigate", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                Window window = new Window();
                WebBrowser wb = new WebBrowser();
                wb.Source = e.Hyperlink.NavigateUri;
                window.Content = wb;
                window.Show();
            }
        }

        #region ** menu actions
        #region Font Size
        private void fontSize_Click(object sender, SourcedEventArgs e)
        {
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                rtb.Selection.TrimRuns();
                rtb.Selection = range;
                foreach (var r in range.EditRanges)
                {
                    r.FontSize = (sender as C1RadialNumericItem).Value;
                }
            }

        }

        private void GrowFont(int size)
        {
            // grow font
            rtb.Selection.TrimRuns();
            foreach (var run in rtb.Selection.Runs)
            {
                run.FontSize += size;
            }
        }

        private void ShrinkFont(int size)
        {
            // shrink font
            rtb.Selection.TrimRuns();
            foreach (var run in rtb.Selection.Runs)
            {
                run.FontSize -= size;
            }
        }
        #endregion

        #region Clipboard
        private void menuCopy_Click(object sender, SourcedEventArgs e)
        {
            rtb.ClipboardCopy();
            rtb_SelectionChanged(null, null); // update menu items
        }

        private void menuPaste_Click(object sender, SourcedEventArgs e)
        {
            if (!rtb.IsReadOnly)
            {
                _skipMenuUpdate = true; // don't update menu on RTB.SelectionChanged event after this operation
                rtb.ClipboardPaste();
            }
        }

        private void menuCut_Click(object sender, SourcedEventArgs e)
        {
            if (rtb.IsReadOnly)
                rtb.ClipboardCopy();
            else
            {
                _skipMenuUpdate = true; // don't update menu on RTB.SelectionChanged event after this operation
                rtb.ClipboardCut();
            }
        }
        #endregion

        #region Font Format
        private void menuBold_Click(object sender, SourcedEventArgs e)
        {
            var value = rtb.Selection.EditRanges.All(r => (FontWeights.Bold).Equals(r.FontWeight)) ? FontWeights.Normal : FontWeights.Bold;
            if (rtb != null)
            {
                using (new DocumentHistoryGroup(rtb.DocumentHistory))
                {
                    var range = rtb.Selection;
                    range.TrimRuns();
                    rtb.Selection = range;
                    foreach (var r in range.EditRanges)
                    {
                        r.FontWeight = value;
                    }
                    rtb.Focus();
                }
            }
        }

        private void menuItalic_Click(object sender, SourcedEventArgs e)
        {
            var value = rtb.Selection.EditRanges.All(r => (FontStyles.Italic).Equals(r.FontStyle)) ? FontStyles.Normal : FontStyles.Italic;
            if (rtb != null)
            {
                using (new DocumentHistoryGroup(rtb.DocumentHistory))
                {
                    var range = rtb.Selection;
                    range.TrimRuns();
                    rtb.Selection = range;
                    foreach (var r in range.EditRanges)
                    {
                        r.FontStyle = value;
                    }
                    rtb.Focus();
                }
            }
        }

        private void menuUnderline_Click(object sender, SourcedEventArgs e)
        {
            var isMenuUnderlineChecked = rtb.Selection.Runs.Any() &&
                rtb.Selection.Runs.All(r => r.TextDecorations != null && r.TextDecorations.Contains(C1TextDecorations.Underline[0]));
            var value = isMenuUnderlineChecked ? null : C1TextDecorations.Underline;
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                foreach (var r in range.EditRanges)
                {
                    foreach (var run in range.Runs)
                    {
                        var collection = new C1TextDecorationCollection();
                        if (value == null)
                        {
                            if(run.TextDecorations != null)
                            {
                                foreach (var decoration in run.TextDecorations)
                                    collection.Add(decoration);
                            }
                            
                            collection.Remove(C1TextDecorations.Underline[0]);
                            if (collection.Count == 0)
                                collection = null;
                        }
                        else if (run.TextDecorations == null)
                        {
                            collection.Add(value[0]);
                        }
                        else if (!run.TextDecorations.Contains(value[0]))
                        {
                            foreach (var decoration in run.TextDecorations)
                                collection.Add(decoration);
                            collection.Add(value[0]);
                        }
                        else
                        {
                            continue;
                        }
                        run.TextDecorations = null;
                        run.TextDecorations = collection;

                    }
                }
                rtb.Focus();
            }
        }

        private void menuStrikethrough_Click(object sender, SourcedEventArgs e)
        {
            var isMenuStrikeChecked = rtb.Selection.Runs.Any() &&
                rtb.Selection.Runs.All(r => r.TextDecorations != null && r.TextDecorations.Contains(C1TextDecorations.Strikethrough[0]));

            var value = isMenuStrikeChecked ? null : C1TextDecorations.Strikethrough;

            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                foreach (var r in range.EditRanges)
                {
                    foreach (var run in range.Runs)
                    {
                        var collection = new C1TextDecorationCollection();
                        if (value == null)
                        {
                            foreach (var decoration in run.TextDecorations)
                                collection.Add(decoration);

                            collection.Remove(C1TextDecorations.Strikethrough[0]);
                            if (collection.Count == 0)
                                collection = null;
                        }
                        else if (run.TextDecorations == null)
                        {
                            collection.Add(value[0]);
                        }
                        else if (!run.TextDecorations.Contains(value[0]))
                        {
                            foreach (var decoration in run.TextDecorations)
                                collection.Add(decoration);
                            collection.Add(value[0]);
                        }
                        else
                        {
                            continue;
                        }
                        run.TextDecorations = null;
                        run.TextDecorations = collection;

                    }
                }
                rtb.Focus();
            }
        }


        private void menuSuperscript_Click(object sender, SourcedEventArgs e)
        {
            // superscript
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                foreach (var ran in range.EditRanges)
                {
                    var isSub = ran.EditRanges.All(r => (C1VerticalAlignment.Sub).Equals(r.InlineAlignment));
                    var isSuper = ran.EditRanges.All(r => (C1VerticalAlignment.Super).Equals(r.InlineAlignment));
                    if (isSuper)
                    {
                        ran.InlineAlignment = C1VerticalAlignment.Baseline;
                        GrowFont(4);
                    }
                    else
                    {
                        ran.InlineAlignment = C1VerticalAlignment.Super;
                        if (!isSub)
                            ShrinkFont(4);
                    }
                }
            }
        }

        private void menuSubscript_Click(object sender, SourcedEventArgs e)
        {
            // subscript
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                foreach (var ran in range.EditRanges)
                {
                    var isSub = ran.EditRanges.All(r => (C1VerticalAlignment.Sub).Equals(r.InlineAlignment));
                    var isSuper = ran.EditRanges.All(r => (C1VerticalAlignment.Super).Equals(r.InlineAlignment));
                    if (isSub)
                    {
                        ran.InlineAlignment = C1VerticalAlignment.Baseline;
                        GrowFont(4);
                    }
                    else
                    {
                        ran.InlineAlignment = C1VerticalAlignment.Sub;
                        if (!isSuper)
                            ShrinkFont(4);
                    }
                }
            }
        }

        private void menuAlignLeft_Click(object sender, SourcedEventArgs e)
        {
            rtb.Selection.TextAlignment = C1TextAlignment.Left;
        }

        private void menuAlignCenter_Click(object sender, SourcedEventArgs e)
        {
            rtb.Selection.TextAlignment = C1TextAlignment.Center;
        }

        private void menuAlignRight_Click(object sender, SourcedEventArgs e)
        {
            rtb.Selection.TextAlignment = C1TextAlignment.Right;
        }

        private void menuJustify_Click(object sender, SourcedEventArgs e)
        {
            rtb.Selection.TextAlignment = C1TextAlignment.Justify;
        }
        #endregion

        #region Undo/Redo
        private void menuUndo_Click(object sender, SourcedEventArgs e)
        {
            if (rtb.DocumentHistory.CanUndo)
            {
                _skipMenuUpdate = true; // don't update menu on RTB.SelectionChanged event after this operation
                rtb.DocumentHistory.Undo();
                System.Diagnostics.Debug.WriteLine("Performed Undo");
                rtb.Focus();
            }
        }

        private void menuRedo_Click(object sender, SourcedEventArgs e)
        {
            if (rtb.DocumentHistory.CanRedo)
            {
                _skipMenuUpdate = true; // don't update menu on RTB.SelectionChanged event after this operation
                rtb.DocumentHistory.Redo();
                System.Diagnostics.Debug.WriteLine("Performed Redo");
            }
        }

        private void menuClear_Click(object sender, SourcedEventArgs e)
        {
            _skipMenuUpdate = true;
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                // clear foreground and background colors
                rtb.Selection.InlineBackground = null;
                rtb.Selection.Foreground = rtb.Foreground;

                // clear font
                rtb.Selection.FontWeight = FontWeights.Normal;
                rtb.Selection.FontStyle = FontStyles.Normal;
                rtb.Selection.TextDecorations = null;
            }
        }

        private void menuFormatPainter_Click(object sender, SourcedEventArgs e)
        {
            rtb.FormatCopy();
            IsFormatPainterChecked = true;
            rtb.MouseSelectionCompleted -= OnMouseSelection;
            rtb.MouseSelectionCompleted += OnMouseSelection;
        }

        private bool IsFormatPainterChecked = false;
        void OnMouseSelection(object sender, EventArgs e)
        {
            if (IsFormatPainterChecked)
            {
                using (new DocumentHistoryGroup(rtb.DocumentHistory))
                {
                    rtb.FormatPaste();
                    rtb.Focus();
                }
                IsFormatPainterChecked = false;
            }
        }
        #endregion

        #region Lists
        private void menuBulletedList_Click(object sender, SourcedEventArgs e)
        {
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                // check if selection is already a list
                var isChecked = range.EditRanges.All(r => docs.TextMarkerStyle.Disc.Equals(GetMarkerStyle(r)));
                range.TrimRuns();
                rtb.Selection = range;
                foreach (var r in range.EditRanges)
                {
                    if (isChecked)
                    {
                        // undo list
                        range.UndoList();
                    }
                    else
                    {
                        // make bullet list
                        range.MakeList(docs.TextMarkerStyle.Disc);
                    }
                }
            }
        }

        private void menuNumericList_Click(object sender, SourcedEventArgs e)
        {
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                // check if selection is already a list
                var isChecked = range.EditRanges.All(r => docs.TextMarkerStyle.Decimal.Equals(GetMarkerStyle(r)));
                range.TrimRuns();
                rtb.Selection = range;
                foreach (var r in range.EditRanges)
                {
                    if (isChecked)
                    {
                        // undo list
                        range.UndoList();
                    }
                    else
                    {
                        // make number list
                        range.MakeList(docs.TextMarkerStyle.Decimal);
                    }
                }
            }
        }

        /// <summary>
        /// Get the MarkerStyle of the selection.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        docs.TextMarkerStyle? GetMarkerStyle(C1TextRange range)
        {
            var lists = range.Lists.ToList();
            docs.TextMarkerStyle? marker = lists.Any() ? new docs.TextMarkerStyle?(lists.First().MarkerStyle) : null;
            foreach (var list in lists)
            {
                if (marker != list.MarkerStyle) return null;
            }
            return marker;
        }
        #endregion

        #region Insert Objects
        private void menuInsertTable_Click(object sender, SourcedEventArgs e)
        {
            InsertTableTool tableTool = new InsertTableTool();
            tableTool.RichTextBox = rtb;
            tableTool.Show();
        }
        #endregion

        #region Table
        private void tableMenuDeleteCells_Click(object sender, SourcedEventArgs e)
        {
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                range.DeleteCells();
                rtb.Selection = new C1TextRange(range.Start);
                rtb.Focus();
            }
        }

        private void tableMenuDeleteColumns_Click(object sender, SourcedEventArgs e)
        {
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                range.DeleteColumns();
                rtb.Selection = new C1TextRange(range.Start);
                rtb.Focus();
            }
        }

        private void tableMenuDeleteRows_Click(object sender, SourcedEventArgs e)
        {
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                range.DeleteRows();
                rtb.Selection = new C1TextRange(range.Start);
                rtb.Focus();
            }
        }

        private void tableMenuDeleteTable_Click(object sender, SourcedEventArgs e)
        {
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                range.DeleteTable();
                rtb.Selection = new C1TextRange(range.Start);
                rtb.Focus();
            }
        }

        private void tableMenuMergeCells_Click(object sender, SourcedEventArgs e)
        {
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                var newCell = range.MergeCells();
                if (newCell != null)
                {
                    rtb.Selection = newCell.ContentRange;
                }
                rtb.Selection = new C1TextRange(range.Start);
                rtb.Focus();
            }
        }

        private void tableMenuUnmergeCells_Click(object sender, SourcedEventArgs e)
        {
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                range.UnmergeCell();
                rtb.Selection = new C1TextRange(range.Start);
                rtb.Focus();
            }
        }

        private void tableMenuAlignTop_Click(object sender, SourcedEventArgs e)
        {
            rtb.Selection.VerticalAlignment = C1VerticalAlignment.Top;
        }

        private void tableMenuAlignCenterVertical_Click(object sender, SourcedEventArgs e)
        {
            rtb.Selection.VerticalAlignment = C1VerticalAlignment.Middle;
        }

        private void tableMenuAlignBottom_Click(object sender, SourcedEventArgs e)
        {
            rtb.Selection.VerticalAlignment = C1VerticalAlignment.Bottom;
        }

        private void tableMenuInsertLeft_Click(object sender, SourcedEventArgs e)
        {
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                range.InsertColumnsLeft();
                rtb.Selection = new C1TextRange(range.Start);
                rtb.Focus();
            }
        }

        private void tableMenuInsertRight_Click(object sender, SourcedEventArgs e)
        {
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                range.InsertColumnsRight();
                rtb.Selection = new C1TextRange(range.Start);
                rtb.Focus();
            }
        }

        private void tableMenuInsertAbove_Click(object sender, SourcedEventArgs e)
        {
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                range.InsertRowsAbove();
                rtb.Selection = new C1TextRange(range.Start);
                rtb.Focus();
            }
        }

        private void tableMenuInsertBelow_Click(object sender, SourcedEventArgs e)
        {
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                range.InsertRowsBelow();
                rtb.Selection = new C1TextRange(range.Start);
                rtb.Focus();
            }
        }
        #endregion

        #endregion

        #region ** color menu actions
        private void menuColor_Click(object sender, SourcedEventArgs e)
        {
            C1RadialColorItem item = e.Source as C1RadialColorItem;
            if (item != null)
            {
                if (colorMenu.SelectedIndex == 1)
                {
                    // apply foreground and update selected item color
                    UpdateForeground(item.Brush);
                    // update Tag so that to show correct color under the Icon and use it at clicks on the Color item in menu root
                    fontColorItem.Tag = item.Brush;
                    if (item.ParentItem != null)
                    {
                        // keep index, so that we can use it at switching context
                        _selectedForegroundColors[item.GroupName] = ((ItemsControl)item.ParentItem).Items.IndexOf(item);
                    }
                }
                else if (colorMenu.SelectedIndex == 2)
                {
                    // apply background and update selected item color
                    UpdateBackground(item.Brush);
                    // update Tag so that to show correct color under the Icon and use it at clicks on the Color item in menu root
                    textHighlightItem.Tag = item.Brush;
                    if (item.ParentItem != null)
                    {
                        // keep index, so that we can use it at switching context
                        _selectedBackroundColors[item.GroupName] = ((ItemsControl)item.ParentItem).Items.IndexOf(item);
                    }
                }
            }
        }

        private void textHighlightItem_Click(object sender, SourcedEventArgs e)
        {
            if (contextMenu.CurrentItem == contextMenu)
            {
                // click on the Color item in the menu root
                UpdateBackground(textHighlightItem.Tag as Brush);
            }
            else
            {
                // update selected indices according to the context 
                orangeItem.SelectedIndex = _selectedBackroundColors["Orange"];
                redItem.SelectedIndex = _selectedBackroundColors["Red"];
                greenItem.SelectedIndex = _selectedBackroundColors["Green"];
                blueItem.SelectedIndex = _selectedBackroundColors["Blue"];
                grayItem.SelectedIndex = _selectedBackroundColors["Gray"];
            }
        }

        private void fontColorItem_Click(object sender, SourcedEventArgs e)
        {
            if (contextMenu.CurrentItem == contextMenu)
            {
                // click on the Color item in the menu root
                UpdateForeground(fontColorItem.Tag as Brush);
            }
            else
            {
                // update selected indices according to the context 
                orangeItem.SelectedIndex = _selectedForegroundColors["Orange"];
                redItem.SelectedIndex = _selectedForegroundColors["Red"];
                greenItem.SelectedIndex = _selectedForegroundColors["Green"];
                blueItem.SelectedIndex = _selectedForegroundColors["Blue"];
                grayItem.SelectedIndex = _selectedForegroundColors["Gray"];
            }
        }

        // update font color in RTB selection
        private void UpdateForeground(Brush brush)
        {
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                foreach (var r in range.EditRanges)
                {
                    r.Foreground = brush;
                }
                rtb.Focus();
            }
        }
        // update text background in RTB selection
        private void UpdateBackground(Brush brush)
        {
            using (new DocumentHistoryGroup(rtb.DocumentHistory))
            {
                var range = rtb.Selection;
                range.TrimRuns();
                rtb.Selection = range;
                foreach (var r in range.EditRanges)
                {
                    r.InlineBackground = brush;
                }
                rtb.Focus();
            }
        }
        #endregion

        #region ** update C1RichTextBox.ContextMenu depending on the current context
        // cache last tap point, so that to show updated context menu in the correct position
        private Point LastLocation
        {
            get;
            set;
        }

        private void rtb_RightTapped(object sender, MouseButtonEventArgs e)
        {
            LastLocation = e.GetPosition(rtb);
            UpdateContextMenu();
            e.Handled = true;
        }

        private void rtb_PointerPressed(object sender, MouseButtonEventArgs e)
        {
            LastLocation = e.GetPosition(rtb);
            UpdateContextMenu();
        }

        private void rtb_SelectionChanged(object sender, EventArgs e)
        {
            tableMenuCopy.IsEnabled = tableMenuCut.IsEnabled = menuCopy.IsEnabled = menuCut.IsEnabled = menuFormatPainter.IsEnabled = tableFormatPainter.IsEnabled = !rtb.Selection.IsEmpty;

            //table buttons
            tableMenuMergeCells.IsEnabled = rtb.Selection.CanMergeCells();
            tableMenuDeleteCells.IsEnabled = rtb.Selection.CanDeleteCells();
            tableMenuInsertAbove.IsEnabled = tableMenuInsertBelow.IsEnabled = rtb.Selection.CanInsertColumnsOrRows();
            tableMenuInsertRight.IsEnabled = tableMenuInsertLeft.IsEnabled = rtb.Selection.CanInsertColumnsOrRows();
            
        }

        // some menu operations change C1RichTextBox content, in such case there is no need to reset menu
        private bool _skipMenuUpdate = false;

        C1RadialMenu currentC1RadialMenu;
        private void UpdateContextMenu()
        {
            if (_skipMenuUpdate)
            {
                _skipMenuUpdate = false;
                return;
            }
            bool isTable = rtb.Selection.Cells.Count<C1TableCell>() > 0 && string.IsNullOrEmpty(rtb.Selection.Text);
            if (isTable)
            {
                if (currentC1RadialMenu != tableMenu)
                {
                    // the table is selected, show table menu
                    currentC1RadialMenu = tableMenu;
                    tableMenu.Visibility = Visibility.Visible;
                    contextMenu.Visibility = Visibility.Collapsed;
                    contextMenu.Hide();
                }
            }
            else
            {
                if (currentC1RadialMenu != contextMenu)
                {
                    // hide table menu, show common one
                    currentC1RadialMenu = contextMenu;
                    contextMenu.Visibility = Visibility.Visible;
                    tableMenu.Visibility = Visibility.Collapsed;
                    tableMenu.Hide();
                }
            }
            // update Paste
            try
            {
                tableMenuPaste.IsEnabled = menuPaste.IsEnabled = (System.Windows.Clipboard.GetDataObject().GetFormats().Length > 0);
            }
            catch
            {
                // it's possible that Clipboard access is denied. Don't fail application in such case
            }
            DocumentHistory_HistoryChanged(null, null);

            if (LastLocation.X > 0)
            {
                C1RadialMenu menu = currentC1RadialMenu;
                if (menu != null)
                {
                    // count menu Offset, so that it doesn't overlap current selection in the RichTextBox
                    // offset to right
                    Point offset = new Point(150, 0);
                    if (LastLocation.X > this.ActualWidth - 300)
                    {
                        // if menu is close to the right side of the screen, it will be automatically moved so that
                        // all menu is visible on the screen. In such case it will overlap current pointer position.
                        // So, use additional offset to top or bottom
                        offset.Y = LastLocation.Y > this.ActualHeight - 300 ? -150 : 150;
                    }
                    menu.Offset = offset;
                    // show menu in collapsed state, with some offset to the right, so that it doesn't overlap current selection
                    menu.Show(rtb, LastLocation, false);
                }
            }
        }

        void DocumentHistory_HistoryChanged(object sender, EventArgs e)
        {
            tableRedo.IsEnabled = menuRedo.IsEnabled = rtb.DocumentHistory.CanRedo;
            tableUndo.IsEnabled = menuUndo.IsEnabled = rtb.DocumentHistory.CanUndo;
        }
        #endregion

        [DllImport("user32.dll")]
        public static extern IntPtr GetAncestor(IntPtr hWnd, int flags);
    }

    public class StringToPathConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            return Geometry.Parse(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
