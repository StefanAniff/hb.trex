using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented.Factories
{
    public class AppStateSessionFactory : IAppStateSessionFactory
    {
        private static ApplicationStateSession _instance;
        private readonly IAppSettings _appSettings;
        private readonly object _padLock = new object();
        private readonly IVersionRepository _versionRepository;

        public AppStateSessionFactory(IVersionRepository versionRepository, IAppSettings appSettings)
        {
            _versionRepository = versionRepository;
            _appSettings = appSettings;
        }

        #region IAppStateSessionFactory Members

        public ApplicationStateSession AppState
        {
            get
            {
                lock (_padLock)
                {
                    if (_instance == null)
                    {
                        _instance = new ApplicationStateSession(_versionRepository, _appSettings);
                    }
                    return _instance;
                }
            }
        }

        #endregion
    }
}