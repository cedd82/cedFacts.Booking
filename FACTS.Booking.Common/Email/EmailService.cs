using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

using FACTS.GenericBooking.Common.Configuration;
using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Common.Models.Domain.Messages;
using FACTS.GenericBooking.Common.Models.Email;

using NLog;

namespace FACTS.GenericBooking.Common.Email
{
    public class EmailService : IEmailService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly CommonAppSettings _commonAppSettings;
        private readonly EmailSettings _emailSettings;

        public EmailService(EmailSettings emailSettings, CommonAppSettings commonAppSettings)
        {
            _emailSettings     = emailSettings;
            _commonAppSettings = commonAppSettings;
        }

        public Result SendEmail(string toEmailAddresses, string subject, string body, Attachment attachment = null)
        {
            string fromEmail = !string.IsNullOrEmpty(_emailSettings.FromEmail) ? _emailSettings.FromEmail : "au.sys@cevalogistics.com";
            string smtpServer = !string.IsNullOrEmpty(_emailSettings.SmtpServer) ? _emailSettings.SmtpServer : "cevasmtp";

            List<string> allEmails = toEmailAddresses.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries).Select(e => e.ToLower()).ToList();
            List<string> cevaEmails = allEmails.Where(e => e.Contains("@cevalogistics.com")).ToList();
            List<string> emailsToSend = _commonAppSettings.EnvironmentName == "Production" ? allEmails : cevaEmails;

            try
            {
                using SmtpClient mailClient = new SmtpClient(smtpServer);
                using MailMessage message = new MailMessage();
                foreach (string address in emailsToSend)
                {
                    message.To.Add(address);
                }

                MailAddress fromMail = new MailAddress(fromEmail);
                message.From    = fromMail;
                message.Subject = subject;
                message.Body    = body;
                if (attachment != null)
                    message.Attachments.Add(attachment);

                mailClient.Send(message);
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "Unable to send email");
                return Result.Fail(ErrorMessages.UnableToSendEmail);
            }

            return Result.Ok();
        }

        public void Send(string toEmailAddresses, string subject, string body, Attachment attachment = null)
        {
            string fromEmail = !string.IsNullOrEmpty(_emailSettings.FromEmail) ? _emailSettings.FromEmail : "au.sys@cevalogistics.com";
            string smtpServer = !string.IsNullOrEmpty(_emailSettings.SmtpServer) ? _emailSettings.SmtpServer : "cevasmtp";

            List<string> allEmails = toEmailAddresses.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries).Select(e => e.ToLower()).ToList();
            List<string> cevaEmails = allEmails.Where(e => e.Contains("@cevalogistics.com")).ToList();
            List<string> emailsToSend = _commonAppSettings.EnvironmentName == "Production" ? allEmails : cevaEmails;

            try
            {
                using SmtpClient mailClient = new SmtpClient(smtpServer);
                using MailMessage message = new MailMessage();
                foreach (string address in emailsToSend)
                {
                    message.To.Add(address);
                }

                MailAddress fromMail = new MailAddress(fromEmail);
                message.From    = fromMail;
                message.Subject = subject;
                message.Body    = body;
                if (attachment != null)
                    message.Attachments.Add(attachment);

                mailClient.Send(message);
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "Unable to send email");
            }
        }

        public void SendErrorEmail(EmailDto emailDto)
        {
            SendErrorEmail(emailDto.Subject, emailDto.Body, emailDto.Attachment);
        }

        public void SendErrorEmail(string subject, string body, Attachment attachment = null)
        {
            if (!_emailSettings.SendErrorEmail)
                return;

            try
            {
                subject = _commonAppSettings.EnvironmentName == "Staging" 
                    ? $"ERROR: {_commonAppSettings.ApplicationName} (UAT): ({subject})" 
                    : $"ERROR: {_commonAppSettings.ApplicationName} ({_commonAppSettings.EnvironmentName}: {subject})";

                subject = subject.Trim().Replace('\r', ' ').Replace('\n', ' ');
                subject = subject.Substring(0, Math.Min(subject.Length, 200));
                string recipients = _emailSettings.ErrorEmailRecipients;
                SendErrorEmail(recipients, subject, body, attachment);
            }
            catch (Exception exception)
            {
                Logger.Error(exception, $"Unable to send error email Recipients {_emailSettings.ErrorEmailRecipients}");
            }
        }

        private void SendErrorEmail(string toEmailAddresses, string subject, string body, Attachment attachment = null)
        {
            string fromEmail = !string.IsNullOrEmpty(_emailSettings.FromEmail) ? _emailSettings.FromEmail : "au.sys@cevalogistics.com";
            string smtpServer = !string.IsNullOrEmpty(_emailSettings.SmtpServer) ? _emailSettings.SmtpServer : "cevasmtp";

            List<string> emailsToSend = toEmailAddresses.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries).Select(e => e.ToLower()).ToList();
            using SmtpClient smtpClient = new SmtpClient(smtpServer);
            using MailMessage message = new MailMessage();
            foreach (string address in emailsToSend)
            {
                message.To.Add(address);
            }

            MailAddress fromMail = new MailAddress(fromEmail);
            message.From    = fromMail;
            message.Subject = subject;
            message.Body    = body;
            if (attachment != null)
                message.Attachments.Add(attachment);

            smtpClient.Send(message);
        }
    }
}