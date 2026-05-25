using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Input;
using System.Data.Services.Client;
using System.Windows.Data;
using System.Windows;

namespace DataViewDashboard.ViewModels
{
    public class ProductsViewModel : DataViewModelBase
    {
        public ProductsViewModel(AdventureWorksService.AdventureWorksEntities client)
            : base(client.Products.Expand(
            "ProductModel/ProductModelProductDescriptions/ProductModel," +
            "ProductModel/ProductModelProductDescriptions/ProductDescription," +
            "ProductCategory," +
            "SalesOrderDetails"))
        {
            base.DisplayName = "Products";
        }
        protected override void OnAsyncQueryCompleted(IAsyncResult result)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                var products = new List<AdventureWorksService.Product>();
                foreach (var item in _query.EndExecute(result))
                {
                    products.Add((AdventureWorksService.Product)item);
                }
                ProxyServiceQuery.AddData(_query.ToString(), products);
                OnCompleteSource(products);
            });
        }
    }

}
