using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.FlexGrid;
using C1.WPF;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace DragCells
{
    /// <summary>
    /// Cell factory used to create the MyElement view objects (cells, vistualized) that 
    /// correspond to the MyDataObject model objects (actual data, permanent).
    /// </summary>
    public class MyCellFactory : CellFactory
    {
        C1DragDropManager _ddMgr;

        public MyCellFactory(C1DragDropManager ddMgr)
        {
            _ddMgr = ddMgr;
        }
        public override void CreateCellContent(C1FlexGrid grid, Border bdr, CellRange range)
        {
            // get the data object stored in the unbound grid
            var obj = grid[range.Row, range.Column] as MyDataObject;
            if (obj != null)
            {
                // create a custom view object
                var e = new MyViewObject(grid, obj);

                // assign it to the cell
                bdr.Child = e;

                // register cell border as a drag source
                _ddMgr.RegisterDragSource(bdr, DragDropEffect.Move, ModifierKeys.None);
            }
            else
            {
                base.CreateCellContent(grid, bdr, range);
            }
        }
        public override void DisposeCell(C1FlexGrid grid, CellType cellType, FrameworkElement cell)
        {
            _ddMgr.RegisterDragSource(cell, DragDropEffect.None, ModifierKeys.None);
            base.DisposeCell(grid, cellType, cell);
        }
    }
}
