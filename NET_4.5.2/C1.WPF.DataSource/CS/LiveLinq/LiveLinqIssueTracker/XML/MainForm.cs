using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiveLinqIssueTrackerXML.Data;
using LiveLinqIssueTrackerXML.GUI;

namespace LiveLinqIssueTrackerXML
{
    public partial class MainForm : Form
    {
        IssueTracker data = new IssueTracker();

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddIssue frmAddIssue = new AddIssue(data);
            frmAddIssue.Show(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AssignedIssues frmAssignedIssue = new AssignedIssues(data);
            frmAssignedIssue.Show(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AssignedIssues2 frmAssignedIssue2 = new AssignedIssues2(data);
            frmAssignedIssue2.Show(this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ReassignIssue frmReassignIssue = new ReassignIssue(data);
            frmReassignIssue.Show(this);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            EmployeesIssues frmEmployeesIssues = new EmployeesIssues(data);
            frmEmployeesIssues.Show(this);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ReassignFeatures frmReassignFeatures = new ReassignFeatures(data);
            frmReassignFeatures.Show(this);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            BatchProcessing frmBatchProcessing = new BatchProcessing(data);
            frmBatchProcessing.Show(this);
        }
    }
}
