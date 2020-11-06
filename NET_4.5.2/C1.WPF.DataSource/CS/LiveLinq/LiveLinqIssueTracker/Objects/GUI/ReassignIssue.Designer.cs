namespace LiveLinqIssueTrackerObjects.GUI
{
    partial class ReassignIssue
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
            this.employeesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.employeesBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.comboAssignedTo = new System.Windows.Forms.ComboBox();
            this.comboReassign = new System.Windows.Forms.ComboBox();
            this.buttonReassign = new System.Windows.Forms.Button();
            this.issuesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.employeesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeesBindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.issuesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // employeesBindingSource
            // 
            this.employeesBindingSource.DataMember = "Employees";
            // 
            // employeesBindingSource2
            // 
            this.employeesBindingSource2.DataMember = "Employees";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 39);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(964, 412);
            this.dataGridView1.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(224, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Issues assigned to:";
            // 
            // comboAssignedTo
            // 
            this.comboAssignedTo.DataSource = this.employeesBindingSource;
            this.comboAssignedTo.DisplayMember = "FullName";
            this.comboAssignedTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAssignedTo.FormattingEnabled = true;
            this.comboAssignedTo.Location = new System.Drawing.Point(342, 12);
            this.comboAssignedTo.Name = "comboAssignedTo";
            this.comboAssignedTo.Size = new System.Drawing.Size(406, 21);
            this.comboAssignedTo.TabIndex = 14;
            this.comboAssignedTo.ValueMember = "EmployeeID";
            this.comboAssignedTo.SelectedIndexChanged += new System.EventHandler(this.comboAssignedTo_SelectedIndexChanged);
            // 
            // comboReassign
            // 
            this.comboReassign.DataSource = this.employeesBindingSource2;
            this.comboReassign.DisplayMember = "FullName";
            this.comboReassign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboReassign.FormattingEnabled = true;
            this.comboReassign.Location = new System.Drawing.Point(383, 470);
            this.comboReassign.Name = "comboReassign";
            this.comboReassign.Size = new System.Drawing.Size(406, 21);
            this.comboReassign.TabIndex = 17;
            this.comboReassign.ValueMember = "EmployeeID";
            // 
            // buttonReassign
            // 
            this.buttonReassign.Location = new System.Drawing.Point(195, 468);
            this.buttonReassign.Name = "buttonReassign";
            this.buttonReassign.Size = new System.Drawing.Size(182, 25);
            this.buttonReassign.TabIndex = 19;
            this.buttonReassign.Text = "Reassign selected issue to:";
            this.buttonReassign.UseVisualStyleBackColor = true;
            this.buttonReassign.Click += new System.EventHandler(this.buttonReassign_Click);
            // 
            // ReassignIssue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 503);
            this.Controls.Add(this.buttonReassign);
            this.Controls.Add(this.comboReassign);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboAssignedTo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ReassignIssue";
            this.Text = "Reassign Issues";
            this.Load += new System.EventHandler(this.ReassignIssue_Load);
            ((System.ComponentModel.ISupportInitialize)(this.employeesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeesBindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.issuesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource employeesBindingSource;
        private System.Windows.Forms.BindingSource employeesBindingSource2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboAssignedTo;
        private System.Windows.Forms.ComboBox comboReassign;
        private System.Windows.Forms.Button buttonReassign;
        private System.Windows.Forms.BindingSource issuesBindingSource;
    }
}