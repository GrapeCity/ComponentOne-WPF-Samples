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
using C1.LiveLinq.Collections;

namespace BasicBinding
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        string names = "Paul,Ringo,John,George,Robert,Jimmy,John Paul,Bonzo";

        private void button1_Click(object sender, EventArgs e)
        {
          // create data source
          var contacts = new IndexedCollection<Contact>();

          // bind list to grid (before adding elements)
          this.dataGridView1.DataSource =
            from c in contacts.AsLive()
            where c.Name.Contains("g")
            select c;

          // add elements to collection (after binding)
          foreach (string s in names.Split(','))
          {
            contacts.Add(new Contact() { Name = s });
          }
        }

        public class Contact : IndexableObject
        {
            private string _name;
            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
    }
}
