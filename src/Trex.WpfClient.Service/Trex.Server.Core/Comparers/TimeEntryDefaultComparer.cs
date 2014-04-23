using System.Collections.Generic;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Comparers
{
    public class TimeEntryDefaultComparer:IComparer<TimeEntry>
    {
        public int Compare(TimeEntry x, TimeEntry y)
        {
            int projectCompare = x.Task.Project.ProjectName.CompareTo(y.Task.Project.ProjectName);
            if (projectCompare != 0)
                return projectCompare;

            int taskCompare = x.Task.TaskName.CompareTo(y.Task.TaskName);
            if (taskCompare != 0)
                return taskCompare;

            return x.StartTime.CompareTo(y.StartTime);
        }
    }
}