using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using C1.WPF.FlexGrid;

namespace C1.WPF.FlexGridBook
{
    /// <summary>
    /// Extends CalcEngine to deal with cell ranges and to use the grid's DataContext.
    /// </summary>
    public class ExcelCalcEngine : CalcEngine.CalcEngine
    {
        // ** fields
        C1FlexGridBook _flex;
        static Brush _brLinkForeground = new SolidColorBrush(Colors.Blue);

        // ** ctor
        public ExcelCalcEngine(C1FlexGridBook grid)
        {
            // save reference to parent grid
            _flex = grid;

            // parse multi-cell range references ($A2:B$4)
            IdentifierChars = "$:";

            // register Hyperlink function (returns a HyperlinkButton)
            CacheExpressions = false;
            RegisterFunction("hyperlink", 2, (parms) => 
                {
                    var h = new HyperlinkButton();
                    //h.NavigateUri = new Uri((string)parms[0]);
                    h.Content = (string)parms[1];
                    h.BorderThickness = new Thickness(0);
                    h.BorderBrush = null;
                    h.VerticalAlignment = VerticalAlignment.Center;
                    h.HorizontalAlignment = HorizontalAlignment.Left;
                    h.Foreground = _brLinkForeground;
                    //h.TargetName = "_blank";
                    return h; 
                });
        }

        // ** object model

        /// <summary>
        /// Exposes the grid's DataContext to the CalcEngine.
        /// </summary>
        public override object DataContext
        {
            get { return _flex != null ? _flex.DataContext : base.DataContext; }
            set 
            {
                if (_flex != null)
                {
                    _flex.DataContext = value;
                }
                else
                {
                    base.DataContext = value;
                }
            }
        }
        /// <summary>
        /// Parses references to cell ranges.
        /// </summary>
        /// <param name="identifier">String representing a cell range (e.g. "A1" or "A1:B12".</param>
        /// <returns>A <see cref="CellRange"/> object that represents the given range.</returns>
        public override object GetExternalObject(string identifier)
        {
            if (_flex != null)
            {
                var cells = identifier.Split(':');
                if (cells.Length > 0 && cells.Length < 3)
                {
                    var rng = GetRange(cells[0]);
                    if (cells.Length > 1)
                    {
                        rng = rng.Union(GetRange(cells[1]));
                    }
                    if (rng.IsValid)
                    {
                        return new CellRangeReference(_flex, rng);
                    }
                }
            }

            // this doesn't look like a range
            return null;
        }

        // ** implementation
        CellRange GetRange(string cell)
        {
            int index = 0;

            // parse column
            int col = -1;
            bool absCol = false;
            for (; index < cell.Length; index++)
            {
                var c = cell[index];
                if (c == '$' && !absCol)
                {
                    absCol = true;
                    continue;
                }
                if (!char.IsLetter(c))
                {
                    break;
                }
                if (col < 0) col = 0;
                col = 26 * col + (char.ToUpper(c) - 'A' + 1);
            }

            // parse row
            int row = -1;
            bool absRow = false;
            for (; index < cell.Length; index++)
            {
                var c = cell[index];
                if (c == '$' && !absRow)
                {
                    absRow = true;
                    continue;
                }
                if (!char.IsDigit(c))
                {
                    break;
                }
                if (row < 0) row = 0;
                row = 10 * row + (c - '0');
            }

            // sanity
            if (index < cell.Length)
            {
                row = col = -1;
            }

            // done
            return new CellRange(row - 1, col - 1);
        }
        class CellRangeReference : CalcEngine.IValueObject, IEnumerable
        {
            C1FlexGrid _grid;
            CellRange _rng;
            static Dictionary<CellRange, bool> _evaluating = new Dictionary<CellRange, bool>();

            public CellRangeReference(C1FlexGrid grid, CellRange rng)
            {
                _grid = grid;
                _rng = rng;
            }

            // ** IValueObject
            public object GetValue()
            {
                return GetValue(_rng);
            }

            // ** IEnumerable
            public IEnumerator GetEnumerator()
            {
                foreach (var cell in _rng.Cells)
                {
                    yield return GetValue(cell);
                }
            }

            // ** implementation
            object GetValue(CellRange rng)
            {
                // detect circular references
                if (_evaluating.ContainsKey(rng))
                {
                    throw new Exception("Circular Reference");
                }

                // track this range while evaluating it
                try
                {
                    _evaluating.Add(rng, true);
                    var row = _grid.Rows[rng.Row] as ExcelRow;
                    var col = _grid.Columns[rng.Column];
                    return row != null
                        ? row.GetValue(col) : _grid[rng.Row, rng.Column];
                }
                finally
                {
                    _evaluating.Remove(rng);
                }
            }
        }
    }
}
