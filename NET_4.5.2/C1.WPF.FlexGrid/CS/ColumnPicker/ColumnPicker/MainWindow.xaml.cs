using System;
using System.Collections.Generic;
using System.Linq;
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
using C1.WPF.FlexGrid;

namespace ColumnPicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // use the ColumnLayout to save/restore column layout
        // the ColumnLayout property gets/sets an XML string that contains the 
        // order, visibility, and width of the columns on the grid.
        string _columnLayout;
        public MainWindow()
        {
            InitializeComponent();
            // populate grid
            _flex.ItemsSource = Product.GetProducts(100);

            // set clipboard copy/paste mode
            _flex.ClipboardCopyMode = ClipboardCopyMode.ExcludeHeader;
            _flex.ClipboardPasteMode = ClipboardCopyMode.ExcludeHeader;


            // set up event handlers for context menu
            _flex.MouseRightButtonDown += (s, e) =>
            {
                e.Handled = true; // << disable standard "Silverlight" menu
            };
            _flex.MouseRightButtonUp += (s, e) =>
            {
                e.Handled = true;
                ShowContextMenu(e);
            };
        }

        // show the context menu
        void ShowContextMenu(MouseEventArgs e)
        {
            // create context menu
            var ctxMenu = new ContextMenu();
            // check what type of cell was clicked to show the right menu
            var ht = _flex.HitTest(e);
            switch (ht.CellType)
            {
                // if the click was on the column headers, show column picker
                case CellType.ColumnHeader:
                    {
                        CreateColumnPickerMenu(ctxMenu);
                        break;
                    }
                // show clipboard menu if the click was on a regular cell
                case CellType.Cell:
                    {
                        // select cell if not yet selected
                        if (!_flex.Selection.Contains(ht.CellRange))
                        {
                            _flex.Select(ht.CellRange, false);
                        }
                        CreateClipboardMenu(ctxMenu);
                        break;
                    }
            }

            // show the menu
            var pt = e.GetPosition(LayoutRoot);
            Point offset = new Point(pt.X, pt.Y);
            ctxMenu.InvalidateMeasure();
            ctxMenu.InvalidateArrange();
            ctxMenu.IsOpen = true;
            
        }

        // populate context menu with clipboard commands
        void CreateClipboardMenu(ContextMenu ctxMenu)
        {
            foreach (string cmd in "Cut|Copy|Paste|Delete".Split('|'))
            {
                // create menu item to hold the command
                var mi = new MenuItem();
                mi.Header = cmd;
                ctxMenu.Items.Add(mi);

                // execute commands
                mi.Click += (s, ea) =>
                {
                    switch ((string)mi.Header)
                    {
                        case "Cut":
                            _flex.Copy();
                            foreach (var cell in _flex.Selection.Cells)
                            {
                                _flex[cell.Row, cell.Column] = null;
                            }
                            break;

                        case "Copy":
                            _flex.Copy();
                            break;

                        case "Paste":
                            _flex.Paste();
                            break;

                        case "Delete":
                            foreach (var cell in _flex.Selection.Cells)
                            {
                                _flex[cell.Row, cell.Column] = null;
                            }
                            break;
                    }

                    // done, close the menu

                };
            }
        }
        // populate context menu with column picker
        void CreateColumnPickerMenu(ContextMenu ctxMenu)
        {
            bool isFirstCol = true;
            foreach (var c in _flex.Columns)
            {
                if (isFirstCol)
                {
                    isFirstCol = false;
                    continue;
                }    
                // create checkbox to represent this column
                var chk = new CheckBox();
                chk.Tag = c;
                chk.IsChecked = c.Visible;
                chk.Content = !string.IsNullOrEmpty(c.Header)
                    ? c.Header
                    : c.BoundPropertyName;

                // create menu item to hold the checkbox
                var mi = new MenuItem();
                mi.Header = chk;
                ctxMenu.Items.Add(mi);

                // toggle column visibility when checkbox is clicked
                chk.Click += (s, ea) =>
                {
                    var col = chk.Tag as Column;
                    col.Visible = chk.IsChecked.Value;
                    ctxMenu.IsOpen = false;
                };
            }
        }
        private void SaveLayout_Click(object sender, RoutedEventArgs e)
        {
            _columnLayout = _flex.ColumnLayout;
        }
        private void LoadLayout_Click(object sender, RoutedEventArgs e)
        {
            _flex.ColumnLayout = _columnLayout;
            _flex.Invalidate();
        }
    }
}
