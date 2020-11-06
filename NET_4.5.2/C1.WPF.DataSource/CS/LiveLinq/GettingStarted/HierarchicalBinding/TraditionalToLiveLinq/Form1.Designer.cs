namespace Traditional
{
    partial class Form1
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.nORTHWNDDataSet = new Traditional.NORTHWNDDataSet();
            this.categoriesTableAdapter = new Traditional.NORTHWNDDataSetTableAdapters.CategoriesTableAdapter();
            this.fKProductsCategoriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.productsTableAdapter = new Traditional.NORTHWNDDataSetTableAdapters.ProductsTableAdapter();
            this.productIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplierIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoryIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityPerUnitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unitPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unitsInStockDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unitsOnOrderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reorderLevelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discontinuedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nORTHWNDDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKProductsCategoriesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DataSource = this.bindingSource1;
            this.comboBox1.DisplayMember = "CategoryName";
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(326, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(338, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.ValueMember = "CategoryID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(268, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Category:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.productIDDataGridViewTextBoxColumn,
            this.productNameDataGridViewTextBoxColumn,
            this.supplierIDDataGridViewTextBoxColumn,
            this.categoryIDDataGridViewTextBoxColumn,
            this.quantityPerUnitDataGridViewTextBoxColumn,
            this.unitPriceDataGridViewTextBoxColumn,
            this.unitsInStockDataGridViewTextBoxColumn,
            this.unitsOnOrderDataGridViewTextBoxColumn,
            this.reorderLevelDataGridViewTextBoxColumn,
            this.discontinuedDataGridViewCheckBoxColumn});
            this.dataGridView1.DataSource = this.fKProductsCategoriesBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(12, 39);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(652, 217);
            this.dataGridView1.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.fKProductsCategoriesBindingSource, "ProductName", true));
            this.textBox1.Location = new System.Drawing.Point(302, 275);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(362, 20);
            this.textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.fKProductsCategoriesBindingSource, "UnitPrice", true));
            this.textBox2.Location = new System.Drawing.Point(302, 301);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(362, 20);
            this.textBox2.TabIndex = 4;
            // 
            // textBox3
            // 
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.fKProductsCategoriesBindingSource, "QuantityPerUnit", true));
            this.textBox3.Location = new System.Drawing.Point(302, 327);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(362, 20);
            this.textBox3.TabIndex = 5;
            // 
            // textBox4
            // 
            this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.fKProductsCategoriesBindingSource, "UnitsInStock", true));
            this.textBox4.Location = new System.Drawing.Point(302, 353);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(362, 20);
            this.textBox4.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(218, 278);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Product Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(240, 308);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Unit Price:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(207, 334);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Quantity per Unit:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(218, 356);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Units in Stock:";
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "Categories";
            this.bindingSource1.DataSource = this.nORTHWNDDataSet;
            // 
            // nORTHWNDDataSet
            // 
            this.nORTHWNDDataSet.DataSetName = "NORTHWNDDataSet";
            this.nORTHWNDDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // categoriesTableAdapter
            // 
            this.categoriesTableAdapter.ClearBeforeFill = true;
            // 
            // fKProductsCategoriesBindingSource
            // 
            this.fKProductsCategoriesBindingSource.DataMember = "FK_Products_Categories";
            this.fKProductsCategoriesBindingSource.DataSource = this.bindingSource1;
            // 
            // productsTableAdapter
            // 
            this.productsTableAdapter.ClearBeforeFill = true;
            // 
            // productIDDataGridViewTextBoxColumn
            // 
            this.productIDDataGridViewTextBoxColumn.DataPropertyName = "ProductID";
            this.productIDDataGridViewTextBoxColumn.HeaderText = "ProductID";
            this.productIDDataGridViewTextBoxColumn.Name = "productIDDataGridViewTextBoxColumn";
            this.productIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // productNameDataGridViewTextBoxColumn
            // 
            this.productNameDataGridViewTextBoxColumn.DataPropertyName = "ProductName";
            this.productNameDataGridViewTextBoxColumn.HeaderText = "ProductName";
            this.productNameDataGridViewTextBoxColumn.Name = "productNameDataGridViewTextBoxColumn";
            // 
            // supplierIDDataGridViewTextBoxColumn
            // 
            this.supplierIDDataGridViewTextBoxColumn.DataPropertyName = "SupplierID";
            this.supplierIDDataGridViewTextBoxColumn.HeaderText = "SupplierID";
            this.supplierIDDataGridViewTextBoxColumn.Name = "supplierIDDataGridViewTextBoxColumn";
            // 
            // categoryIDDataGridViewTextBoxColumn
            // 
            this.categoryIDDataGridViewTextBoxColumn.DataPropertyName = "CategoryID";
            this.categoryIDDataGridViewTextBoxColumn.HeaderText = "CategoryID";
            this.categoryIDDataGridViewTextBoxColumn.Name = "categoryIDDataGridViewTextBoxColumn";
            // 
            // quantityPerUnitDataGridViewTextBoxColumn
            // 
            this.quantityPerUnitDataGridViewTextBoxColumn.DataPropertyName = "QuantityPerUnit";
            this.quantityPerUnitDataGridViewTextBoxColumn.HeaderText = "QuantityPerUnit";
            this.quantityPerUnitDataGridViewTextBoxColumn.Name = "quantityPerUnitDataGridViewTextBoxColumn";
            // 
            // unitPriceDataGridViewTextBoxColumn
            // 
            this.unitPriceDataGridViewTextBoxColumn.DataPropertyName = "UnitPrice";
            this.unitPriceDataGridViewTextBoxColumn.HeaderText = "UnitPrice";
            this.unitPriceDataGridViewTextBoxColumn.Name = "unitPriceDataGridViewTextBoxColumn";
            // 
            // unitsInStockDataGridViewTextBoxColumn
            // 
            this.unitsInStockDataGridViewTextBoxColumn.DataPropertyName = "UnitsInStock";
            this.unitsInStockDataGridViewTextBoxColumn.HeaderText = "UnitsInStock";
            this.unitsInStockDataGridViewTextBoxColumn.Name = "unitsInStockDataGridViewTextBoxColumn";
            // 
            // unitsOnOrderDataGridViewTextBoxColumn
            // 
            this.unitsOnOrderDataGridViewTextBoxColumn.DataPropertyName = "UnitsOnOrder";
            this.unitsOnOrderDataGridViewTextBoxColumn.HeaderText = "UnitsOnOrder";
            this.unitsOnOrderDataGridViewTextBoxColumn.Name = "unitsOnOrderDataGridViewTextBoxColumn";
            // 
            // reorderLevelDataGridViewTextBoxColumn
            // 
            this.reorderLevelDataGridViewTextBoxColumn.DataPropertyName = "ReorderLevel";
            this.reorderLevelDataGridViewTextBoxColumn.HeaderText = "ReorderLevel";
            this.reorderLevelDataGridViewTextBoxColumn.Name = "reorderLevelDataGridViewTextBoxColumn";
            // 
            // discontinuedDataGridViewCheckBoxColumn
            // 
            this.discontinuedDataGridViewCheckBoxColumn.DataPropertyName = "Discontinued";
            this.discontinuedDataGridViewCheckBoxColumn.HeaderText = "Discontinued";
            this.discontinuedDataGridViewCheckBoxColumn.Name = "discontinuedDataGridViewCheckBoxColumn";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 395);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nORTHWNDDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKProductsCategoriesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.BindingSource bindingSource1;
        private NORTHWNDDataSet nORTHWNDDataSet;
        private Traditional.NORTHWNDDataSetTableAdapters.CategoriesTableAdapter categoriesTableAdapter;
        private System.Windows.Forms.BindingSource fKProductsCategoriesBindingSource;
        private Traditional.NORTHWNDDataSetTableAdapters.ProductsTableAdapter productsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplierIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoryIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityPerUnitDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn unitPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn unitsInStockDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn unitsOnOrderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reorderLevelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn discontinuedDataGridViewCheckBoxColumn;
    }
}

