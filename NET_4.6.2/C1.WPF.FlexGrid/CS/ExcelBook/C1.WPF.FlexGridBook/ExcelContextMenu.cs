using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using C1.WPF;
using C1.WPF.FlexGrid;

namespace C1.WPF.FlexGridBook
{
    /// <summary>
    /// Specifies commands that may be executed from the context menu.
    /// </summary>
    public enum ContextMenuCommands
    {
        /// <summary>
        /// Cut the selection to the clipboard.
        /// </summary>
        Cut,
        /// <summary>
        /// Copy the selection to the clipboard.
        /// </summary>
        Copy,
        /// <summary>
        /// Paste the contents of the selection over the current selection.
        /// </summary>
        Paste,
        /// <summary>
        /// Clear the content of the selection without copying it to the clipboard.
        /// </summary>
        Clear,
        /// <summary>
        /// Insert new rows at the cursor.
        /// </summary>
        InsertRows,
        /// <summary>
        /// Delete the selected rows.
        /// </summary>
        DeleteRows,
        /// <summary>
        /// Insert new columns at the cursor.
        /// </summary>
        InsertColumns,
        /// <summary>
        /// Delete the selected columns.
        /// </summary>
        DeleteColumns
    }

    /// <summary>
    /// Context menu customized for use with the C1FlexGridBook control.
    /// </summary>
    public class ExcelContextMenu : /*C1*/ContextMenu
    {
        //---------------------------------------------------------------------------
        #region ** fields

        C1FlexGridBook _grid;
        Point _ptMouseMove;

        #endregion

        //---------------------------------------------------------------------------
        #region ** ctor

        /// <summary>
        /// Initializes a new instance of an <see cref="ExcelContextMenu"/>.
        /// </summary>
        /// <param name="grid"><see cref="C1FlexGridBook"/> that owns this context menu.</param>
        internal ExcelContextMenu(C1FlexGridBook grid)
        {
            // save reference to parent grid
            _grid = grid;

            // build menu
            Reset();

            // set up event handlers for context menu
            _grid.MouseMove += _grid_MouseMove;
            _grid.MouseRightButtonDown += _grid_MouseRightButtonDown;
            _grid.MouseRightButtonUp += _grid_MouseRightButtonUp;
        }

        #endregion

        //---------------------------------------------------------------------------
        #region ** object model
        
        /// <summary>
        /// Re-initializes the menu to its default content.
        /// </summary>
        void Reset()
        {
            // disconnect existing items
            foreach (var item in this.Items)
            {
                var mi = item as MenuItem;
                if (mi != null)
                {
                    mi.Click -= menuItem_Click;
                }
            }

            // remove all existing items
            Items.Clear();

            // populate the menu
            CreateMenuItem("Cut", ContextMenuCommands.Cut);
            CreateMenuItem("Copy", ContextMenuCommands.Copy);
            CreateMenuItem("Paste", ContextMenuCommands.Paste);
            Items.Add(new C1Separator());
            CreateMenuItem("Insert Rows", ContextMenuCommands.InsertRows);
            CreateMenuItem("Delete Rows", ContextMenuCommands.DeleteRows);
            CreateMenuItem("Insert Columns", ContextMenuCommands.InsertColumns);
            CreateMenuItem("Delete Columns", ContextMenuCommands.DeleteColumns);
        }
        /// <summary>
        /// Get a <see cref="MenuItem"/> based on a command code.
        /// </summary>
        /// <param name="cmd"><see cref="ContextMenuCommands"/> that determines the item to be retrieved.</param>
        /// <returns><see cref="MenuItem"/> that matches the given <paramref name="cmd"/>.</returns>
        public MenuItem GetMenuItem(ContextMenuCommands cmd)
        {
            foreach (var item in this.Items)
            {
                var mi = item as MenuItem;
                if (mi != null && object.Equals(mi.Tag, cmd))
                {
                    return mi;
                }
            }
            return null;
        }
        /// <summary>
        /// Executes a context menu command.
        /// </summary>
        /// <param name="cmd"><see cref="ContextMenuCommands"/> that specifies which 
        /// command should be executed.</param>
        public void ExecuteCommand(ContextMenuCommands cmd)
        {
            // save selection
            var sel = _grid.Selection;

            // execute command
            IUndoableAction action;
            switch (cmd)
            {
                case ContextMenuCommands.Cut:
                    _grid.Copy();
                    _grid.ClearSelection();
                    break;

                case ContextMenuCommands.Copy:
                    _grid.Copy();
                    break;

                case ContextMenuCommands.Paste:
                    _grid.Paste();
                    break;

                case ContextMenuCommands.Clear:
                    _grid.ClearSelection();
                    break;

                case ContextMenuCommands.InsertRows:
                    if (sel.IsValid)
                    {
                        // to clone styles from row above (like Excel)
                        var index = sel.TopRow > 0 ? sel.TopRow - 1 : sel.TopRow;
                        var styleRow = _grid.Rows[index] as ExcelRow;

                        action = new InsertDeleteRowAction(_grid);
                        for (int i = 0; i < sel.RowSpan; i++)
                        {
                            _grid.Rows.Insert(sel.TopRow, new ExcelRow(styleRow));
                        }
                        if (action.SaveNewState())
                        {
                            _grid.UndoStack.AddAction(action);
                        }
                    }
                    else
                    {
                        action = new InsertDeleteRowAction(_grid);
                        _grid.Rows.Add(new ExcelRow());
                        if (action.SaveNewState())
                        {
                            _grid.UndoStack.AddAction(action);
                        }
                    }
                    break;

                case ContextMenuCommands.DeleteRows:
                    if (sel.IsValid)
                    {
                        action = new InsertDeleteRowAction(_grid);
                        for (int i = 0; i < sel.RowSpan; i++)
                        {
                            _grid.Rows.RemoveAt(sel.TopRow);
                        }
                        if (action.SaveNewState())
                        {
                            _grid.UndoStack.AddAction(action);
                        }
                    }
                    break;

                case ContextMenuCommands.InsertColumns:
                    if (sel.IsValid)
                    {
                        action = new InsertDeleteColumnAction(_grid);
                        for (int i = 0; i < sel.ColumnSpan; i++)
                        {
                            _grid.Columns.Insert(sel.LeftColumn, new Column());
                        }
                        if (action.SaveNewState())
                        {
                            _grid.UndoStack.AddAction(action);
                        }
                    }
                    else
                    {
                        action = new InsertDeleteColumnAction(_grid);
                        _grid.Columns.Add(new Column());
                        if (action.SaveNewState())
                        {
                            _grid.UndoStack.AddAction(action);
                        }
                    }
                    break;

                case ContextMenuCommands.DeleteColumns:
                    if (sel.IsValid)
                    {
                        action = new InsertDeleteColumnAction(_grid);
                        for (int i = 0; i < sel.ColumnSpan; i++)
                        {
                            _grid.Columns.RemoveAt(sel.LeftColumn);
                        }
                        if (action.SaveNewState())
                        {
                            _grid.UndoStack.AddAction(action);
                        }
                    }
                    break;
            }

            // restore selection
            try
            {
                _grid.Focus();
                _grid.Selection = sel;
            }
            catch
            {
                // old selection became invalid after deleting rows/columns
            }
        }

