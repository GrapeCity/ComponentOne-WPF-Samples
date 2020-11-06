
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using C1.WPF.FlexGrid;
using C1.WPF.Excel;

using System.Globalization;
using System.Windows;

namespace ExcelExport
{

    /// <summary>
    /// Grid row that can be edited, used as a tree node, maintains a 
    /// collection of cell styles associated with each column
    /// </summary>
    public class ExcelRow : GroupRow
    {
        // ** fields

        private Dictionary<Column, CellStyle> _cellStyles;
        // set default precision to 6 digits
        private const string DEFAULT_FORMAT = "#,##0.######";

        // ** ctor
        public ExcelRow(ExcelRow styleRow)
		{
			IsReadOnly = false;
			if (styleRow != null && styleRow.Grid != null) {
				foreach (var c in styleRow.Grid.Columns) {
					dynamic cs = styleRow.GetCellStyle(c);
					if (cs != null) {
						this.SetCellStyle(c, cs.Clone());
					}
				}
			}
		}
        public ExcelRow()
            : this(null)
        {
        }

        // ** object model

        /// <summary>
        /// Overridden to apply formatting when getting data.
        /// </summary>
        public override string GetDataFormatted(Column col)
        {
            // get data as usual
            dynamic data = GetDataRaw(col);

            // apply format
            dynamic ifmt = data as IFormattable;
            if (ifmt != null)
            {
                // get cell format
                dynamic s = GetCellStyle(col) as ExcelCellStyle;
                dynamic fmt = s != null && (!string.IsNullOrEmpty(s.Format)) ? s.Format : DEFAULT_FORMAT;
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
            if (!object.ReferenceEquals(style, GetCellStyle(col)))
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
                _cellStyles.TryGetValue(col,out s);
            }
            return s;
        }

    }
}

