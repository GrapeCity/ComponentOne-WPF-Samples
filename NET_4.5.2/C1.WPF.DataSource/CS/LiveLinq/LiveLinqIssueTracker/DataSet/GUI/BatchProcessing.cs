using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using C1.LiveLinq.AdoNet;
using C1.LiveLinq.LiveViews;
using LiveLinqIssueTrackerDataSet.Data;


namespace LiveLinqIssueTrackerDataSet.GUI
{
    public partial class BatchProcessing : Form
    {
        public class IssueAssignment
        {
            public virtual int IssueID { get; set; }
            public virtual int EmployeeID { get; set; }
            public virtual string EmployeeName { get; set; }
        }

        IssueTracking _dataSet;
        View<IssueAssignment> _issuesToAssign;
        Dictionary<int, View<int>> views = new Dictionary<int, View<int>>();

        public BatchProcessing(IssueTracker data)
            : this()
        {
            _dataSet = data.DataSet;

            View<IssueTracking.IssuesRow> issues = _dataSet.Issues.AsLive();
            _issuesToAssign =
                from i in issues
                where i.AssignedTo == 0
                join a in _dataSet.Assignments.AsLive() on new { i.ProductID, i.FeatureID } equals new { a.ProductID, a.FeatureID }
                join i1 in issues
                        on new { i.ProductID, i.FeatureID, a.EmployeeID } equals new { i1.ProductID, i1.FeatureID, EmployeeID = i1.AssignedTo } into g
                where g.Count() == 0
                join e in _dataSet.Employees.AsLive() on a.EmployeeID equals e.EmployeeID
                select new IssueAssignment { IssueID = i.IssueID, EmployeeID = a.EmployeeID, EmployeeName = e.FullName };
        }
        public BatchProcessing()
        {
            InitializeComponent();
        }

        private void BatchProcessing_Load(object sender, EventArgs e)
        {
            employeesBindingSource.DataSource = _dataSet;
            CreateViewIfNeeded();
        }

        private void comboAssignedTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateViewIfNeeded();
        }

        private void buttonAssign_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            foreach (IssueAssignment ia in _issuesToAssign.ToList())
            {
                _dataSet.Issues.FindByIssueID(ia.IssueID).AssignedTo = ia.EmployeeID;
                msg += "\nto " + ia.EmployeeName + " issue #" + ia.IssueID;
            }
            if (msg.Length == 0)
                msg = "\nNone";
            msg = "Assigned issues:" + msg;
            MessageBox.Show(msg, "Assigned issues");
        }

        private void buttonGetIssues_Click(object sender, EventArgs e)
        {
            MessageBox.Show(GetOpenIssues(views[comboAssignedTo.SelectedIndex]), "Open issues");

        }

        private void CreateViewIfNeeded()
        {
            int employeeID = comboAssignedTo.SelectedIndex;
            if (!views.ContainsKey(employeeID))
            {
                View<int> view =
                    from i in _dataSet.Issues.AsLive()
                    join p in _dataSet.Products.AsLive() on i.ProductID equals p.ProductID
                    join f in _dataSet.Features.AsLive() on new { i.ProductID, i.FeatureID } equals new { f.ProductID, f.FeatureID }
                    join em in _dataSet.Employees.AsLive() on i.AssignedTo equals em.EmployeeID
                    where i.AssignedTo == employeeID && !i.Fixed
                    select i.IssueID;

                views[employeeID] = view;
            }
        }

        private string GetOpenIssues(View<int> view)
        {
            string issueList = string.Empty;
            foreach (int i in view)
            {
                if (issueList.Length != 0)
                    issueList += ", ";
                issueList += i.ToString();
            }
            return issueList;
        }
    }
}
