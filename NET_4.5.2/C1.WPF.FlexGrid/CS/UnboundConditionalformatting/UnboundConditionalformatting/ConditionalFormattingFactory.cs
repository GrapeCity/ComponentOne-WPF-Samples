using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.FlexGrid;
using System.Windows.Media;
using System.Windows.Controls;

namespace UnboundConditionalformatting
{
    class ConditionalFormattingFactory : CellFactory
    {
        // create brushes used to indicate low and high values
        static Brush _brLowValue = new SolidColorBrush(Colors.Red);
        static Brush _brHighValue = new SolidColorBrush(Colors.Green);

        // overridden to apply the custom brushes based on the cell value
        public override void ApplyCellStyles(C1FlexGrid grid, CellType cellType, CellRange range, Border bdr)
        {
            // we are interested only in data cells (no headers)
            if (cellType == CellType.Cell)
            {
                // we are interested in double values only
                var col = grid.Columns[range.Column];
                if (col.DataType == typeof(double))
                {
                    // get the cell value
                    var value = (double)grid[range.Row, col];

                    // apply formatting if value is out of range
                    if (value < 10 || value > 100)
                    {
                        var tb = bdr.Child as TextBlock;
                        if (tb != null)
                        {
                            tb.Foreground = value < 10 ? _brLowValue : _brHighValue;
                        }
                    }
                }
            }
        }
    }
}
