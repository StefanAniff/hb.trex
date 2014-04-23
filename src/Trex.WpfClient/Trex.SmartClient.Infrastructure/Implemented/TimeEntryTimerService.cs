using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;
using ApplicationCommands = Trex.SmartClient.Infrastructure.Commands.ApplicationCommands;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class TimeEntryTimerService : ITimeEntryTimerService, IDisposable
    {
        private readonly TimeEntry _timeEntry;
        private DispatcherTimer _timer;
        private DelegateCommand<object> ResumedPcCommand;
        public event EventHandler TimeEntryUpdated;
        public event EventHandler TimerStateChanged;


       
        public TimeEntryTimerService(TimeEntry timeEntry)
        {
            State = timeEntry.IsStopped ? TimerState.Stopped : TimerState.Paused;

            _timeEntry = timeEntry;
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += _timer_Tick;

            ResumedPcCommand = new DelegateCommand<object>(ResumedPCExecute);
            ApplicationCommands.ResumedPC.RegisterCommand(ResumedPcCommand);
        }


        public DateTime? LastStartTime { get; private set; }
        public DateTime? PauseStartTime { get; private set; }

        public TimerState State { get; private set; }

        public TimeEntry TimeEntry
        {
            get { return _timeEntry; }
        }


        public void Start()
        {
            LastStartTime = DateTime.Now;
            PauseStartTime = null;
            State = TimerState.Running;
            _timer.Start();
            OnTimerStateChanged();
        }

        public void Stop()
        {
            if (_timeEntry.IsStopped)
            {
                return;
            }

            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }
            _timeEntry.EndTime = DateTime.Now;
            _timeEntry.IsStopped = true;
            _timeEntry.TimeSpent = GetTimeElapsed();
            State = TimerState.Stopped;
            OnTimerStateChanged();
        }

        public void Pause()
        {
            if (State != TimerState.Running)
            {
                return;
            }

            if (_timer != null && _timer.IsEnabled)
            {
                _timer.Stop();
            }
            
            PauseStartTime = DateTime.Now;

            _timeEntry.TimeEntryHistory.TimeLog.Add(new TimeInterval()
                {
                    EndTime = DateTime.Now,
                    StartTime = LastStartTime.Value
                });

            LastStartTime = null;
            State = TimerState.Paused;
            OnTimerStateChanged();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timeEntry.TimeSpent = GetTimeElapsed();
            _timeEntry.BillableTime = _timeEntry.TimeSpent;
            if (TimeEntryUpdated != null)
            {
                TimeEntryUpdated(this, null);
            }
        }


        /// <summary>
        /// Gets the time elapsed.
        /// The Time elapsed is a sum of all the timehistory intervals plus the latest interval
        /// </summary>
        private TimeSpan GetTimeElapsed()
        {
            var timeSpent = _timeEntry.TimeEntryHistory.TotalTime();

            if (LastStartTime.HasValue)
            {
                var timeElapsed = (DateTime.Now - LastStartTime.Value);
                var newTimeSpent = timeSpent + timeElapsed;

                return newTimeSpent;

            }
            return timeSpent;
        }

        private void ResumedPCExecute(object obj)
        {
            if (LastStartTime.HasValue)
            {
                return;
                var timeElapsed = (DateTime.Now - LastStartTime.Value);
                if (timeElapsed < TimeSpan.FromMinutes(2))
                {
                    return;
                }
                if (_timer.IsEnabled)
                {
                    _timer.Stop();
                }
                var sb = new StringBuilder();
                sb.AppendLine("The timer has been running while your pc was suspended. Was this intentional?");
                sb.AppendFormat("The pc was suspended for {0:hh} hours and {0:mm} minutes.", timeElapsed);
                var result = MessageBox.Show(sb.ToString(), "Timer was running", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    //_timeEntry.TimeSpent = newTimeSpent;
                    _timeEntry.BillableTime = _timeEntry.TimeSpent;
                    if (TimeEntryUpdated != null)
                    {
                        TimeEntryUpdated(this, null);
                    }
                }
                if (_timer.IsEnabled)
                {
                    _timer.Start();
                }
            }
        }


        private void OnTimerStateChanged()
        {
            if (TimerStateChanged != null)
            {
                TimerStateChanged(this, null);
            }
        }

        public void Dispose()
        {
            if (_timer == null)
            {
                return;
            }

            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }
            if (ApplicationCommands.ResumedPC != null && ResumedPcCommand != null)
            {
                ApplicationCommands.ResumedPC.UnregisterCommand(ResumedPcCommand);
            }
            _timer = null;
        }
    }
}