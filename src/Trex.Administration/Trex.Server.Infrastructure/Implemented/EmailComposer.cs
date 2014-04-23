using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Web;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class EmailComposer : IEmailComposer
    {
        #region IEmailComposer Members

        public void SendForgotPasswordEmail(string fullName, string password, string recipient)
        {
            var senderEmail = ConfigurationManager.AppSettings["AdminSenderEmail"];

            var content = File.ReadAllText(HttpContext.Current.Server.MapPath("/EmailTemplates/ResetPasswordEmail.htm"));
            content = content.Replace("#NAME", fullName).Replace("#PASSWORD", password);

            var message = new MailMessage(senderEmail, recipient)
                              {
                                  Body = content,
                                  Subject = "T.Rex Password Reset",
                                  IsBodyHtml = true
                              };

            SendMail(message);
        }

        #endregion

        private void SendMail(MailMessage mailMessage)
        {
            var smtpServer = ConfigurationManager.AppSettings["smtpServer"];
            var smtpClient = new SmtpClient(smtpServer);
            smtpClient.Send(mailMessage);
        }
    }
}