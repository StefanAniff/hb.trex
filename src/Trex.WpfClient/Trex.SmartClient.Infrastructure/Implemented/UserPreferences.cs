using System;
using Trex.SmartClient.Core.Services;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class UserPreferences : IUserPreferences
    {
        /// <summary>
        /// Gets or sets the sync interval.
        /// </summary>
        /// <value>The sync interval.</value>
        public TimeSpan SyncInterval
        {
            get { return appSettings.Default.SyncInterval; }
            set
            {
                appSettings.Default.SyncInterval = value;
                appSettings.Default.Save();
            }
        }

        public int StatisticsNumOfDaysBack
        {
            get { return appSettings.Default.StatisticsNumOfDaysBack; }
            set
            {
                appSettings.Default.StatisticsNumOfDaysBack = value;
                appSettings.Default.Save();
            }
        }


    }
}
