using System;
using System.Collections.Generic;
using System.Windows.Controls;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    public partial class Validation : UserControl
    {
        public Validation()
        {
            InitializeComponent();

            DataContext = GetProducts();
        }

        private static IEnumerable<Product> GetProducts()
        {
            var products = new List<Product>(Data.GetProducts((product) => !string.IsNullOrEmpty(product.Element("ProductModelID").Value)));

            //Data modified to show invalid items
            products[0].Available = true;
            products[0].ExpirationDate = DateTime.Today - TimeSpan.FromDays(1);
            products[4].Available = true;
            products[4].ExpirationDate = DateTime.Today - TimeSpan.FromDays(5);
            products[20].Available = true;
            products[20].ExpirationDate = DateTime.Today - TimeSpan.FromDays(6);
            return products;
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
    }
}
