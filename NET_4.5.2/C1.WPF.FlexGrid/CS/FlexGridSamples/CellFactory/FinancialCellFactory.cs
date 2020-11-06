using System;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
#if SILVERLIGHT
using C1.Silverlight.FlexGrid;
#else
using C1.WPF.FlexGrid;
#endif

namespace MainTestApplication
{
    public class FinancialCellFactory : CellFactory
    {
        static Thickness _thicknessEmpty = new Thickness(0);
        
        // bind cell to ticker
        public override void CreateCellContent(C1FlexGrid grid, Border bdr, CellRange range)
        {
            // create binding for this cell
            var r = grid.Rows[range.Row];
            var c = grid.Columns[range.Column];
            var pi = c.PropertyInfo;
            if (r.DataItem is FinancialData &&
               (pi.Name == "LastSale" || pi.Name == "Bid" || pi.Name == "Ask"))
            {
                // create stock ticker cell
                FlexGridSamples.StockTicker ticker = new FlexGridSamples.StockTicker();
                bdr.Child = ticker;
                bdr.Padding = _thicknessEmpty;

                // to show sparklines
                ticker.Tag = r.DataItem;
                ticker.BindingSource = pi.Name;

                // traditional binding
                var binding = new Binding(pi.Name);
                binding.Source = r.DataItem;
                binding.Mode = BindingMode.OneWay;
                ticker.SetBinding(FlexGridSamples.StockTicker.ValueProperty, binding);
            }
            else
            {
                // use default implementation
                base.CreateCellContent(grid, bdr, range);
            }
        }

        // override horizontal alignment to make ticker cell stretch and fill the column width
        public override void ApplyCellStyles(C1FlexGrid grid, CellType cellType, CellRange range, Border bdr)
        {
            var ticker = bdr.Child as FlexGridSamples.StockTicker;
            if (ticker != null)
            {
                ticker.HorizontalAlignment = HorizontalAlignment.Stretch;
            }
        }
    }
}
