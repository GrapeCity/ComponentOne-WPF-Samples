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
    using C1.Data.Entities;
    using C1.Data;

    public partial class DataSourcesInCode : Window
    {
        // This client scope will be used to load the data.
        private EntityClientScope _scope = App.ClientCache.CreateScope();

        public DataSourcesInCode()
        {
            InitializeComponent();

            // Bind the category combo box to the category view
            // and show the products of the first category in the data grid.

            ClientView<Category> viewCategories = _scope.GetItems<Category>();

            comboBox1.DisplayMemberPath = "CategoryName";
            comboBox1.ItemsSource = viewCategories;

            BindGrid(viewCategories.First().CategoryID);
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Rebind the data grid, display the products of the selected category.
            if (comboBox1.SelectedValue != null)
                BindGrid(((Category)comboBox1.SelectedValue).CategoryID);
        }

        private void BindGrid(int categoryID)
        {
            // Filter products by CategoryID on the server
            // and bind them to the data grid.
            dataGrid1.ItemsSource =
                (from p in _scope.GetItems<Product>().AsFiltered(p => p.CategoryID == categoryID)
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
                 }).AsDynamic(); // AsDynamic() is required for data binding because an anonymous class is used (select new...)
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            App.ClientCache.SaveChanges();
        }
    }
}
