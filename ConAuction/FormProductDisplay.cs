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

        private string ReadFinishFile() {
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
                labelType.Text = (string) row["Type"];
                labelName.Text = (string) row["Name"];
                labelDescription.Text = (string) row["Description"];
            }
            Refresh();
        }

        private void IterateToNextUnsoldProduct(int step) {
            bool isSold;
            do {
                _currentRowId += step;
				if (_currentRowId >= _tableProducts.Rows.Count) {
                    return;
                }
                var row = _tableProducts.Rows[_currentRowId];
                isSold = (int) row["Price"] > 0;
            } while (isSold);
        }

	    private void FormProductDisplay_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.PageDown || e.KeyCode == Keys.Down || e.KeyCode == Keys.Space || e.KeyCode == Keys.Right) {
                if (e.Modifiers == Keys.Control) {
                    _currentRowId += 10;
                }
                IterateToNextUnsoldProduct(1);
                DisplayCurrentProduct();
            }
			else if (e.KeyCode == Keys.PageUp || e.KeyCode == Keys.Up || e.KeyCode == Keys.Left) {
                if (e.Modifiers == Keys.Control) {
                    _currentRowId -= 10;
                }
				_currentRowId--;
                DisplayCurrentProduct();
            }
            else if (e.KeyCode == Keys.Escape) {
                Close();
            }
        }
    }
}