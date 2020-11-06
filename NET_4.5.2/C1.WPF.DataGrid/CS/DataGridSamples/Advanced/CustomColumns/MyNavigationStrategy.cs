using System.Windows.Input;
using C1.WPF;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    public class MyNavigationStrategy : DataGridDefaultInputHandlingStrategy
    {
        public MyNavigationStrategy(C1DataGrid dataGrid)
            : base(dataGrid)
        {
        }

        public override void HandleKeyDown(KeyEventArgs e)
        {
            base.HandleKeyDown(e);
            //If the current cell is editing
            if (DataGrid.CurrentCell != null && DataGrid.CurrentCell.IsEditing)
            {
                switch (e.Key)
                {
                    case Key.Up:
                        if (!KeyboardUtil.Ctrl)
                        {
                            if (GoToUpCell(DataGrid.CurrentCell))
                            {
                                e.Handled = true;
                            }
                        }
                        break;

                    case Key.Down:
                        if (!KeyboardUtil.Ctrl)
                        {
                            if (GoToDownCell(DataGrid.CurrentCell))
                            {
                                e.Handled = true;
                            }
                        }
                        break;
                }
            }
        }
    }
}
