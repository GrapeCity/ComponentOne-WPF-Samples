namespace LiveLinqIssueTrackerXML.GUI
{
    partial class ReassignFeatures
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label4 = new System.Windows.Forms.Label();
            this.comboAssignedTo = new System.Windows.Forms.ComboBox();
            this.employeesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.treeViewFeatures = new System.Windows.Forms.TreeView();
            this.buttonApply = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.employeesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(77, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Features assigned to:";
            // 
            // comboAssignedTo
            // 
            this.comboAssignedTo.DataSource = this.employeesBindingSource;
            this.comboAssignedTo.DisplayMember = "FullName";
            this.comboAssignedTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAssignedTo.FormattingEnabled = true;
            this.comboAssignedTo.Location = new System.Drawing.Point(195, 18);
            this.comboAssignedTo.Name = "comboAssignedTo";
            this.comboAssignedTo.Size = new System.Drawing.Size(406, 21);
            this.comboAssignedTo.TabIndex = 11;
            this.comboAssignedTo.ValueMember = "EmployeeID";
            this.comboAssignedTo.SelectedIndexChanged += new System.EventHandler(this.comboAssignedTo_SelectedIndexChanged);
            // 
            // employeesBindingSource
            // 
            this.employeesBindingSource.DataMember = "Employees";
            // 
            // treeViewFeatures
            // 
            this.treeViewFeatures.CheckBoxes = true;
            this.treeViewFeatures.Location = new System.Drawing.Point(33, 67);
            this.treeViewFeatures.Name = "treeViewFeatures";
            this.treeViewFeatures.Size = new System.Drawing.Size(596, 311);
            this.treeViewFeatures.TabIndex = 13;
            this.treeViewFeatures.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFeatures_AfterCheck);
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(307, 396);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 14;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // ReassignFeatures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 435);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.treeViewFeatures);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboAssignedTo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ReassignFeatures";
            this.Text = "Change Assignments of Features to Employees";
            this.Load += new System.EventHandler(this.ReassignFeatures_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReassignFeatures_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.employeesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource employeesBindingSource;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboAssignedTo;
        private System.Windows.Forms.TreeView treeViewFeatures;
        private System.Windows.Forms.Button buttonApply;
    }
}