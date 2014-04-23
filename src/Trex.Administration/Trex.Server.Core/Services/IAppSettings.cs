namespace Trex.Server.Core.Services
{
    public interface IAppSettings
    {
        string RequiredDatabaseVersion { get; }
        string RequiredAppVersion { get; }
        bool WeekLockEnabled { get; }
        string AppConnectionString { get; }
        string ExcelExportPath { get; }
    }
}