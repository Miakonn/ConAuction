using System.Windows;
using System.Windows.Controls;

namespace ConAuction3.Views {
    /// <summary>
    /// Interaction logic for ReceiptBrowserWindow.xaml
    /// </summary>
    public partial class ReceiptBrowserWindow {

        public ReceiptBrowserWindow() {
            InitializeComponent();
        }

        private void TxtResponse_TextChanged(object sender, TextChangedEventArgs e) {
            btnOk.IsEnabled = !(string.IsNullOrWhiteSpace(MailAddress.Text));
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }
    }
}
