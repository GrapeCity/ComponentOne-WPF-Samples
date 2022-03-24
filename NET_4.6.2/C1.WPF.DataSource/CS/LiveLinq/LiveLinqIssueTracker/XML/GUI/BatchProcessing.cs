using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using C1.LiveLinq.LiveViews;
using LiveLinqIssueTrackerXML.Data;

namespace LiveLinqIssueTrackerXML.GUI
{
    public partial class BatchProcessing : Form
    {
        public class IssueAssignment
        {
            public virtual int IssueID { get; set; }
            public virtual int EmployeeID { get; set; }
            public virtual string EmployeeName { get; set; }
        }

        IssueTracker _data;
        View<IssueAssignment> _issuesToAssign;
        Dictionary<int, View<int>> views = new Dictionary<int, View<int>>();

        public BatchProcessing(IssueTracker data)
            : this()
        {
            _data = data;

            _issuesToAssign =
                from i in _data.Issues
                where i.AssignedTo == 0
                join a in _data.Assignments on new { i.ProductID, i.FeatureID } equals new { a.ProductID, a.FeatureID }
                join i1 in _data.Issues
                        on new { i.ProductID, i.FeatureID, a.EmployeeID } equals new { i1.ProductID, i1.FeatureID, EmployeeID = i1.AssignedTo } into g
                where g.Count() == 0
                join e in _data.Employees on a.EmployeeID equals e.EmployeeID
                select new IssueAssignment { IssueID = i.IssueID, EmployeeID = a.EmployeeID, EmployeeName = e.FullName };
        }
        public BatchProcessing()
        {
            InitializeComponent();
        }

        private void BatchProcessing_Load(object sender, EventArgs e)
        {
            employeesBindingSource.DataSource = _data.Employees;
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
                ViewRow issueRow = _data.FindIssueByID(ia.IssueID);
                issueRow["AssignedTo"] = ia.EmployeeID;
                issueRow.EndEdit();
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
                    from i in _data.Issues
                    join p in _data.Products on i.ProductID equals p.ProductID
                    join f in _data.Features on new { i.ProductID, i.FeatureID } equals new { f.ProductID, f.FeatureID }
                    join em in _data.Employees on i.AssignedTo equals em.EmployeeID
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
