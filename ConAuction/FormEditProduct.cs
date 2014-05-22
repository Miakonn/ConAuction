using System;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;

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

		string ExtractFromJson(string sMess, string sKey){
			int iStart = sMess.IndexOf("\"" + sKey + "\":");
			if (iStart >= 0) {
				iStart += (sKey.Length + 3);
				iStart = sMess.IndexOf("\"", iStart);
				int iEnd = sMess.IndexOf("\"", iStart + 1);
				if (iEnd > (iStart + 1)) {
					return sMess.Substring(iStart + 1, iEnd - iStart - 1);
				}
			}
			return "";
		}


		string FetchEANFromNet(string barcode) {
			const string sUrl = "http://www.outpan.com/api/get_product.php";
			String finalUrl = string.Format("{0}?barcode={1}", sUrl, barcode);
			System.Diagnostics.Trace.WriteLine(finalUrl);
			WebRequest request = WebRequest.Create(finalUrl);
			request.Method = "GET";
			WebResponse response = request.GetResponse();
			System.Diagnostics.Trace.WriteLine(((HttpWebResponse)response).StatusDescription);

			String responseString = "";
			if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK) {
			
				using (Stream stream = response.GetResponseStream()) {
					StreamReader reader = new StreamReader(stream, Encoding.UTF8);
					responseString = reader.ReadToEnd();
				}
				System.Diagnostics.Trace.WriteLine(responseString);
			}
			return responseString;
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

		private bool IsNumber(string s) {
			return s.Length > 0 && s.All(c => Char.IsDigit(c));
		}

		private void textBoxName_TextChanged(object sender, EventArgs e)
		{
			EnableDisableButtons();
			if (textBoxName.Text.Length >= 12 && IsNumber(textBoxName.Text)) {
				string sResponse =  FetchEANFromNet(textBoxName.Text);
				if (sResponse.Length > 0) {
					string sName = ExtractFromJson(sResponse, "name");
					if (sName.Length > 0) {
						textBoxName.Text = sName;
					}
					string sDescr = ExtractFromJson(sResponse, "description");
					if (sDescr.Length > 0) {
						textBoxProductDescription.Text += sDescr;
					}
				}

			}
		}

		private void textBoxProductDescription_TextChanged(object sender, EventArgs e) {
			
		}

	}
}
