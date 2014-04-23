using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class EmailComposer : IEmailComposer
    {
        private readonly IAppSettings _appSettings;

        public EmailComposer(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        /// <summary>
        /// Sends the email async.
        /// Dont want to lock process while autenticating email-account
        /// </summary>
        public void SendForgotPasswordEmail(string fullName, string password, string recipient)
        {
            var senderEmail = _appSettings.AdminEmailSender;

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates",
                                    "ResetPasswordEmail.htm");
            var content = File.ReadAllText(path);
            content = content.Replace("#NAME", fullName).Replace("#PASSWORD", password);

            var message = new MailMessage(senderEmail, recipient)
                {
                    Body = content,
                    Subject = _appSettings.ResetPasswordEmailSubject,
                    IsBodyHtml = true
                };

            SendMail(message);
        }

        private void SendMail(MailMessage mailMessage)
        {
            var smtpServer = _appSettings.SmtpServer;
            var smtpPort = _appSettings.SmtpServerPort;
            var smtpUser = _appSettings.SmtpUser;
            var smtpUserPassword = _appSettings.SmtpUserPassword;
            var smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpUserPassword),
                EnableSsl = _appSettings.SmtpEnableSsl // Mandatory by Office365 smtp
            };

            // Send async since external smtp is slow
            smtpClient.SendAsync(mailMessage, "!Dont care for userToken for now!");
        }
    }
}
