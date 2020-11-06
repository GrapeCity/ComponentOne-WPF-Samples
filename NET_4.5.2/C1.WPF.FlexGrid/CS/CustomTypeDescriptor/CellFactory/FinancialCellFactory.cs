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
using C1.WPF.FlexGrid;

namespace CustomTypeDescriptor
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
            var propName = c.BoundPropertyName;
            if (r.DataItem is FinancialData &&
               (propName == "LastSale" || propName == "Bid" || propName == "Ask"))
            {
                StockTicker ticker = new StockTicker();
                bdr.Child = ticker;
                bdr.Padding = _thicknessEmpty;

                // to show sparklines
                ticker.Tag = r.DataItem;
                ticker.BindingSource = propName;

                // traditional binding
                var binding = new Binding(propName);
                binding.Source = r.DataItem;
                binding.Mode = BindingMode.OneWay;
                ticker.SetBinding(StockTicker.ValueProperty, binding);
            }
            else
            {
                // use default implementation
                base.CreateCellContent(grid, bdr, range);
            }
        }
    }
}
