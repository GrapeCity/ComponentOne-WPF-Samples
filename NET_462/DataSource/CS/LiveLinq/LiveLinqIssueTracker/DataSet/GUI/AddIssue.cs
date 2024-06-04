using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using LiveLinqIssueTrackerDataSet.Data;

namespace LiveLinqIssueTrackerDataSet.GUI
{
    public partial class AddIssue : Form
    {
        IssueTracking _dataSet;

        public AddIssue(IssueTracker data) : this()
        {
            _dataSet = data.DataSet;
        }
        public AddIssue()
        {
            InitializeComponent();
        }

        private void AddIssue_Load(object sender, EventArgs e)
        {
            productsBindingSource.DataSource = _dataSet;
            featuresBindingSource.DataSource = _dataSet;
            employeesBindingSource.DataSource = _dataSet;
            txtIssueID.Text = (_dataSet.Issues.Select(x => x.IssueID).Max() + 1).ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _dataSet.Issues.AddIssuesRow(Int32.Parse(txtIssueID.Text), (int)comboProduct.SelectedValue, (int)comboFeature.SelectedValue, txtDescription.Text, (int)comboAssignedTo.SelectedValue, false);
            Close();
        }

    }
}
