using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using LiveLinqIssueTrackerObjects.Data;
using C1.LiveLinq.Indexing;
using C1.LiveLinq.LiveViews;

namespace LiveLinqIssueTrackerObjects.GUI
{
    public partial class ReassignFeatures : Form
    {
        IssueTracker _data;
        bool _dirty = false;
        int _currEmployeeId = -1;

        public ReassignFeatures(IssueTracker data)
            : this()
        {
            _data = data;
        }
        public ReassignFeatures()
        {
            InitializeComponent();
        }

        void ReassignFeatures_Load(object sender, EventArgs e)
        {
            employeesBindingSource.DataSource = _data.Employees;
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
                from p in _data.Products
                join f in _data.Features on p.ProductID equals f.ProductID into features
                select new {p, features};
            var indexedAssignments = _data.Assignments;
            var index = indexedAssignments.Indexes.Add(a => new {a.ProductID, a.FeatureID, a.EmployeeID});

            Index<AssignmentItem> productIndex = indexedAssignments.Indexes.Add(a => a.ProductID);

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
            foreach (AssignmentItem a in _data.GetAssignmentsByEmployeeID(_currEmployeeId).ToList())
                _data.Assignments.Remove(a);
            foreach (ProductTreeNode productNode in treeViewFeatures.Nodes)
                foreach (FeatureTreeNode featureNode in productNode.Nodes)
                    if (featureNode.Checked)
                    {
                        _data.Assignments.Add(new AssignmentItem
                        {
                            EmployeeID = _currEmployeeId,
                            ProductID = productNode.ProductID,
                            FeatureID = featureNode.FeatureID
                        });
                    }
        }
    }
}