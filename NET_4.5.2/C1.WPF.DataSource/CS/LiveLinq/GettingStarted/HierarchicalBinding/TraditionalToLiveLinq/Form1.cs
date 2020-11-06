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


namespace Traditional
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // generated automatically
            this.productsTableAdapter.Fill(this.nORTHWNDDataSet.Products);
            this.categoriesTableAdapter.Fill(this.nORTHWNDDataSet.Categories);

            // Create a live view for Categories.
            // Each category contains a list with the products of that category. 
            var categoryView =
                (from c in nORTHWNDDataSet.Categories.AsLive()
                 join p in nORTHWNDDataSet.Products.AsLive()
                     on c.CategoryID equals p.CategoryID into g
                 select new
                 {
                     c.CategoryID,
                     c.CategoryName,
                     FK_Products_Categories = g
                 }).AsDynamic();

            // replace DataSource on the form to use our LiveLinq Query
            this.bindingSource1.DataSource = categoryView;
        }
    }
}
