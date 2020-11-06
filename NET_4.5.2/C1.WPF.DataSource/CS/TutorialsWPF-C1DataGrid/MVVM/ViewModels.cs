using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TutorialsWPF
{
    // This file contains View Models used in the CategoryProductsView.

    // The properties of the following two view models 
    // must be virtual to make live view items modifiable.
    public class CategoryViewModel
    {
        public virtual int CategoryID { get; set; }
        public virtual string CategoryName { get; set; }
    }

    public class ProductViewModel
    {
        public virtual int ProductID { get; set; }
        public virtual string ProductName { get; set; }
        public virtual int? CategoryID { get; set; }
        public virtual string CategoryName { get; set; }
        public virtual int? SupplierID { get; set; }
        public virtual string SupplierName { get; set; }
        public virtual decimal? UnitPrice { get; set; }
        public virtual string QuantityPerUnit { get; set; }
        public virtual short? UnitsInStock { get; set; }
        public virtual short? UnitsOnOrder { get; set; }
    }

    public class CategoryProductsViewModel
    {
        // This client scope will be used to access the data.
        private C1.Data.Entities.EntityClientScope _scope;

        // Data grids in this view are bound to these data sources.
        public System.ComponentModel.ICollectionView Categories { get; private set; }
        public System.ComponentModel.ICollectionView Products { get; private set; }

        public CategoryProductsViewModel()
        {
            if (App.ClientCache == null)
                return;

            _scope = App.ClientCache.CreateScope();

            // These live views will be used by the CategoryProductsView
            // to display categories and products.
            Categories =
                from c in _scope.GetItems<Category>()
                select new CategoryViewModel()
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.CategoryName
                };

            // Products are filtered by CategoryID on the server.
            // Filtering is performed automatically when the current Category changes.
            // The product suppliers are fetched along with the products.
            Products =
                from p in _scope.GetItems<Product>().AsFilteredBound(p => p.CategoryID)
                    .BindFilterKey(Categories, "CurrentItem.CategoryID").Include("Supplier")
                select new ProductViewModel()
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    CategoryID = p.CategoryID,
                    CategoryName = p.Category.CategoryName,
                    SupplierID = p.SupplierID,
                    SupplierName = p.Supplier.CompanyName,
                    UnitPrice = p.UnitPrice,
                    QuantityPerUnit = p.QuantityPerUnit,
                    UnitsInStock = p.UnitsInStock,
                    UnitsOnOrder = p.UnitsOnOrder
                };
        }
    }
}
