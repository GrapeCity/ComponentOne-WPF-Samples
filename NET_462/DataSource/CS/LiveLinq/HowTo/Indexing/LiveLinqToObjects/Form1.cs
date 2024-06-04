using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using C1.LiveLinq;
using C1.LiveLinq.Collections;

namespace LiveLinqToObjects
{
    public partial class Form1 : Form
    {
        IndexedCollection<Customer> customers = new IndexedCollection<Customer>();
        IndexedCollection<Order> orders = new IndexedCollection<Order>();

        class Pair
        {
#pragma warning disable 649
            public Customer customer;
            public Order order;
#pragma warning restore 649
        }

        public Form1()
        {
            InitializeComponent();

            XDocument doc;
            using (var xmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LiveLinqToObjects.Northwind.xml"))
            {
                System.Diagnostics.Debug.Assert(xmlStream != null);
                XmlReaderSettings settings = new XmlReaderSettings { IgnoreWhitespace = true };
                XmlReader reader = XmlReader.Create(xmlStream, settings);
                doc = XDocument.Load(reader);
            }

            foreach (XElement c in doc.Descendants("Customers"))
                customers.Add(new Customer
                                   {
                                       CustomerID = (string)c.Element("CustomerID"),
                                       City = (string)c.Element("City"),
                                       DateRegistered = (DateTime)c.Element("DateRegistered")
                                   });
            foreach (XElement o in doc.Descendants("Orders"))
                orders.Add(new Order
                                {
                                    OrderID = (int)o.Attribute("OrderID"),
                                    CustomerID = (string)o.Attribute("CustomerID"),
                                    ShipCity = (string)o.Attribute("ShipCity"),
                                    Freight = (decimal)o.Attribute("Freight"),
                                    OrderDate = (DateTime)o.Attribute("OrderDate"),
                                });
        }

        private void simpleButtonImplicitIndexing_Click(object sender, EventArgs e)
        {
            var result =
                from o in orders
                where o.CustomerID.Indexed() == "ALFKI"
                select o;

            ShowResult(result, false);
        }

        private void joinButtonImplicitIndexing_Click(object sender, EventArgs e)
        {
            var result =
                from c in customers
                where c.City.Indexed() == "London"
                join o in orders on c.CustomerID.Indexed() equals o.CustomerID.Indexed()
                select new Pair { customer = c, order = o };

            ShowResult(result, true);
        }

        private void indexesButton_Click(object sender, EventArgs e)
        {
            if (customers.Indexes.Count != 0 && orders.Indexes.Count != 0)
            {
                MessageBox.Show("Indexes already exist");
                return;
            }

            customers.Indexes.Add(c => c.CustomerID, true);
            customers.Indexes.Add(c => c.City);

            orders.Indexes.Add(o => o.CustomerID);

            MessageBox.Show("Indexes created");
        }

        private void simpleButton_Click(object sender, EventArgs e)
        {
            if (customers.Indexes.Count == 0 || orders.Indexes.Count == 0)
            {
                MessageBox.Show("Create the indexes first");
                return;
            }

            var result =
                from o in orders
                where o.CustomerID == "ALFKI"
                select o;

            ShowResult(result, false);
        }

        private void joinButton_Click(object sender, EventArgs e)
        {
            if (customers.Indexes.Count == 0 || orders.Indexes.Count == 0)
            {
                MessageBox.Show("Create the indexes first");
                return;
            }

            var result =
                from c in customers
                where c.City == "London"
                join o in orders on c.CustomerID equals o.CustomerID
                select new Pair { customer = c, order = o };

            ShowResult(result, true);
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
                            p.customer.CustomerID + ", City: " + (string)p.customer.City,
                            p.order.OrderID + ", OrderDate: " + p.order.OrderDate
                        }));
                }
                else
                {
                    Order o = item as Order;
                    listView1.Items.Add(o.OrderID + ", CustomerID: " + o.CustomerID + ", ShipCity: " + o.ShipCity);
                }
            }
        }

    }
}
