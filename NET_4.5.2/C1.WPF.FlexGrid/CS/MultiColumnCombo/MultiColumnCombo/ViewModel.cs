using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Data;
using MultiColumnCombo.NorthwindServiceReference;
using C1.WPF.FlexGrid;

namespace MultiColumnCombo
{
    /// <summary>
    /// Provides NorthWind data loaded from the standard NorthWind OData provider.
    /// </summary>
    class ViewModel
    {
        // ** fields
        static NorthwindEntities _ctx;
        CollectionViewSource _categories, _products, _suppliers;

        // ** ctor
        public ViewModel()
        {
            if (_ctx == null)
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var root = new Uri("https://services.odata.org/Northwind/Northwind.svc/", UriKind.Absolute);
                _ctx = new NorthwindEntities(root);
            }

            _categories = new CollectionViewSource();
            _categories.Source = _ctx.Categories;

            _products = new CollectionViewSource();
            _products.Source = _ctx.Products;

            _suppliers = new CollectionViewSource();
            _suppliers.Source = _ctx.Suppliers;
        }

        // ** object model
        public ICollectionView Categories
        {
            get { return _categories.View; }
        }
        public ICollectionView Products
        {
            get { return _products.View; }
        }
        public ICollectionView Suppliers
        {
            get { return _suppliers.View; }
        }
    }
    /// <summary>
    /// ColumnConverter for product category column.
    /// </summary>
    public class CategoryConverter : ColumnValueConverter
    {
        public CategoryConverter()
        {
            var vm = new ViewModel();
            base.SetSource(vm.Categories, "CategoryID", "CategoryName");
        }
    }
    /// <summary>
    /// ColumnConverter for supplier column.
    /// </summary>
    public class SupplierConverter : ColumnValueConverter
    {
        public SupplierConverter()
        {
            var vm = new ViewModel();
            base.SetSource(vm.Suppliers, "SupplierID", "CompanyName");
        }
    }
}
