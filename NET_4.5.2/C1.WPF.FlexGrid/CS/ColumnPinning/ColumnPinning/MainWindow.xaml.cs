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

namespace ColumnPinning
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var data = Customer.GetCustomerList(100);
            grid.ItemsSource = data;
            // Setting AllowFreezing to None will prevent the availability of dragging frozen columns/rows.
            grid.AllowFreezing = C1.WPF.FlexGrid.AllowFreezing.None;
            // Disable the Sorting option to prevent sorting icon disrupting the pinning icon.
            grid.AllowSorting = false;
            grid.Columns["Country"].AllowMerging = true;
            grid.MinColumnWidth = 85;
            this.Loaded += GridColumnPinning_Loaded;
        }

        // Override the CellFactory of the control with custom-defined CellFactory.
        private void GridColumnPinning_Loaded(object sender, RoutedEventArgs e)
        {
            grid.CellFactory = new ColumnPinningCellFactory();
        }
    }
}
