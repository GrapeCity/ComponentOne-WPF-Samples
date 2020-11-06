using C1.WPF.FlexGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlexSheetSample.CustomUndoActions
{
    public class CellRangeEditAction : IUndoableAction
    {
        //-------------------------------------------------------------------------
        #region ** fields

        C1FlexGrid _flex;
        object[,] _oldCellValues, _newCellValues;
        CellRange _oldSelection, _newSelection;
        int _oldTopRow, _oldLeftCol, _newTopRow, _newLeftCol;


        #endregion

        //-------------------------------------------------------------------------
        #region ** ctor

        /// <summary>
        /// Initializes a new instance of an <see cref="CellRangeEditAction"/>.
        /// </summary>
        public CellRangeEditAction(C1FlexGrid flex)
        {
            _flex = flex;
            var sel = _flex.Selection;
            _oldTopRow = sel.TopRow;
            _oldLeftCol = sel.LeftColumn;
            _oldSelection = sel;
            RecordState(ref _oldCellValues, sel);
        }

        #endregion

        //-------------------------------------------------------------------------
        #region ** IUndoableAction

        public bool SaveNewState()
        {
            var sel = _flex.Selection;
            _newTopRow = sel.TopRow;
            _newLeftCol = sel.LeftColumn;
            _newSelection = sel;
            RecordState(ref _newCellValues, sel);
            return sel.IsValid;
        }
        public void Undo()
        {
            SetValues(_oldTopRow, _oldLeftCol, _oldCellValues);
            _flex.Selection = _oldSelection;
        }
        public void Redo()
        {
            SetValues(_newTopRow, _newLeftCol, _newCellValues);
            _flex.Selection = _newSelection;
        }

        #endregion

        //-------------------------------------------------------------------------
        #region ** implementation

        /// <summary>
        /// Records the values of all the cells within the current selection of the <see cref="C1FlexGrid"/>.
        /// </summary>
        /// <param name="values">The array where cell values are stored.</param>
        /// /// <param name="range">The range of cells whose values will be stored.</param>
        void RecordState(ref object[,] values, CellRange range)
        {
            values = new object[range.BottomRow - range.TopRow + 1, range.RightColumn - range.LeftColumn + 1];
            for (int r = range.TopRow; r <= range.BottomRow; r++)
            {
                // skip rows that are not selected
                if (_flex.SelectionMode == SelectionMode.ListBox && !_flex.Rows[r].Selected)
                {
                    continue;
                }

                var row = _flex.Rows[r];
                for (int c = range.LeftColumn; c <= range.RightColumn; c++)
                {
                    var col = _flex.Columns[c];
                    values[r - range.TopRow, c - range.LeftColumn] = row.GetDataRaw(col);
                }
            }
        }
        void SetValues(int topRow, int leftCol, object[,] values)
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    _flex[i + topRow, j + leftCol] = values[i, j];
                }
            }
        }

        #endregion
    }
}
