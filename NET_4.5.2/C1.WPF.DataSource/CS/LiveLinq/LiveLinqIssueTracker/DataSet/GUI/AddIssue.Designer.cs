namespace LiveLinqIssueTrackerDataSet.GUI
{
    partial class AddIssue
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
            this.txtIssueID = new System.Windows.Forms.TextBox();
            this.comboProduct = new System.Windows.Forms.ComboBox();
            this.productsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.issueTracking = new LiveLinqIssueTrackerDataSet.Data.IssueTracking();
            this.comboFeature = new System.Windows.Forms.ComboBox();
            this.fKProductsFeaturesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.featuresBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.comboAssignedTo = new System.Windows.Forms.ComboBox();
            this.employeesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.productsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.issueTracking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKProductsFeaturesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.featuresBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // txtIssueID
            // 
            this.txtIssueID.Location = new System.Drawing.Point(415, 24);
            this.txtIssueID.Name = "txtIssueID";
            this.txtIssueID.Size = new System.Drawing.Size(117, 20);
            this.txtIssueID.TabIndex = 0;
            // 
            // comboProduct
            // 
            this.comboProduct.DataSource = this.productsBindingSource;
            this.comboProduct.DisplayMember = "ProductName";
            this.comboProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProduct.FormattingEnabled = true;
            this.comboProduct.Location = new System.Drawing.Point(124, 62);
            this.comboProduct.Name = "comboProduct";
            this.comboProduct.Size = new System.Drawing.Size(406, 21);
            this.comboProduct.TabIndex = 1;
            this.comboProduct.ValueMember = "ProductID";
            // 
            // productsBindingSource
            // 
            this.productsBindingSource.DataMember = "Products";
            this.productsBindingSource.DataSource = this.issueTracking;
            // 
            // issueTracking
            // 
            this.issueTracking.DataSetName = "IssueTracking";
            this.issueTracking.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // comboFeature
            // 
            this.comboFeature.DataSource = this.fKProductsFeaturesBindingSource;
            this.comboFeature.DisplayMember = "FeatureName";
            this.comboFeature.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFeature.FormattingEnabled = true;
            this.comboFeature.Location = new System.Drawing.Point(124, 101);
            this.comboFeature.Name = "comboFeature";
            this.comboFeature.Size = new System.Drawing.Size(406, 21);
            this.comboFeature.TabIndex = 2;
            this.comboFeature.ValueMember = "FeatureID";
            // 
            // fKProductsFeaturesBindingSource
            // 
            this.fKProductsFeaturesBindingSource.DataMember = "FK_Products_Features";
            this.fKProductsFeaturesBindingSource.DataSource = this.productsBindingSource;
            // 
            // featuresBindingSource
            // 
            this.featuresBindingSource.DataSource = this.fKProductsFeaturesBindingSource;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(43, 197);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(487, 168);
            this.txtDescription.TabIndex = 3;
            // 
            // comboAssignedTo
            // 
            this.comboAssignedTo.DataSource = this.employeesBindingSource;
            this.comboAssignedTo.DisplayMember = "FullName";
            this.comboAssignedTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAssignedTo.FormattingEnabled = true;
            this.comboAssignedTo.Location = new System.Drawing.Point(124, 140);
            this.comboAssignedTo.Name = "comboAssignedTo";
            this.comboAssignedTo.Size = new System.Drawing.Size(406, 21);
            this.comboAssignedTo.TabIndex = 4;
            this.comboAssignedTo.ValueMember = "EmployeeID";
            // 
            // employeesBindingSource
            // 
            this.employeesBindingSource.DataMember = "Employees";
            this.employeesBindingSource.DataSource = this.issueTracking;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(360, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Issue ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Product:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Feature:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Assigned To:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 181);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Description:";
            // 
            // btnAdd
            // 
            this.btnAdd.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAdd.Location = new System.Drawing.Point(363, 382);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 12;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(455, 382);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AddIssue
            // 
            this.AcceptButton = this.btnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(539, 419);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboAssignedTo);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.comboFeature);
            this.Controls.Add(this.comboProduct);
            this.Controls.Add(this.txtIssueID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AddIssue";
            this.Text = "Add New Issue";
            this.Load += new System.EventHandler(this.AddIssue_Load);
            ((System.ComponentModel.ISupportInitialize)(this.productsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.issueTracking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKProductsFeaturesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.featuresBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIssueID;
        private System.Windows.Forms.ComboBox comboProduct;
        private System.Windows.Forms.ComboBox comboFeature;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.ComboBox comboAssignedTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.BindingSource productsBindingSource;
        private LiveLinqIssueTrackerDataSet.Data.IssueTracking issueTracking;
        private System.Windows.Forms.BindingSource featuresBindingSource;
        private System.Windows.Forms.BindingSource employeesBindingSource;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.BindingSource fKProductsFeaturesBindingSource;
    }
}