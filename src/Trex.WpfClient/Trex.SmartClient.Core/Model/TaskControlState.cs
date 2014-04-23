using System;

namespace Trex.SmartClient.Core.Model
{
    [Serializable]
    public class TaskControlState
    {

        public static TaskControlState Create()
        {
            return new TaskControlState()
            {
                TimeEntry = Model.TimeEntry.Create(),
                Active = false,
                Assigned = false,
                Paused = false,
                PauseStartTime = null,
                TaskName = "Unassigned Task"

            };
        }

        public static TaskControlState Create(TimeEntry timeEntry)
        {
            return new TaskControlState()
            {
                TimeEntry = timeEntry,
                Active = true,
                Assigned = true,
                Paused = false,
                PauseStartTime = null,
                Saved = false,
                EditMode = true


            };
        }


        public string TaskName { get; set; }
        public bool Active { get; set; }
        public bool Saved { get; set; }
        public bool Assigned { get; set; }
        public bool Paused { get; set; }
        public DateTime? LastStartTime { get; set; }
        public DateTime? PauseStartTime { get; set; }
        public TimeEntry TimeEntry { get; set; }
        public bool EditMode { get; set; }

        public TimeSpan GetTimeElapsed()
        {
            TimeSpan timeSpent = TimeEntry.TimeEntryHistory.TotalTime();
            if (LastStartTime.HasValue)
                timeSpent = timeSpent + (DateTime.Now - LastStartTime.Value);

            return timeSpent;


        }

        public void Start()
        {
            LastStartTime = DateTime.Now;
            PauseStartTime = null;
            Saved = false;
            Paused = false;
        }

        public void Stop()
        {
            PauseStartTime = null;
            Paused = false;
            TimeEntry.EndTime = DateTime.Now;
            TimeEntry.TimeSpent = GetTimeElapsed();

        }

        public void Pause()
        {
            PauseStartTime = DateTime.Now;
            Paused = true;

            TimeEntry.TimeEntryHistory.TimeLog.Add(new TimeInterval()
                                                        {
                                                            EndTime = DateTime.Now,
                                                            StartTime = LastStartTime.Value
                                                        });
                                                    
            LastStartTime = null;
        }


    }
}
