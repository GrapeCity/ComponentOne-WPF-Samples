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

namespace OData
{
    using OData.ServiceReference1;
    using System.Data.Services.Client;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Uri _svcRoot = new Uri("https://services.odata.org/Northwind/Northwind.svc/");
        NorthwindEntities _context;
        public MainWindow()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            _context = new NorthwindEntities(_svcRoot);
            InitializeComponent();
           _flexCategories.ItemsSource = _context.Categories;
           _lblCategories.Text = "Categories (Ready)";
           _flexCategories.SelectionChanged += _flexCategories_SelectionChanged;
           _flexProducts.ItemsSource = GetProducts(_context.Categories.FirstOrDefault());
        }

        void _flexCategories_SelectionChanged(object sender, C1.WPF.FlexGrid.CellRangeEventArgs e)
        {
             // get selected category
            var category = _flexCategories.SelectedItem;
            _flexProducts.ItemsSource = GetProducts(_flexCategories.SelectedItem as Category);
           
        }
        /// <summary>
        /// Returns product per category
        /// </summary>
        /// <param name="ctgry"></param>
        /// <returns></returns>
        IOrderedQueryable<Product> GetProducts(Category ctgry)
        {
            if (ctgry != null)
            {
                var products = from p in _context.Products
                               where p.CategoryID == ctgry.CategoryID
                               orderby p.ProductName
                               select p;
                return products;
            }
            return null;
        }
    }
}
