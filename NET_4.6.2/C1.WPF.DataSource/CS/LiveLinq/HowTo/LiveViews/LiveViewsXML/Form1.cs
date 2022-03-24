using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using C1.LiveLinq;
using C1.LiveLinq.LiveViews;
using C1.LiveLinq.LiveViews.Xml;

namespace LiveViewsXML
{
    public partial class Form1 : Form
    {
        public class CustomerOrder
        {
            public virtual string CustomerID { get; set; }
            public virtual string City { get; set; }
            public virtual int OrderID { get; set; }
            public virtual string ShipCity { get; set; }
        }

        View<XDocument> documentView;
        View<XElement> customers;
        View<XElement> orders;
        View<CustomerOrder> customerOrderView;

        public Form1()
        {
            InitializeComponent();


            // Fill the document with data
            using (var xmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LiveViewsXML.Northwind.xml"))
            {
                System.Diagnostics.Debug.Assert(xmlStream != null);
                XmlReaderSettings settings = new XmlReaderSettings { IgnoreWhitespace = true };
                XmlReader reader = XmlReader.Create(xmlStream, settings);
                documentView = XDocument.Load(reader).AsLive();
            }

            // Create base views (like base tables in a database or data tables in a DataSet)
            customers = documentView.Descendants("Customers");
            orders = documentView.Descendants("Orders").AsLive().AsUpdatable(); // AsUpdatable() makes it possible for 
            // the user to change data directly in the grid showing the join view below.
            // Order fields can be changed by the user there, Customer fields can't be changed by the user
            // Specifying this argument is needed only if you need to change data directly in the view instead
            // of changing the base data, and then only for query operators with more than one argument such as Join.

            // Creating indexes. It is optional, only for performance optimization.
            // If you delete this, the functionality will be the same and slowdown will be noticeable only on large data sets.
            customers.Indexes.Add(c => (string)c.Element("CustomerID"), true);
            orders.Indexes.Add(o => (string)o.Attribute("CustomerID"));
            orders.Indexes.Add(o => (string)o.Attribute("ShipCity"));
            //...end of creating indexes
            
            // Creating a live view over a base view
            IListSource view =
                (from o in orders.AsLive().AsUpdatable()
                 where (string) o.Attribute("ShipCity") == "London" || (string) o.Attribute("ShipCity") == "Colchester"
                 select new
                 {
                     OrderID = (int) o.Attribute("OrderID"),
                     CustomerID = (string) o.Attribute("CustomerID"),
                     ShipCity = (string) o.Attribute("ShipCity")
                 }).AsDynamic();

            ShowViewInGrid(view, dataGridView1);

            // Creating another live view over the base data
            customerOrderView =
                from c in customers
                join o in orders.AsLive().AsUpdatable() on (string)c.Element("CustomerID") equals (string)o.Attribute("CustomerID")
                where (string)o.Attribute("ShipCity") == "London" || (string)o.Attribute("ShipCity") == "Colchester"
                // Using a user-defined class like CustomerOrder is not mandatory. Anonymous class could be used instead as in the
                // query above, but in that case we could not assign it to a variable like customerOrderView, defined outside
                // of the scope of this method, because anonymous classes are only available in local scope.
                select new CustomerOrder
                {
                    CustomerID = (string)c.Element("CustomerID"),
                    City = (string)c.Element("City"),
                    OrderID = (int)o.Attribute("OrderID"),
                    ShipCity = (string)o.Attribute("ShipCity")
                };

            ShowViewInGrid(customerOrderView, dataGridView2);

            // See the readme.txt file in the project folder for a description of the live view functionality you can try in this sample
        }

        private void ShowViewInGrid(IListSource view, DataGridView grid)
        {
            grid.DataSource = view;
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                grid.Columns[i].FillWeight = 100 / grid.Columns.Count;
            }
        }

    }
}
