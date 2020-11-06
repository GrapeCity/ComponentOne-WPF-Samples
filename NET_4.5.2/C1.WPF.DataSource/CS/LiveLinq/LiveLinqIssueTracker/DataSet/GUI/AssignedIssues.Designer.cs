namespace LiveLinqIssueTrackerDataSet.GUI
{
    partial class AssignedIssues
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
            this.issueTracking = new LiveLinqIssueTrackerDataSet.Data.IssueTracking();
            this.employeesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.comboAssignedTo = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.radioButtonImmediate = new System.Windows.Forms.RadioButton();
            this.radioButtonDeferred = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.issueTracking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // issueTracking
            // 
            this.issueTracking.DataSetName = "IssueTracking";
            this.issueTracking.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // employeesBindingSource
            // 
            this.employeesBindingSource.DataMember = "Employees";
            this.employeesBindingSource.DataSource = this.issueTracking;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(246, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Issues assigned to:";
            // 
            // comboAssignedTo
            // 
            this.comboAssignedTo.DataSource = this.employeesBindingSource;
            this.comboAssignedTo.DisplayMember = "FullName";
            this.comboAssignedTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAssignedTo.FormattingEnabled = true;
            this.comboAssignedTo.Location = new System.Drawing.Point(364, 12);
            this.comboAssignedTo.Name = "comboAssignedTo";
            this.comboAssignedTo.Size = new System.Drawing.Size(406, 21);
            this.comboAssignedTo.TabIndex = 9;
            this.comboAssignedTo.ValueMember = "EmployeeID";
            this.comboAssignedTo.SelectedIndexChanged += new System.EventHandler(this.comboAssignedTo_SelectedIndexChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 97);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(964, 283);
            this.dataGridView1.TabIndex = 11;
            // 
            // radioButtonImmediate
            // 
            this.radioButtonImmediate.AutoSize = true;
            this.radioButtonImmediate.Checked = true;
            this.radioButtonImmediate.Location = new System.Drawing.Point(46, 38);
            this.radioButtonImmediate.Name = "radioButtonImmediate";
            this.radioButtonImmediate.Size = new System.Drawing.Size(109, 17);
            this.radioButtonImmediate.TabIndex = 12;
            this.radioButtonImmediate.TabStop = true;
            this.radioButtonImmediate.Text = "Immediate update";
            this.radioButtonImmediate.UseVisualStyleBackColor = true;
            this.radioButtonImmediate.CheckedChanged += new System.EventHandler(this.radioButtonImmediate_CheckedChanged);
            // 
            // radioButtonDeferred
            // 
            this.radioButtonDeferred.AutoSize = true;
            this.radioButtonDeferred.Location = new System.Drawing.Point(46, 61);
            this.radioButtonDeferred.Name = "radioButtonDeferred";
            this.radioButtonDeferred.Size = new System.Drawing.Size(116, 17);
            this.radioButtonDeferred.TabIndex = 13;
            this.radioButtonDeferred.Text = "Update on demand";
            this.radioButtonDeferred.UseVisualStyleBackColor = true;
            // 
            // AssignedIssues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 394);
            this.Controls.Add(this.radioButtonDeferred);
            this.Controls.Add(this.radioButtonImmediate);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboAssignedTo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AssignedIssues";
            this.Text = "Issues Assigned to an Employee";
            this.Load += new System.EventHandler(this.AssignedIssues_Load);
            ((System.ComponentModel.ISupportInitialize)(this.issueTracking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LiveLinqIssueTrackerDataSet.Data.IssueTracking issueTracking;
        private System.Windows.Forms.BindingSource employeesBindingSource;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboAssignedTo;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.RadioButton radioButtonImmediate;
        private System.Windows.Forms.RadioButton radioButtonDeferred;
    }
}