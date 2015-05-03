using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace ConAuction
{
	public partial class FormProductDisplay : Form
	{
		int currentRowId = -1;
		DataTable tableProducts;


		public FormProductDisplay(DataTable table)
		{
			InitializeComponent();
			LoadImage();

			tableProducts = table;

			this.WindowState = FormWindowState.Normal;
			this.FormBorderStyle = FormBorderStyle.None;
			this.WindowState = FormWindowState.Maximized;

			DisplayCurrentProduct();
		}

		private void LoadImage()
		{
			Stream _imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ConAuction.LinconAuktionLiten.png");
			Bitmap bitmapLogo = new Bitmap(_imageStream);
			pictureBoxLogo.Image = bitmapLogo;

			this.Icon = Icon.FromHandle(bitmapLogo.GetHicon());
		}

		private string ReadRulesFile() {
			try {
				using (StreamReader reader = new StreamReader("ConAuctionRules.txt", Encoding.UTF8)) {
					return reader.ReadToEnd();
				}
			}
			catch {
				return "";
			}
		}

		private string ReadFinishFile() {
			try {
				using (StreamReader reader = new StreamReader("ConAuctionFinish.txt", Encoding.UTF8)) {
					return reader.ReadToEnd();
				}
			}
			catch {
				return "";
			}
		}

		private void DisplayCurrentProduct()
		{
			if (currentRowId < 0) {
				labelLabel.Text = "";
				labelType.Text = "";
				labelName.Text = "Välkomna!";
				labelDescription.Text = ReadRulesFile();
				currentRowId = -1;
			}
			else if (currentRowId >= tableProducts.Rows.Count) {
				currentRowId = tableProducts.Rows.Count;
				labelLabel.Text = "";
				labelType.Text = "";
				labelName.Text = "Nu är det slut!";
				labelDescription.Text = ReadFinishFile();
			}
			else {
				DataRow row = tableProducts.Rows[currentRowId];
				int label = (int)row["Label"];
				string sss = row["Label"].ToString();
				labelLabel.Text = label.ToString();
				labelType.Text = (string)row["Type"];
				labelName.Text = (string)row["Name"];
				labelDescription.Text = (string)row["Description"];
			}
			Refresh();
		}

		private void IterateToNextUnsoldProduct(int step)
		{
			bool isSold = false;
			do {
				currentRowId+= step;
				if (currentRowId >= tableProducts.Rows.Count) {
					return;
				}
				DataRow row = tableProducts.Rows[currentRowId];
				isSold = ((int)row["Price"] > 0);
			} while (isSold);
		}

		private void FormProductDisplay_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.PageDown || e.KeyCode == Keys.Down || e.KeyCode == Keys.Space) {
				if (e.Modifiers == Keys.Control) {
					currentRowId += 10;
				}
				IterateToNextUnsoldProduct(1);
				DisplayCurrentProduct();				
			}
			else if (e.KeyCode == Keys.PageUp || e.KeyCode == Keys.Up) {
				if (e.Modifiers == Keys.Control) {
					currentRowId -= 10;
				}
				IterateToNextUnsoldProduct(1);
				DisplayCurrentProduct();
			}
			else if (e.KeyCode == Keys.Escape) {
				this.Close();
			}
		}
	}
}
