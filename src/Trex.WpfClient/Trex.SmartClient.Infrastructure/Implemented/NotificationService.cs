using System;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Eventargs;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Infrastructure.Resources;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class NotificationService : INotificationService
    {
        private readonly IAppSettings _appSettings;
        private DispatcherTimer _dispatcherTimer;
        private TimeSpan _timeElapsed;
        private readonly DelegateCommand<TimeEntry> _taskStartedCommand;
        private readonly DelegateCommand<object> _taskIdleCommand;

        public event EventHandler<NotificationEventArgs> OnNotification;

        public NotificationService(IAppSettings appSettings)
        {
            _appSettings = appSettings;
            _taskStartedCommand = new DelegateCommand<TimeEntry>(TaskStarted);
            TaskCommands.TaskStarted.RegisterCommand(_taskStartedCommand);
            _taskIdleCommand = new DelegateCommand<object>(TaskStopped);
            TaskCommands.TaskIdle.RegisterCommand(_taskIdleCommand);
            TaskStopped(null);
        }

        private void TaskStopped(object obj)
        {
            TryInitializeTimer(_appSettings.IdleTimeNotificationInterval, InactiveTimerTick);            
        }

        private void TaskStarted(TimeEntry timeEntry)
        {
            TryInitializeTimer(_appSettings.ActiveTimeNotificationInterval, ActiveTimeTick);            
        }

        private void TryInitializeTimer(TimeSpan interval, EventHandler tickAction)
        {
            if (!_appSettings.NotificationEnabled || interval == TimeSpan.Zero)
                return;

            if (_dispatcherTimer != null && _dispatcherTimer.IsEnabled)
            {
                _dispatcherTimer.Stop();
            }

            _timeElapsed = TimeSpan.FromHours(0);
            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal) { Interval = interval };
            _dispatcherTimer.Tick += tickAction;
            _dispatcherTimer.Start();
        }

        private void InactiveTimerTick(object sender, EventArgs e)
        {
            _timeElapsed = _timeElapsed.Add(_appSettings.IdleTimeNotificationInterval);

            var text = string.Format("{0} {1:N2} {2}", NotificationResources.SystemIdleText, _timeElapsed.TotalHours, NotificationResources.HourText);
            if (_appSettings.NotificationEnabled)
            {
                ApplicationCommands.DoNotification.Execute(new NotificationEventArgs(NotificationResources.SystemIdleTitle, text, TimeSpan.FromSeconds(30)));
            }
        }

        private void ActiveTimeTick(object sender, EventArgs e)
        {
            _timeElapsed = _timeElapsed.Add(_appSettings.ActiveTimeNotificationInterval);
            if (_appSettings.NotificationEnabled)
            {
                ApplicationCommands.DoNotification.Execute(new NotificationEventArgs(NotificationResources.SystemActiveTitle, NotificationResources.SystemActiveText, TimeSpan.FromSeconds(20)));
            }
        }

        public void Dispose()
        {
            if (_taskStartedCommand != null)
            {
                TaskCommands.TaskStarted.UnregisterCommand(_taskStartedCommand);
            }
            if (_taskIdleCommand != null)
            {
                TaskCommands.TaskIdle.UnregisterCommand(_taskIdleCommand);
            }
            if (_dispatcherTimer != null)
            {
                _dispatcherTimer.Stop();
                _dispatcherTimer = null;
            }
        }
    }
}