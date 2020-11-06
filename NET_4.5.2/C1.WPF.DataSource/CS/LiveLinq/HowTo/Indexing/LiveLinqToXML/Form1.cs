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
using C1.LiveLinq.LiveViews;
using C1.LiveLinq.LiveViews.Xml;

namespace LiveLinqToXML
{
    public partial class Form1 : Form
    {
        View<XDocument> document;
        View<XElement> customers;
        View<XElement> orders;

        class Pair
        {
#pragma warning disable 649
            public XElement customer;
            public XElement order;
#pragma warning restore 649
        }

        public Form1()
        {
            InitializeComponent();

            using (var xmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LiveLinqToXML.Northwind.xml"))
            {
                System.Diagnostics.Debug.Assert(xmlStream != null);
                XmlReaderSettings settings = new XmlReaderSettings { IgnoreWhitespace = true };
                XmlReader reader = XmlReader.Create(xmlStream, settings);
                XDocument doc = XDocument.Load(reader);
                document = doc.AsLive();
            }

            customers = document.Descendants("Customers");
            orders = document.Descendants("Orders");
        }

        private void simpleButtonImplicitIndexing_Click(object sender, EventArgs e)
        {
            var result =
                from Order in orders.AsIndexed()
                where (string)Order.IndexedAttribute("CustomerID") == "ALFKI"
                select Order;

            ShowResult(result, false);
        }

        private void joinButtonImplicitIndexing_Click(object sender, EventArgs e)
        {
            var result =
                from Customer in customers.AsIndexed()
                where (string)Customer.IndexedElement("City") == "London"
                join Order in orders on (string)Customer.Element("CustomerID") equals (string)Order.Attribute("CustomerID")
                select new Pair { customer = Customer, order = Order };

            ShowResult(result, true);
        }

        private void indexesButton_Click(object sender, EventArgs e)
        {
            if (customers.Indexes.Count != 0 && orders.Indexes.Count != 0)
            {
                MessageBox.Show("Indexes already exist", "LiveLinq");
                return;
            }

            customers.Indexes.Add(c => (string)c.Element("CustomerID"), true);
            customers.Indexes.Add(c => (string)c.Element("City"));

            orders.Indexes.Add(o => (string)o.Attribute("CustomerID"));

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
                from Order in orders
                where (string)Order.Attribute("CustomerID") == "ALFKI"
                select Order;

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
                from Customer in customers
                where (string)Customer.Element("City") == "London"
                join Order in orders on (string)Customer.Element("CustomerID") equals (string)Order.Attribute("CustomerID")
                select new Pair { customer = Customer, order = Order };

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
                    listView1.Items.Add(new ListViewItem(new string[]
                    {
                        (string)p.customer.Element("CustomerID") + ", City: " + (string)p.customer.Element("City"),
                        ((int)p.order.Attribute("OrderID")).ToString() + ", OrderDate: " + ((DateTime)p.order.Attribute("OrderDate")).ToString()
                    }));
                }
                else
                {
                    XElement o = item as XElement;
                    listView1.Items.Add(((int)o.Attribute("OrderID")).ToString() + ", CustomerID: " + (string)o.Attribute("CustomerID") + ", ShipCity: " + (string)o.Attribute("ShipCity"));
                }
            }
        }

    }
}
