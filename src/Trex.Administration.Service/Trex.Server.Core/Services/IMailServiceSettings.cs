namespace Trex.Server.Core.Services
{
    public interface IMailServiceSettings
    {
        string SmtpServer { get; }
        string SmtpUser { get; }
        string SmtpUserPassword { get; }
        string SmtpPort { get; }
        bool SmtpEnableSsl { get; }
    }
}
