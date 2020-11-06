namespace LiveLinqToXML
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
            this.joinButton = new System.Windows.Forms.Button();
            this.simpleButton = new System.Windows.Forms.Button();
            this.indexesButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.simpleButtonImplicitIndexing = new System.Windows.Forms.Button();
            this.joinButtonImplicitIndexing = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // joinButton
            // 
            this.joinButton.Location = new System.Drawing.Point(377, 21);
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size(131, 23);
            this.joinButton.TabIndex = 1;
            this.joinButton.Text = "Query with a Join";
            this.joinButton.UseVisualStyleBackColor = true;
            this.joinButton.Click += new System.EventHandler(this.joinButton_Click);
            // 
            // simpleButton
            // 
            this.simpleButton.Location = new System.Drawing.Point(195, 21);
            this.simpleButton.Name = "simpleButton";
            this.simpleButton.Size = new System.Drawing.Size(131, 23);
            this.simpleButton.TabIndex = 2;
            this.simpleButton.Text = "Simple query";
            this.simpleButton.UseVisualStyleBackColor = true;
            this.simpleButton.Click += new System.EventHandler(this.simpleButton_Click);
            // 
            // indexesButton
            // 
            this.indexesButton.Location = new System.Drawing.Point(6, 21);
            this.indexesButton.Name = "indexesButton";
            this.indexesButton.Size = new System.Drawing.Size(131, 23);
            this.indexesButton.TabIndex = 3;
            this.indexesButton.Text = "Create indexes";
            this.indexesButton.UseVisualStyleBackColor = true;
            this.indexesButton.Click += new System.EventHandler(this.indexesButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.simpleButtonImplicitIndexing);
            this.groupBox1.Controls.Add(this.joinButtonImplicitIndexing);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(572, 64);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Indexes created implicitly";
            // 
            // simpleButtonImplicitIndexing
            // 
            this.simpleButtonImplicitIndexing.Location = new System.Drawing.Point(194, 21);
            this.simpleButtonImplicitIndexing.Name = "simpleButtonImplicitIndexing";
            this.simpleButtonImplicitIndexing.Size = new System.Drawing.Size(131, 23);
            this.simpleButtonImplicitIndexing.TabIndex = 4;
            this.simpleButtonImplicitIndexing.Text = "Simple query";
            this.simpleButtonImplicitIndexing.UseVisualStyleBackColor = true;
            this.simpleButtonImplicitIndexing.Click += new System.EventHandler(this.simpleButtonImplicitIndexing_Click);
            // 
            // joinButtonImplicitIndexing
            // 
            this.joinButtonImplicitIndexing.Location = new System.Drawing.Point(376, 21);
            this.joinButtonImplicitIndexing.Name = "joinButtonImplicitIndexing";
            this.joinButtonImplicitIndexing.Size = new System.Drawing.Size(131, 23);
            this.joinButtonImplicitIndexing.TabIndex = 3;
            this.joinButtonImplicitIndexing.Text = "Query with a Join";
            this.joinButtonImplicitIndexing.UseVisualStyleBackColor = true;
            this.joinButtonImplicitIndexing.Click += new System.EventHandler(this.joinButtonImplicitIndexing_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.simpleButton);
            this.groupBox2.Controls.Add(this.joinButton);
            this.groupBox2.Controls.Add(this.indexesButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(573, 64);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Indexes created explicitly";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.Location = new System.Drawing.Point(12, 178);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(573, 238);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 428);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "LiveLinq to XML Quick Start";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.Button simpleButton;
        private System.Windows.Forms.Button indexesButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button simpleButtonImplicitIndexing;
        private System.Windows.Forms.Button joinButtonImplicitIndexing;
        private System.Windows.Forms.ListView listView1;
    }
}

