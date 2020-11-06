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

namespace LiveLinq
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // get the data
            var ds = GetData();

            // create a live view with Categories and Products:
            var liveView =
                (from c in ds.Categories.AsLive()
                 join p in ds.Products.AsLive()
                     on c.CategoryID equals p.CategoryID into g
                 select new
                 {
                     c.CategoryID,
                     c.CategoryName,
                     Products = g
                 }).AsDynamic();

            // bind view to controls
            DataBind(liveView);
        }

        NORTHWNDDataSet GetData()
        {
            NORTHWNDDataSet ds = new NORTHWNDDataSet();
            new NORTHWNDDataSetTableAdapters.ProductsTableAdapter()
              .Fill(ds.Products);
            new NORTHWNDDataSetTableAdapters.CategoriesTableAdapter()
              .Fill(ds.Categories);
            return ds;
        }

        void DataBind(object dataSource)
        {
            // bind ComboBox
            comboBox1.DataSource = dataSource;
            comboBox1.DisplayMember = "CategoryName";
            comboBox1.ValueMember = "CategoryID";

            // bind DataGridView
            dataGridView1.DataMember = "Products";
            dataGridView1.DataSource = dataSource;

            // bind TextBox controls
            BindTextBox(textBox1, dataSource, "ProductName");
            BindTextBox(textBox2, dataSource, "UnitPrice");
            BindTextBox(textBox3, dataSource, "QuantityPerUnit");
            BindTextBox(textBox4, dataSource, "UnitsInStock");
        }

        void BindTextBox(TextBox txt, object dataSource, string dataMember)
        {
            var b = new Binding("Text", dataSource, "Products." + dataMember);
            txt.DataBindings.Add(b);
        }
    
    }
}
