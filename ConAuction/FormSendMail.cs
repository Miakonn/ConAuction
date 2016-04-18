using System;
using System.Windows.Forms;

namespace ConAuction {
    public partial class FormSendMail : Form {
        private readonly string _message;


        public FormSendMail(string message) {
            _message = message;
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e) {

            var smtpmail = new SendMail();
            smtpmail.Send(textBoxMailAddress.Text, _message);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}