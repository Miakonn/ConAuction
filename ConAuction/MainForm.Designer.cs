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
            this.buttonDeleteProduct = new System.Windows.Forms.Button();
            this.buttonNewProduct = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonNewCustomer = new System.Windows.Forms.Button();
            this.buttonSendSMS = new System.Windows.Forms.Button();
            this.labelSoldAmount = new System.Windows.Forms.Label();
            this.textBoxAmount = new System.Windows.Forms.TextBox();
            this.labelSoldCount = new System.Windows.Forms.Label();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.textBoxSoldCount = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.buttonSoldFixedPrice = new System.Windows.Forms.Button();
            this.textBoxUnsold = new System.Windows.Forms.TextBox();
            this.labelUnsoldPerCustomer = new System.Windows.Forms.Label();
            this.textBoxTotalAmount = new System.Windows.Forms.TextBox();
            this.labelNetPerCustomer = new System.Windows.Forms.Label();
            this.labelTotalPerCustomer = new System.Windows.Forms.Label();
            this.textBoxNetAmount = new System.Windows.Forms.TextBox();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.comboBoxMode = new System.Windows.Forms.ComboBox();
            this.buttonSendResult = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCustomers)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSave.Location = new System.Drawing.Point(17, 587);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(116, 33);
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
            this.dataGridViewProducts.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.dataGridViewProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProducts.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewProducts.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.dataGridViewProducts.Location = new System.Drawing.Point(4, 4);
            this.dataGridViewProducts.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewProducts.Name = "dataGridViewProducts";
            this.dataGridViewProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewProducts.Size = new System.Drawing.Size(918, 557);
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
            this.dataGridViewCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCustomers.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewCustomers.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.dataGridViewCustomers.Location = new System.Drawing.Point(4, 4);
            this.dataGridViewCustomers.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewCustomers.MultiSelect = false;
            this.dataGridViewCustomers.Name = "dataGridViewCustomers";
            this.dataGridViewCustomers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCustomers.Size = new System.Drawing.Size(457, 557);
            this.dataGridViewCustomers.TabIndex = 14;
            this.dataGridViewCustomers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCustomers_CellClick);
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
            this.textBoxTotalCount.Location = new System.Drawing.Point(379, 585);
            this.textBoxTotalCount.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxTotalCount.Name = "textBoxTotalCount";
            this.textBoxTotalCount.ReadOnly = true;
            this.textBoxTotalCount.Size = new System.Drawing.Size(80, 26);
            this.textBoxTotalCount.TabIndex = 16;
            // 
            // buttonDeleteProduct
            // 
            this.buttonDeleteProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDeleteProduct.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDeleteProduct.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonDeleteProduct.Location = new System.Drawing.Point(27, 638);
            this.buttonDeleteProduct.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDeleteProduct.Name = "buttonDeleteProduct";
            this.buttonDeleteProduct.Size = new System.Drawing.Size(134, 33);
            this.buttonDeleteProduct.TabIndex = 24;
            this.buttonDeleteProduct.Text = "Radera objekt";
            this.buttonDeleteProduct.UseVisualStyleBackColor = true;
            this.buttonDeleteProduct.Click += new System.EventHandler(this.buttonDeleteProduct_Click);
            // 
            // buttonNewProduct
            // 
            this.buttonNewProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNewProduct.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNewProduct.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonNewProduct.Location = new System.Drawing.Point(27, 587);
            this.buttonNewProduct.Margin = new System.Windows.Forms.Padding(4);
            this.buttonNewProduct.Name = "buttonNewProduct";
            this.buttonNewProduct.Size = new System.Drawing.Size(134, 33);
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
            this.splitContainer1.Panel1.Controls.Add(this.buttonNewCustomer);
            this.splitContainer1.Panel1.Controls.Add(this.buttonSendSMS);
            this.splitContainer1.Panel1.Controls.Add(this.labelSoldAmount);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxAmount);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewCustomers);
            this.splitContainer1.Panel1.Controls.Add(this.labelSoldCount);
            this.splitContainer1.Panel1.Controls.Add(this.buttonUpdate);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxSoldCount);
            this.splitContainer1.Panel1.Controls.Add(this.buttonSave);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxTotalCount);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.buttonSendResult);
            this.splitContainer1.Panel2.Controls.Add(this.buttonSearch);
            this.splitContainer1.Panel2.Controls.Add(this.buttonSoldFixedPrice);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxUnsold);
            this.splitContainer1.Panel2.Controls.Add(this.buttonDeleteProduct);
            this.splitContainer1.Panel2.Controls.Add(this.labelUnsoldPerCustomer);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewProducts);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxTotalAmount);
            this.splitContainer1.Panel2.Controls.Add(this.labelNetPerCustomer);
            this.splitContainer1.Panel2.Controls.Add(this.labelTotalPerCustomer);
            this.splitContainer1.Panel2.Controls.Add(this.buttonNewProduct);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxNetAmount);
            this.splitContainer1.Size = new System.Drawing.Size(1396, 694);
            this.splitContainer1.SplitterDistance = 463;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 23;
            // 
            // buttonNewCustomer
            // 
            this.buttonNewCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNewCustomer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNewCustomer.Location = new System.Drawing.Point(157, 638);
            this.buttonNewCustomer.Margin = new System.Windows.Forms.Padding(4);
            this.buttonNewCustomer.Name = "buttonNewCustomer";
            this.buttonNewCustomer.Size = new System.Drawing.Size(116, 33);
            this.buttonNewCustomer.TabIndex = 31;
            this.buttonNewCustomer.Text = "Ny kund...";
            this.toolTip1.SetToolTip(this.buttonNewCustomer, "Skapa ny kund, F2 för att editera");
            this.buttonNewCustomer.UseVisualStyleBackColor = true;
            this.buttonNewCustomer.Click += new System.EventHandler(this.buttonNewCustomer_Click);
            // 
            // buttonSendSMS
            // 
            this.buttonSendSMS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSendSMS.Location = new System.Drawing.Point(17, 588);
            this.buttonSendSMS.Name = "buttonSendSMS";
            this.buttonSendSMS.Size = new System.Drawing.Size(116, 33);
            this.buttonSendSMS.TabIndex = 30;
            this.buttonSendSMS.Text = "Skicka SMS...";
            this.buttonSendSMS.UseVisualStyleBackColor = true;
            this.buttonSendSMS.Click += new System.EventHandler(this.buttonSendSMS_Click);
            // 
            // labelSoldAmount
            // 
            this.labelSoldAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSoldAmount.AutoSize = true;
            this.labelSoldAmount.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSoldAmount.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelSoldAmount.Location = new System.Drawing.Point(244, 655);
            this.labelSoldAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSoldAmount.Name = "labelSoldAmount";
            this.labelSoldAmount.Size = new System.Drawing.Size(98, 21);
            this.labelSoldAmount.TabIndex = 29;
            this.labelSoldAmount.Text = "Total summa";
            // 
            // textBoxAmount
            // 
            this.textBoxAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAmount.Location = new System.Drawing.Point(379, 654);
            this.textBoxAmount.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxAmount.Name = "textBoxAmount";
            this.textBoxAmount.ReadOnly = true;
            this.textBoxAmount.Size = new System.Drawing.Size(57, 26);
            this.textBoxAmount.TabIndex = 28;
            // 
            // labelSoldCount
            // 
            this.labelSoldCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSoldCount.AutoSize = true;
            this.labelSoldCount.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSoldCount.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelSoldCount.Location = new System.Drawing.Point(209, 621);
            this.labelSoldCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSoldCount.Name = "labelSoldCount";
            this.labelSoldCount.Size = new System.Drawing.Size(133, 21);
            this.labelSoldCount.TabIndex = 26;
            this.labelSoldCount.Text = "Antal sålda objekt";
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUpdate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUpdate.Location = new System.Drawing.Point(17, 638);
            this.buttonUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(116, 33);
            this.buttonUpdate.TabIndex = 27;
            this.buttonUpdate.Text = "Uppdatera";
            this.toolTip1.SetToolTip(this.buttonUpdate, "Skapa ny kund, F2 för att editera");
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // textBoxSoldCount
            // 
            this.textBoxSoldCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSoldCount.Location = new System.Drawing.Point(379, 620);
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
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(209, 588);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(132, 21);
            this.label6.TabIndex = 24;
            this.label6.Text = "Totalt antal objekt";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSearch.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSearch.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonSearch.Location = new System.Drawing.Point(242, 638);
            this.buttonSearch.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(134, 33);
            this.buttonSearch.TabIndex = 26;
            this.buttonSearch.Text = "Sök objekt...";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // buttonSoldFixedPrice
            // 
            this.buttonSoldFixedPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSoldFixedPrice.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSoldFixedPrice.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonSoldFixedPrice.Location = new System.Drawing.Point(242, 587);
            this.buttonSoldFixedPrice.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSoldFixedPrice.Name = "buttonSoldFixedPrice";
            this.buttonSoldFixedPrice.Size = new System.Drawing.Size(134, 33);
            this.buttonSoldFixedPrice.TabIndex = 25;
            this.buttonSoldFixedPrice.Text = "Såld på loppis";
            this.buttonSoldFixedPrice.UseVisualStyleBackColor = true;
            this.buttonSoldFixedPrice.Click += new System.EventHandler(this.buttonSoldFixedPrice_Click);
            // 
            // textBoxUnsold
            // 
            this.textBoxUnsold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUnsold.Location = new System.Drawing.Point(622, 588);
            this.textBoxUnsold.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxUnsold.Name = "textBoxUnsold";
            this.textBoxUnsold.ReadOnly = true;
            this.textBoxUnsold.Size = new System.Drawing.Size(57, 26);
            this.textBoxUnsold.TabIndex = 22;
            // 
            // labelUnsoldPerCustomer
            // 
            this.labelUnsoldPerCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelUnsoldPerCustomer.AutoSize = true;
            this.labelUnsoldPerCustomer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUnsoldPerCustomer.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelUnsoldPerCustomer.Location = new System.Drawing.Point(554, 593);
            this.labelUnsoldPerCustomer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUnsoldPerCustomer.Name = "labelUnsoldPerCustomer";
            this.labelUnsoldPerCustomer.Size = new System.Drawing.Size(58, 21);
            this.labelUnsoldPerCustomer.TabIndex = 23;
            this.labelUnsoldPerCustomer.Text = "Osålda";
            this.labelUnsoldPerCustomer.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxTotalAmount
            // 
            this.textBoxTotalAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTotalAmount.Location = new System.Drawing.Point(803, 587);
            this.textBoxTotalAmount.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxTotalAmount.Name = "textBoxTotalAmount";
            this.textBoxTotalAmount.ReadOnly = true;
            this.textBoxTotalAmount.Size = new System.Drawing.Size(57, 26);
            this.textBoxTotalAmount.TabIndex = 18;
            // 
            // labelNetPerCustomer
            // 
            this.labelNetPerCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNetPerCustomer.AutoSize = true;
            this.labelNetPerCustomer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNetPerCustomer.Location = new System.Drawing.Point(746, 638);
            this.labelNetPerCustomer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNetPerCustomer.Name = "labelNetPerCustomer";
            this.labelNetPerCustomer.Size = new System.Drawing.Size(49, 21);
            this.labelNetPerCustomer.TabIndex = 21;
            this.labelNetPerCustomer.Text = "Netto";
            this.labelNetPerCustomer.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelTotalPerCustomer
            // 
            this.labelTotalPerCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTotalPerCustomer.AutoSize = true;
            this.labelTotalPerCustomer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalPerCustomer.Location = new System.Drawing.Point(744, 591);
            this.labelTotalPerCustomer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTotalPerCustomer.Name = "labelTotalPerCustomer";
            this.labelTotalPerCustomer.Size = new System.Drawing.Size(49, 21);
            this.labelTotalPerCustomer.TabIndex = 19;
            this.labelTotalPerCustomer.Text = "Saldo";
            this.labelTotalPerCustomer.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxNetAmount
            // 
            this.textBoxNetAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNetAmount.Location = new System.Drawing.Point(803, 635);
            this.textBoxNetAmount.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxNetAmount.Name = "textBoxNetAmount";
            this.textBoxNetAmount.ReadOnly = true;
            this.textBoxNetAmount.Size = new System.Drawing.Size(57, 26);
            this.textBoxNetAmount.TabIndex = 20;
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
            this.comboBoxMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMode.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.Location = new System.Drawing.Point(1181, 61);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(219, 38);
            this.comboBoxMode.TabIndex = 34;
            this.comboBoxMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxMode_SelectedIndexChanged);
            // 
            // buttonSendResult
            // 
            this.buttonSendResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSendResult.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSendResult.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonSendResult.Location = new System.Drawing.Point(242, 587);
            this.buttonSendResult.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSendResult.Name = "buttonSendResult";
            this.buttonSendResult.Size = new System.Drawing.Size(134, 33);
            this.buttonSendResult.TabIndex = 27;
            this.buttonSendResult.Text = "Skicka resultat...";
            this.buttonSendResult.UseVisualStyleBackColor = true;
            this.buttonSendResult.Click += new System.EventHandler(this.buttonSendResult_Click);
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
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
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
		private System.Windows.Forms.SplitContainer splitContainer1;
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
		private System.Windows.Forms.TextBox textBoxUnsold;
		private System.Windows.Forms.Label labelUnsoldPerCustomer;
		private System.Windows.Forms.TextBox textBoxTotalAmount;
		private System.Windows.Forms.Label labelNetPerCustomer;
		private System.Windows.Forms.Label labelTotalPerCustomer;
		private System.Windows.Forms.TextBox textBoxNetAmount;
		private System.Windows.Forms.Button buttonSendSMS;
		private System.Windows.Forms.Button buttonSoldFixedPrice;
        private System.Windows.Forms.Button buttonNewCustomer;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Button buttonSendResult;
	}
}

