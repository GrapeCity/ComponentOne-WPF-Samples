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

        IssueTracker _data;

        public EmployeesIssues(IssueTracker data)
            : this()
        {
            _data = data;
        }
        public EmployeesIssues()
        {
            InitializeComponent();
        }

        private void EmployeesIssues_Load(object sender, EventArgs e)
        {
            employeesBindingSource.DataSource = _data.Employees;
            Bind();
        }

        private void Bind()
        {
            int employeeID = comboAssignedTo.SelectedIndex;
            dataGridView1.DataSource = 
                    from a in _data.Assignments
                    join p in _data.Products on a.ProductID equals p.ProductID
                    join f in _data.Features on new { a.ProductID, a.FeatureID } equals new { f.ProductID, f.FeatureID }
                    join i in _data.Issues on new { a.ProductID, a.FeatureID } equals new { i.ProductID, i.FeatureID }
                    join e in _data.Employees on i.AssignedTo equals e.EmployeeID
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
