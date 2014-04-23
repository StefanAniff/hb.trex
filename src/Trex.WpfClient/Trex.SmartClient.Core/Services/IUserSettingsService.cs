namespace Trex.SmartClient.Core.Services
{
    public interface IUserSettingsService
    {
        ILoginSettings GetSettings();
        void SaveSettings(ILoginSettings loginSettings);
        void DeleteSettings();
        bool SettingsExists();
    }
}