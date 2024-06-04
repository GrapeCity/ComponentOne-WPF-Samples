using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiveLinqIssueTrackerXML.Data;
using C1.LiveLinq.LiveViews;

namespace LiveLinqIssueTrackerXML.GUI
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

        IssueTracker _data;
        Dictionary<int, View<Issue>> views = new Dictionary<int, View<Issue>>();

        public AssignedIssues(IssueTracker data)
            : this()
        {
            _data = data;
        }
        public AssignedIssues()
        {
            InitializeComponent();
        }

        private void AssignedIssues_Load(object sender, EventArgs e)
        {
            employeesBindingSource.DataSource = _data.Employees;
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
                    from i in _data.Issues
                    join p in _data.Products on i.ProductID equals (int)p.ProductID
                    join f in _data.Features
                        on new { pID = i.ProductID, fID = i.FeatureID }
                        equals new { pID = f.ProductID, fID = f.FeatureID } 
                    join e in _data.Employees on i.AssignedTo equals e.EmployeeID
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
