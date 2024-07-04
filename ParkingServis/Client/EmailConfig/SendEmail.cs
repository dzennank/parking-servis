using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using Newtonsoft.Json;

namespace ParkingServis.Client.EmailConfig
{
    public class SendEmail
    {

        public void SendingEmail(string emailTo)
        {
            string ApplicationName = "ParkingSerivs";
            string[] Scopes = { GmailService.Scope.GmailSend };

            UserCredential credential;

            using (var stream = new FileStream("C:\\Users\\amar-next\\Desktop\\Faks\\googleApiCreds.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Create a new email message
            var msg = new Google.Apis.Gmail.v1.Data.Message();
            msg.Raw = Base64UrlEncode(CreateHtmlMessage("dzenankrlic@gmail.com", emailTo, "Subject", "Hello, this is a test email!"));

            try
            {
                // Send the email
                var result = service.Users.Messages.Send(msg, "me").Execute();
                Console.WriteLine("Message sent successfully.");
            }
            catch (Google.GoogleApiException gae)
            {
                Console.WriteLine("Google API error:");
                Console.WriteLine($"HTTP Status Code: {gae.HttpStatusCode}");
                Console.WriteLine($"Error message: {gae.Message}");
            }
        }

        static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }

        static string CreateHtmlMessage(string from, string to, string subject, string htmlBody)
        {
            var message = new
            {
                raw = Base64UrlEncode(CreateMimeMessage(from, to, subject, htmlBody))
            };
            return JsonConvert.SerializeObject(message);
        }

        static string CreateMimeMessage(string from, string to, string subject, string htmlBody)
        {
            var mimeMessage = $"From: {from}\n" +
                              $"To: {to}\n" +
                              $"Subject: {subject}\n" +
                              "Content-Type: text/html; charset=utf-8\n\n" +
                              $"{htmlBody}";
            return mimeMessage;
        }
    }
}
