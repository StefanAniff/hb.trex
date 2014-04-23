using System.Collections.Generic;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_8_0 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Added tooltip on tasks when saving an unassigned task");
                list.Add("- New 'inactive task layout' settings, to support high resolution screens");
                list.Add("- Fixed an issue with fire-and-forget commands");
                list.Add("- Log out button now asks if you are sure you want to log out");
                list.Add("- Added a new menu: Overview");
                list.Add("- Added a new overview: Daily");
                return list.ToArray();
            }
        }
    }
}