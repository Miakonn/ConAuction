using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;

namespace ConAuction
{
	public partial class FormSendSMS : Form
	{
		DataGridViewSelectedRowCollection rows;


		public FormSendSMS(DataGridViewSelectedRowCollection rowsT) {
			rows = rowsT;
			InitializeComponent();

			 
			textBoxMessage.Text = "Auktionen är avslutad. Du kan hämta ut dina pengar mellan kl 9 och 13 under söndagen. /LinCon auktionen";

			string sRecipient = "";
			foreach (DataGridViewRow row in rows) {
				string sName = row.Cells["Name"].Value.ToString();
				sRecipient += sName + ", ";
			}

			textBoxRecipient.Text = sRecipient;
		}

		string FixPhoneNo(string phoneno) {
			phoneno.Trim();
			string sDestination = "0046" + phoneno.Substring(1);
			sDestination = sDestination.Replace(" ", "");
			sDestination = sDestination.Replace("-", "");
			return sDestination;
		}


		bool SendSMS(string phoneno, string message) {
			string sDestination = FixPhoneNo(phoneno);

			const string sPassword = "kYJhMyCn";
			const string sUser = "andersblom";
			const string sOriginator = "46705542120";
			const string sUrl = "https://se-1.cellsynt.net/sms.php";
//			string data = "username=andersblom&password=kYJhMyCn&destination=0046705542120&type=text&charset=UTF-8&text=Testing 123&originatortype=numeric&originator={}";

			String finalUrl = string.Format("{0}?username={1}&password={2}&destination={3}&type=text&charset=UTF-8&text={4}&originatortype=numeric&originator={5}", 
				sUrl, sUser, sPassword, sDestination, message, sOriginator);
			System.Diagnostics.Trace.WriteLine(finalUrl);
			WebRequest request = WebRequest.Create(finalUrl);
			request.Method = "GET";
			WebResponse response = request.GetResponse();
			System.Diagnostics.Trace.WriteLine(((HttpWebResponse)response).StatusDescription);

			return (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK);
		}





		private void buttonOK_Click(object sender, EventArgs e) {
			string message = textBoxMessage.Text;

			foreach (DataGridViewRow row in rows) {
				string phoneno = row.Cells["Phone"].Value.ToString();				
				SendSMS(phoneno, message);
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e) {
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
