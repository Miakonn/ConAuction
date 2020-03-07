using System;
using System.ComponentModel;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Windows.Forms;
using ConAuction3.Views;
using MessageBox = System.Windows.MessageBox;

namespace ConAuction3.Utilities
{
    internal class SendMail {
        private static string password;

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            var token = (string)e.UserState;

            if (e.Cancelled) {
                MessageBox.Show($"[{token}] Send canceled.");
            }
            else if (e.Error != null) {
                MessageBox.Show($"[{token}] {e.Error}");
            }
            else {
                MessageBox.Show("Message sent.");
            }
        }

        public static void Send(string address, string text)
        {
            Cursor.Current = Cursors.WaitCursor;

            try {

                password = ConfigurationManager.AppSettings["MailPassword"];
                var port = int.Parse(ConfigurationManager.AppSettings["MailPort"]);
                var server = ConfigurationManager.AppSettings["MailServer"];
                var user = ConfigurationManager.AppSettings["MailUser"];
                var senderAddress = ConfigurationManager.AppSettings["MailSenderAddress"];
                var senderName = ConfigurationManager.AppSettings["MailSenderName"];
                var subject = ConfigurationManager.AppSettings["MailSubject"];

                // Set destinations for the e-mail message.
                MailAddress to = new MailAddress(address);
                // Specify the e-mail sender.
                MailAddress from = new MailAddress(senderAddress, senderName, Encoding.UTF8);

                if (string.IsNullOrWhiteSpace(password)) {
                    password = PromptDialog.Prompt("Lösenord för " + user, "Mejl server", "",
                        PromptDialog.InputType.Password);
                }

                var client = new SmtpClient(server, port) {
                    Credentials = new System.Net.NetworkCredential(user, password)
                };

                // Specify the message content.
                MailMessage message = new MailMessage(from, to) {
                    Body = text,
                    BodyEncoding = Encoding.UTF8,
                    Subject = subject,
                    SubjectEncoding = Encoding.UTF8
                };

                // Add the alternate body to the message.

                AlternateView alternate =
                    AlternateView.CreateAlternateViewFromString(text, new ContentType("text/html"));
                message.AlternateViews.Add(alternate);

                // Include some non-ASCII characters in body and subject.

                // Set the method that is called back when the send operation ends.
                client.SendCompleted += SendCompletedCallback;

                // The userState can be any object that allows your callback 
                // method to identify this send operation.
                // For this example, the userToken is a string constant.
                string userState = subject;
                client.SendAsync(message, userState);

                System.Threading.Thread.Sleep(2000);
                // Clean up.
                message.Dispose();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

            Cursor.Current = Cursors.Default;
        }
   
    }
}
