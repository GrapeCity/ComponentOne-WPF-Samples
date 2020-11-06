using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.WPF;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    /// <summary>
    /// Interaction logic for Hierarchical.xaml
    /// </summary>
    public partial class Hierarchical : System.Windows.Controls.UserControl
    {
        public Hierarchical()
        {
            InitializeComponent();

            gridCategories.ItemsSource = Data.GetSubCategories(null).Take(10);
        }

        private void gridCategories_LoadedRowDetailsPresenter(object sender, DataGridRowDetailsEventArgs e)
        {
            if (e.Row.DetailsVisibility == Visibility.Visible)
            {
                var detailGrid = e.DetailsElement as C1DataGrid;

                if (detailGrid != null)
                {
                    detailGrid.Tag = e.Row;//Save the parent row to be used in the navigation.

                    if (detailGrid.ItemsSource == null)
                    {
                        int subcategory = (e.Row.DataItem as Subcategory).ProductSubcategoryID;
                        detailGrid.ItemsSource = Data.GetProducts((product) => product.Element("ProductSubcategoryID") != null && !string.IsNullOrWhiteSpace(product.Element("ProductSubcategoryID").Value) && int.Parse(product.Element("ProductSubcategoryID").Value) == subcategory).Take(10);
                    }
                }
            }
        }

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            var dataGrid = (C1DataGrid)sender;
            var defaultNavigation = new DataGridHierarchicalNavigation(dataGrid);
            defaultNavigation.HandleKeyDown(e);
        }

        private static void SelectionChanged(C1DataGrid dataGrid)
        {
            var parentRow = dataGrid.Tag as DataGridRow;
            if (parentRow != null)
            {
                parentRow.DataGrid.Selection.Clear();
            }
        }

        private void grid_CurrentCellChanged(object sender, DataGridCellEventArgs e)
        {
            if (e.Cell != null)
            {
                RemoveCurrentCell(gridCategories, e);
            }
        }

        private void RemoveCurrentCell(C1DataGrid dataGrid, DataGridCellEventArgs e)
        {
            if (dataGrid != e.Cell.DataGrid)
            {
                dataGrid.CurrentRow = null;
            }
            foreach (var row in dataGrid.Rows.GetLoadedRows())
            {
                if (row.Type == DataGridRowType.Item ||
                    row.Type == DataGridRowType.New)
                {
                    if (row.DetailsPresenter != null)
                    {
                        var childDataGrid = row.DetailsPresenter.Content as C1DataGrid;
                        if (childDataGrid != null)
                        {
                            RemoveCurrentCell(childDataGrid, e);
                        }
                    }
                }
            }
        }

        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            if (((C1DataGrid)sender).Selection.SelectedRows.Count > 0)
            {
                RemoveSelection(gridCategories, (C1DataGrid)sender);
            }
        }

        private void RemoveSelection(C1DataGrid dataGrid, C1DataGrid changedDataGrid)
        {
            if (dataGrid != changedDataGrid)
            {
                dataGrid.Selection.Clear();
            }
            foreach (var row in dataGrid.Rows.GetLoadedRows())
            {
                if (row.Type == DataGridRowType.Item ||
                    row.Type == DataGridRowType.New)
                {
                    if (row.DetailsPresenter != null)
                    {
                        var childDataGrid = row.DetailsPresenter.Content as C1DataGrid;
                        if (childDataGrid != null)
                        {
                            RemoveSelection(childDataGrid, changedDataGrid);
                        }
                    }
                }
            }
        }

        private void gridProducts_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Property.Name == "ImageUrl")
            {
                e.Cancel = true;
            }
            Common.HandleColumnAutoGeneration(e);
        }
    }
}
