using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.WPF.FlexGrid;

namespace C1.WPF.FlexGridBook
{
    /// <summary>
    /// Undo/Redo stack for the C1FlexGrid.
    /// </summary>
    public class ExcelUndoStack : UndoStack
    {
        //-------------------------------------------------------------------------
        #region ** fields

        C1FlexGridBook _flex;
        IUndoableAction _pendingAction;

        #endregion

        //-------------------------------------------------------------------------
        #region ** ctor

        /// <summary>
        /// Initializes a new instance of a <see cref="ExcelUndoStack"/>.
        /// </summary>
        /// <param name="flex"><see cref="C1FlexGrid"/> that owns this undo stack.</param>
        public ExcelUndoStack(C1FlexGridBook flex)
        {
            _flex = flex;
            flex.PrepareCellForEdit += flex_PrepareCellForEdit;
            flex.CellEditEnded += flex_CellEditEnded;
            flex.ResizingColumn += flex_ResizingColumn;
            flex.ResizedColumn += flex_ResizedColumn;
            flex.ResizingRow += flex_ResizingRow;
            flex.ResizedRow += flex_ResizedRow;
            flex.SortingColumn += flex_SortingColumn;
        }

        #endregion

        //-------------------------------------------------------------------------
        #region ** event handlers

        // undo/redo column sizes
        void flex_ResizingColumn(object sender, CellRangeEventArgs e)
        {
            if (!(_pendingAction is ColumnResizeAction))
            {
                _pendingAction = new ColumnResizeAction(_flex);
            }
        }
        void flex_ResizedColumn(object sender, CellRangeEventArgs e)
        {
            if (_pendingAction is ColumnResizeAction && _pendingAction.SaveNewState())
            {
                _flex.UndoStack.AddAction(_pendingAction);
            }
            _pendingAction = null;
        }

        // undo/redo row sizes
        void flex_ResizingRow(object sender, CellRangeEventArgs e)
        {
            if (!(_pendingAction is RowResizeAction))
            {
                _pendingAction = new RowResizeAction(_flex, e);
            }
        }
        void flex_ResizedRow(object sender, CellRangeEventArgs e)
        {
            if (_pendingAction is RowResizeAction && _pendingAction.SaveNewState())
            {
                _flex.UndoStack.AddAction(_pendingAction);
            }
            _pendingAction = null;
        }

        // undo/redo edits
        void flex_PrepareCellForEdit(object sender, CellEditEventArgs e)
        {
            _pendingAction = new EditAction(_flex);
        }
        void flex_CellEditEnded(object sender, CellEditEventArgs e)
        {
            if (!e.Cancel && _pendingAction is EditAction && _pendingAction.SaveNewState())
            {
                _flex.UndoStack.AddAction(_pendingAction);                
            }
            _pendingAction = null;
        }

        // record original sort values
        void flex_SortingColumn(object sender, CellRangeEventArgs e)
        {
            _pendingAction = new SortAction(_flex);
        }

        #endregion
    }
}
