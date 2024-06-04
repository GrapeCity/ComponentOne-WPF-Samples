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
using C1.WPF;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    /// <summary>
    /// Interaction logic for AddRemoveRowsPage.xaml
    /// </summary>
    public partial class AddRemoveRows : UserControl
    {
        public AddRemoveRows()
        {
            InitializeComponent();

            grid.ItemsSource = Data.GetProducts((product) => product.Element("Image") != null && product.Element("Image").Value != "no_image_available_small.jpg");

            if (grid.NewRowVisibility == DataGridNewRowVisibility.Top)
            {
                rbnTop.IsChecked = true;
            }
            else
            {
                rbnBottom.IsChecked = true;
            }
            rbnTop.Checked += (s, e) =>
            {
                UpdateNewRowVisibility();
            };
            rbnBottom.Checked += (s, e) =>
            {
                UpdateNewRowVisibility();
            };
            UpdateNewRowVisibility();
        }

        private void UpdateNewRowVisibility()
        {
            if (rbnTop.IsChecked.Value)
            {
                grid.NewRowVisibility = DataGridNewRowVisibility.Top;
                grid.FrozenTopRowsCount = 1;
                grid.FrozenBottomRowsCount = 0;
            }
            else
            {
                grid.NewRowVisibility = DataGridNewRowVisibility.Bottom;
                grid.FrozenTopRowsCount = 0;
                grid.FrozenBottomRowsCount = 1;
            }
        }

        private void grid_AutoGeneratingColumn(object sender, C1.WPF.DataGrid.DataGridAutoGeneratingColumnEventArgs e)
        {
            Common.HandleColumnAutoGeneration(e);

            // avoid image columns for add/remove samples
            if (e.Column is DataGridImageColumn)
            {
                e.Cancel = true;
            }
        }

        private void grid_BeginningNewRow(object sender, DataGridBeginningNewRowEventArgs e)
        {
            Product product = (e.Item as Product);
            product.ExpirationDate = DateTime.Now.AddDays(7);
            product.Available = true;
            product.ImageUrl = "no_image_available_small.jpg";
        }

        private void grid_CommittingNewRow(object sender, DataGridEndingNewRowEventArgs e)
        {
            txtMsg.Text = "";
            Product product = e.NewRow.DataItem as Product;
            if (string.IsNullOrEmpty(product.ProductNumber))
            {
                e.Cancel = true;
                txtMsg.Foreground = new SolidColorBrush(Colors.Red);
                txtMsg.Text = "You must enter the product number.";
            }
        }

        private void grid_CancelingNewRow(object sender, DataGridEndingNewRowEventArgs e)
        {
            txtMsg.Text = "";
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            C1.WPF.DataGrid.DataGridCell cell = grid.GetCellFromFrameworkElement(sender as Button);
            grid.RemoveRow(cell.Row.Index);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            grid.BeginNewRow();
        }

        private void grid_RowsAdded(object sender, DataGridRowsAddedEventArgs e)
        {
            txtMsg.Foreground = new SolidColorBrush(Colors.Black);
            txtMsg.Text = string.Format("Product {0} Added", (e.AddedRows[0].DataItem as Product).ProductNumber);
        }

        private void grid_RowsDeleted(object sender, DataGridRowsDeletedEventArgs e)
        {
            txtMsg.Foreground = new SolidColorBrush(Colors.Black);
            string[] products = e.DeletedRows.Select(row => (row.DataItem as Product).ProductNumber).ToArray();
            txtMsg.Text = string.Format("Products ( {0} ) Deleted", string.Join(", ", products));
        }

        bool deletingAccepted = false;
        private void grid_DeletingRows(object sender, DataGridDeletingRowsEventArgs e)
        {
            if (!deletingAccepted)
            {
                e.Cancel = true;
                if (MessageBox.Show(string.Format("Do you want to delete the following rows {0} ?", string.Join(",", e.DeletedRows.Select(row => (row.DataItem as Product).ProductNumber).ToArray())), "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    deletingAccepted = true;
                    grid.RemoveRows(e.DeletedRows);
                    deletingAccepted = false;
                };
            }
        }

    }
}
