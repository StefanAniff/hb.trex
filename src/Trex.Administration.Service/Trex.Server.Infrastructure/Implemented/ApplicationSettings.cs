#region

using System.Configuration;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Services;

#endregion

namespace Trex.Server.Infrastructure.Implemented
{
    public class ApplicationSettings : IAppSettings, IMailServiceSettings
    {
        #region IAppSettings Members

        /// <summary>
        /// 	Gets the required app version.
        /// </summary>
        /// <value> The required app version. </value>
        public string RequiredAppVersion
        {
            get { return GetSetting("RequiredAppVersion"); }
        }

        /// <summary>
        /// 	Gets the required database version.
        /// </summary>
        /// <value> The required database version. </value>
        public string RequiredDatabaseVersion
        {
            get { return GetSetting("RequiredDatabaseVersion"); }
        }

        public string DefaultConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["TrexDefault"].ConnectionString; }
        }

        public string CreateDBScriptPath
        {
            get { return GetSetting("trexCreateDBScriptPath"); }
        }

        public string CreateSchemeScriptPath
        {
            get { return GetSetting("trexSchemeScriptPath"); }
        }

        public string DataScriptPath
        {
            get { return GetSetting("trexDataScriptPath"); }
        }

        public string TrexBaseConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["TrexBase"].ConnectionString; }
        }

        public string AdministratorDefaultRole
        {
            get { return GetSetting("AdministratorDefaultRole"); }
        }

        public string TrexSupportEmail
        {
            get { return GetSetting("TrexSupportMail"); }
        }

        public string TrexInvoicetMail
        {
            get { return GetSetting("TrexInvoicetMail"); }
        }                

        public string RegistrationNotificationEmail
        {
            get { return GetSetting("RegistrationNotificationEmail"); }
        }                

        public string HostResourcesUrl
        {
            get { return GetSetting("HostResourcesUrl"); }
        }

        public string AdministrationUrl
        {
            get { return GetSetting("AdministrationUrl"); }
        }

        public string SmartClientDownloadUrl
        {
            get { return GetSetting("SmartClientDownloadUrl"); }
        }

        #endregion

        #region IMailServiceSettings Members

        public string SmtpServer
        {
            get { return GetSetting("smtpServer"); }
        }

        public string SmtpPort
        {
            get { return GetSetting("smtpServerPort"); }
        }

        public string SmtpUser
        {
            get { return GetSetting("smtpUser"); }
        }

        public string SmtpUserPassword
        {
            get { return GetSetting("smtpUserPassword"); }
        }

        public bool SmtpEnableSsl
        {
            get { return bool.Parse(GetSetting("smtpEnableSsl")); }
        }        

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