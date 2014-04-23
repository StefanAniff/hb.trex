using System.Threading.Tasks;
using log4net.Appender;

namespace TrexSL.Web.CustomLog4netAppenders
{
    /// <summary>
    /// Custom log4net smtpappender, since sending mails is slow
    /// </summary>
    public class AsyncSmtpAppender : SmtpAppender
    {
        protected override void SendEmail(string messageBody)
        {
            Task.Factory.StartNew(() => base.SendEmail(messageBody));
        }
    }
}