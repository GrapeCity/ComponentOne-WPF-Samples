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
    using C1.LiveLinq;
    using C1.LiveLinq.LiveViews;

    public partial class ClientSideQuerying : Window
    {
        // This client scope will be used to load the data.
        private EntityClientScope _scope = App.ClientCache.CreateScope();
        private ClientView<Product> _seafoodProductsView;
        private View<dynamic> _viewProducts;

        public ClientSideQuerying()
        {
            InitializeComponent();

            // Define and bind a live view of expensive non-discontinued products ordered by price.
            _viewProducts =
                (from p in _scope.GetItems<Product>()
                 where !p.Discontinued && p.UnitPrice >= 30
                 orderby p.UnitPrice
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

            dataGrid1.ItemsSource = _viewProducts;

            // Define a view of seafood products. Filtering is performed on the server.
            _seafoodProductsView = _scope.GetItems<Product>().AsFiltered(p => p.CategoryID == 8);

            // Bind the label text to the number of products in the view
            textBlockCount.SetBinding(TextBlock.TextProperty, new Binding("Value") { Source = _viewProducts.LiveCount() });
        }

        private void addItem(object sender, RoutedEventArgs e)
        {
            // add a new product
            object newItem = ((System.ComponentModel.IEditableCollectionView)dataGrid1.ItemsSource).AddNew();
            dataGrid1.ScrollIntoView(newItem);
        }

        private void raiseSeafood(object sender, RoutedEventArgs e)
        {
            // Increase the price of the seafood products.
            // The data grid contents will be updated automatically.
            foreach (var p in _seafoodProductsView)
                p.UnitPrice *= 1.2m;
        }

        private void cutSeafood(object sender, RoutedEventArgs e)
        {
            // Decrease the price of the seafood products.
            // The data grid contents will be updated automatically.
            foreach (var p in _seafoodProductsView)
                p.UnitPrice /= 1.2m;
        }
    }
}
