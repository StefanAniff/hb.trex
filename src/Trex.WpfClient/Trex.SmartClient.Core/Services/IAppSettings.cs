using System;
using System.Collections.Generic;
using System.ComponentModel;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Core.Services
{
    public enum EnviromentEnum
    {
        [Description("Debug")]
        Debug,
        [Description("Test")]
        Test,
        [Description("D60 Internal")]
        Release,
        [Description("External customers")]
        ReleaseExternal // Customers
    }
    public interface IAppSettings
    {
        IEnumerable<Guid> FavoriteTaskGuids { get; set; }
        IRegionNames RegionNames { get; }
        TimeSpan SessionTimeOut { get; }
        string UserName { get; }
        string UserDisplayName { get; }
        int UserId { get; }
        string Password { get; }
        string CustomerId { get; }
        bool PersistLogin { get; }
        TimeSpan SyncInterval { get; set; }
        DateTime LastSyncDate { get; set; }
        bool NotificationEnabled { get; set; }
        TimeSpan IdleTimeNotificationInterval { get; set; }
        TimeSpan ActiveTimeNotificationInterval { get; set; }
        bool HideWhenMinimized { get; set; }
        bool StartTaskWhenActivated { get; set; }
        bool StartTaskWhenApplicationStarts { get; set; }
        int HistoryNumOfDaysBack { get; set; }
        double TabHistoryHeight { get; set; }
        double ActiveTaskPositionX { get; set; }
        double ActiveTaskPositionY { get; set; }
        double InActiveTaskWidth { get; set; }
        double InactiveTaskHeight { get; set; }
        double InactiveTaskDescriptionFontSize { get; set; }
        double InactiveTaskTaskNameFontSize { get; set; }
        double InactiveTaskTimeSpentFontSize { get; set; }
        double TabForecastStatisticsHeight { get; set; }
        bool StartScreenIsRegistration { get; set; }
        bool StartScreenIsWeekOverview { get; set; }
        bool TimeEntryViewTimeSpentSelected { get; set; }
        bool TimeEntryViewPeriodSelected { get; set; }
        bool ShowTreeviewSelector { get; set; }
        string WLansBoundToTimeEntryTypes { get; set; }
        int? UserDefaultTimeEntryTypeId { get; set; }
        bool AdvancedSettingsEnabled { get; set; }
        string DataDirectory { get; set; }
        EnviromentEnum Enviroment { get; }
        string JsonEndpointUri { get; }
        string TrexWcfServiceEndpointUri { get; }
        string AuthenticationServiceUri { get; }
        bool ShouldResyncOnNewDeployment { get;  }
        bool WorkPlanRealizedHourBillableOnly { get; set; }
        DateTime MinTimeEntryDate { get; set; }
        string EnvironmentDescription { get; }
        void Save();

        void PersistUser(string userName, string password, string serviceUrl, bool persistLogin, int userId,
                         string userFullName);

        void DeleteUser();
        void Reset();
    }
}
