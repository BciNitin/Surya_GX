using System.Net;
using System.Net.Mail;

namespace MobiVUE.Utility
{
    public class EmailProcessor
    {
        private SmtpClient smtp;
        private KeyValue<string, string> _Sender;

        public EmailProcessor(MailServer server)
        {
            smtp = new SmtpClient(server.SmtpServer);
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential(server.UserId, server.Password);
            _Sender = server.Sender;
        }

        public void SendEmail(Email email)
        {
            smtp.EnableSsl = true;
            smtp.Port = 587;
            MailMessage message = new MailMessage();
            message.Sender = new MailAddress(_Sender.Key, _Sender.Value);
            message.From = new MailAddress(_Sender.Key, _Sender.Value);

            foreach (var receiver in email.Receivers)
            {
                message.To.Add(new MailAddress(receiver.Key, receiver.Value));
            }

            message.Subject = email.Subject;
            message.Body = email.Body;

            message.IsBodyHtml = true;
            smtp.Send(message);
        }
    }
}