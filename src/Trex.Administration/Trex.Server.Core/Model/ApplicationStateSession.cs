using System;
using System.Reflection;
using Trex.Server.Core.Services;

namespace Trex.Server.Core.Model
{
    public class ApplicationStateSession
    {
        private readonly IVersionRepository _versionRepository;

        public ApplicationStateSession(IVersionRepository versionRepository, IAppSettings appSettings)
        {
            AppSettings = appSettings;
            _versionRepository = versionRepository;
        }

        /// <summary>
        /// Gets the app version.
        /// </summary>
        /// <value>The app version.</value>
        public Version AppVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        /// <summary>
        /// Gets the current database version.
        /// </summary>
        /// <value>The current database version.</value>
        public DBVersion DatabaseVersion
        {
            get { return _versionRepository.GetCurrentVersion(); }
        }

        public IAppSettings AppSettings { get; private set; }

        /// <summary>
        /// Checks the version concurrency.
        /// </summary>
        /// <returns></returns>
        public bool CheckDatabaseVersionConcurrency()
        {
            return (DatabaseVersion.VersionNumber.CompareTo(AppSettings.RequiredDatabaseVersion) == 0);
        }

        /// <summary>
        /// Checks the app version concurrency.
        /// </summary>
        /// <returns></returns>
        public bool CheckAppVersionConcurrency()
        {
            return (AppVersion.ToString().CompareTo(AppSettings.RequiredAppVersion) == 0);
        }
    }
}