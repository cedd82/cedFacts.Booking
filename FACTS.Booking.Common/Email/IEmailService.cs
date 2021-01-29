using System.Net.Mail;

using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Common.Models.Email;

namespace FACTS.GenericBooking.Common.Email
{
    public interface IEmailService
    {
        void Send(string toEmailAddresses, string subject, string body, Attachment attachment = null);
        Result SendEmail(string toEmailAddresses, string subject, string body, Attachment attachment = null);
        void SendErrorEmail(EmailDto emailDto);
        void SendErrorEmail(string subject, string body, Attachment attachment = null);
        //void SendErrorEmailAsync(string subject, string body, Attachment attachment = null);
    }
}