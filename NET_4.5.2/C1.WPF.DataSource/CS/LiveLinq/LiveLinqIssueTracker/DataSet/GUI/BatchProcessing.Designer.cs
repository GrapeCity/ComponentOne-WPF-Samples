namespace LiveLinqIssueTrackerDataSet.GUI
{
    partial class BatchProcessing
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
            this.buttonAssign = new System.Windows.Forms.Button();
            this.issueTracking = new LiveLinqIssueTrackerDataSet.Data.IssueTracking();
            this.employeesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.comboAssignedTo = new System.Windows.Forms.ComboBox();
            this.buttonGetIssues = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.issueTracking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonAssign
            // 
            this.buttonAssign.Location = new System.Drawing.Point(55, 44);
            this.buttonAssign.Name = "buttonAssign";
            this.buttonAssign.Size = new System.Drawing.Size(164, 23);
            this.buttonAssign.TabIndex = 0;
            this.buttonAssign.Text = "Assign Issues to Employees";
            this.buttonAssign.UseVisualStyleBackColor = true;
            this.buttonAssign.Click += new System.EventHandler(this.buttonAssign_Click);
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
            // comboAssignedTo
            // 
            this.comboAssignedTo.DataSource = this.employeesBindingSource;
            this.comboAssignedTo.DisplayMember = "FullName";
            this.comboAssignedTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAssignedTo.FormattingEnabled = true;
            this.comboAssignedTo.Location = new System.Drawing.Point(55, 135);
            this.comboAssignedTo.Name = "comboAssignedTo";
            this.comboAssignedTo.Size = new System.Drawing.Size(307, 21);
            this.comboAssignedTo.TabIndex = 15;
            this.comboAssignedTo.ValueMember = "EmployeeID";
            this.comboAssignedTo.SelectedIndexChanged += new System.EventHandler(this.comboAssignedTo_SelectedIndexChanged);
            // 
            // buttonGetIssues
            // 
            this.buttonGetIssues.Location = new System.Drawing.Point(55, 106);
            this.buttonGetIssues.Name = "buttonGetIssues";
            this.buttonGetIssues.Size = new System.Drawing.Size(164, 23);
            this.buttonGetIssues.TabIndex = 16;
            this.buttonGetIssues.Text = "Get the list of open issues for:";
            this.buttonGetIssues.UseVisualStyleBackColor = true;
            this.buttonGetIssues.Click += new System.EventHandler(this.buttonGetIssues_Click);
            // 
            // BatchProcessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 216);
            this.Controls.Add(this.buttonGetIssues);
            this.Controls.Add(this.comboAssignedTo);
            this.Controls.Add(this.buttonAssign);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "BatchProcessing";
            this.Text = "Batch Processing";
            this.Load += new System.EventHandler(this.BatchProcessing_Load);
            ((System.ComponentModel.ISupportInitialize)(this.issueTracking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeesBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAssign;
        private LiveLinqIssueTrackerDataSet.Data.IssueTracking issueTracking;
        private System.Windows.Forms.BindingSource employeesBindingSource;
        private System.Windows.Forms.ComboBox comboAssignedTo;
        private System.Windows.Forms.Button buttonGetIssues;
    }
}