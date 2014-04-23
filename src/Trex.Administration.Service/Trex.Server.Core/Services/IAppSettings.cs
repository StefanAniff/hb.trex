
namespace Trex.Server.Core.Services
{
    public interface IAppSettings
    {
        string RequiredDatabaseVersion {get;}
        string RequiredAppVersion { get; }
        string DefaultConnectionString { get;  }
        string CreateDBScriptPath { get; }
        string CreateSchemeScriptPath { get; }
        string DataScriptPath { get; }
        string TrexBaseConnectionString { get; }
        string AdministratorDefaultRole { get; }
        string TrexSupportEmail { get; }        
        string RegistrationNotificationEmail { get; }
        string TrexInvoicetMail { get; }        
        string HostResourcesUrl { get; }
        string AdministrationUrl { get; }
        string SmartClientDownloadUrl { get; }
    }
}
