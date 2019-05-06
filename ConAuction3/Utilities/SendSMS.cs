using System.Diagnostics;
using System.Net;

namespace ConAuction3 {
    public class SendSMS {

        private static string FixPhoneNo(string phoneno) {
            phoneno = phoneno.Trim();
            var sDestination = "0046" + phoneno.Substring(1);
            sDestination = sDestination.Replace(" ", "");
            sDestination = sDestination.Replace("-", "");
            return sDestination;
        }

        public static void Send(string phoneNo, string message) {
            var dest = FixPhoneNo(phoneNo);

            const string pw = "kYJhMyCn";
            const string user = "andersblom";
            const string sender = "46705542120";
            const string url = "https://se-1.cellsynt.net/sms.php";

            var finalUrl =
                $"{url}?username={user}&password={pw}&destination={dest}&type=text&charset=UTF-8&text={message}&originatortype=numeric&originator={sender}";
            Trace.WriteLine(finalUrl);
            var request = WebRequest.Create(finalUrl);
            request.Method = "GET";
            var response = request.GetResponse();
            Trace.WriteLine(((HttpWebResponse)response).StatusDescription);
        }
    }
}
