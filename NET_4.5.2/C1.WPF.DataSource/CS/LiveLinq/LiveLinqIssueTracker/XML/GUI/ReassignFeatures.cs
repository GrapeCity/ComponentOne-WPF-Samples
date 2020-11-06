using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using LiveLinqIssueTrackerXML.Data;
using C1.LiveLinq.Indexing;
using C1.LiveLinq.LiveViews;

namespace LiveLinqIssueTrackerXML.GUI
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
                (from p in _data.Products
                 join f in _data.Features on p.ProductID equals f.ProductID into features
                 select new {p, features}).AsDynamic();
            var indexedAssignments = _data.Assignments;
            var index = indexedAssignments.Indexes.Add(a => new Tuple<int, int, int>(a.ProductID, a.FeatureID, a.EmployeeID));

            Index<Assignment> productIndex = indexedAssignments.Indexes.Add(a => a.ProductID);

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

                    bool isChecked = index.ContainsKey(new Tuple<int, int, int>(product.p.ProductID, feature.FeatureID, employeeID));
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
            for (int i = _data.Assignments.Count - 1; i >= 0; i--)
            {
                ViewRow row = _data.Assignments.Rows[i];
                if ((int)row["EmployeeID"] == _currEmployeeId)
                    row.Delete();
            }
            foreach (ProductTreeNode productNode in treeViewFeatures.Nodes)
                foreach (FeatureTreeNode featureNode in productNode.Nodes)
                    if (featureNode.Checked)
                    {
                        ViewRow newRow = _data.Assignments.Rows.CreateRow();
                        newRow["EmployeeID"] = _currEmployeeId;
                        newRow["ProductID"] = productNode.ProductID;
                        newRow["FeatureID"] = featureNode.FeatureID;
                        newRow.EndEdit();
                    }
        }
    }
}