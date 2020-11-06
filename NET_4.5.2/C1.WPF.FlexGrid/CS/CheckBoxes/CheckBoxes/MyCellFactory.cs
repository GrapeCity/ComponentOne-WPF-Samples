using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.FlexGrid;
using System.Windows.Controls;
using System.Windows;

namespace CheckBoxes
{
    public class MyCellFactory : CellFactory
    {
        public override void CreateCellContent(C1FlexGrid grid, System.Windows.Controls.Border bdr, CellRange rng)
        {

            if (grid.Cells[rng.Row, rng.Column] is bool)
            {
                
                // it does, so create a checkbox to show/edit the value
                CheckBox chk = new CheckBox();
                 chk.IsChecked =(bool?)grid.Cells[rng.Row, rng.Column];
                chk.VerticalAlignment = VerticalAlignment.Center;
                chk.HorizontalAlignment = HorizontalAlignment.Center;
                ToolTipService.SetToolTip(chk, "This CheckBox represents a boolean value stored in a grid cell.");

                // assign the checkbox to the cell element (a Border)
                bdr.Child = chk;

                // connect the checkbox so it updates the content of the grid cell
                chk.Tag = grid;
                chk.AddHandler(CheckBox.ClickEvent, new RoutedEventHandler(chk_Click));


            }
            else if (grid.Cells[rng.Row, rng.Column] is Control)
            {
                // add the control to the cell (if it doesn't have a parent)
                Control ctl = (Control)grid.Cells[rng.Row, rng.Column];
                if (ctl.Parent == null)
                {
                      bdr.Child =(System.Windows.Controls.Control) grid.Cells[rng.Row, rng.Column];
                }


            }
            else
            {
                // not a boolean, allow default behavior
                base.CreateCellContent(grid, bdr, rng);

            }

        }

        // when our checkbox is clicked, update the underlying value

        private void chk_Click(object sender, EventArgs e)
        {
            // get the checkbox that was clicked
            CheckBox chk = (CheckBox)sender;

            // get the grid that owns the checkbox
            C1FlexGrid flex = (C1FlexGrid)chk.Tag;

            // get the cell that contains the checkbox
            Border bdr = (Border)chk.Parent;

            //int row = bdr.GetValue(Grid.RowProperty.GlobalIndex);
            int row =(int) bdr.GetValue(Grid.RowProperty);
            int col = (int)bdr.GetValue(Grid.ColumnProperty);

            //// assign new value to the cell
            flex[row, col] = chk.IsChecked;

        }
        // when cells are destroyed, remove content from borders so it can be re-used
        public override void DisposeCell(C1.WPF.FlexGrid.C1FlexGrid grid, C1.WPF.FlexGrid.CellType cellType, System.Windows.FrameworkElement cell)
        {
            Border bdr = (Border)cell;
            bdr.Child = null;
        }
    }
}
