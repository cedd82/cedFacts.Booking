using System.Net.Mail;

namespace FACTS.GenericBooking.Common.Models.Email
{
    public class EmailDto
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public Attachment Attachment { get; set; } = null;
    }
}