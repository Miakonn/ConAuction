using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConAuction;

namespace ConAuction
{
	public partial class FormProduct : Form
	{

		private Product productCurrent= null;
		private Product productLast = null;

		public FormProduct(Product product, Product productLastT, OpMode mode)
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
			EnableDisableButtons();
			SetCompletion();
		}


		void SetCompletion ()
		{
			var source = new AutoCompleteStringCollection();
			source.AddRange(new string[]
                    {
                        "Böcker",
                        "Warhammer 40k",
                        "Warhammer fantasy",
                        "Magic",
                        "Sagan om ringen",
						"AD&D",
						"Yu-Gi-Oh!",
						"Middle Earth",
						"Drakar & demoner",
						"Werewolf",
						"Star wars",
						"Shadowrun",
						"Space Marine",
						"SF-böcker",
						"Fantasy-böcker",
						"Urban fantasy-böcker"
                    });

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
