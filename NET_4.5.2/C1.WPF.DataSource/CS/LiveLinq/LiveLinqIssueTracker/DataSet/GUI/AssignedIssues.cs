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
    public partial class AssignedIssues : Form
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
        Dictionary<int, View<Issue>> views = new Dictionary<int, View<Issue>>();

        public AssignedIssues(IssueTracker data)
            : this()
        {
            _dataSet = data.DataSet;
        }
        public AssignedIssues()
        {
            InitializeComponent();
        }

        private void AssignedIssues_Load(object sender, EventArgs e)
        {
            employeesBindingSource.DataSource = _dataSet;
            Bind();
        }

        private void comboAssignedTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            bool immediateUpdate = radioButtonImmediate.Checked;
            int employeeID = comboAssignedTo.SelectedIndex;
            if (!views.ContainsKey(employeeID))
            {
                View<Issue> view =
                    from i in _dataSet.Issues.AsLive()
                    join p in _dataSet.Products.AsLive() on i.ProductID equals p.ProductID
                    join f in _dataSet.Features.AsLive() on new { i.ProductID, i.FeatureID } equals new { f.ProductID, f.FeatureID } 
                    join e in _dataSet.Employees.AsLive() on i.AssignedTo equals e.EmployeeID
                    where i.AssignedTo == employeeID
                    select new Issue
                    {
                        IssueID = i.IssueID,
                        ProductName = p.ProductName,
                        FeatureName = f.FeatureName,
                        Description = i.Description,
                        AssignedTo = e.FullName
                    };

                views[employeeID] = view;
                view.MaintenanceMode = immediateUpdate ? ViewMaintenanceMode.Immediate : ViewMaintenanceMode.Deferred;
            }

            if (immediateUpdate)
                dataGridView1.DataSource = views[employeeID];
            else
                dataGridView1.DataSource = views[employeeID].ToList();

            dataGridView1.Columns["ProductName"].Width = 200;
            dataGridView1.Columns["FeatureName"].Width = 150;
            dataGridView1.Columns["Description"].Width = 300;
            dataGridView1.Columns["AssignedTo"].Width = 150;
        }

        private void radioButtonImmediate_CheckedChanged(object sender, EventArgs e)
        {
            views.Clear();
            Bind();
        }
    }
}
