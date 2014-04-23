using System.IO.IsolatedStorage;
using System.Reflection;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Services;
using log4net;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class IsolatedStorageFileProvider : IIsolatedStorageFileProvider
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IAppSettings _appSettings;

        public IsolatedStorageFileProvider(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public IsolatedStorageFile GetIsolatedStorage()
        {
            var environment = _appSettings.Enviroment;

            switch (environment)
            {
                case EnviromentEnum.Release:
                    Logger.InfoFormat("IsolatedStorageFile -> Using GetUserStoreForApplication");
                    return IsolatedStorageFile.GetUserStoreForApplication(); // Availiable in ClickOnce managed application, where application-identification is available

                case EnviromentEnum.ReleaseExternal:
                    Logger.InfoFormat("IsolatedStorageFile -> Using GetUserStoreForApplication");
                    return IsolatedStorageFile.GetUserStoreForApplication(); // Availiable in ClickOnce managed application, where application-identification is available

                default: // DEBUG, TEST
                    Logger.InfoFormat("IsolatedStorageFile -> Using GetUserStoreForAssembly");
                    return IsolatedStorageFile.GetUserStoreForAssembly(); // Availiable as Stand-alone application (run exe directly). 
            }
        }
    }
}