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
using System.Windows.Shapes;

namespace TutorialsWPF
{
    public partial class CustomColumns : Window
    {
        public CustomColumns()
        {
            InitializeComponent();

            // Define a live view with custom fields.
            // The data grid will automatically generate columns based on these fields.
            dataGrid1.ItemsSource =
                (from p in c1DataSource1["Products"].AsLive<Product>()
                 select new
                 {
                     ProductID = p.ProductID,
                     ProductName = p.ProductName,
                     CategoryID = p.CategoryID,
                     CategoryName = p.Category.CategoryName,
                     SupplierID = p.SupplierID,
                     Supplier = p.Supplier.CompanyName,
                     UnitPrice = p.UnitPrice,
                     QuantityPerUnit = p.QuantityPerUnit,
                     UnitsInStock = p.UnitsInStock,
                     UnitsOnOrder = p.UnitsOnOrder
                 }).AsDynamic(); // AsDynamic() is required for data binding because an anonymous class is used (select new ...)
        }

        private void addItem(object sender, RoutedEventArgs e)
        {
            // add a new product
            object newItem = ((System.ComponentModel.IEditableCollectionView)dataGrid1.ItemsSource).AddNew();
            dataGrid1.ScrollIntoView(dataGrid1.Rows.Count - 1, 0);
        }
    }
}
