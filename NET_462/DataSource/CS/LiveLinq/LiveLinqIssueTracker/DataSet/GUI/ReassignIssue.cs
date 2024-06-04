using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiveLinqIssueTrackerDataSet.Data;
using C1.LiveLinq.LiveViews;
using C1.LiveLinq.AdoNet;


namespace LiveLinqIssueTrackerDataSet.GUI
{
    public partial class ReassignIssue : Form
    {
        public class Issue
        {
            internal IssueTracking.IssuesRow I;
            public virtual int IssueID { get; set; }
            public virtual string ProductName { get; set; }
            public virtual string FeatureName { get; set; }
            public virtual string Description { get; set; }
            public virtual int AssignedToID { get; set; }
            public virtual string AssignedToName { get; set; }
        }

        IssueTracking _dataSet;

        public ReassignIssue(IssueTracker data)
            : this()
        {
            _dataSet = data.DataSet;
        }
        public ReassignIssue()
        {
            InitializeComponent();
        }

        private void ReassignIssue_Load(object sender, EventArgs e)
        {
            employeesBindingSource.DataSource = _dataSet;
            employeesBindingSource2.DataSource = _dataSet;
            Bind();
        }

        private void Bind()
        {
            int employeeID = comboAssignedTo.SelectedIndex;
            var issues =
                from i in _dataSet.Issues.AsLive()
                join p in _dataSet.Products.AsLive() on i.ProductID equals p.ProductID
                join f in _dataSet.Features.AsLive() on new { i.ProductID, i.FeatureID } equals new { f.ProductID, f.FeatureID }
                join e in _dataSet.Employees.AsLive() on i.AssignedTo equals e.EmployeeID
                where i.AssignedTo == employeeID
                select new Issue
                {
                    I = i,
                    IssueID = i.IssueID,
                    ProductName = p.ProductName,
                    FeatureName = f.FeatureName,
                    Description = i.Description,
                    AssignedToID = e.EmployeeID,
                    AssignedToName = e.FullName
                };            

            issuesBindingSource.DataSource = issues;
            dataGridView1.DataSource = issuesBindingSource;

            dataGridView1.Columns["ProductName"].Width = 200;
            dataGridView1.Columns["FeatureName"].Width = 100;
            dataGridView1.Columns["Description"].Width = 250;
            dataGridView1.Columns["AssignedToName"].Width = 150;
        }

        private void comboAssignedTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind();
        }

        private void buttonReassign_Click(object sender, EventArgs e)
        {
            ViewRow row = issuesBindingSource.Current as ViewRow;
            if (row == null)
                return;
            Issue issue = row.Value as Issue;
            if (issue == null)
                return;
            issue.I.AssignedTo = comboReassign.SelectedIndex;
        }

    }
}
