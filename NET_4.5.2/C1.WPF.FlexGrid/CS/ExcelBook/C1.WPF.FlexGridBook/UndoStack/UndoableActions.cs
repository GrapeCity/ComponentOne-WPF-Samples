using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using C1.WPF.FlexGrid;

namespace C1.WPF.FlexGridBook
{
    /// <summary>
    /// Undoable edit action.
    /// </summary>
    public class EditAction : IUndoableAction
    {
        //-------------------------------------------------------------------------
        #region ** fields

        C1FlexGridBook _flex;
        CellRange _range;
        object _oldValue, _newValue;

        #endregion

        //-------------------------------------------------------------------------
        #region ** ctor

        public EditAction(C1FlexGridBook flex)
        {
            _flex = flex;
            _range = _flex.Selection;
            _oldValue = GetValue();
        }
        object GetValue()
        {
            var row = _flex.Rows[_range.Row];
            var col = _flex.Columns[_range.Column];
            return row[col];
        }

        #endregion

        //-------------------------------------------------------------------------
        #region ** IUndoableAction

        /// <summary>
        /// Saves the state of the control after the action.
        /// </summary>
        public bool SaveNewState()
        {
            _newValue = GetValue();
            return !object.Equals(_oldValue, _newValue);
        }
        /// <summary>
        /// Reverts the effect of the last action.
        /// </summary>
        public void Undo()
        {
            _flex[_range.Row, _range.Column] = _oldValue;
            _flex.Select(_range, true);
        }
        /// <summary>
        /// Restores the effect of the last undone action.
        /// </summary>
        public void Redo()
        {
            _flex[_range.Row, _range.Column] = _newValue;
            _flex.Select(_range, true);
        }
        #endregion
    }
    /// <summary>
    /// Undoable resize column action.
    /// </summary>
    public class ColumnResizeAction : IUndoableAction
    {
        //-------------------------------------------------------------------------
        #region ** fields

        C1FlexGridBook _flex;
        CellRange _range;
        string _oldCols, _newCols;

        #endregion

        //-------------------------------------------------------------------------
        #region ** ctor

        public ColumnResizeAction(C1FlexGridBook flex)
        {
            _flex = flex;
            _oldCols = _flex.ColumnLayout;
            _range = _flex.Selection;
        }
        #endregion

        //-------------------------------------------------------------------------
        #region ** IUndoableAction

        /// <summary>
        /// Saves the state of the control after the action.
        /// </summary>
        public bool SaveNewState()
        {
            _newCols = _flex.ColumnLayout;
            return true;
        }
        /// <summary>
        /// Reverts the effect of the last action.
        /// </summary>
        public void Undo()
        {
            _flex.ColumnLayout = _oldCols;
        }
        /// <summary>
        /// Restores the effect of the last undone action.
        /// </summary>
        public void Redo()
        {
            _flex.ColumnLayout = _newCols;
        }
        #endregion
    }
    /// <summary>
    /// Undoable insert/delete column action.
    /// </summary>
    public class InsertDeleteColumnAction : IUndoableAction
    {
        //-------------------------------------------------------------------------
        #region ** fields

        C1FlexGridBook _flex;
        List<Column> _oldColumns, _newColumns;
        CellRange _oldSel, _newSel;

        #endregion

        //-------------------------------------------------------------------------
        #region ** ctor

        public InsertDeleteColumnAction(C1FlexGridBook flex)
        {
            _flex = flex;
            _oldSel = _flex.Selection;
            _oldColumns = new List<Column>();
            foreach (var col in _flex.Columns)
            {
                _oldColumns.Add(col);
            }
        }
        #endregion

        //-------------------------------------------------------------------------
        #region ** IUndoableAction

        /// <summary>
        /// Saves the state of the control after the action.
        /// </summary>
        public bool SaveNewState()
        {
            _newSel = _flex.Selection;
            _newColumns = new List<Column>();
            foreach (var col in _flex.Columns)
            {
                _newColumns.Add(col);
            }
            return true;
        }
        /// <summary>
        /// Reverts the effect of the last action.
        /// </summary>
        public void Undo()
        {
            _flex.Columns.Clear();
            foreach (var col in _oldColumns)
            {
                _flex.Columns.Add(col);
            }
            _flex.Selection = _oldSel;
        }
        /// <summary>
        /// Restores the effect of the last undone action.
        /// </summary>
        public void Redo()
        {
            _flex.Columns.Clear();
            foreach (var col in _newColumns)
            {
                _flex.Columns.Add(col);
            }
            _flex.Selection = _newSel;
        }
        #endregion
    }
    /// <summary>
    /// Undoable resize row action.
    /// </summary>
    public class RowResizeAction : IUndoableAction
    {
        //-------------------------------------------------------------------------
        #region ** fields

        C1FlexGridBook _flex;
        int _row;
        double _oldSize, _newSize;

        #endregion

        //-------------------------------------------------------------------------
        #region ** ctor

        public RowResizeAction(C1FlexGridBook flex, CellRangeEventArgs e)
        {
            _flex = flex;
            _row = e.Row;
            _oldSize = _flex.Rows[_row].Height;
        }
        #endregion

        //-------------------------------------------------------------------------
        #region ** IUndoableAction

        /// <summary>
        /// Saves the state of the control after the action.
        /// </summary>
        public bool SaveNewState()
        {
            _newSize = _flex.Rows[_row].Height;
            return true;
        }
        /// <summary>
        /// Reverts the effect of the last action.
        /// </summary>
        public void Undo()
        {
            _flex.Rows[_row].Height = _oldSize;
        }
        /// <summary>
        /// Restores the effect of the last undone action.
        /// </summary>
        public void Redo()
        {
            _flex.Rows[_row].Height = _newSize;
        }
        #endregion
    }
    /// <summary>
    /// Undoable insert/delete row action.
    /// </summary>
    public class InsertDeleteRowAction : IUndoableAction
    {
        //-------------------------------------------------------------------------
        #region ** fields

