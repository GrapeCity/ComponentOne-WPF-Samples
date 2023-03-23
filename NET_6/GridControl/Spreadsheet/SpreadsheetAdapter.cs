using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using C1.WPF.Grid;

namespace Spreadsheet
{
    public class SpreadsheetAdapter : GridControlAdapter
    {
        #region ** fields

        private Dictionary<KeyValuePair<int, int>, string> _storedValues = new Dictionary<KeyValuePair<int, int>, string>();
        private int A = 'A';
        private int Z = 'Z';
        private Thickness CellPadding { get; set; } = new Thickness(2, 1, 2, 1);
        private Dictionary<int, double> _columnSizes = new Dictionary<int, double>();
        private Dictionary<int, double> _rowSizes = new Dictionary<int, double>();
        private ColumnInfo DefaultColumnInfo = new ColumnInfo(new GridLength(65), 27, double.PositiveInfinity, true);
        private RowInfo DefaultRowInfo = new RowInfo(new GridLength(20), 0, double.PositiveInfinity, true);
        private int ROWS_COUNT = 1_000_000_000;
        private int COLUMNS_COUNT = 1_000_000_000;

        #endregion

        #region ** dimensions

        public override int RowsCount => ROWS_COUNT + 1;
        public override int ColumnsCount => COLUMNS_COUNT + 1;

        public override ColumnInfo GetDefaultColumn()
        {
            return DefaultColumnInfo;
        }

        public override RowInfo GetDefaultRow()
        {
            return DefaultRowInfo;
        }

        protected override ColumnInfo GetColumnInfo(int column)
        {
            double width;
            if (column < 1)
            {
                return new ColumnInfo(GridLength.Auto, 27, double.PositiveInfinity, true);
            }
            else if (_columnSizes.TryGetValue(column + 1, out width))
            {
                return new ColumnInfo(new GridLength(width), 0, double.PositiveInfinity, true);
            }
            else
            {
                return GetDefaultColumn();
            }
        }

        protected override RowInfo GetRowInfo(int row)
        {
            double height;
            if (row < 1)
            {
                return GetDefaultRow();
            }
            else if (_rowSizes.TryGetValue(row + 1, out height))
            {
                return new RowInfo(new GridLength(height), 0, double.PositiveInfinity, true);
            }
            else
            {
                return GetDefaultRow();
            }
        }

        public override GridControlRange NavigableRange => new GridControlRange(1, 1, ROWS_COUNT, COLUMNS_COUNT);

        public override int GetFrozenColumns()
        {
            return 1;
        }

        public override int GetFrozenRows()
        {
            return 1;
        }

        protected override bool CanSetColumnWidth(int row, int column)
        {
            return column >= 1 && row == 0;
        }

        protected override bool CanSetRowHeight(int row, int column)
        {
            return row >= 1 && column == 0;
        }

        protected override void SetColumnWidth(int column, double width)
        {
            if (!double.IsFinite(width))
                _columnSizes.Remove(column + 1);
            else
                _columnSizes[column + 1] = width;
            base.SetColumnWidth(column, width);
        }

        protected override void SetRowHeight(int row, double height)
        {
            if (!double.IsFinite(height))
                _rowSizes.Remove(row + 1);
            else
                _rowSizes[row + 1] = height;
            base.SetRowHeight(row, height);
        }

        protected override bool SaveDesiredCellSize(GridControlRange range)
        {
            return range.Column < 1;
        }
        #endregion

        #region ** cell

        public Style ColumnHeaderSelectedStyle { get; set; }

        public Style RowHeaderSelectedStyle { get; set; }

        public override void PrepareCell(GridControlRange range, GridControlCellView cell, Thickness internalBorders)
        {
            base.PrepareCell(range, cell, internalBorders);

            if (range.Row == 0 || range.Column == 0)
            {
                cell.BorderThickness = new Thickness(0);
            }
            if (range.Row == 0 && Grid.GetSelectedTimes(new GridControlRange(-1, range.Column)) > 0)
            {
                cell.Style = ColumnHeaderSelectedStyle;
            }
            else if (range.Column == 0 && Grid.GetSelectedTimes(new GridControlRange(range.Row, -1)) > 0)
            {
                cell.Style = RowHeaderSelectedStyle;
            }

        }
        //public override void PrepareCell(GridCellType cellType, GridCellRange range, GridCellView cell)
        //{
        //    if (cellType != GridCellType.Cell)
        //    {
        //        //cell.BorderThickness = new Thickness(1);
        //        //cell.Background = new SolidColorBrush(Colors.LightGray);
        //        //cell.BorderBrush = new SolidColorBrush(Colors.LightGray);
        //    }
        //}

