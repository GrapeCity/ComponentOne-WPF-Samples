using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using C1.LiveLinq;
using C1.LiveLinq.AdoNet;
using C1.LiveLinq.LiveViews;

namespace DeclarativeProgramming
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // get the data
            NORTHWNDDataSet ds = GetData();

            // create a live views to update order details
            this.dataGridView1.DataSource = GetOrderDetails(ds);

            // create live views that provide up-to-date order information
            this.dataGridView2.DataSource = GetSalesByCategory(ds);
            this.dataGridView3.DataSource = GetSalesByProduct(ds);
        }

        NORTHWNDDataSet GetData()
        {
            var ds = new NORTHWNDDataSet();
            new NORTHWNDDataSetTableAdapters.ProductsTableAdapter()
              .Fill(ds.Products);
            new NORTHWNDDataSetTableAdapters.Order_DetailsTableAdapter()
              .Fill(ds.Order_Details);
            new NORTHWNDDataSetTableAdapters.CategoriesTableAdapter()
              .Fill(ds.Categories);
            return ds;
        }

        object GetSalesByCategory(NORTHWNDDataSet ds)
        {
            var products = ds.Products;
            var details = ds.Order_Details;
            var categories = ds.Categories;

            var salesByCategory =
              (from p in products.AsLive()
               join c in categories.AsLive()
                 on p.CategoryID equals c.CategoryID
               join d in details.AsLive()
                 on p.ProductID equals d.ProductID

               let detail = new
               {
                   CategoryName = c.CategoryName,
                   SaleAmount = d.UnitPrice * d.Quantity
                     * (decimal)(1f - d.Discount)
               }

               group detail
                 by detail.CategoryName into categorySales

               let total = categorySales.Sum(x => x.SaleAmount)
               orderby total descending
               select new
               {
                   CategoryName = categorySales.Key,
                   TotalSales = total
               }).AsDynamic();

            return salesByCategory;
        }

        public class ProductInfo
        {
            public string ProductName;
            public string CategoryName;
            public decimal TotalSales;    
        }

        object GetSalesByProduct(NORTHWNDDataSet ds)
        {
            var products = ds.Products;
            var details = ds.Order_Details;
            var categories = ds.Categories;

            var salesByProduct =
              from p in products.AsLive()
              join c in categories.AsLive()
                on p.CategoryID equals c.CategoryID
              join d in details.AsLive()
                on p.ProductID equals d.ProductID
              into sales
              let productSales = new ProductInfo()
              {
                  ProductName = p.ProductName,
                  CategoryName = c.CategoryName,
                  TotalSales = sales.Sum(
                     d => d.UnitPrice * d.Quantity *
                          (decimal)(1f - d.Discount))
              }
              orderby productSales.TotalSales descending
              select productSales;

            return salesByProduct;
        }

        object GetOrderDetails(NORTHWNDDataSet ds)
        {
            var products = ds.Products;
            var details = ds.Order_Details;
            var categories = ds.Categories;

            var orderDetails =
                (from d in details.AsLive().AsUpdatable()
                 join p in products.AsLive()
                     on d.ProductID equals p.ProductID
                 join c in categories.AsLive()
                     on p.CategoryID equals c.CategoryID
                 select new
                 {
                     c.CategoryName,
                     p.ProductName,
                     d.UnitPrice,
                     d.Quantity,
                     d.Discount
                 }).AsDynamic();

            return orderDetails;
        }

    }
}
