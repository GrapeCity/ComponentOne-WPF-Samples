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
        static SolidColorBrush RedBackground = new SolidColorBrush(Color.FromArgb(0xFF, 0xC4, 0x2B, 0x1C));
        static SolidColorBrush RedForeground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFD, 0xE7, 0xE9));
        static SolidColorBrush GreenBackground = new SolidColorBrush(Color.FromArgb(0xFF, 0x0F, 0x7B, 0x0F));
        static SolidColorBrush GreenForeground = new SolidColorBrush(Color.FromArgb(0xFF, 0xDF, 0xF6, 0xDD));

        public override void PrepareCell(GridCellType cellType, GridCellRange range, GridCellView cell, Thickness internalBorders)
        {
            base.PrepareCell(cellType, range, cell, internalBorders);
            var orderCountColumn = Grid.Columns["OrderCount"];
            if (cellType == GridCellType.Cell && range.Column == orderCountColumn.Index)
            {
                var cellValue = Grid[range.Row, range.Column] as int?;
                if (cellValue.HasValue)
                {
                    cell.Background = cellValue < 50.0 ? RedForeground : GreenForeground;
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
            if (cellType == GridCellType.Cell && range.Column == orderTotalColumn.Index)
            {
                var label = cellContent as TextBlock;
                if (label != null)
                {
                    var cellValue = Grid[range.Row, range.Column] as double?;
                    if (cellValue.HasValue)
                    {
                        label.Foreground = cellValue < 5000.0 ? RedBackground : GreenBackground;
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
                label.ClearValue(Label.ForegroundProperty);
            }
        }
    }
}
