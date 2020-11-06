using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FlexGridExplorer
{
    public partial class ConditionalFormatting : UserControl
    {
        public ConditionalFormatting()
        {
            InitializeComponent();
            Tag = AppResources.ConditionalFormattingDescription;
            var data = Customer.GetCustomerList(100);
            grid.ItemsSource = data;
            grid.Columns[4].DataMap = new GridDataMap() { ItemsSource = Customer.GetCountries(), DisplayMemberPath = "Value", SelectedValuePath = "Key" };
            grid.CellFactory = new MyCellFactory();
            grid.MinColumnWidth = 85;
        }
    }

    public class MyCellFactory : GridCellFactory
    {
        public override void PrepareCell(GridCellType cellType, GridCellRange range, GridCellView cell)
        {
            base.PrepareCell(cellType, range, cell);
            var orderCountColumn = Grid.Columns["OrderCount"];
            if (cellType == GridCellType.Cell && range.Column == orderCountColumn.Index)
            {
                var cellValue = Grid[range.Row, range.Column] as int?;
                if (cellValue.HasValue)
                {
                    cell.Background = new SolidColorBrush(cellValue < 50.0 ? Color.FromRgb(0xFF, 0x70, 0x70) : Color.FromRgb(0x8E, 0xE9, 0x8E));
                }
            }
        }

        /// <summary>
        /// Binds the content of the cell.
        /// </summary>
        /// <param name="cellType">Type of the cell.</param>
        /// <param name="range">The range.</param>
        /// <param name="cellContent">Content of the cell.</param>
        public override void BindCellContent(GridCellType cellType, GridCellRange range, FrameworkElement cellContent)
        {
            base.BindCellContent(cellType, range, cellContent);
            var orderTotalColumn = Grid.Columns["OrderTotal"];
            var orderCountColumn = Grid.Columns["OrderCount"];
            if (cellType == GridCellType.Cell && range.Column == orderTotalColumn.Index)
            {
                var label = cellContent as TextBlock;
                if (label != null)
                {
                    var cellValue = Grid[range.Row, range.Column] as double?;
                    if (cellValue.HasValue)
                    {
                        label.Foreground = new SolidColorBrush(cellValue < 5000.0 ? Colors.Red : Colors.Green);
                    }
                }
            }
            if (cellType == GridCellType.Cell && range.Column == orderCountColumn.Index)
            {
                var label = cellContent as TextBlock;
                if (label != null)
                {
                    var cellValue = Grid[range.Row, range.Column] as int?;
                    if (cellValue.HasValue)
                    {
                        label.Foreground = new SolidColorBrush(Colors.Black);
                    }
                }
            }
        }

        public override void UnbindCellContent(GridCellType cellType, GridCellRange range, FrameworkElement cellContent)
        {
            base.UnbindCellContent(cellType, range, cellContent);
            var label = cellContent as TextBlock;
            if (label != null)
            {
                label.Foreground = null;
                label.Background = null;
            }
        }
    }
}
