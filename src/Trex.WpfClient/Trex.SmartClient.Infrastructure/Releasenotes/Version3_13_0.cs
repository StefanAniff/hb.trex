using System.Collections.Generic;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_13_0 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Inactive tasks handling: Timeentries on invalid tasks now shows up properly in T-Rex");
                list.Add("- Better handling of full resync");
                list.Add("- Disabled ability to create new task from T-Rex smartclient");
                list.Add("- Favorite task list! Manage in settings. Use the star when selecting a task");
                return list.ToArray();
            }
        }
    }
}