using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented.Factories
{
    public class AppStateSessionFactory:IAppStateSessionFactory
    {
        private static ApplicationStateSession _instance;
        private object padLock = new object();
        private IVersionRepository _versionRepository;
        private IAppSettings _appSettings;

        public AppStateSessionFactory(IVersionRepository versionRepository, IAppSettings appSettings)
        {
            _versionRepository = versionRepository;
            _appSettings = appSettings;
        }

        public ApplicationStateSession AppState
        {
            get
            {
                lock (padLock)
                {
                    if (_instance == null)
                    {
                        _instance = new ApplicationStateSession(_versionRepository, _appSettings);
                    }
                    return _instance;
                }
            }
        }

       
    }
}
