using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiveLinqIssueTrackerDataSet.Data;
using C1.LiveLinq.Indexing;
using C1.LiveLinq.AdoNet;
using C1.LiveLinq.LiveViews;

namespace LiveLinqIssueTrackerDataSet.GUI
{
    public partial class ReassignFeatures : Form
    {
        IssueTracking _dataSet;
        bool _dirty = false;
        int _currEmployeeId = -1;

        public ReassignFeatures(IssueTracker data)
            : this()
        {
            _dataSet = data.DataSet;
        }
        public ReassignFeatures()
        {
            InitializeComponent();
        }

        void ReassignFeatures_Load(object sender, EventArgs e)
        {
            employeesBindingSource.DataSource = _dataSet;
            Bind();
        }

        void comboAssignedTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_currEmployeeId != comboAssignedTo.SelectedIndex)
            {
                if (_currEmployeeId != -1)
                    SaveAssignments();
            }
            _currEmployeeId = comboAssignedTo.SelectedIndex;
            Bind();
        }

        void ReassignFeatures_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveAssignments();
        }

        void treeViewFeatures_AfterCheck(object sender, TreeViewEventArgs e)
        {
            _dirty = true;
        }

        void buttonApply_Click(object sender, EventArgs e)
        {
            SaveAssignments();
        }

        class ProductTreeNode : TreeNode
        {
            public int ProductID;
        }

        class FeatureTreeNode : TreeNode
        {
            public int FeatureID;
        }

        void Bind()
        {
            var products =
                from p in _dataSet.Products.AsIndexed()
                join f in _dataSet.Features.AsIndexed() on p.ProductID equals f.ProductID into features
                select new {p, features};
            var indexedAssignments = _dataSet.Assignments.AsIndexed();
            var index = indexedAssignments.Indexes.Add(a => new {a.ProductID, a.FeatureID, a.EmployeeID});

            Index<IssueTracking.AssignmentsRow> productIndex = indexedAssignments.Indexes.Add(a => a.ProductID);

            treeViewFeatures.BeginUpdate();
            treeViewFeatures.Nodes.Clear();
            int employeeID = comboAssignedTo.SelectedIndex;
            foreach (var product in products)
            {
                var productNode = new ProductTreeNode();
                productNode.ProductID = product.p.ProductID;
                productNode.Text = product.p.ProductName;
                treeViewFeatures.Nodes.Add(productNode);
                bool productChecked = false;
                foreach (var feature in product.features)
                {
                    FeatureTreeNode featureNode = new FeatureTreeNode();
                    featureNode.FeatureID = feature.FeatureID;
                    featureNode.Text = feature.FeatureName;
                    productNode.Nodes.Add(featureNode);

                    bool isChecked = index.ContainsKey(new {product.p.ProductID, feature.FeatureID, EmployeeID = employeeID});
                    if (isChecked)
                    {
                        featureNode.Checked = true;
                        productChecked = true;
                    }
                }
                productNode.Checked = productChecked;
            }
            treeViewFeatures.ExpandAll();
            treeViewFeatures.EndUpdate();
            _dirty = false;
        }

        void SaveAssignments()
        {
            if (!_dirty || _currEmployeeId < 1)
                return;
            _dirty = false;
            for (int i = _dataSet.Assignments.Count - 1; i >= 0; i--)
            {
                IssueTracking.AssignmentsRow row = _dataSet.Assignments[i];
                if (row.EmployeeID == _currEmployeeId)
                    row.Delete();
            }
            foreach (ProductTreeNode productNode in treeViewFeatures.Nodes)
                foreach (FeatureTreeNode featureNode in productNode.Nodes)
                    if (featureNode.Checked)
                        _dataSet.Assignments.AddAssignmentsRow(_currEmployeeId, productNode.ProductID, featureNode.FeatureID);
        }
    }
}