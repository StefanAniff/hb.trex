using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Services;
using System.Linq;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class AppSettings : IAppSettings
    {
        private readonly IRegionNames _regionNames;

        public AppSettings(IRegionNames regionNames)
        {
            _regionNames = regionNames;
        }

        public IRegionNames RegionNames
        {
            get { return _regionNames; }
        }

        public EnviromentEnum Enviroment
        {
            get
            {
                var environment = ConfigurationManager.AppSettings["environment"];

                //enviroment specific
                switch (environment)
                {
                    case "Test":
                        return EnviromentEnum.Test;
                    case "Release":
                        return EnviromentEnum.Release;
                    case "ReleaseExternal":
                        return EnviromentEnum.ReleaseExternal;
                    default:
                        return EnviromentEnum.Debug;
                }
            }
        }

        public string EnvironmentDescription
        {
            get { return GetEnumDescriptionAttribute(Enviroment); }
        }

        public string JsonEndpointUri
        {
            get
            {
                return ConfigurationManager.AppSettings["servicestackEndpointUri"];
            }
        }

        public string TrexWcfServiceEndpointUri
        {
            get
            {
                return ConfigurationManager.AppSettings["wcfEndpointUri"];
            }
        }

        public string AuthenticationServiceUri
        {
            get
            {
                return ConfigurationManager.AppSettings["authEndPointUri"];
            }
        }

        public bool ShouldResyncOnNewDeployment
        {
            get { return appSettings.Default.ShouldResyncOnNewDeployment; }
        }

        public bool WorkPlanRealizedHourBillableOnly
        {
            get { return appSettings.Default.WorkPlanRealizedHourBillableOnly; }
            set { appSettings.Default.WorkPlanRealizedHourBillableOnly = value; }
        }

        public DateTime MinTimeEntryDate
        {
            get { return appSettings.Default.MinSelectedDate; }
            set { appSettings.Default.MinSelectedDate = value; }
        }


        public TimeSpan SessionTimeOut
        {
            get { return TimeSpan.FromMinutes(1); }
        }

        public string UserName
        {
            get { return appSettings.Default.UserName; }
        }

        public string UserDisplayName
        {
            get { return appSettings.Default.UserFullName; }
        }

        public int UserId
        {
            get { return appSettings.Default.UserId; }
        }

        public string Password
        {
            get
            {
                //TODO: Encrypt!
                return appSettings.Default.Password;
            }
        }

        public string CustomerId
        {
            get { return appSettings.Default.CustomerId; }
        }

        public bool PersistLogin
        {
            get { return appSettings.Default.PersistLogin; }
        }

        public TimeSpan SyncInterval
        {
            get { return appSettings.Default.SyncInterval; }
            set { appSettings.Default.SyncInterval = value; }
        }

        public DateTime LastSyncDate
        {
            get { return appSettings.Default.LastSyncDate; }
            set { appSettings.Default.LastSyncDate = value; }
        }

        public bool NotificationEnabled
        {
            get { return appSettings.Default.NoficationEnabled; }
            set { appSettings.Default.NoficationEnabled = value; }
        }

        public TimeSpan IdleTimeNotificationInterval
        {
            get { return appSettings.Default.IdleTimeNotificationInterval; }
            set { appSettings.Default.IdleTimeNotificationInterval = value; }
        }

        public TimeSpan ActiveTimeNotificationInterval
        {
            get { return appSettings.Default.ActiveTimeNotificationInterval; }
            set { appSettings.Default.ActiveTimeNotificationInterval = value; }
        }

        public bool HideWhenMinimized
        {
            get { return appSettings.Default.HideWhenMinimized; }
            set { appSettings.Default.HideWhenMinimized = value; }
        }

        public bool StartTaskWhenActivated
        {
            get { return appSettings.Default.StartTaskWhenActivated; }
            set { appSettings.Default.StartTaskWhenActivated = value; }
        }

        public bool StartTaskWhenApplicationStarts
        {
            get { return appSettings.Default.StartTaskWhenApplicationStarts; }
            set { appSettings.Default.StartTaskWhenApplicationStarts = value; }
        }

        public int HistoryNumOfDaysBack
        {
            get { return appSettings.Default.StatisticsNumOfDaysBack; }
            set { appSettings.Default.StatisticsNumOfDaysBack = value; }
        }

        public double TabHistoryHeight
        {
            get { return appSettings.Default.TabHistoryHeight; }
            set { appSettings.Default.TabHistoryHeight = value; }
        }

        public double TabForecastStatisticsHeight
        {
            get { return appSettings.Default.TabForecastStatisticsHeight; }
            set { appSettings.Default.TabForecastStatisticsHeight = value; }
        }

        public double ActiveTaskPositionX
        {
            get { return appSettings.Default.ActiveTaskPositionX; }
            set { appSettings.Default.ActiveTaskPositionX = value; }
        }

        public double ActiveTaskPositionY
        {
            get { return appSettings.Default.ActiveTaskPositionY; }
            set { appSettings.Default.ActiveTaskPositionY = value; }
        }

        public double InActiveTaskWidth
        {
            get { return appSettings.Default.InActiveTaskWidth; }
            set { appSettings.Default.InActiveTaskWidth = value; }
        }

        public double InactiveTaskHeight
        {
            get { return appSettings.Default.InactiveTaskHeight; }
            set { appSettings.Default.InactiveTaskHeight = value; }
        }

        public double InactiveTaskDescriptionFontSize
        {
            get { return appSettings.Default.InactiveTaskDescriptionFontSize; }
            set { appSettings.Default.InactiveTaskDescriptionFontSize = value; }
        }

        public double InactiveTaskTaskNameFontSize
        {
            get { return appSettings.Default.InactiveTaskTaskNameFontSize; }
            set { appSettings.Default.InactiveTaskTaskNameFontSize = value; }
        }

        public double InactiveTaskTimeSpentFontSize
        {
            get { return appSettings.Default.InactiveTaskTimeSpentFontSize; }
            set { appSettings.Default.InactiveTaskTimeSpentFontSize = value; }
        }

        public bool StartScreenIsRegistration
        {
            get { return appSettings.Default.StartScreenIsRegistration; }
            set { appSettings.Default.StartScreenIsRegistration = value; }
        }

        public bool StartScreenIsWeekOverview
        {
            get { return appSettings.Default.StartScreenIsWeekOverview; }
            set { appSettings.Default.StartScreenIsWeekOverview = value; }
        }

        public bool TimeEntryViewTimeSpentSelected
        {
            get { return appSettings.Default.TimeEntryViewTimeSpentSelected; }
            set { appSettings.Default.TimeEntryViewTimeSpentSelected = value; }
        }

        public bool TimeEntryViewPeriodSelected
        {
            get { return appSettings.Default.TimeEntryViewPeriodSelected; }
            set { appSettings.Default.TimeEntryViewPeriodSelected = value; }
        }

        public bool ShowTreeviewSelector
        {
            get { return appSettings.Default.ShowTreeviewSelector; }
            set { appSettings.Default.ShowTreeviewSelector = value; }
        }

        public string WLansBoundToTimeEntryTypes
        {
            get { return appSettings.Default.WLansBoundToTimeEntryTypes; }
            set { appSettings.Default.WLansBoundToTimeEntryTypes = value; }
        }

        private int? _userDefaultTimeEntryTypeId;

        public int? UserDefaultTimeEntryTypeId
        {
            get { return _userDefaultTimeEntryTypeId; }
            set { _userDefaultTimeEntryTypeId = value; }
        }

        public bool AdvancedSettingsEnabled
        {
            get { return appSettings.Default.AdvancedSettingsEnabled; }
            set { appSettings.Default.AdvancedSettingsEnabled = value; }
        }

        public string DataDirectory { get; set; }

        public void Save()
        {
            appSettings.Default.Save();
        }

        public void PersistUser(string userName, string password, string serviceUrl, bool persistLogin, int userId, string userFullName)
        {
            appSettings.Default.UserName = userName;
            appSettings.Default.Password = password;
            appSettings.Default.CustomerId = serviceUrl;
            appSettings.Default.PersistLogin = persistLogin;
            appSettings.Default.UserFullName = userFullName;
            appSettings.Default.UserId = userId;
            appSettings.Default.Save();
        }

        public void DeleteUser()
        {
            appSettings.Default.Password = string.Empty;
            appSettings.Default.PersistLogin = false;
            appSettings.Default.Save();
        }

        public void Reset()
        {
            appSettings.Default.Reload();
        }

        public IEnumerable<Guid> FavoriteTaskGuids
        {

            get
            {
                var favoriteTaskGuids = appSettings.Default.FavoriteTaskGuids
                                                   .Split(new[] { '#' }, StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(Guid.Parse);
                return favoriteTaskGuids;
            }
            set { appSettings.Default.FavoriteTaskGuids = string.Join("#", value); }
        }

        #region Helpers

        private string GetEnumDescriptionAttribute(Enum someEnum)
        {
            var fi = someEnum.GetType().GetField(someEnum.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }

            return someEnum.ToString();
        }

        #endregion
    }
}
