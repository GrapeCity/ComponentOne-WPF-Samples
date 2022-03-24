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
    public partial class EmployeesIssues : Form
    {
        public class Issue
        {
            public virtual int IssueID { get; set; }
            public virtual string ProductName { get; set; }
            public virtual string FeatureName { get; set; }
            public virtual string Description { get; set; }
            public virtual string AssignedTo { get; set; }
        }

        IssueTracking _dataSet;

        public EmployeesIssues(IssueTracker data)
            : this()
        {
            _dataSet = data.DataSet;
        }
        public EmployeesIssues()
        {
            InitializeComponent();
        }

        private void EmployeesIssues_Load(object sender, EventArgs e)
        {
            employeesBindingSource.DataSource = _dataSet;
            Bind();
        }

        private void Bind()
        {
            int employeeID = comboAssignedTo.SelectedIndex;
            dataGridView1.DataSource = 
                    from a in _dataSet.Assignments.AsLive()
                    join p in _dataSet.Products.AsLive() on a.ProductID equals p.ProductID
                    join f in _dataSet.Features.AsLive() on new { a.ProductID, a.FeatureID } equals new { f.ProductID, f.FeatureID }
                    join i in _dataSet.Issues.AsLive() on new { a.ProductID, a.FeatureID } equals new { i.ProductID, i.FeatureID }
                    join e in _dataSet.Employees.AsLive() on i.AssignedTo equals e.EmployeeID
                    where a.EmployeeID == employeeID
                    select new Issue
                    {
                        IssueID = i.IssueID,
                        ProductName = p.ProductName,
                        FeatureName = f.FeatureName,
                        Description = i.Description,
                        AssignedTo = e.FullName
                    };

            dataGridView1.Columns["ProductName"].Width = 200;
            dataGridView1.Columns["FeatureName"].Width = 150;
            dataGridView1.Columns["Description"].Width = 300;
            dataGridView1.Columns["AssignedTo"].Width = 150;
        }

        private void comboAssignedTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind();
        }

    }
}
