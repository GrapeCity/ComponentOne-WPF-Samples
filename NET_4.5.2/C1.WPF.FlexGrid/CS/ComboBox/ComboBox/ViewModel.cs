using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.ComponentModel;

namespace ComboBox
{
    class ViewModel
    {
        ListCollectionView _products, _categories;

        public ViewModel()
        {
            // get service to Northwind OData source 

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var url = new Uri("https://services.odata.org/Northwind/Northwind.svc/");
            var svc = new ServiceReference1.NorthwindEntities(url);

            // load product categories
            var lcat = new List<ServiceReference1.Category>();
            foreach (var c in svc.Categories)
            {
                lcat.Add(c);
            }
            _categories = new ListCollectionView(lcat);

            // load products and expand their "Category" property
            var lprod = new List<ServiceReference1.Product>();
            foreach (var p in svc.Products.Expand("Category"))
            {
                lprod.Add(p);
            }
            _products = new ListCollectionView(lprod);
        }
        public ICollectionView Products
        {
            get { return _products; }
        }
        public ICollectionView Categories
        {
            get { return _categories; }
        }
    }
}
