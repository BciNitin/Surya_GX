namespace MobiVUE.Utility
{
    public class MailServer
    {
        public string SmtpServer { get; set; }

        public string UserId { get; set; }

        public string Password { get; set; }

        public KeyValue<string, string> Sender { get; set; }
    }
}