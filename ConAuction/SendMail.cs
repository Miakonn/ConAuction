using System;
using System.Text;
using System.ComponentModel;
using System.Net.Mail;
using System.Configuration;
using System.Net.Mime;
using System.Windows.Forms;

namespace ConAuction
{
    internal class SendMail
    {

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            var token = (string)e.UserState;

            if (e.Cancelled) {
                Console.WriteLine("[{0}] Send canceled.", token);
            }

            if (e.Error != null) {
                Console.WriteLine("[{0}] {1}", token, e.Error);
            }
            else {
                Console.WriteLine("Message sent.");
            }
        }

        public void Send(string address, string text)
        {
            Cursor.Current = Cursors.WaitCursor;


            var port = int.Parse(ConfigurationManager.AppSettings["MailPort"]);
            var server = ConfigurationManager.AppSettings["MailServer"];
            var user = ConfigurationManager.AppSettings["MailUser"];
            var password = ConfigurationManager.AppSettings["MailPassword"];
            var senderAddress= ConfigurationManager.AppSettings["MailSenderAddress"];
            var senderName = ConfigurationManager.AppSettings["MailSenderName"];
            var subject = ConfigurationManager.AppSettings["MailSubject"];

            var client = new SmtpClient(server, port) {
                Credentials = new System.Net.NetworkCredential(user, password)
            };

            // Specify the e-mail sender.
            MailAddress from = new MailAddress(senderAddress, senderName, Encoding.UTF8);
            // Set destinations for the e-mail message.
            MailAddress to = new MailAddress(address);
            // Specify the message content.
            MailMessage message = new MailMessage(from, to) {
                Body = text,
                BodyEncoding = Encoding.UTF8,
                Subject = subject,
                SubjectEncoding = Encoding.UTF8
            };

            // Add the alternate body to the message.

            AlternateView alternate = AlternateView.CreateAlternateViewFromString(text, new ContentType("text/html"));
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
            Cursor.Current = Cursors.Default;
        }
   
    }
}
