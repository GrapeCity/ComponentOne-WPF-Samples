using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.FlexGrid;
using System.Windows.Controls;

namespace GridToolTips
{
    /// <summary>
    /// Custom cell factory class that adds a tooltip to the cells.
    /// </summary>
    public class ToolTipCellFactory : CellFactory
    {
        public override void CreateCellContent(C1FlexGrid grid, Border bdr, CellRange rng)
        {
            // crate the content
            base.CreateCellContent(grid, bdr, rng);

            // add the tooltip
            var tip = string.Format("row: {0} col: {1}\r\ncontent: {2}\r\ncolumn Tag: {3}",
                rng.Row,
                rng.Column,
                grid[rng.Row, rng.Column],
                grid.Columns[rng.Column].Tag);
            ToolTipService.SetToolTip(bdr, tip);
        }
    }
}
