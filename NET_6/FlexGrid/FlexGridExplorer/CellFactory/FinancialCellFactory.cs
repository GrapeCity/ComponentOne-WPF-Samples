using C1.WPF.Grid;
using System;
using System.Windows;

namespace FlexGridExplorer
{
    public class FinancialCellFactory : GridCellFactory
    {
        public override object GetCellContentType(GridCellType cellType, GridCellRange range)
        {
            if (cellType == GridCellType.Cell)
            {
                var c = base.Grid.Columns[range.Column];
                if (c.Binding == "LastSale" || c.Binding == "Bid" || c.Binding == "Ask")
                {
                    return typeof(StockTicker);
                }
            }
            return base.GetCellContentType(cellType, range);
        }

        public override FrameworkElement CreateCellContent(GridCellType cellType, GridCellRange range, object cellContentType)
        {
            if (cellContentType as Type == typeof(StockTicker))
                return new StockTicker();
            return base.CreateCellContent(cellType, range, cellContentType);
        }

        public override void BindCellContent(GridCellType cellType, GridCellRange range, FrameworkElement cellContent)
        {
            var stockTicker = cellContent as StockTicker;
            if (stockTicker != null)
            {
                stockTicker.Tag = Grid.Rows[range.Row].DataItem;
                stockTicker.BindingSource = Grid.Columns[range.Column].Binding;
                stockTicker.Value = (double)(decimal)Grid[range.Row, range.Column];
            }
            else
            {
                base.BindCellContent(cellType, range, cellContent);
            }
        }
    }
}
