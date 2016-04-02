using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ConAuction {
    public partial class FormEditProduct : Form {
        private readonly Product productCurrent;
        private readonly Product productLast;

        public FormEditProduct(Product product, Product productLastT, OpMode mode) {
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
            buttonSaveProduct.Visible = mode == OpMode.Receiving;
            EnableDisableButtons();
            SetCompletion();
        }

        private void SetCompletion() {
            var source = new AutoCompleteStringCollection();
            try {
                using (var reader = new StreamReader("ConAuctionDictionary.txt")) {
                    while (!reader.EndOfStream) {
                        var line = reader.ReadLine();
                        source.Add(line);
                    }
                }
            }
            catch {}

            textBoxName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBoxName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBoxName.AutoCompleteCustomSource = source;
        }

        private void SetProductContents(Product product) {
            textBoxName.Text = product.Name;
            comboBoxProductType.Text = product.Type;
            textBoxProductDescription.Text = product.Description;
            textBoxProductNote.Text = product.Note;
            textBoxFixedPrice.Text = product.FixedPrice.ToString();
        }

        private string ExtractFromJson(string sMess, string sKey) {
            var iStart = sMess.IndexOf("\"" + sKey + "\":", StringComparison.Ordinal);
            if (iStart >= 0) {
                iStart += sKey.Length + 3;
                iStart = sMess.IndexOf("\"", iStart, StringComparison.Ordinal);
                var iEnd = sMess.IndexOf("\"", iStart + 1, StringComparison.Ordinal);
                if (iEnd > iStart + 1) {
                    return sMess.Substring(iStart + 1, iEnd - iStart - 1);
                }
            }
            return "";
        }

        private string FetchEANFromNet(string barcode) {
            const string sUrl = "http://www.outpan.com/api/get_product.php";
            var finalUrl = string.Format("{0}?barcode={1}", sUrl, barcode);
            Trace.WriteLine(finalUrl);
            var request = WebRequest.Create(finalUrl);
            request.Method = "GET";
            var response = request.GetResponse();
            Trace.WriteLine(((HttpWebResponse) response).StatusDescription);

            var responseString = "";
            if (((HttpWebResponse) response).StatusCode == HttpStatusCode.OK) {
                using (var stream = response.GetResponseStream()) {
                    if (stream != null) {
                        var reader = new StreamReader(stream, Encoding.UTF8);
                        responseString = reader.ReadToEnd();
                    }
                }
                Trace.WriteLine(responseString);
            }
            return responseString;
        }

        private void UpdateProduct() {
            //productCurrent.Id = textBoxProductId.Text;
            productCurrent.Name = textBoxName.Text;
            productCurrent.Type = comboBoxProductType.Text;
            productCurrent.Description = textBoxProductDescription.Text;
            productCurrent.Note = textBoxProductNote.Text;

            int limit;
            if (int.TryParse(textBoxFixedPrice.Text, out limit)) {
                productCurrent.FixedPrice = limit;
            }
        }

        private void EnableDisableButtons() {
            buttonSaveProduct.Enabled = comboBoxProductType.Text != "" && textBoxName.Text != "";
        }

        private void buttonCancelProduct_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonSaveProduct_Click(object sender, EventArgs e) {
            UpdateProduct();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCopy_Click(object sender, EventArgs e) {
            SetProductContents(productLast);
        }

        private void comboBoxProductType_SelectedIndexChanged(object sender, EventArgs e) {
            EnableDisableButtons();
        }

        private bool IsNumber(string s) {
            return s.Length > 0 && s.All(char.IsDigit);
        }

        private string CheckLastPartForEANcode(TextBox tb) {
            for (var len = 13; len >= 12; len --) {
                if (tb.Text.Length >= len) {
                    var sEnd = tb.Text.Substring(tb.Text.Length - len, len);
                    if (IsNumber(sEnd)) {
                        var sResponse = FetchEANFromNet(sEnd);
                        if (sResponse.Length > 0 && ExtractFromJson(sResponse, "message") != "No product found.") {
                            tb.Text = tb.Text.Substring(0, tb.Text.Length - len);
                            var sName = ExtractFromJson(sResponse, "name");
                            if (sName.Length > 0) {
                                tb.Text = tb.Text.TrimStart() + sName.Trim();
                                tb.SelectionStart = tb.Text.Length + 1;
                            }
                            var sDescr = ExtractFromJson(sResponse, "description");
                            return sDescr;
                        }
                        return "-";
                    }
                }
            }
            return "";
        }

        private void textBoxName_TextChanged(object sender, EventArgs e) {
            EnableDisableButtons();
            if (textBoxName.Text.Length >= 12) {
                var sDescr = CheckLastPartForEANcode(textBoxName);
                if (sDescr.Length > 1) {
                    textBoxName.SelectionStart = textBoxName.Text.Length;
                    textBoxProductDescription.Text = sDescr;
                    textBoxProductDescription.SelectionStart = textBoxProductDescription.Text.Length;
                }
            }
        }

        private void textBoxProductDescription_TextChanged(object sender, EventArgs e) {
            if (textBoxProductDescription.Text.Length >= 12) {
                var sDescr = CheckLastPartForEANcode(textBoxProductDescription);
                if (sDescr.Length > 0) {
                    textBoxProductDescription.SelectionStart = textBoxProductDescription.Text.Length;
                }
            }
        }
    }
}