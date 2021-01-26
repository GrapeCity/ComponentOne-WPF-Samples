## UnboundConditionalFormatting
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.FlexGrid/CS/UnboundConditionalformatting)
____
#### Shows how you can implement conditional formatting with unbound grids.
____
The usual approach for implementing conditional formatting is using a CellTemplate and
binding control properties to underlying data values. This works well in bound scenarios,
but doesn't work when the grid is being used in unbound mode.

This sample shows how you can implement a CellFactory that formats cells based on their
values, regardless of whether the grid is bound or unbound.

The custom cell factory in this case is very simple. It derives from the base CellFactory
and overrides the ApplyCellStyles method to apply brushes selected based on the cell 
value. 

The code looks like this:

```
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
```
