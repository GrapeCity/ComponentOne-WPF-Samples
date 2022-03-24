using C1.WPF.Grid;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GridShowCase
{
    public class GridColorColumn : GridColumn
    {
        public double ColorRectangleSize { get; set; } = 20;

        protected override object GetCellContentType(GridCellType cellType, GridRow row)
        {
            if (cellType != GridCellType.Cell)
            {
                return base.GetCellContentType(cellType, row);
            }
            return typeof(GridColorColumn);
        }

        protected override FrameworkElement CreateCellContent(GridCellType cellType, object cellContentType, GridRow row)
        {
            if (cellType != GridCellType.Cell)
            {
                return base.CreateCellContent(cellType, cellContentType, row);
            }
            var grid = new Grid();
            grid.Margin = Grid.CellPadding;
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            var bitmapImage = new Rectangle() { Width = ColorRectangleSize, Height = ColorRectangleSize, Margin = new Thickness(0, 0, 5, 0), VerticalAlignment = VerticalAlignment.Center };
            var textBlock = new TextBlock() { VerticalAlignment = VerticalAlignment.Center };
            System.Windows.Controls.Grid.SetColumn(textBlock, 1);
            grid.Children.Add(bitmapImage);
            grid.Children.Add(textBlock);
            return grid;
        }

        protected override void BindCellContent(FrameworkElement cellContent, GridCellType cellType, GridRow row)
        {
            if (cellType != GridCellType.Cell)
            {
                base.BindCellContent(cellContent, cellType, row);
                return;
            }
            var grid = cellContent as Grid;
            var colorRectangle = grid.Children[0] as Rectangle;
            var textBlock = grid.Children[1] as TextBlock;
            var colorString = Grid.GetCellValue(cellType, row, this) as string;
            if (string.IsNullOrEmpty(colorString))
            {
                colorRectangle.Fill = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                colorRectangle.Fill = new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString));
            }
            textBlock.Text = Grid.GetCellText(cellType, row, this);
        }
    }
}
