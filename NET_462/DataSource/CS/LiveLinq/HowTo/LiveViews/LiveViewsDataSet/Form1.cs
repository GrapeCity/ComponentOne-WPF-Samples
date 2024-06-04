using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using C1.LiveLinq;
using C1.LiveLinq.AdoNet;
using C1.LiveLinq.LiveViews;


namespace LiveViewsDataSet
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

        NorthwindDataSet typedDataSet = new NorthwindDataSet();
        View<NorthwindDataSet.CustomersRow> customersView;
        View<NorthwindDataSet.OrdersRow> ordersView;
        View<CustomerOrder> customerOrderView;

        public Form1()
        {
            InitializeComponent();

            // Filling the data set with data
            string dataPath = string.Empty;
            foreach (DataColumn col in typedDataSet.Tables["Orders"].Columns)
                col.ColumnMapping = MappingType.Attribute; // to make our XML file more compact
            typedDataSet.Namespace = ""; // need this because our XML file does not have xmlns defined
            
            using (var xmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LiveViewsDataSet.Northwind.xml"))
            {
                System.Diagnostics.Debug.Assert(xmlStream != null);
                XmlReaderSettings settings = new XmlReaderSettings { IgnoreWhitespace = true };
                XmlReader reader = XmlReader.Create(xmlStream, settings);
                typedDataSet.ReadXml(reader);
            }

            // Creating indexes. It is optional, only for performance optimization.
            // If you delete this, the functionality will be the same and slowdown will be noticeable only on large data sets.
            // If you don't need to create indexes, you can use typedDataSet.Customers.AsLive() below instead of customers.AsLive(), so you don't need the
            // IndexedDataTable<> class, can proceed directly to View<NorthwindDataSet.CustomersRow>.
            IndexedDataTable<NorthwindDataSet.CustomersRow> customers = typedDataSet.Customers.AsIndexed();
            IndexedDataTable<NorthwindDataSet.OrdersRow> orders = typedDataSet.Orders.AsIndexed();
            customers.Indexes.Add(c => c.CustomerID, true);
            orders.Indexes.Add(o => o.CustomerID);
            orders.Indexes.Add(o => o.ShipCity);
            //...end of creating indexes

            // Make the tables live:
            customersView = typedDataSet.Customers.AsLive();
            ordersView = typedDataSet.Orders.AsLive().AsUpdatable(); // AsUpdatable() makes it possible for 
                // the user to change data directly in the grid showing the join view below.
                // Order fields can be changed by the user there, Customer fields can't be changed by the user
                // Specifying this argument is needed only if you need to change data directly in the view instead
                // of changing the base data, and then only for query operators with more than one argument such as Join.

            // Creating a live view over the base data
            IListSource view =
                (from o in ordersView
                 where o.ShipCity == "London" || o.ShipCity == "Colchester"
                 select new { OrderID = o.OrderID, CustomerID = o.CustomerID, ShipCity = o.ShipCity }).AsDynamic();

            ShowViewInGrid(view, dataGridView1);

            // Creating another live view over the base data
            customerOrderView =
                from c in customersView
                join o in ordersView on c.CustomerID equals o.CustomerID
                where o.ShipCity == "London" || o.ShipCity == "Colchester"
                // Using a user-defined class like CustomerOrder is not mandatory. Anonymous class could be used instead as in the
                // query above, but in that case we could not assign it to a variable like customerOrderView, defined outside
                // of the scope of this method, because anonymous classes are only available in local scope.
                select new CustomerOrder { CustomerID = c.CustomerID, City = c.City, OrderID = o.OrderID, ShipCity = o.ShipCity };

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
