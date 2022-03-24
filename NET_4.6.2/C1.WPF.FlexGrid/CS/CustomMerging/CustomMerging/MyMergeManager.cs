using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.FlexGrid;

namespace CustomMerging
{
    /// <summary>
    /// Custom merge manager that creates cell ranges spanning multiple rows and columns
    /// </summary>
    public class MyMergeManager : IMergeManager
    {
        public CellRange GetMergedRange(C1FlexGrid grid, CellType cellType, CellRange rg)
        {
            // we are only interested in data cells 
            // (not merging row or column headers)
            if (cellType == CellType.Cell)
            {
                // expand left/right
                for (int i = rg.Column; i < grid.Columns.Count - 1; i++)
                {
                    if (GetDataDisplay(grid, rg.Row, i) != GetDataDisplay(grid, rg.Row, i + 1)) break;
                    rg.Column2 = i + 1;
                }
                for (int i = rg.Column; i > 0; i--)
                {
                    if (GetDataDisplay(grid, rg.Row, i) != GetDataDisplay(grid, rg.Row, i - 1)) break;
                    rg.Column = i - 1;
                }

                // expand up/down
                for (int i = rg.Row; i < grid.Rows.Count - 1; i++)
                {
                    if (GetDataDisplay(grid, i, rg.Column) != GetDataDisplay(grid, i + 1, rg.Column)) break;
                    rg.Row2 = i + 1;
                }
                for (int i = rg.Row; i > 0; i--)
                {
                    if (GetDataDisplay(grid, i, rg.Column) != GetDataDisplay(grid, i - 1, rg.Column)) break;
                    rg.Row = i - 1;
                }
            }

            // done
            return rg;
        }
        string GetDataDisplay(C1FlexGrid grid, int r, int c)
        {
            return grid[r, c].ToString();
        }
    }
}
