using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ConAuction
{
	public partial class FormEditProduct : Form
	{
		private Product productCurrent= null;
		private Product productLast = null;

		public FormEditProduct(Product product, Product productLastT, OpMode mode)
		{
			InitializeComponent();

			productCurrent = product;

			comboBoxProductType.Items.Add("Rollspel");
			comboBoxProductType.Items.Add("Krigsspel");
			comboBoxProductType.Items.Add("Figurspel");
			comboBoxProductType.Items.Add("Brädspel");
			comboBoxProductType.Items.Add("Kortspel");
			comboBoxProductType.Items.Add("Övrigt");

			textBoxProductLabel.Text = product.Label;

			SetProductContents(product);

			productLast = productLastT;
			buttonCopy.Visible = (mode == OpMode.Receiving) && (productLast != null);
			buttonSaveProduct.Visible = (mode == OpMode.Receiving);
			EnableDisableButtons();
			SetCompletion();
		}

		void SetCompletion ()
		{
			var source = new AutoCompleteStringCollection();
			try {
				using (StreamReader reader = new StreamReader("ConAuctionDictionary.txt")) {
					while (!reader.EndOfStream) {
						string line = reader.ReadLine();
						source.Add(line);
					}
				}
			}
			catch {}

			textBoxName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			textBoxName.AutoCompleteSource = AutoCompleteSource.CustomSource;
			textBoxName.AutoCompleteCustomSource = source;
		}

		void SetProductContents(Product product)
		{
			textBoxName.Text = product.Name;
			comboBoxProductType.Text = product.Type;
			textBoxProductDescription.Text = product.Description;
		}

		private void UpdateProduct()
		{
			//productCurrent.Id = textBoxProductId.Text;
			productCurrent.Name = textBoxName.Text;
			productCurrent.Type = comboBoxProductType.Text;
			productCurrent.Description = textBoxProductDescription.Text;
			//int limit = 0;
			//Int32.TryParse(, out limit);
			productCurrent.Note = textBoxProductNote.Text;
		}

		private void EnableDisableButtons()
		{
			buttonSaveProduct.Enabled = comboBoxProductType.Text != "" && textBoxName.Text != "";
		}

		private void buttonCancelProduct_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void buttonSaveProduct_Click(object sender, EventArgs e)
		{
			UpdateProduct();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void buttonCopy_Click(object sender, EventArgs e)
		{
			SetProductContents(productLast);
		}

		private void comboBoxProductType_SelectedIndexChanged(object sender, EventArgs e)
		{
			EnableDisableButtons();

		}

		private void textBoxName_TextChanged(object sender, EventArgs e)
		{
			EnableDisableButtons();

		}

	}
}
