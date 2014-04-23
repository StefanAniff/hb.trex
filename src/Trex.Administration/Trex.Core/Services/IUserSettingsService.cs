namespace Trex.Core.Services
{
    public interface IUserSettingsService
    {
        ILoginSettings GetSettings();
        void SaveSettings(ILoginSettings loginSettings);
        void DeleteSettings();
    }
}