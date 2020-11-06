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

namespace HeaderTooltips
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var p = Product.GetProducts(100);
            _flex.ItemsSource = p;
            dataGrid1.ItemsSource = _flex.CollectionView;

            _flex.CellFactory = new TooltipCellFactory();
        }

        /// <summary>
        /// Class factory that attaches a tooltip to column headers.
        /// </summary>
        class TooltipCellFactory : C1.WPF.FlexGrid.CellFactory
        {
            public override FrameworkElement CreateCell(C1.WPF.FlexGrid.C1FlexGrid grid, C1.WPF.FlexGrid.CellType cellType, C1.WPF.FlexGrid.CellRange range)
            {
                var cell = base.CreateCell(grid, cellType, range);
                if (cellType == C1.WPF.FlexGrid.CellType.ColumnHeader)
                {
                    var tip = string.Format("This is the tooltip for column '{0}'.", 
                        grid.Columns[range.Column].ColumnName);
                    cell.ToolTip = tip;
                }
                return cell;
            }
        }
    }
}
