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
using C1.WPF.FlexGrid;

namespace CustomCellFactory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // bind olap page to data
            _c1OlapPage.Loaded += (s, ea) =>
            {
                _c1OlapPage.DataSource = Product.GetProducts(500);

                // show product prices by color and line
                var olap = _c1OlapPage.OlapPanel.OlapEngine;
                olap.BeginUpdate();
                olap.RowFields.Add("Color");
                olap.ColumnFields.Add("Line");
                var value = olap.Fields["Price"];
                olap.ValueFields.Add(value);
                value.Format = "n0";
                value.Subtotal = C1.Olap.Subtotal.Average;
                olap.EndUpdate();

                // apply conditional formatting to grid cells
                _c1OlapPage.OlapGrid.CellFactory = new ConditionalCellFactory();
            };
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            // refresh olap output
            _c1OlapPage.OlapPanel.OlapEngine.Update();
        }
    }

    /// <summary>
    /// Custom cell factory class for the C1FlexGrid. This class applies a custom
    /// green background to cells with values over 500.
    /// </summary>
    public class ConditionalCellFactory : CellFactory
    {
        public override FrameworkElement CreateCell(C1FlexGrid grid, CellType cellType, CellRange range)
        {
            // let base class to most of the work
            var cell = base.CreateCell(grid, cellType, range);

            // keep the selectionbackground
            if (grid.GetSelectedState(range) != SelectedState.None)
                return cell;

            // apply green background if necessary
            if (cellType == CellType.Cell)
            {
                var cellValue = grid[range.Row, range.Column];
                if (cellValue is double && (double)cellValue > 500)
                {
                    var border = cell as Border;
                    border.Background = _greenBrush;
                }
            }

            // done
            return cell;
        }
        static Brush _greenBrush = new SolidColorBrush(Color.FromArgb(0xff, 150, 255, 150));
    }
}
