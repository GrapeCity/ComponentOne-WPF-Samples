namespace LiveLinqIssueTrackerObjects.GUI
{
    partial class EmployeesIssues
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.comboAssignedTo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.employeesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // employeesBindingSource
            // 
            this.employeesBindingSource.DataMember = "Employees";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(1, 39);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(954, 324);
            this.dataGridView1.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "All issues for features assigned to:";
            // 
            // comboAssignedTo
            // 
            this.comboAssignedTo.DataSource = this.employeesBindingSource;
            this.comboAssignedTo.DisplayMember = "FullName";
            this.comboAssignedTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAssignedTo.FormattingEnabled = true;
            this.comboAssignedTo.Location = new System.Drawing.Point(227, 12);
            this.comboAssignedTo.Name = "comboAssignedTo";
            this.comboAssignedTo.Size = new System.Drawing.Size(406, 21);
            this.comboAssignedTo.TabIndex = 17;
            this.comboAssignedTo.ValueMember = "EmployeeID";
            this.comboAssignedTo.SelectedIndexChanged += new System.EventHandler(this.comboAssignedTo_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(639, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "(includes issues assigned to other employees)";
            // 
            // EmployeesIssues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 375);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboAssignedTo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "EmployeesIssues";
            this.Text = "Issues for Features Assigned to an Employee";
            this.Load += new System.EventHandler(this.EmployeesIssues_Load);
            ((System.ComponentModel.ISupportInitialize)(this.employeesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource employeesBindingSource;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboAssignedTo;
        private System.Windows.Forms.Label label1;
    }
}