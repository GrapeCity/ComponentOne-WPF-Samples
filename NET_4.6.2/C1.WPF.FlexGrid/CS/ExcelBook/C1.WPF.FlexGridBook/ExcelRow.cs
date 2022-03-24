using System;
using System.Globalization;
using System.Windows;
using System.Collections.Generic;

namespace C1.WPF.FlexGridBook
{
    using C1.WPF.Excel;
    using C1.WPF.FlexGrid;

    /// <summary>
    /// Grid row that can be edited, used as a tree node, maintains a 
    /// collection of cell styles associated with each column, and 
    /// evaluates cells that contain formulas (strings starting with
    /// an equals sign).
    /// </summary>
    public class ExcelRow : GroupRow
    {
        // ** fields
        Dictionary<Column, CellStyle> _cellStyles;

        const string DEFAULT_FORMAT = "#,##0.######"; // set default precision to 6 digits

        // ** ctor
        public ExcelRow(ExcelRow styleRow)
        {
            IsReadOnly = false;
            if (styleRow != null && styleRow.Grid != null)
            {
                foreach (var c in styleRow.Grid.Columns)
                {
                    var cs = styleRow.GetCellStyle(c);
                    if (cs != null)
                    {
                        this.SetCellStyle(c, cs.Clone());
                    }
                }
            }
        }
        public ExcelRow() : this(null)
        {
        }

        // ** object model

        /// <summary>
        /// Gets the formula in a cell.
        /// </summary>
        /// <param name="col">Column that defines the cell to retrieve.</param>
        /// <returns>A formula string or null.</returns>
        public string GetFormula(Column col)
        {
            var value = base.GetUnboundValue(col) as string;
            return !string.IsNullOrEmpty(value) && value[0] == '='
                ? value
                : null;
        }
        /// <summary>
        /// Gets the data in a cell as it should be shown in the editor
        /// (full formulas, raw values).
        /// </summary>
        /// <param name="col">Column that defines the cell to retrieve.</param>
        /// <returns></returns>
        public string GetDataEditor(Column col)
        {
            var text = GetFormula(col);
            if (text == null)
            {
                var val = GetDataRaw(col);
                text = val != null ? val.ToString() : string.Empty;
            }
            return text;
        }
        /// <summary>
        /// Gets the value in a cell, evaluating expressions represented by 
        /// strings that start with an equals sign.
        /// </summary>
        /// <param name="col">Column that defines the cell to retrieve.</param>
        /// <returns></returns>
        public object GetValue(Column col)
        {
            var val = base.GetUnboundValue(col);
            return Evaluate(val);
        }

        // ** overrides

        /// <summary>
        /// Gets the raw data in a cell (evaluates formulas but does not format the values).
        /// </summary>
        /// <param name="col">Column that defines the cell to retrieve.</param>
        /// <returns>An object representing the cell value.</returns>
        public override object GetDataRaw(Column col)
        {
            try
            {
                return GetValue(col);
            }
            catch (Exception x)
            {
                return "** ERR: " + x.Message;
            }
        }
        /// <summary>
        /// Overridden to apply formatting when getting data.
        /// </summary>
        public override string GetDataFormatted(Column col)
        {
            // get data as usual
            var data = GetDataRaw(col);

            // apply format
            var ifmt = data as IFormattable;
            if (ifmt != null)
            {
                // get cell format
                var s = GetCellStyle(col) as ExcelCellStyle;
                var fmt = s != null && !string.IsNullOrEmpty(s.Format)
                    ? s.Format
                    : DEFAULT_FORMAT;
                data = ifmt.ToString(fmt, CultureInfo.CurrentUICulture);
            }

            // done
            return data != null ? data.ToString() : string.Empty;
        }

        // ** object model

        /// <summary>
        /// Applies a style to a cell in this row.
        /// </summary>
        public void SetCellStyle(Column col, CellStyle style)
        {
            if (style != GetCellStyle(col))
            {
                if (_cellStyles == null)
                {
                    _cellStyles = new Dictionary<Column, CellStyle>();
                }
                _cellStyles[col] = style;
                if (Grid != null)
                {
                    Grid.Invalidate(new CellRange(this.Index, col.Index));
                }
            }
        }
        /// <summary>
        /// Gets the style applied to a cell in this row.
        /// </summary>
        public CellStyle GetCellStyle(Column col)
        {
            CellStyle s = null;
            if (_cellStyles != null)
            {
                _cellStyles.TryGetValue(col, out s);
            }
            return s;
        }

        // ** implementation

        // evaluates the content of a cell
        object Evaluate(object val)
        {
            var str = val as string;
            if (!string.IsNullOrEmpty(str))
            {
                if (str[0] == '\'')
                {
                    return str.Substring(1);
                }
                if (str[0] == '=')
                {
                    var flex = Grid as C1FlexGridBook;
                    if (flex != null)
                    {
                        return flex.CalcEngine.Evaluate(str);
                    }
                }
            }
            return val;
        }
    }
}
