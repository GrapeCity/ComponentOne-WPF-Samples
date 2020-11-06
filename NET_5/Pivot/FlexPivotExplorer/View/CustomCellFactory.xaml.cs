using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.WPF.Grid;

namespace FlexPivotExplorer
{
    /// <summary>
    /// Interaction logic for CustomCellFactory.xaml
    /// </summary>
    public partial class CustomCellFactory : UserControl
    {
        bool isLoaded = false;
        public CustomCellFactory()
        {
            InitializeComponent();
            Tag = "Shows how to apply conditional formatting to an FlexPivotGrid control using the FlexGrid's CustomCellFactory feature.\r" +
                "The FlexPivotGrid derives from the FlexGrid control, so you can use the standard CustomCellFactory mechanism to apply styles to cells based on their contents(or to draw the entire cell if you prefer).\r" +
                "This sample shows a grid where values greater than 500 appear with a light green background";
            // bind olap page to data
            pivotPage.Loaded += (s, ea) =>
            {
                if (!pivotPage.IsVisible)
                    return;
                if (isLoaded)
                    return;
                isLoaded = true;

                pivotPage.DataSource = Product.GetProducts(500);

                // show product prices by color and line
                var fpEngine = pivotPage.FlexPivotPanel.FlexPivotEngine;
                fpEngine.BeginUpdate();
                fpEngine.RowFields.Add("Color");
                fpEngine.ColumnFields.Add("Line");
                var value = fpEngine.Fields["Price"];
                fpEngine.ValueFields.Add(value);
                value.Format = "n0";
                value.Subtotal = C1.FlexPivot.Subtotal.Average;
                fpEngine.EndUpdate();

                // apply conditional formatting to grid cells
                pivotPage.FlexPivotGrid.CellFactory = new ConditionalCellFactory();
            };
        }
    }

    /// <summary>
    /// Custom cell factory class for the FlexGrid. This class applies a custom
    /// green background to cells with values over 500.
    /// </summary>
    public class ConditionalCellFactory : GridCellFactory
    {
        public override void PrepareCell(GridCellType cellType, GridCellRange range, GridCellView cell)
        {
            base.PrepareCell(cellType, range, cell);

            // apply green background if necessary
            if (cellType == GridCellType.Cell)
            {
                var cellValue = Grid[range.Row, range.Column];
                if (cellValue is double && (double)cellValue > 500)
                {
                    cell.Background = _greenBrush;
                }
            }
        }

        static Brush _greenBrush = new SolidColorBrush(Color.FromArgb(0xff, 150, 255, 150));
    }
}
