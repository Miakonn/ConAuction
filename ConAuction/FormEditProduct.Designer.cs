﻿namespace ConAuction
{
	partial class FormEditProduct
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
			this.buttonCancelProduct = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxProductNote = new System.Windows.Forms.TextBox();
			this.buttonSaveProduct = new System.Windows.Forms.Button();
			this.comboBoxProductType = new System.Windows.Forms.ComboBox();
			this.textBoxProductDescription = new System.Windows.Forms.TextBox();
			this.textBoxProductLabel = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxName = new System.Windows.Forms.TextBox();
			this.buttonCopy = new System.Windows.Forms.Button();
			this.textBoxFixedPrice = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxCustomer = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonCancelProduct
			// 
			this.buttonCancelProduct.Location = new System.Drawing.Point(595, 380);
			this.buttonCancelProduct.Margin = new System.Windows.Forms.Padding(4);
			this.buttonCancelProduct.Name = "buttonCancelProduct";
			this.buttonCancelProduct.Size = new System.Drawing.Size(112, 36);
			this.buttonCancelProduct.TabIndex = 5;
			this.buttonCancelProduct.Text = "Avbryt";
			this.buttonCancelProduct.UseVisualStyleBackColor = true;
			this.buttonCancelProduct.Click += new System.EventHandler(this.buttonCancelProduct_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(601, 91);
			this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(69, 20);
			this.label6.TabIndex = 8;
			this.label6.Text = "Notering";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(46, 27);
			this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(23, 20);
			this.label5.TabIndex = 7;
			this.label5.Text = "Id";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(181, 27);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(34, 20);
			this.label4.TabIndex = 6;
			this.label4.Text = "Typ";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(46, 159);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(90, 20);
			this.label3.TabIndex = 5;
			this.label3.Text = "Beskrivning";
			// 
			// textBoxProductNote
			// 
			this.textBoxProductNote.Location = new System.Drawing.Point(605, 115);
			this.textBoxProductNote.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxProductNote.MaxLength = 15;
			this.textBoxProductNote.Name = "textBoxProductNote";
			this.textBoxProductNote.Size = new System.Drawing.Size(103, 26);
			this.textBoxProductNote.TabIndex = 3;
			// 
			// buttonSaveProduct
			// 
			this.buttonSaveProduct.Location = new System.Drawing.Point(474, 380);
			this.buttonSaveProduct.Margin = new System.Windows.Forms.Padding(4);
			this.buttonSaveProduct.Name = "buttonSaveProduct";
			this.buttonSaveProduct.Size = new System.Drawing.Size(112, 36);
			this.buttonSaveProduct.TabIndex = 4;
			this.buttonSaveProduct.Text = "Spara";
			this.buttonSaveProduct.UseVisualStyleBackColor = true;
			this.buttonSaveProduct.Click += new System.EventHandler(this.buttonSaveProduct_Click);
			// 
			// comboBoxProductType
			// 
			this.comboBoxProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxProductType.FormattingEnabled = true;
			this.comboBoxProductType.Location = new System.Drawing.Point(180, 53);
			this.comboBoxProductType.Margin = new System.Windows.Forms.Padding(4);
			this.comboBoxProductType.Name = "comboBoxProductType";
			this.comboBoxProductType.Size = new System.Drawing.Size(121, 28);
			this.comboBoxProductType.TabIndex = 0;
			this.comboBoxProductType.SelectedIndexChanged += new System.EventHandler(this.comboBoxProductType_SelectedIndexChanged);
			// 
			// textBoxProductDescription
			// 
			this.textBoxProductDescription.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
			this.textBoxProductDescription.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.textBoxProductDescription.Location = new System.Drawing.Point(50, 182);
			this.textBoxProductDescription.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxProductDescription.MaxLength = 250;
			this.textBoxProductDescription.Multiline = true;
			this.textBoxProductDescription.Name = "textBoxProductDescription";
			this.textBoxProductDescription.Size = new System.Drawing.Size(658, 185);
			this.textBoxProductDescription.TabIndex = 2;
			this.textBoxProductDescription.TextChanged += new System.EventHandler(this.textBoxProductDescription_TextChanged);
			// 
			// textBoxProductLabel
			// 
			this.textBoxProductLabel.Location = new System.Drawing.Point(51, 53);
			this.textBoxProductLabel.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxProductLabel.Name = "textBoxProductLabel";
			this.textBoxProductLabel.ReadOnly = true;
			this.textBoxProductLabel.Size = new System.Drawing.Size(80, 26);
			this.textBoxProductLabel.TabIndex = 0;
			this.textBoxProductLabel.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(348, 27);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 20);
			this.label1.TabIndex = 13;
			this.label1.Text = "Namn";
			// 
			// textBoxName
			// 
			this.textBoxName.Location = new System.Drawing.Point(352, 53);
			this.textBoxName.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxName.MaxLength = 40;
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.Size = new System.Drawing.Size(356, 26);
			this.textBoxName.TabIndex = 1;
			this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
			// 
			// buttonCopy
			// 
			this.buttonCopy.Location = new System.Drawing.Point(51, 380);
			this.buttonCopy.Margin = new System.Windows.Forms.Padding(4);
			this.buttonCopy.Name = "buttonCopy";
			this.buttonCopy.Size = new System.Drawing.Size(175, 36);
			this.buttonCopy.TabIndex = 6;
			this.buttonCopy.Text = "Kopiera föregående";
			this.buttonCopy.UseVisualStyleBackColor = true;
			this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
			// 
			// textBoxFixedPrice
			// 
			this.textBoxFixedPrice.Location = new System.Drawing.Point(393, 115);
			this.textBoxFixedPrice.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxFixedPrice.Name = "textBoxFixedPrice";
			this.textBoxFixedPrice.Size = new System.Drawing.Size(103, 26);
			this.textBoxFixedPrice.TabIndex = 14;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(389, 91);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(82, 20);
			this.label2.TabIndex = 15;
			this.label2.Text = "LoppisPris";
			// 
			// textBoxCustomer
			// 
			this.textBoxCustomer.Location = new System.Drawing.Point(51, 115);
			this.textBoxCustomer.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxCustomer.Name = "textBoxCustomer";
			this.textBoxCustomer.ReadOnly = true;
			this.textBoxCustomer.Size = new System.Drawing.Size(301, 26);
			this.textBoxCustomer.TabIndex = 16;
			this.textBoxCustomer.TabStop = false;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(46, 91);
			this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(52, 20);
			this.label7.TabIndex = 17;
			this.label7.Text = "Ägare";
			// 
			// FormEditProduct
			// 
			this.AcceptButton = this.buttonSaveProduct;
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(730, 432);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textBoxCustomer);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBoxFixedPrice);
			this.Controls.Add(this.buttonCopy);
			this.Controls.Add(this.textBoxName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonCancelProduct);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textBoxProductDescription);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textBoxProductLabel);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.comboBoxProductType);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.buttonSaveProduct);
			this.Controls.Add(this.textBoxProductNote);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "FormEditProduct";
			this.Text = "Objekt";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonCancelProduct;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxProductNote;
		private System.Windows.Forms.Button buttonSaveProduct;
		private System.Windows.Forms.ComboBox comboBoxProductType;
		private System.Windows.Forms.TextBox textBoxProductDescription;
		private System.Windows.Forms.TextBox textBoxProductLabel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxName;
		private System.Windows.Forms.Button buttonCopy;
		private System.Windows.Forms.TextBox textBoxFixedPrice;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxCustomer;
		private System.Windows.Forms.Label label7;
	}
}