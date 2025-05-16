using System.Windows;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    public class DataGridHierarchicalNavigation : DataGridDefaultInputHandlingStrategy
    {
        public DataGridHierarchicalNavigation(C1DataGrid dataGrid)
            : base(dataGrid)
        {
        }

        public override bool GoToLeftCell(DataGridCell currentCell, bool shift, bool ctrl)
        {
            if (currentCell != null)
            {
                C1DataGrid dataGrid = currentCell.DataGrid;
                DataGridRow selectedRow = currentCell.Row;
                switch (selectedRow.Type)
                {
                    case DataGridRowType.New:
                    case DataGridRowType.Item:
                        var column = GetPreviousVisibleColumn(currentCell.Column);
                        if (column == null)
                        {
                            var row = GetPreviousVisibleRow(currentCell.Row);

                            if (row != null)
                            {
                                if (row.DataGrid != currentCell.DataGrid)
                                {
                                    row.DataGrid.Focus();
                                    return SetCurrentCell(row.DataGrid[row, GetRightMostVisibleColumn(row.DataGrid)], shift, ctrl);
                                }
                                else
                                {
                                    return SetCurrentCell(dataGrid[row, GetRightMostVisibleColumn(dataGrid)], shift, ctrl);
                                }
                            }
                        }
                        break;
                }
            }
            return base.GoToLeftCell(currentCell, shift, ctrl);
        }

        public override bool GoToRightCell(DataGridCell currentCell, bool shift, bool ctrl)
        {
            if (currentCell != null)
            {
                C1DataGrid dataGrid = currentCell.DataGrid;
                DataGridRow selectedRow = currentCell.Row;
                switch (selectedRow.Type)
                {
                    case DataGridRowType.New:
                    case DataGridRowType.Item:
                        var column = GetNextVisibleColumn(currentCell.Column);
                        if (column == null)
                        {
                            var row = GetNextVisibleRow(currentCell.Row);
                            if (row != null)
                            {
                                if (row.DataGrid != currentCell.DataGrid)
                                {
                                    row.DataGrid.Focus();
                                    return SetCurrentCell(row.DataGrid[row, GetLeftMostVisibleColumn(row.DataGrid)], shift, ctrl);
                                }
                                else
                                {
                                    return SetCurrentCell(dataGrid[row, GetLeftMostVisibleColumn(dataGrid)], shift, ctrl);
                                }
                            }
                        }
                        break;
                }
            }
            return base.GoToRightCell(currentCell, shift, ctrl);
        }

        public override bool GoToDownCell(DataGridCell currentCell, bool shift, bool ctrl)
        {
            if (currentCell != null)
            {
                var nextRow = GetNextVisibleRow(currentCell.Row);
                if (nextRow != null)
                {
                    if (nextRow.DataGrid == currentCell.DataGrid)
                    {
                        return SetCurrentCell(currentCell.DataGrid[nextRow, currentCell.Column], shift, ctrl);
                    }
                    else
                    {
                        nextRow.DataGrid.Focus();
                        return SetCurrentCell(nextRow.DataGrid[nextRow, nextRow.DataGrid.CurrentColumn != null ? nextRow.DataGrid.CurrentColumn : GetLeftMostVisibleColumn(nextRow.DataGrid)], shift, ctrl);
                    }
                }
            }
            return false;
        }

        public override bool GoToUpCell(DataGridCell currentCell, bool shift, bool ctrl)
        {
            if (currentCell != null)
            {
                var previousRow = GetPreviousVisibleRow(currentCell.Row);
                if (previousRow != null)
                {
                    if (previousRow.DataGrid == currentCell.DataGrid)
                    {
                        return SetCurrentCell(currentCell.DataGrid[previousRow, currentCell.Column], shift, ctrl);
                    }
                    else
                    {
                        previousRow.DataGrid.Focus();
                        return SetCurrentCell(previousRow.DataGrid[previousRow, previousRow.DataGrid.CurrentColumn != null ? previousRow.DataGrid.CurrentColumn : GetLeftMostVisibleColumn(previousRow.DataGrid)], shift, ctrl);
                    }
                }
            }
            return false;
        }

        public override DataGridRow GetNextVisibleRow(DataGridRow currentRow)
        {
            if (currentRow.DetailsVisibility == Visibility.Visible)
            {
                var nestedGrid = GetNestedGrid(currentRow);
                if (nestedGrid != null && nestedGrid.Rows.GetFirstVisibleRow() != null && nestedGrid.Columns.GetFirstVisibleColumn() != null)
                {
                    return GetTopVisibleRow(nestedGrid);
                }
            }
            var nextRow = base.GetNextVisibleRow(currentRow);
            if (nextRow != null)
            {
                return nextRow;
            }
            else
            {
                var dataGrid = currentRow.DataGrid;
                while (true)
                {
                    var parentRow = GetParentRow(dataGrid);
                    if (parentRow != null)
                    {
                        dataGrid = parentRow.DataGrid;
                        nextRow = base.GetNextVisibleRow(parentRow);
                        if (nextRow != null)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                return nextRow;
            }
        }

        public override DataGridRow GetPreviousVisibleRow(DataGridRow currentRow)
        {
            DataGridRow previousRow = base.GetPreviousVisibleRow(currentRow);

            if (previousRow == null)
            {
                var dataGrid = currentRow.DataGrid;
                return (dataGrid.Tag as DataGridRow);
            }
            else
            {
                if (previousRow.DetailsVisibility == Visibility.Visible)
                {
                    while (true)
                    {
                        var nestedGrid = GetNestedGrid(previousRow);
                        if (nestedGrid != null &&
                            nestedGrid.Rows.GetFirstVisibleRow() != null &&
                            nestedGrid.Columns.GetFirstVisibleColumn() != null)
                        {
                            previousRow = GetBottomVisibleRow(nestedGrid);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                return previousRow;
            }
        }

        public C1DataGrid GetNestedGrid(DataGridRow row)
        {
            if ((row.Type == DataGridRowType.Item ||
                   row.Type == DataGridRowType.New) &&
               row.DetailsVisibility == Visibility.Visible)
            {
                return row.DetailsPresenter.Content as C1DataGrid;
            }
            return null;
        }

        public DataGridRow GetParentRow(C1DataGrid dataGrid)
        {
            return (dataGrid.Tag as DataGridRow);
        }

        public C1DataGrid GetRootGrid(C1DataGrid dataGrid)
        {
            var rootGrid = dataGrid;
            while (true)
            {
                var row = GetParentRow(rootGrid);
                if (row != null)
                {
                    rootGrid = row.DataGrid;
                }
                else
                {
                    break;
                }
            }
            return rootGrid;
        }

        public override bool SetCurrentCell(DataGridCell cell, bool shift, bool ctrl)
        {
            if (cell != null)
            {
                C1DataGrid dataGrid = cell.DataGrid;
                dataGrid.CurrentCell = cell;
                dataGrid.Selection.BeginUpdate();
                dataGrid.Selection.Clear();
                dataGrid.Selection.Add(cell, cell);
                dataGrid.Selection.EndUpdate();
                while (true)
                {
                    if (cell.DataGrid == dataGrid)
                    {
                        dataGrid.ScrollIntoView();
                    }
                    else
                    {
                        dataGrid.ScrollIntoView(cell.Presenter);
                    }
                    var row = GetParentRow(dataGrid);
                    if (row != null)
                    {
                        dataGrid = row.DataGrid;
                    }
                    else
                    {
                        break;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