        #endregion

        #region ** cell content

        public override object GetCellKind(GridControlRange range)
        {
            if (range.Column < 1 && range.Row < 1)
            {
                return typeof(TopLeftCell);
            }
            else if (range.Row < 1)
            {
                return typeof(ColumnHeaderCell);
            }
            else if (range.Column < 1)
            {
                return typeof(RowHeaderCell);
            }
            else if (_storedValues.ContainsKey(new KeyValuePair<int, int>(range.Row, range.Column)))
            {
                return typeof(TextBlock);
            }
            else
            {
                return base.GetCellKind(range);
            }
        }

        public override GridControlCellView CreateCell(GridControlRange range, object cellContentType)
        {
            if (cellContentType as Type == typeof(TopLeftCell))
            {
                return new GridControlCellView { Content = new TopLeftCell() };
            }
            else if (cellContentType as Type == typeof(ColumnHeaderCell))
            {
                return new GridControlCellView { Content = new ColumnHeaderCell() };
            }
            else if (cellContentType as Type == typeof(RowHeaderCell))
            {
                return new GridControlCellView { Content = new RowHeaderCell() };
            }
            else if (cellContentType as Type == typeof(TextBlock))
            {
                return new GridControlCellView { Content = new TextBlock { Margin = CellPadding, VerticalAlignment = VerticalAlignment.Bottom } };
            }
            else
                return base.CreateCell(range, cellContentType);
        }

        public override void BindCell(GridControlRange range, GridControlCellView cell)
        {
            var button = cell.Content as Button;
            if (button != null)
            {
                button.Click += OnTopLeftCellClicked;
            }
            if (cell.Content is TopLeftCell topLeftCell)
            {
            }
            if (cell.Content is ColumnHeaderCell columnHeaderCell)
            {
                columnHeaderCell.Header = GetColumnHeader(range.Column - 1);
            }
            if (cell.Content is RowHeaderCell rowHeaderCell)
            {
                rowHeaderCell.Header = (range.Row).ToString("N0");
            }
            var textBlock = cell.Content as TextBlock;
            if (textBlock != null)
            {
                var value = _storedValues[new KeyValuePair<int, int>(range.Row, range.Column)];
                textBlock.Text = value;
                textBlock.TextAlignment = TextAlignment.Left;
            }
        }

        public override void UnbindCell(GridControlRange range, GridControlCellView cell)
        {
            var button = cell.Content as Button;
            if (button != null)
            {
                button.Click -= OnTopLeftCellClicked;
            }
        }

        private void OnTopLeftCellClicked(object sender, RoutedEventArgs e)
        {
        }

        private string GetColumnHeader(int column)
        {
            var charactersCount = (Z - A + 1);
            var sb = new StringBuilder();
            do
            {
                var c = column % charactersCount;
                sb.Insert(0, (char)(A + c));
                column = (column / charactersCount) - 1;
            }
            while (column >= 0);
            return sb.ToString();
        }

        #endregion

        #region ** edit

        public override bool AllowEditing(GridControlRange range)
        {
            return range.Row >= 1 && range.Column >= 1;
        }

        public override FrameworkElement CreateCellEditor(GridControlRange range)
        {
            string value;
            if (!_storedValues.TryGetValue(new KeyValuePair<int, int>(range.Row, range.Column), out value))
            {
                value = "";
            }
            return new TextBox { BorderThickness = new Thickness(0), Padding = CellPadding, Text = value, VerticalContentAlignment = VerticalAlignment.Bottom };
        }

        public override void OnEditEnded(GridControlRange range, FrameworkElement editor, bool editCancelled)
        {
            if (!editCancelled)
            {
                var value = (editor as TextBox).Text;
                _storedValues[new KeyValuePair<int, int>(range.Row, range.Column)] = value;
            }
        }

        #endregion    
    }
}
