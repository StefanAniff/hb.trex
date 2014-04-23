﻿namespace Trex.Server.Core.Services
{
    public interface IUserPreferences
    {
        bool ShowAllTasks { get; set; }
        bool ShowOnlyTasksCreatedByUser { get; set; }
        bool ShowOnlyTasksWithTimeEntriesCreatedByUser { get; set; }
        void Save();
    }
}