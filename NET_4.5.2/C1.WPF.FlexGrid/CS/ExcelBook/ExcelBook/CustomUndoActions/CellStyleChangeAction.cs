using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Collections.Generic;
using C1.WPF.FlexGrid;
using C1.WPF.FlexGridBook;

namespace ExcelBook
{
    public class CellStyleChangeAction : IUndoableAction
    {
        C1FlexGridBook _flex;
        Dictionary<CellRange, CellStyle> _oldStyles, _newStyles;

        public CellStyleChangeAction(C1FlexGridBook flex)
        {
            _flex = flex;
            _oldStyles = new Dictionary<CellRange, CellStyle>();
            SaveStyles(_oldStyles);
        }
        public bool SaveNewState()
        {
            _newStyles = new Dictionary<CellRange, CellStyle>();
            SaveStyles(_newStyles);
            return true;
        }
        public void Undo()
        {
            ApplyStyles(_oldStyles);
        }
        public void Redo()
        {
            ApplyStyles(_newStyles);
        }
        void SaveStyles(Dictionary<CellRange, CellStyle> styles)
        {
            foreach (var rng in _flex.Selection.Cells)
            {
                var row = _flex.Rows[rng.Row] as ExcelRow;
                var col = _flex.Columns[rng.Column];
                styles[rng] = row.GetCellStyle(col);
            }
        }
        void ApplyStyles(Dictionary<CellRange, CellStyle> styles)
        {
            foreach (var rng in styles.Keys)
            {
                var row = _flex.Rows[rng.Row] as ExcelRow;
                var col = _flex.Columns[rng.Column];
                row.SetCellStyle(col, styles[rng]);
            }
        }
    }
}
