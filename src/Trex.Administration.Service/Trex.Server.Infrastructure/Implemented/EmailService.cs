using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class EmailService:IEmailService
    {
        private readonly IMailServiceSettings _settings;

        public EmailService(IMailServiceSettings settings)
        {
            _settings = settings;
        }

        public bool SendEmail(IEmailComposer emailComposer)
        {
            ValidateComposer(emailComposer);

            using(var smtpClient = new SmtpClient
                {
                    Host = _settings.SmtpServer,
                    Port = int.Parse(_settings.SmtpPort),
                    EnableSsl = _settings.SmtpEnableSsl,
                    Credentials = new NetworkCredential(_settings.SmtpUser, _settings.SmtpUserPassword)
                })
            {
                try
                {
                    using(var mail = new MailMessage())
                    {
                        foreach (var recipient in emailComposer.Recipients)
                        {
                            mail.To.Add(recipient);
                        }

                        foreach (var ccRecipient in emailComposer.CcRecipients)
                        {
                            mail.CC.Add(ccRecipient);
                        }

                        foreach (var bccRecipient in emailComposer.BccRecipients)
                        {
                            mail.Bcc.Add(bccRecipient);
                        }

                        // NB! From/Sender must be the same as Username for NetworkCredential, or else send will be denied by smtp
                        mail.From = new MailAddress(_settings.SmtpUser, "No Reply D60");
                        mail.Sender = new MailAddress(_settings.SmtpUser, "No Reply D60");

                        foreach (var attachment in emailComposer.Attachments)
                        {
                            mail.Attachments.Add(attachment);
                        }

                        mail.Subject = emailComposer.Title;
                        mail.IsBodyHtml = emailComposer.IsHtml;
                        mail.Body = emailComposer.ComposeContent();

                        smtpClient.Send(mail); // IVA: Try send Async
                        return true;
                    }
                }
                catch (FormatException ex)
                {
                    throw new MailServiceException("One or more email addresses are invalid", ex);
                }
                catch(Exception ex)
                {
                    throw new Exception("Email was not sent", ex);
                }
            }
        }

        private void ValidateComposer(IEmailComposer composer)
        {
            if (composer.Recipients.Count == 0 && composer.CcRecipients.Count == 0 && composer.BccRecipients.Count == 0)
                throw new MailServiceException("No recipients has been added in the email composer");

            if (string.IsNullOrWhiteSpace(composer.Sender))
                throw new MailServiceException("No sender has been added in the email composer");

        }
    }
    
}
