
using System.Collections.Generic;

namespace Trex.Server.Core.Services
{
    public interface IAppSettings
    {
        string RequiredDatabaseVersion {get;}
        string RequiredAppVersion { get; }
        bool WeekLockEnabled { get; }
        string AppConnectionString { get; }
        string AdminEmailSender { get; }
        string SmtpServer { get; }
        int SmtpServerPort { get; }
        string SmtpUser { get; }
        string SmtpUserPassword { get; }
        bool SmtpEnableSsl { get; }
        string ResetPasswordEmailSubject { get; }

        /// <summary>
        /// Gets environmentsetting evaluating if it's debug
        /// </summary>
        bool IsDebugMode { get; }
    }
}
