using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ConAuction {
    public partial class FormProductDisplay : Form {
        private int _currentRowId = -1;
        private readonly DataTable _tableProducts;

        public FormProductDisplay(DataTable table) {
            InitializeComponent();
            LoadImage();

            _tableProducts = table;

            WindowState = FormWindowState.Normal;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            DisplayCurrentProduct();
        }

        private void LoadImage() {
            var imageStream =
                Assembly.GetExecutingAssembly().GetManifestResourceStream("ConAuction.LinconAuktionLiten.png");
            if (imageStream != null) {
                var bitmapLogo = new Bitmap(imageStream);
                pictureBoxLogo.Image = bitmapLogo;

                Icon = Icon.FromHandle(bitmapLogo.GetHicon());
            }
        }

        private static string ReadRulesFile() {
            try {
                using (var reader = new StreamReader("ConAuctionRules.txt", Encoding.UTF8)) {
                    return reader.ReadToEnd();
                }
            }
            catch {
                return "";
            }
        }

        private static string ReadFinishFile() {
            try {
                using (var reader = new StreamReader("ConAuctionFinish.txt", Encoding.UTF8)) {
                    return reader.ReadToEnd();
                }
            }
            catch {
                return "";
            }
        }

        private void DisplayCurrentProduct() {
            if (_currentRowId < 0) {
                labelLabel.Text = "";
                labelType.Text = "";
                labelName.Text = "Välkomna!";
                labelDescription.Text = ReadRulesFile();
                _currentRowId = -1;
            }
            else if (_currentRowId >= _tableProducts.Rows.Count) {
                _currentRowId = _tableProducts.Rows.Count;
                labelLabel.Text = "";
                labelType.Text = "";
                labelName.Text = "Nu är det slut!";
                labelDescription.Text = ReadFinishFile();
            }
            else {
                var row = _tableProducts.Rows[_currentRowId];
                var label = (int) row["Label"];
                labelLabel.Text = label.ToString();
                labelName.Text = (string) row["Name"];
				labelType.Text = (string)row["Type"];
	            if ((int) row["Price"] > 0) {
					labelDescription.Text = "Sålt för " + row["Price"] + ":-";
				}
	            else {
					labelDescription.Text = (string)row["Description"];
				}
            }
            Refresh();
        }

        private void IterateToNextUnsoldProduct() {
            bool isSold;
	        bool isFixedPrice;
            do {
                _currentRowId++;
				if (_currentRowId >= _tableProducts.Rows.Count) {
                    return;
                }
                var row = _tableProducts.Rows[_currentRowId];
                isSold = (int) row["Price"] > 0;
				isFixedPrice = ((int)row["FixedPrice"] > 0) && !row["Note"].ToString().ToLowerInvariant().Contains("auktion");

            } while (isSold || isFixedPrice);
        }

		private void IterateToPreviousAuctionedProduct() {
			bool isFixedPrice;
			do {
				_currentRowId--;
				if (_currentRowId < 0) {
					return;
				}
				var row = _tableProducts.Rows[_currentRowId];
				isFixedPrice = ((int)row["FixedPrice"] > 0) && !row["Note"].ToString().ToLowerInvariant().Contains("auktion");

			} while (isFixedPrice);
		}

	    private void FormProductDisplay_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.PageDown || e.KeyCode == Keys.Down || e.KeyCode == Keys.Space || e.KeyCode == Keys.Right) {
                if (e.Modifiers == Keys.Control) {
                    _currentRowId += 10;
                }
                IterateToNextUnsoldProduct();
                DisplayCurrentProduct();
            }
			else if (e.KeyCode == Keys.PageUp || e.KeyCode == Keys.Up || e.KeyCode == Keys.Left) {
                if (e.Modifiers == Keys.Control) {
                    _currentRowId -= 10;
                }
				IterateToPreviousAuctionedProduct();
                DisplayCurrentProduct();
            }
            else if (e.KeyCode == Keys.Escape) {
                Close();
            }
        }
    }
}