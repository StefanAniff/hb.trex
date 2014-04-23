using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Utils;


namespace Trex.SmartClient.Infrastructure.Commands
{
    public static class ApplicationCommands
    {
        public static CompositeCommand ChangeScreenCommand = new ExtendedCompositeCommand();
        public static CompositeCommand ChangeSubmenuCommand = new ExtendedCompositeCommand();
        public static CompositeCommand InActiveTaskLayoutChanged = new ExtendedCompositeCommand();

        public static CompositeCommand LoginSucceeded = new ExtendedCompositeCommand();
        public static CompositeCommand UserLoggedOut = new ExtendedCompositeCommand();
        public static CompositeCommand SyncCompleted = new ExtendedCompositeCommand();
        public static CompositeCommand SyncStarted = new ExtendedCompositeCommand();
        public static CompositeCommand StartSync = new ExtendedCompositeCommand();
        public static CompositeCommand SyncFailed = new ExtendedCompositeCommand();
        public static CompositeCommand SyncProgressChanged = new ExtendedCompositeCommand();
        public static CompositeCommand LoadDesktopTask = new ExtendedCompositeCommand();
        public static CompositeCommand ToggleWindow = new ExtendedCompositeCommand();
        public static CompositeCommand DoNotification = new ExtendedCompositeCommand();
        public static CompositeCommand ExitApplication = new ExtendedCompositeCommand();
        public static CompositeCommand DeskTopWindowClosed = new ExtendedCompositeCommand();
        public static CompositeCommand ConnectivityChanged = new ExtendedCompositeCommand();
        public static CompositeCommand ConnectivityUnstable = new ExtendedCompositeCommand();
        public static CompositeCommand UpdateDesktopWindow = new ExtendedCompositeCommand();
        public static CompositeCommand BootCompleted = new ExtendedCompositeCommand();
        public static CompositeCommand ChangePasswordSucceeded = new ExtendedCompositeCommand();
        public static CompositeCommand GetLatestTasks = new ExtendedCompositeCommand();
        public static CompositeCommand GetLatestTasksFinished = new ExtendedCompositeCommand();

        public static CompositeCommand SettingsSaved = new ExtendedCompositeCommand();
        public static CompositeCommand ToggleHistory = new ExtendedCompositeCommand();
        public static CompositeCommand OpenMainWindow = new ExtendedCompositeCommand();
        public static CompositeCommand Resync = new ExtendedCompositeCommand();
        public static CompositeCommand LoginFailed = new ExtendedCompositeCommand();
        public static CompositeCommand ChangePasswordDialogOpen = new ExtendedCompositeCommand();

        public static CompositeCommand PremiseSettingChanged = new ExtendedCompositeCommand();

        public static CompositeCommand ResumedPC = new ExtendedCompositeCommand();

        // Forecasts
        public static CompositeCommand ToggleStatistics = new ExtendedCompositeCommand();
        public static CompositeCommand GetForecastStatistics = new ExtendedCompositeCommand();
    }
}