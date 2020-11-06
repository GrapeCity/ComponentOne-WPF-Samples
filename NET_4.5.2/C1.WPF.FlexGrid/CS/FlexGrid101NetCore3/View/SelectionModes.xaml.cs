using FlexGrid101.Resources;
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

namespace FlexGrid101
{
    /// <summary>
    /// Interaction logic for SelectionModes.xaml
    /// </summary>
    public partial class SelectionModes : Page
    {
        public SelectionModes()
        {
            InitializeComponent();

            foreach (var value in Enum.GetValues(typeof(C1.WPF.FlexGrid.SelectionMode)))
            {
                selectionMode.Items.Add(value.ToString());
            }
            selectionMode.SelectedIndex = selectionMode.Items.IndexOf(C1.WPF.FlexGrid.SelectionMode.CellRange.ToString());

            var data = Customer.GetCustomerList(100);
            grid.ItemsSource = data;
            grid.MinColumnWidth = 85;
        }

        private void selectionMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            grid.SelectionMode = (C1.WPF.FlexGrid.SelectionMode)Enum.Parse(typeof(C1.WPF.FlexGrid.SelectionMode), selectionMode.Items[selectionMode.SelectedIndex].ToString());
        }

        private void grid_SelectionChanging(object sender, C1.WPF.FlexGrid.CellRangeEventArgs e)
        {
            // e.Cancel = true;
        }

        private void grid_SelectionChanged(object sender, C1.WPF.FlexGrid.CellRangeEventArgs e)
        {
            if (e.CellRange != null && e.CellRange.Row != -1)
            {
                int rowsSelected = Math.Abs(e.CellRange.Row2 - e.CellRange.Row) + 1;
                int colsSelected = Math.Abs(e.CellRange.Column2 - e.CellRange.Column) + 1;

                lblSelection.Text = (rowsSelected * colsSelected).ToString() + " " + AppResources.CellsSelectedText;
            }
        }
    }
}
