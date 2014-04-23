using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using Trex.Server.Core;

namespace Trex.Server.Core.Model
{
    public class ApplicationStateSession
    {
        private IVersionRepository _versionRepository;

        public ApplicationStateSession(IVersionRepository versionRepository, IAppSettings appSettings) {

            this.AppSettings = appSettings;
            _versionRepository = versionRepository;

        }

       
        /// <summary>
        /// Gets the app version.
        /// </summary>
        /// <value>The app version.</value>
        public Version AppVersion { 
            get{
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        /// <summary>
        /// Gets the current database version.
        /// </summary>
        /// <value>The current database version.</value>
        public DBVersion DatabaseVersion {
            get {
                return _versionRepository.GetCurrentVersion();
            }
        }


        

        /// <summary>
        /// Checks the version concurrency.
        /// </summary>
        /// <returns></returns>
        public bool CheckDatabaseVersionConcurrency()
        {

            return (this.DatabaseVersion.VersionNumber.CompareTo(AppSettings.RequiredDatabaseVersion)  == 0);
        }

        /// <summary>
        /// Checks the app version concurrency.
        /// </summary>
        /// <returns></returns>
        public bool CheckAppVersionConcurrency()
        {
            return (this.AppVersion.ToString().CompareTo(AppSettings.RequiredAppVersion) == 0);
        }


        #region IApplicationStateSession Members


        public IAppSettings AppSettings
        {
            get;
            private set;
        }

        #endregion
    }
}