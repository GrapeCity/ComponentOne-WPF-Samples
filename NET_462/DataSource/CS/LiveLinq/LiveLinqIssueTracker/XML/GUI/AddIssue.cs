﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
using C1.LiveLinq.LiveViews;
using LiveLinqIssueTrackerXML.Data;

namespace LiveLinqIssueTrackerXML.GUI
{
    public partial class AddIssue : Form
    {
        IssueTracker _data;

        public AddIssue(IssueTracker data)
            : this()
        {
            _data = data;
        }
        public AddIssue()
        {
            InitializeComponent();
        }

        private void AddIssue_Load(object sender, EventArgs e)
        {
            productsBindingSource.DataSource = _data.Products;
            featuresBindingSource.DataSource = _data.Features;
            employeesBindingSource.DataSource = _data.Employees;
            FillFeatures();

            txtIssueID.Text = (_data.Issues.Select(x => (int)x.IssueID).Max() + 1).ToString();
        }

        private void FillFeatures()
        {
            fKProductsFeaturesBindingSource.DataSource = comboProduct.SelectedIndex == -1 ? null
                : from f in _data.Features where f.ProductID == comboProduct.SelectedIndex + 1 select f;
        }

        private void comboProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillFeatures();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ViewRow newRow = _data.Issues.Rows.CreateRow();
            newRow["IssueID"] = Int32.Parse(txtIssueID.Text);
            newRow["ProductID"] = (int)comboProduct.SelectedValue;
            newRow["FeatureID"] = (int)comboFeature.SelectedValue;
            newRow["Description"] = txtDescription.Text;
            newRow["AssignedTo"] = (int)comboAssignedTo.SelectedValue;
            newRow["Fixed"] = false;
            newRow.EndEdit();
            Close();
        }
    }
}
