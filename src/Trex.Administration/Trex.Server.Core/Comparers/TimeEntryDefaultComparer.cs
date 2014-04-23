using System.Collections.Generic;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Comparers
{
    public class TimeEntryDefaultComparer : IComparer<TimeEntry>
    {
        #region IComparer<TimeEntry> Members

        public int Compare(TimeEntry x, TimeEntry y)
        {
            var projectCompare = x.Task.Project.Name.CompareTo(y.Task.Project.Name);
            if (projectCompare != 0)
            {
                return projectCompare;
            }

            var taskCompare = x.Task.Name.CompareTo(y.Task.Name);
            if (taskCompare != 0)
            {
                return taskCompare;
            }

            return x.StartTime.CompareTo(y.StartTime);
        }

        #endregion
    }
}