        #endregion

        //---------------------------------------------------------------------------
        #region ** event handlers

        // save mouse coordinates to update selection when the context menu pops up
        void _grid_MouseMove(object sender, MouseEventArgs e)
        {
            _ptMouseMove = e.GetPosition(null);
        }
        void _grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            IsOpen = false;
            //Hide();
        }
        void _grid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // update selection
            var ht = _grid.HitTest(e);
            if (!_grid.Selection.Contains(ht.CellRange))
            {
                _grid.Selection = ht.CellRange;
            }

            // adjust offset to account for bug in ContextMenu class
            Point ptMouseDown = e.GetPosition(null);

            // show menu
            IsOpen = true;
            //Show(null, ptMouseDown);
        }
        /// <summary>
        /// Overridden to disable paste command if the clipboard is empty when the context menu opens.
        /// </summary>
        /// <param name="e"><see cref="CancelEventArgs"/> that provides the event data.</param>
        protected override void OnOpened(RoutedEventArgs e)
        {
            base.OnOpened(e);
            var pasteItem = GetMenuItem(ContextMenuCommands.Paste);
            if (pasteItem != null)
            {
                pasteItem.IsEnabled = System.Windows.Clipboard.ContainsText();
            }
        }

        #endregion

        //---------------------------------------------------------------------------
        #region ** implementation

        // create a menu item
        void CreateMenuItem(string hdr, ContextMenuCommands cmd)
        {
            // create item, attach event handler
            var mi = new MenuItem();
            mi.Header = hdr;
            mi.Tag = cmd;
            mi.Click += menuItem_Click;

            // create image for this item
            var resName = string.Format("{0}_small.png", cmd.ToString());
            var stream = GetResourceStream(resName);
            if (stream != null)
            {
                var bmp = new BitmapImage();
                bmp.StreamSource = stream;
                var img = new Image();
                img.Source = bmp;
                img.Stretch = Stretch.None;
                img.Margin = new Thickness(4, 0, 0, 0);
                mi.Icon = img;
            }

            // done
            Items.Add(mi);
        }

        // handle clicks on the menu items
        void menuItem_Click(object sender, EventArgs e)
        {
            var mi = sender as MenuItem;
            if (mi.Tag is ContextMenuCommands)
            {
                ExecuteCommand((ContextMenuCommands)mi.Tag);
            }
        }

        // get an embedded resource stream
        Stream GetResourceStream(string resName)
        {
            var asm = Assembly.GetExecutingAssembly();
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
}
