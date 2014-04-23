using System.Collections.Generic;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_9 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- New submenu: Week overview!!");
                list.Add("- New setting: Choose startscreen");
                list.Add("- Now 2 input types when saving: 'Time Spent' and 'Period'");
                list.Add("- Tooltip on active task name'");
                list.Add("- Slight GUI enhancements to day view");
                list.Add("- There is now a 'searching' indicator when searching for a task");
                list.Add("- Fixed a bug preventing you from changing task on existing timeentry in historyfeed or reports view");
                list.Add("- 'Resync all data' now doesn't remove unsynced timeentries");
                list.Add("- Moved UI settings to its own tab.");
                list.Add("- 1 new task selection view: 'Treeview'");
                list.Add("- Fixed issue that made assigning tasks slow");
                return list.ToArray();
            }
        }
    }
}