        C1FlexGridBook _flex;
        List<Row> _oldRows, _newRows;
        CellRange _oldSel, _newSel;

        #endregion

        //-------------------------------------------------------------------------
        #region ** ctor

        public InsertDeleteRowAction(C1FlexGridBook flex)
        {
            _flex = flex;
            _oldSel = _flex.Selection;
            _oldRows = new List<Row>();
            foreach (var row in _flex.Rows)
            {
                _oldRows.Add(row);
            }
        }
        #endregion

        //-------------------------------------------------------------------------
        #region ** IUndoableAction

        /// <summary>
        /// Saves the state of the control after the action.
        /// </summary>
        public bool SaveNewState()
        {
            _newSel = _flex.Selection;
            _newRows = new List<Row>();
            foreach (var row in _flex.Rows)
            {
                _newRows.Add(row);
            }
            return true;
        }
        /// <summary>
        /// Reverts the effect of the last action.
        /// </summary>
        public void Undo()
        {
            _flex.Rows.Clear();
            foreach (var row in _oldRows)
            {
                _flex.Rows.Add(row);
            }
            _flex.Selection = _oldSel;
        }
        /// <summary>
        /// Restores the effect of the last undone action.
        /// </summary>
        public void Redo()
        {
            _flex.Rows.Clear();
            foreach (var row in _newRows)
            {
                _flex.Rows.Add(row);
            }
            _flex.Selection = _newSel;
        }
        #endregion
    }
    /// <summary>
    /// Undoable Sort action.
    /// </summary>
    public class SortAction : IUndoableAction
    {
        //-------------------------------------------------------------------------
        #region ** fields

        C1FlexGridBook _flex;
        List<Row> _rowsBefore, _rowsAfter;

        #endregion

        //-------------------------------------------------------------------------
        #region ** ctor

        public SortAction(C1FlexGridBook flex)
        {
            _flex = flex;
            _rowsBefore = new List<Row>();
            foreach (var row in _flex.Rows)
            {
                _rowsBefore.Add(row);
            }
        }

        #endregion

        //-------------------------------------------------------------------------
        #region ** IUndoableAction

        /// <summary>
        /// Saves the state of the control after the action.
        /// </summary>
        public bool SaveNewState()
        {
            _rowsAfter = new List<Row>();
            foreach (var row in _flex.Rows)
            {
                _rowsAfter.Add(row);
            }
            return true;
        }
        /// <summary>
        /// Reverts the effect of the last action.
        /// </summary>
        public void Undo()
        {
            CopyRows(_rowsBefore);
        }
        /// <summary>
        /// Restores the effect of the last undone action.
        /// </summary>
        public void Redo()
        {
            CopyRows(_rowsAfter);
        }
        void CopyRows(List<Row> rows)
        {
            using (_flex.Rows.DeferNotifications())
            {
                _flex.Rows.Clear();
                foreach (var row in rows)
                {
                    _flex.Rows.Add(row);
                }
            }
        }
        #endregion
    }
    /// <summary>
    /// Undoable Paste action.
    /// </summary>
    public class PasteAction : IUndoableAction
    {
        //-------------------------------------------------------------------------
        #region ** fields

        C1FlexGridBook _flex;
        CellRange _oldSel, _newSel;
        List<PasteCellInfo> _pasteCells = new List<PasteCellInfo>();

        #endregion

        //-------------------------------------------------------------------------
        #region ** ctor

        /// <summary>
        /// Initializes a new instance of an <see cref="PasteAction"/>.
        /// </summary>
        public PasteAction(C1FlexGridBook flex)
        {
            _flex = flex;
            _oldSel = _newSel = flex.Selection;
        }

        #endregion

        //-------------------------------------------------------------------------
        #region ** object model

        /// <summary>
        /// Adds information about an individual cell changing within a paste operation.
        /// </summary>
        public void PasteCell(int row, int col, object oldValue, object newValue)
        {
            _pasteCells.Add(new PasteCellInfo(row, col, oldValue, newValue));
        }

        #endregion

        //-------------------------------------------------------------------------
        #region ** IUndoableAction

        /// <summary>
        /// Saves the state of the control after the action.
        /// </summary>
        public bool SaveNewState()
        {
            _newSel = _flex.Selection;
            return _pasteCells.Count > 0;
        }
        /// <summary>
        /// Reverts the effect of the last action.
        /// </summary>
        public void Undo()
        {
            foreach (var pc in _pasteCells)
            {
                _flex[pc.Row, pc.Col] = pc.OldValue;
            }
            _flex.Selection = _oldSel;
        }
        /// <summary>
        /// Restores the effect of the last undone action.
        /// </summary>
        public void Redo()
        {
            foreach (var pc in _pasteCells)
            {
                _flex[pc.Row, pc.Col] = pc.NewValue;
            }
            _flex.Selection = _newSel;
        }

        #endregion

        //-------------------------------------------------------------------------
        #region ** implementation

        struct PasteCellInfo
        {
            public int Row, Col;
            public object OldValue, NewValue;
            public PasteCellInfo(int row, int col, object oldValue, object newValue)
            {
                Row = row;
                Col = col;
                OldValue = oldValue;
                NewValue = newValue;
            }
        }

        #endregion
    }
}
