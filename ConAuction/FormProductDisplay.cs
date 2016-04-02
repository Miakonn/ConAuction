using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ConAuction {
    public partial class FormProductDisplay : Form {
        private int currentRowId = -1;
        private readonly DataTable tableProducts;

        public FormProductDisplay(DataTable table) {
            InitializeComponent();
            LoadImage();

            tableProducts = table;

            WindowState = FormWindowState.Normal;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            DisplayCurrentProduct();
        }

        private void LoadImage() {
            var _imageStream =
                Assembly.GetExecutingAssembly().GetManifestResourceStream("ConAuction.LinconAuktionLiten.png");
            if (_imageStream != null) {
                var bitmapLogo = new Bitmap(_imageStream);
                pictureBoxLogo.Image = bitmapLogo;

                Icon = Icon.FromHandle(bitmapLogo.GetHicon());
            }
        }

        private string ReadRulesFile() {
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
                var row = tableProducts.Rows[currentRowId];
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
                currentRowId += step;
                if (currentRowId >= tableProducts.Rows.Count) {
                    return;
                }
                var row = tableProducts.Rows[currentRowId];
                isSold = (int) row["Price"] > 0;
            } while (isSold);
        }

        private void FormProductDisplay_KeyDown(object sender, KeyEventArgs e) {
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
                Close();
            }
        }
    }
}