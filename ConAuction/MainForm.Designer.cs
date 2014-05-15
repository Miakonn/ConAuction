namespace ConAuction
{
	partial class MainForm
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
			if (disposing && (components != null)) {
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
			this.buttonSave = new System.Windows.Forms.Button();
			this.dataGridViewProducts = new System.Windows.Forms.DataGridView();
			this.dataGridViewCustomers = new System.Windows.Forms.DataGridView();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxTotalCount = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxTotalAmount = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxNetAmount = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.buttonDeleteProduct = new System.Windows.Forms.Button();
			this.textBoxUnsold = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.buttonNewProduct = new System.Windows.Forms.Button();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.labelSoldAmount = new System.Windows.Forms.Label();
			this.textBoxAmount = new System.Windows.Forms.TextBox();
			this.buttonUpdate = new System.Windows.Forms.Button();
			this.labelSoldCount = new System.Windows.Forms.Label();
			this.textBoxSoldCount = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.comboBoxMode = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewCustomers)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonSave
			// 
			this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSave.Location = new System.Drawing.Point(8, 25);
			this.buttonSave.Margin = new System.Windows.Forms.Padding(4);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(97, 33);
			this.buttonSave.TabIndex = 7;
			this.buttonSave.Text = "Spara";
			this.buttonSave.UseVisualStyleBackColor = true;
			this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
			// 
			// dataGridViewProducts
			// 
			this.dataGridViewProducts.AllowUserToAddRows = false;
			this.dataGridViewProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridViewProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridViewProducts.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.dataGridViewProducts.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dataGridViewProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridViewProducts.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.dataGridViewProducts.GridColor = System.Drawing.SystemColors.ControlLight;
			this.dataGridViewProducts.Location = new System.Drawing.Point(4, 7);
			this.dataGridViewProducts.Margin = new System.Windows.Forms.Padding(4);
			this.dataGridViewProducts.Name = "dataGridViewProducts";
			this.dataGridViewProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridViewProducts.Size = new System.Drawing.Size(918, 554);
			this.dataGridViewProducts.TabIndex = 13;
			this.dataGridViewProducts.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewProducts_CellDoubleClick);
			this.dataGridViewProducts.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewProducts_CellValueChanged);
			this.dataGridViewProducts.SelectionChanged += new System.EventHandler(this.dataGridViewProducts_SelectionChanged);
			this.dataGridViewProducts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewProducts_KeyDown);
			// 
			// dataGridViewCustomers
			// 
			this.dataGridViewCustomers.AllowUserToResizeRows = false;
			this.dataGridViewCustomers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridViewCustomers.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.dataGridViewCustomers.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dataGridViewCustomers.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
			this.dataGridViewCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridViewCustomers.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.dataGridViewCustomers.GridColor = System.Drawing.SystemColors.ControlLight;
			this.dataGridViewCustomers.Location = new System.Drawing.Point(9, 4);
			this.dataGridViewCustomers.Margin = new System.Windows.Forms.Padding(4);
			this.dataGridViewCustomers.MultiSelect = false;
			this.dataGridViewCustomers.Name = "dataGridViewCustomers";
			this.dataGridViewCustomers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridViewCustomers.Size = new System.Drawing.Size(444, 557);
			this.dataGridViewCustomers.TabIndex = 14;
			this.dataGridViewCustomers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCustomers_CellValueChanged);
			this.dataGridViewCustomers.SelectionChanged += new System.EventHandler(this.dataGridViewCustomers_SelectionChanged);
			this.dataGridViewCustomers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewCustomers_KeyDown);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Impact", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(207, 88);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(178, 34);
			this.label1.TabIndex = 15;
			this.label1.Text = "LinCon Auction";
			// 
			// textBoxTotalCount
			// 
			this.textBoxTotalCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxTotalCount.Location = new System.Drawing.Point(372, 24);
			this.textBoxTotalCount.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxTotalCount.Name = "textBoxTotalCount";
			this.textBoxTotalCount.ReadOnly = true;
			this.textBoxTotalCount.Size = new System.Drawing.Size(57, 26);
			this.textBoxTotalCount.TabIndex = 16;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(172, 596);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(93, 20);
			this.label2.TabIndex = 17;
			this.label2.Text = "Antal objekt";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(791, 24);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(50, 20);
			this.label3.TabIndex = 19;
			this.label3.Text = "Saldo";
			// 
			// textBoxTotalAmount
			// 
			this.textBoxTotalAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxTotalAmount.Location = new System.Drawing.Point(851, 20);
			this.textBoxTotalAmount.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxTotalAmount.Name = "textBoxTotalAmount";
			this.textBoxTotalAmount.ReadOnly = true;
			this.textBoxTotalAmount.Size = new System.Drawing.Size(57, 26);
			this.textBoxTotalAmount.TabIndex = 18;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(791, 72);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 20);
			this.label4.TabIndex = 21;
			this.label4.Text = "Netto";
			// 
			// textBoxNetAmount
			// 
			this.textBoxNetAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxNetAmount.Location = new System.Drawing.Point(851, 68);
			this.textBoxNetAmount.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxNetAmount.Name = "textBoxNetAmount";
			this.textBoxNetAmount.ReadOnly = true;
			this.textBoxNetAmount.Size = new System.Drawing.Size(57, 26);
			this.textBoxNetAmount.TabIndex = 20;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.groupBox1.Controls.Add(this.buttonDeleteProduct);
			this.groupBox1.Controls.Add(this.textBoxUnsold);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.textBoxTotalAmount);
			this.groupBox1.Controls.Add(this.buttonNewProduct);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.textBoxNetAmount);
			this.groupBox1.ForeColor = System.Drawing.Color.Black;
			this.groupBox1.Location = new System.Drawing.Point(5, 569);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
			this.groupBox1.Size = new System.Drawing.Size(918, 121);
			this.groupBox1.TabIndex = 22;
			this.groupBox1.TabStop = false;
			// 
			// buttonDeleteProduct
			// 
			this.buttonDeleteProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonDeleteProduct.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.buttonDeleteProduct.Location = new System.Drawing.Point(26, 76);
			this.buttonDeleteProduct.Margin = new System.Windows.Forms.Padding(4);
			this.buttonDeleteProduct.Name = "buttonDeleteProduct";
			this.buttonDeleteProduct.Size = new System.Drawing.Size(106, 33);
			this.buttonDeleteProduct.TabIndex = 24;
			this.buttonDeleteProduct.Text = "Radera objekt";
			this.buttonDeleteProduct.UseVisualStyleBackColor = true;
			this.buttonDeleteProduct.Click += new System.EventHandler(this.buttonDeleteProduct_Click);
			// 
			// textBoxUnsold
			// 
			this.textBoxUnsold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxUnsold.Location = new System.Drawing.Point(708, 68);
			this.textBoxUnsold.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxUnsold.Name = "textBoxUnsold";
			this.textBoxUnsold.ReadOnly = true;
			this.textBoxUnsold.Size = new System.Drawing.Size(57, 26);
			this.textBoxUnsold.TabIndex = 22;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label5.Location = new System.Drawing.Point(648, 72);
			this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(59, 20);
			this.label5.TabIndex = 23;
			this.label5.Text = "Osålda";
			// 
			// buttonNewProduct
			// 
			this.buttonNewProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonNewProduct.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.buttonNewProduct.Location = new System.Drawing.Point(26, 25);
			this.buttonNewProduct.Margin = new System.Windows.Forms.Padding(4);
			this.buttonNewProduct.Name = "buttonNewProduct";
			this.buttonNewProduct.Size = new System.Drawing.Size(106, 33);
			this.buttonNewProduct.TabIndex = 12;
			this.buttonNewProduct.Text = "Nytt objekt...";
			this.buttonNewProduct.UseVisualStyleBackColor = true;
			this.buttonNewProduct.Click += new System.EventHandler(this.buttonNewProduct_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(18, 178);
			this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
			this.splitContainer1.Panel1.Controls.Add(this.dataGridViewCustomers);
			this.splitContainer1.Panel1.Controls.Add(this.label2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.dataGridViewProducts);
			this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
			this.splitContainer1.Size = new System.Drawing.Size(1396, 694);
			this.splitContainer1.SplitterDistance = 463;
			this.splitContainer1.SplitterWidth = 6;
			this.splitContainer1.TabIndex = 23;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.groupBox2.Controls.Add(this.labelSoldAmount);
			this.groupBox2.Controls.Add(this.textBoxAmount);
			this.groupBox2.Controls.Add(this.buttonUpdate);
			this.groupBox2.Controls.Add(this.labelSoldCount);
			this.groupBox2.Controls.Add(this.textBoxSoldCount);
			this.groupBox2.Controls.Add(this.buttonSave);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.textBoxTotalCount);
			this.groupBox2.Location = new System.Drawing.Point(9, 569);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
			this.groupBox2.Size = new System.Drawing.Size(440, 121);
			this.groupBox2.TabIndex = 18;
			this.groupBox2.TabStop = false;
			// 
			// labelSoldAmount
			// 
			this.labelSoldAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelSoldAmount.AutoSize = true;
			this.labelSoldAmount.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.labelSoldAmount.Location = new System.Drawing.Point(13, 76);
			this.labelSoldAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.labelSoldAmount.Name = "labelSoldAmount";
			this.labelSoldAmount.Size = new System.Drawing.Size(100, 20);
			this.labelSoldAmount.TabIndex = 29;
			this.labelSoldAmount.Text = "Total summa";
			// 
			// textBoxAmount
			// 
			this.textBoxAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxAmount.Location = new System.Drawing.Point(121, 68);
			this.textBoxAmount.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxAmount.Name = "textBoxAmount";
			this.textBoxAmount.ReadOnly = true;
			this.textBoxAmount.Size = new System.Drawing.Size(57, 26);
			this.textBoxAmount.TabIndex = 28;
			// 
			// buttonUpdate
			// 
			this.buttonUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonUpdate.Location = new System.Drawing.Point(120, 25);
			this.buttonUpdate.Margin = new System.Windows.Forms.Padding(4);
			this.buttonUpdate.Name = "buttonUpdate";
			this.buttonUpdate.Size = new System.Drawing.Size(97, 33);
			this.buttonUpdate.TabIndex = 27;
			this.buttonUpdate.Text = "Uppdatera";
			this.toolTip1.SetToolTip(this.buttonUpdate, "Skapa ny kund, F2 för att editera");
			this.buttonUpdate.UseVisualStyleBackColor = true;
			this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
			// 
			// labelSoldCount
			// 
			this.labelSoldCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelSoldCount.AutoSize = true;
			this.labelSoldCount.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.labelSoldCount.Location = new System.Drawing.Point(225, 79);
			this.labelSoldCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.labelSoldCount.Name = "labelSoldCount";
			this.labelSoldCount.Size = new System.Drawing.Size(135, 20);
			this.labelSoldCount.TabIndex = 26;
			this.labelSoldCount.Text = "Antal sålda objekt";
			// 
			// textBoxSoldCount
			// 
			this.textBoxSoldCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxSoldCount.Location = new System.Drawing.Point(372, 72);
			this.textBoxSoldCount.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxSoldCount.Name = "textBoxSoldCount";
			this.textBoxSoldCount.ReadOnly = true;
			this.textBoxSoldCount.Size = new System.Drawing.Size(57, 26);
			this.textBoxSoldCount.TabIndex = 25;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.AutoSize = true;
			this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label6.Location = new System.Drawing.Point(225, 31);
			this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(135, 20);
			this.label6.TabIndex = 24;
			this.label6.Text = "Totalt antal objekt";
			// 
			// pictureBoxLogo
			// 
			this.pictureBoxLogo.Location = new System.Drawing.Point(27, 19);
			this.pictureBoxLogo.Margin = new System.Windows.Forms.Padding(4);
			this.pictureBoxLogo.Name = "pictureBoxLogo";
			this.pictureBoxLogo.Size = new System.Drawing.Size(139, 144);
			this.pictureBoxLogo.TabIndex = 32;
			this.pictureBoxLogo.TabStop = false;
			// 
			// comboBoxMode
			// 
			this.comboBoxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.comboBoxMode.FormattingEnabled = true;
			this.comboBoxMode.Location = new System.Drawing.Point(1161, 60);
			this.comboBoxMode.Name = "comboBoxMode";
			this.comboBoxMode.Size = new System.Drawing.Size(219, 33);
			this.comboBoxMode.TabIndex = 34;
			this.comboBoxMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxMode_SelectedIndexChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.ClientSize = new System.Drawing.Size(1432, 881);
			this.Controls.Add(this.comboBoxMode);
			this.Controls.Add(this.pictureBoxLogo);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "MainForm";
			this.Text = "ConAuction";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewCustomers)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.DataGridView dataGridViewProducts;
		private System.Windows.Forms.DataGridView dataGridViewCustomers;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxTotalCount;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxTotalAmount;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxNetAmount;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TextBox textBoxUnsold;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.PictureBox pictureBoxLogo;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label labelSoldCount;
		private System.Windows.Forms.TextBox textBoxSoldCount;
		private System.Windows.Forms.Button buttonUpdate;
		private System.Windows.Forms.Button buttonNewProduct;
		private System.Windows.Forms.Label labelSoldAmount;
		private System.Windows.Forms.TextBox textBoxAmount;
		private System.Windows.Forms.Button buttonDeleteProduct;
		private System.Windows.Forms.ComboBox comboBoxMode;
	}
}

