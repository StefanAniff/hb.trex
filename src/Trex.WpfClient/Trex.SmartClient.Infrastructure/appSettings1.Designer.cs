﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Trex.SmartClient.Infrastructure {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class appSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static appSettings defaultInstance = ((appSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new appSettings())));
        
        public static appSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("appstate.dat")]
        public string OpenTimeEntriesXmlFile {
            get {
                return ((string)(this["OpenTimeEntriesXmlFile"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("/data/")]
        public string DataStoragePath {
            get {
                return ((string)(this["DataStoragePath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("data.dat")]
        public string TimeTrackerDataFile {
            get {
                return ((string)(this["TimeTrackerDataFile"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string UserName {
            get {
                return ((string)(this["UserName"]));
            }
            set {
                this["UserName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Password {
            get {
                return ((string)(this["Password"]));
            }
            set {
                this["Password"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("01/01/1900 01:01:00")]
        public global::System.DateTime LastSyncDate {
            get {
                return ((global::System.DateTime)(this["LastSyncDate"]));
            }
            set {
                this["LastSyncDate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string CustomerId {
            get {
                return ((string)(this["CustomerId"]));
            }
            set {
                this["CustomerId"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("00:05:00")]
        public global::System.TimeSpan SyncInterval {
            get {
                return ((global::System.TimeSpan)(this["SyncInterval"]));
            }
            set {
                this["SyncInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int UserId {
            get {
                return ((int)(this["UserId"]));
            }
            set {
                this["UserId"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string UserFullName {
            get {
                return ((string)(this["UserFullName"]));
            }
            set {
                this["UserFullName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int StatisticsNumOfDaysBack {
            get {
                return ((int)(this["StatisticsNumOfDaysBack"]));
            }
            set {
                this["StatisticsNumOfDaysBack"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool PersistLogin {
            get {
                return ((bool)(this["PersistLogin"]));
            }
            set {
                this["PersistLogin"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool NoficationEnabled {
            get {
                return ((bool)(this["NoficationEnabled"]));
            }
            set {
                this["NoficationEnabled"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("00:15:00")]
        public global::System.TimeSpan IdleTimeNotificationInterval {
            get {
                return ((global::System.TimeSpan)(this["IdleTimeNotificationInterval"]));
            }
            set {
                this["IdleTimeNotificationInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("01:00:00")]
        public global::System.TimeSpan ActiveTimeNotificationInterval {
            get {
                return ((global::System.TimeSpan)(this["ActiveTimeNotificationInterval"]));
            }
            set {
                this["ActiveTimeNotificationInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool HideWhenMinimized {
            get {
                return ((bool)(this["HideWhenMinimized"]));
            }
            set {
                this["HideWhenMinimized"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool StartTaskWhenActivated {
            get {
                return ((bool)(this["StartTaskWhenActivated"]));
            }
            set {
                this["StartTaskWhenActivated"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool StartTaskWhenApplicationStarts {
            get {
                return ((bool)(this["StartTaskWhenApplicationStarts"]));
            }
            set {
                this["StartTaskWhenApplicationStarts"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("233")]
        public double TabHistoryHeight {
            get {
                return ((double)(this["TabHistoryHeight"]));
            }
            set {
                this["TabHistoryHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("-1")]
        public double ActiveTaskPositionX {
            get {
                return ((double)(this["ActiveTaskPositionX"]));
            }
            set {
                this["ActiveTaskPositionX"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("-1")]
        public double ActiveTaskPositionY {
            get {
                return ((double)(this["ActiveTaskPositionY"]));
            }
            set {
                this["ActiveTaskPositionY"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("126")]
        public double InActiveTaskWidth {
            get {
                return ((double)(this["InActiveTaskWidth"]));
            }
            set {
                this["InActiveTaskWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("84")]
        public double InactiveTaskHeight {
            get {
                return ((double)(this["InactiveTaskHeight"]));
            }
            set {
                this["InactiveTaskHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("18")]
        public double InactiveTaskTimeSpentFontSize {
            get {
                return ((double)(this["InactiveTaskTimeSpentFontSize"]));
            }
            set {
                this["InactiveTaskTimeSpentFontSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("14")]
        public double InactiveTaskTaskNameFontSize {
            get {
                return ((double)(this["InactiveTaskTaskNameFontSize"]));
            }
            set {
                this["InactiveTaskTaskNameFontSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("11")]
        public double InactiveTaskDescriptionFontSize {
            get {
                return ((double)(this["InactiveTaskDescriptionFontSize"]));
            }
            set {
                this["InactiveTaskDescriptionFontSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool StartScreenIsWeekOverview {
            get {
                return ((bool)(this["StartScreenIsWeekOverview"]));
            }
            set {
                this["StartScreenIsWeekOverview"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool StartScreenIsRegistration {
            get {
                return ((bool)(this["StartScreenIsRegistration"]));
            }
            set {
                this["StartScreenIsRegistration"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool TimeEntryViewPeriodSelected {
            get {
                return ((bool)(this["TimeEntryViewPeriodSelected"]));
            }
            set {
                this["TimeEntryViewPeriodSelected"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool TimeEntryViewTimeSpentSelected {
            get {
                return ((bool)(this["TimeEntryViewTimeSpentSelected"]));
            }
            set {
                this["TimeEntryViewTimeSpentSelected"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShowTreeviewSelector {
            get {
                return ((bool)(this["ShowTreeviewSelector"]));
            }
            set {
                this["ShowTreeviewSelector"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string WLansBoundToTimeEntryTypes {
            get {
                return ((string)(this["WLansBoundToTimeEntryTypes"]));
            }
            set {
                this["WLansBoundToTimeEntryTypes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AdvancedSettingsEnabled {
            get {
                return ((bool)(this["AdvancedSettingsEnabled"]));
            }
            set {
                this["AdvancedSettingsEnabled"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("233")]
        public double TabForecastStatisticsHeight {
            get {
                return ((double)(this["TabForecastStatisticsHeight"]));
            }
            set {
                this["TabForecastStatisticsHeight"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ShouldResyncOnNewDeployment {
            get {
                return ((bool)(this["ShouldResyncOnNewDeployment"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string FavoriteTaskGuids {
            get {
                return ((string)(this["FavoriteTaskGuids"]));
            }
            set {
                this["FavoriteTaskGuids"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool WorkPlanRealizedHourBillableOnly {
            get {
                return ((bool)(this["WorkPlanRealizedHourBillableOnly"]));
            }
            set {
                this["WorkPlanRealizedHourBillableOnly"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2013-10-01")]
        public global::System.DateTime MinSelectedDate {
            get {
                return ((global::System.DateTime)(this["MinSelectedDate"]));
            }
            set {
                this["MinSelectedDate"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ForecastData.dat")]
        public string ForecastDataFile {
            get {
                return ((string)(this["ForecastDataFile"]));
            }
        }
    }
}
