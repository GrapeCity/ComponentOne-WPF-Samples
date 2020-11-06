using System;
using System.Linq;
using System.Windows.Forms;
using LiveLinqIssueTrackerObjects.Data;
using C1.LiveLinq;
using C1.LiveLinq.LiveViews;

namespace LiveLinqIssueTrackerObjects.GUI
{
    public partial class AssignedIssues2 : Form
    {
        public class Issue
        {
            public virtual int IssueID { get; set; }
            public virtual string ProductName { get; set; }
            public virtual string FeatureName { get; set; }
            public virtual string Description { get; set; }
            public virtual int AssignedToID { get; set; }
            public virtual string AssignedToName { get; set; }
        }

        public class EmployeeIssueCount
        {
            public virtual string EmployeeName { get; set; }
            public virtual int IssueCount { get; set; }
        }

        IssueTracker _data;
        View<Issue> _bigView;
        View<EmployeeIssueCount> _issueCounts;

        public AssignedIssues2(IssueTracker data)
            : this()
        {
            _data = data;

            _bigView =
                from i in _data.Issues.AsLive()
                join p in _data.Products.AsLive() on i.ProductID equals p.ProductID
                join f in _data.Features.AsLive() on new { i.ProductID, i.FeatureID } equals new { f.ProductID, f.FeatureID }
                join e in _data.Employees.AsLive() on i.AssignedTo equals e.EmployeeID
                select new Issue
                {
                    IssueID = i.IssueID,
                    ProductName = p.ProductName,
                    FeatureName = f.FeatureName,
                    Description = i.Description,
                    AssignedToID = e.EmployeeID,
                    AssignedToName = e.FullName
                };

            _bigView.Indexes.Add(x => x.AssignedToID);

            _issueCounts =
                from i in _data.Issues.AsLive()
                join e in _data.Employees.AsLive() on i.AssignedTo equals e.EmployeeID
                group e by e.FullName into g
                select new EmployeeIssueCount
                {
                    EmployeeName = g.Key,
                    IssueCount = g.Count()
                };
        }

        public AssignedIssues2()
        {
            InitializeComponent();
        }

        private void AssignedIssues2_Load(object sender, EventArgs e)
        {
            dataGridView2.DataSource = _issueCounts;
            Bind(1);
        }

        private void Bind(int employeeID)
        {
            dataGridView1.DataSource = from i in _bigView where i.AssignedToID == employeeID select i;

            dataGridView1.Columns["ProductName"].Width = 200;
            dataGridView1.Columns["FeatureName"].Width = 100;
            dataGridView1.Columns["Description"].Width = 250;
            dataGridView1.Columns["AssignedToName"].Width = 150;
        }

        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            Bind(e.RowIndex + 1);
        }

    }
}
