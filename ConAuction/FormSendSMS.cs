using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace ConAuction {
    public partial class FormSendSMS : Form {
		private readonly List<DataGridViewRow> _rows;

		public FormSendSMS(List<DataGridViewRow> rows) {
            _rows = rows;
            InitializeComponent();

            textBoxMessage.Text =
                "Auktionen är avslutad. Du kan hämta ut dina pengar mellan kl 9 och 13 under lördagen. /LinCon auktionen";

            var sRecipient = _rows.Select(row => row.Cells["Name"].Value.ToString()).Aggregate("", (current, sName) => current + (sName + ", "));

			textBoxRecipient.Text = sRecipient;
        }

        private static string FixPhoneNo(string phoneno) {
            phoneno = phoneno.Trim();
            var sDestination = "0046" + phoneno.Substring(1);
            sDestination = sDestination.Replace(" ", "");
            sDestination = sDestination.Replace("-", "");
            return sDestination;
        }

        private static void SendSMS(string phoneno, string message) {
            var sDestination = FixPhoneNo(phoneno);

            const string sPassword = "kYJhMyCn";
            const string sUser = "andersblom";
            const string sOriginator = "46705542120";
            const string sUrl = "https://se-1.cellsynt.net/sms.php";

            var finalUrl =
                string.Format(
                    "{0}?username={1}&password={2}&destination={3}&type=text&charset=UTF-8&text={4}&originatortype=numeric&originator={5}",
                    sUrl, sUser, sPassword, sDestination, message, sOriginator);
            Trace.WriteLine(finalUrl);
            var request = WebRequest.Create(finalUrl);
            request.Method = "GET";
            var response = request.GetResponse();
            Trace.WriteLine(((HttpWebResponse) response).StatusDescription);
        }

        private void buttonOK_Click(object sender, EventArgs e) {
            var message = textBoxMessage.Text;

            foreach (DataGridViewRow row in _rows) {
                var phoneno = row.Cells["Phone"].Value.ToString();
                SendSMS(phoneno, message);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}