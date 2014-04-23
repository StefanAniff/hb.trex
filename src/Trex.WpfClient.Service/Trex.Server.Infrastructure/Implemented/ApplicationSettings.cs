using System.Configuration;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class ApplicationSettings : IAppSettings
    {
        /// <summary>
        /// Gets the required app version.
        /// </summary>
        /// <value>The required app version.</value>
        public string RequiredAppVersion
        {
            get
            {
                return GetSetting("RequiredAppVersion");

            }
        }

        /// <summary>
        /// Gets the required database version.
        /// </summary>
        /// <value>The required database version.</value>
        public string RequiredDatabaseVersion
        {
            get
            {
                return GetSetting("RequiredDatabaseVersion");
            }
        }

        /// <summary>
        /// Gets a value indicating whether [week lock enabled].
        /// </summary>
        /// <value><c>true</c> if [week lock enabled]; otherwise, <c>false</c>.</value>
        public bool WeekLockEnabled
        {
            get { return bool.Parse(GetSetting("WeekLockEnabled")); }
        }

        /// <summary>
        /// Gets the app connection string.
        /// </summary>
        /// <value>The app connection string.</value>
        public string AppConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["TrexConnectionString"].ConnectionString; }
        }

        /// <summary>
        /// Gets environmentsetting evaluating if it's debug
        /// </summary>
        public bool IsDebugMode { get { return GetSetting("environment").Equals("Debug"); } }


        #region Email

        public string AdminEmailSender { get { return GetSetting("AdminSenderEmail"); } }
        public string SmtpServer { get { return GetSetting("smtpServer"); } }
        public int SmtpServerPort { get { return int.Parse(GetSetting("smtpServerPort")); } }
        public string SmtpUser { get { return GetSetting("smtpUser"); } }
        public string SmtpUserPassword { get { return GetSetting("smtpUserPassword"); } }
        public string ResetPasswordEmailSubject { get { return GetSetting("emailResetPasswordSubject"); } }
        public bool SmtpEnableSsl { get { return bool.Parse(GetSetting("smtpEnableSsl")); } }

        #endregion

        private string GetSetting(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (ConfigurationException ex)
            {
                throw new ApplicationBaseException("Missing key in web.config: " + key, ex);
            }
        }
    }
}
