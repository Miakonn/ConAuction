namespace ConAuction
{
	partial class FormProductDisplay
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
			this.labelLabel = new System.Windows.Forms.Label();
			this.labelName = new System.Windows.Forms.Label();
			this.labelType = new System.Windows.Forms.Label();
			this.labelDescription = new System.Windows.Forms.Label();
			this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// labelLabel
			// 
			this.labelLabel.AutoSize = true;
			this.labelLabel.Font = new System.Drawing.Font("Open Sans", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelLabel.Location = new System.Drawing.Point(219, 36);
			this.labelLabel.Name = "labelLabel";
			this.labelLabel.Size = new System.Drawing.Size(97, 43);
			this.labelLabel.TabIndex = 4;
			this.labelLabel.Text = "label";
			this.labelLabel.UseMnemonic = false;
			// 
			// labelName
			// 
			this.labelName.AutoSize = true;
			this.labelName.Font = new System.Drawing.Font("Open Sans", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelName.Location = new System.Drawing.Point(219, 114);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(109, 43);
			this.labelName.TabIndex = 5;
			this.labelName.Text = "name";
			this.labelName.UseMnemonic = false;
			// 
			// labelType
			// 
			this.labelType.AutoSize = true;
			this.labelType.Font = new System.Drawing.Font("Open Sans", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelType.Location = new System.Drawing.Point(674, 36);
			this.labelType.Name = "labelType";
			this.labelType.Size = new System.Drawing.Size(90, 43);
			this.labelType.TabIndex = 6;
			this.labelType.Text = "type";
			this.labelType.UseMnemonic = false;
			// 
			// labelDescription
			// 
			this.labelDescription.AutoSize = true;
			this.labelDescription.Font = new System.Drawing.Font("Open Sans", 24F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelDescription.Location = new System.Drawing.Point(219, 213);
			this.labelDescription.MaximumSize = new System.Drawing.Size(700, 700);
			this.labelDescription.Name = "labelDescription";
			this.labelDescription.Size = new System.Drawing.Size(174, 43);
			this.labelDescription.TabIndex = 7;
			this.labelDescription.Text = "description";
			this.labelDescription.UseMnemonic = false;
			// 
			// pictureBoxLogo
			// 
			this.pictureBoxLogo.Location = new System.Drawing.Point(13, 13);
			this.pictureBoxLogo.Margin = new System.Windows.Forms.Padding(4);
			this.pictureBoxLogo.Name = "pictureBoxLogo";
			this.pictureBoxLogo.Size = new System.Drawing.Size(139, 144);
			this.pictureBoxLogo.TabIndex = 33;
			this.pictureBoxLogo.TabStop = false;
			// 
			// FormProductDisplay
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.ClientSize = new System.Drawing.Size(965, 689);
			this.Controls.Add(this.pictureBoxLogo);
			this.Controls.Add(this.labelDescription);
			this.Controls.Add(this.labelType);
			this.Controls.Add(this.labelName);
			this.Controls.Add(this.labelLabel);
			this.Name = "FormProductDisplay";
			this.Text = "FormProductDisplay";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormProductDisplay_KeyDown);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label labelLabel;
		private System.Windows.Forms.Label labelName;
		private System.Windows.Forms.Label labelType;
		private System.Windows.Forms.Label labelDescription;
		private System.Windows.Forms.PictureBox pictureBoxLogo;
	}
}