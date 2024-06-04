using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using C1.LiveLinq;
using C1.LiveLinq.AdoNet;

namespace LiveLinqToDataSet
{
    public partial class Form1 : Form
    {
        DataSet untypedDataSet = new DataSet();
        NorthwindDataSet typedDataSet = new NorthwindDataSet();
        IndexedDataTable<NorthwindDataSet.CustomersRow> customers;
        IndexedDataTable<NorthwindDataSet.OrdersRow> orders;
        IndexedDataTable<DataRow> untypedCustomers;
        IndexedDataTable<DataRow> untypedOrders;
        bool useUntypedDataSet;

        class Pair
        {
#pragma warning disable 649
            public DataRow customer;
            public DataRow order;
#pragma warning restore 649
        }

        public Form1()
        {
            InitializeComponent();

            using (var xmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LiveLinqToDataSet.Northwind.xml"))
            {
                System.Diagnostics.Debug.Assert(xmlStream != null);
                XmlReaderSettings settings = new XmlReaderSettings { IgnoreWhitespace = true };
                XmlReader reader = XmlReader.Create(xmlStream, settings);
                untypedDataSet.ReadXml(reader);
            }

            using (var xmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LiveLinqToDataSet.Northwind.xml"))
            {
                System.Diagnostics.Debug.Assert(xmlStream != null);
                XmlReaderSettings settings = new XmlReaderSettings { IgnoreWhitespace = true };
                XmlReader reader = XmlReader.Create(xmlStream, settings);
                foreach (DataColumn col in typedDataSet.Tables["Orders"].Columns)
                    col.ColumnMapping = MappingType.Attribute; // to make our XML file more compact
                typedDataSet.Namespace = ""; // need this because our XML file does not have xmlns defined
                typedDataSet.ReadXml(reader);
            }

            untypedCustomers = untypedDataSet.Tables["Customers"].AsIndexed();
            untypedOrders = untypedDataSet.Tables["Orders"].AsIndexed();

            customers = typedDataSet.Customers.AsIndexed();
            orders = typedDataSet.Orders.AsIndexed();
        }

        private void simpleButtonImplicitIndexing_Click(object sender, EventArgs e)
        {
            if (useUntypedDataSet)
            {
                var result =
                    from o in untypedOrders
                    where o.IndexedField<string>("CustomerID") == "ALFKI"
                    select o;

                ShowResult(result, false);
            }
            else
            {
                var result =
                    from o in orders
                    where o.CustomerID.Indexed() == "ALFKI"
                    select o;

                ShowResult(result, false);
            }
        }

        private void joinButtonImplicitIndexing_Click(object sender, EventArgs e)
        {
            if (useUntypedDataSet)
            {
                var result =
                    from c in untypedCustomers
                    where c.IndexedField<string>("City") == "London"
                    join o in orders on c.IndexedField<string>("CustomerID") equals o.IndexedField<string>("CustomerID")
                    select new Pair { customer = c, order = o };

                ShowResult(result, true);
            }
            else
            {
                var result =
                    from c in customers
                    where c.City.Indexed() == "London"
                    join o in orders on c.CustomerID.Indexed() equals o.CustomerID.Indexed()
                    select new Pair { customer = c, order = o };

                ShowResult(result, true);
            }
        }

        private void indexesButton_Click(object sender, EventArgs e)
        {
            if (useUntypedDataSet && untypedCustomers.Indexes.Count != 0 && untypedOrders.Indexes.Count != 0 ||
                !useUntypedDataSet && customers.Indexes.Count != 0 && orders.Indexes.Count != 0)
            {
                MessageBox.Show("Indexes already exist");
                return;
            }

            if (useUntypedDataSet)
            {
                untypedCustomers.Indexes.Add(c => c.Field<string>("CustomerID"), true);
                untypedCustomers.Indexes.Add(c => c.Field<string>("City"));
                untypedOrders.Indexes.Add(o => o.Field<string>("CustomerID"));
                MessageBox.Show("Created indexes for untyped data set");
            }
            else
            {
                customers.Indexes.Add(c => c.CustomerID, true);
                customers.Indexes.Add(c => c.City); // creates index if not found
                orders.Indexes.Add(o => o.CustomerID); // creates index if not found
                MessageBox.Show("Created indexes for typed data set");
            }
        }

        private void simpleButton_Click(object sender, EventArgs e)
        {
            if (useUntypedDataSet && (untypedCustomers.Indexes.Count == 0 || untypedOrders.Indexes.Count == 0) ||
                !useUntypedDataSet && (customers.Indexes.Count == 0 || orders.Indexes.Count == 0))
            {
                MessageBox.Show("Create the indexes first");
                return;
            }

            if (useUntypedDataSet)
            {
                var result =
                    from o in untypedOrders
                    where o.Field<string>("CustomerID") == "ALFKI"
                    select o;

                ShowResult(result, false);
            }
            else
            {
                var result =
                    from o in orders
                    where o.CustomerID == "ALFKI"
                    select o;

                ShowResult(result, false);
            }
        }

        private void joinButton_Click(object sender, EventArgs e)
        {
            if (useUntypedDataSet && (untypedCustomers.Indexes.Count == 0 || untypedOrders.Indexes.Count == 0) ||
                !useUntypedDataSet && (customers.Indexes.Count == 0 || orders.Indexes.Count == 0))
            {
                MessageBox.Show("Create the indexes first");
                return;
            }

            if (useUntypedDataSet)
            {
                var result =
                    from c in untypedCustomers
                    where c.Field<string>("City") == "London"
                    join o in orders on c.Field<string>("CustomerID") equals o.Field<string>("CustomerID")
                    select new Pair { customer = c, order = o };

                ShowResult(result, true);
            }
            else
            {
                var result =
                    from c in customers
                    where c.City == "London"
                    join o in orders on c.CustomerID equals o.CustomerID
                    select new Pair { customer = c, order = o };

                ShowResult(result, true);
            }
        }


        private void typedDSRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            useUntypedDataSet = !this.typedDSRadioButton.Checked;
        }

        private void ShowResult(IEnumerable result, bool twoColumns)
        {
            listView1.Clear();
            listView1.Columns.Clear();
            if (twoColumns)
            {
                var header1 = new ColumnHeader();
                var header2 = new ColumnHeader();
                listView1.Columns.Add(header1);
                listView1.Columns.Add(header2);
                header1.Width = listView1.ClientSize.Width / 2;
                header2.Width = listView1.ClientSize.Width / 2;
                header1.Text = "Customer";
                header2.Text = "Order";
            }
            else
            {
                var header1 = new ColumnHeader();
                listView1.Columns.Add(header1);
                header1.Width = listView1.ClientSize.Width;
                header1.Text = "Order";
            }

            foreach (object item in result)
            {
                if (twoColumns)
                {
                    Pair p = item as Pair;
                    listView1.Items.Add(
                        new ListViewItem(new string[]
                        {
                            p.customer.Field<string>("CustomerID") + ", City: " + p.customer.Field<string>("City"),
                            p.order.Field<int>("OrderID").ToString() + ", OrderDate: " + p.order.Field<DateTime>("OrderDate").ToString()
                        }));
                }
                else
                {
                    DataRow o = item as DataRow;
                    listView1.Items.Add(o["OrderID"].ToString() + ", CustomerID: " + o["CustomerID"].ToString() + ", ShipCity: " + o["ShipCity"].ToString());
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
