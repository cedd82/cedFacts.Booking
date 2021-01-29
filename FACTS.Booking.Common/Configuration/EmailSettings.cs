namespace FACTS.GenericBooking.Common.Configuration
{
    public class EmailSettings
    {
        public string ErrorEmailRecipients { get; set; }
        public string FromEmail { get; set; }
        public bool SendErrorEmail { get; set; }
        public string SmtpServer { get; set; }
    }
}