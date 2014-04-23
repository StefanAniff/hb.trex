using System;
using Trex.SmartClient.Core.Services;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class AppSettingsUserDataService : IUserSettingsService
    {
        private readonly IAppSettings _appSettings;

        public AppSettingsUserDataService(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public ILoginSettings GetSettings()
        {
            var loginSettings = new LoginSettings()
                                    {
                                        CreateDate = DateTime.Now,
                                        UserName = _appSettings.UserName,
                                        Password = _appSettings.Password,
                                        PersistLogin = _appSettings.PersistLogin,
                                        CustomerId = _appSettings.CustomerId,
                                        UserId = _appSettings.UserId,
                                        UserFullName = _appSettings.UserDisplayName

                                    };

            if (string.IsNullOrEmpty(loginSettings.UserName) || string.IsNullOrEmpty(loginSettings.Password) || string.IsNullOrEmpty(loginSettings.CustomerId))
                return null;

            if (!loginSettings.PersistLogin)
                return null;
           
            return loginSettings;
        }

        public void SaveSettings(ILoginSettings loginSettings)
        {
            _appSettings.PersistUser(loginSettings.UserName, loginSettings.Password, loginSettings.CustomerId, loginSettings.PersistLogin, loginSettings.UserId, loginSettings.UserFullName);
        }

        public void DeleteSettings()
        {
            _appSettings.DeleteUser();
        }

        public bool SettingsExists()
        {
            var settings = GetSettings();
            return (settings != null);
        }
    }
}
