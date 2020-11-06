using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using LiveLinqIssueTrackerXML.Data;
using C1.LiveLinq.LiveViews;

namespace LiveLinqIssueTrackerXML.GUI
{
    public partial class ReassignIssue : Form
    {
        public class IssueInfo
        {
            public virtual int IssueID { get; set; }
            public virtual string ProductName { get; set; }
            public virtual string FeatureName { get; set; }
            public virtual string Description { get; set; }
            public virtual int AssignedToID { get; set; }
            public virtual string AssignedToName { get; set; }
        }

        IssueTracker _data;

        public ReassignIssue(IssueTracker data)
            : this()
        {
            _data = data;
        }
        public ReassignIssue()
        {
            InitializeComponent();
        }

        private void ReassignIssue_Load(object sender, EventArgs e)
        {
            employeesBindingSource.DataSource = _data.Employees;
            employeesBindingSource2.DataSource = _data.Employees;
            Bind();
        }

        private void Bind()
        {
            int employeeID = comboAssignedTo.SelectedIndex;
            var issues =
                from i in _data.Issues
                join p in _data.Products on i.ProductID equals p.ProductID
                join f in _data.Features on new { i.ProductID, i.FeatureID } equals new { f.ProductID, f.FeatureID }
                join e in _data.Employees on i.AssignedTo equals e.EmployeeID
                where i.AssignedTo == employeeID
                select new IssueInfo
                {
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
            IssueInfo issue = row.Value as IssueInfo;
            if (issue == null)
                return;
            if (issue != null)
            {
                ViewRow issueRow = _data.FindIssueByID(issue.IssueID);
                issueRow["AssignedTo"] = comboReassign.SelectedIndex;
            }
        }
    }
}
