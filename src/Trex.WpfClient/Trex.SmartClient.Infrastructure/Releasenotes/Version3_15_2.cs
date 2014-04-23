using System.Collections.Generic;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_15_2 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Added option to remove favorite tasks in settings.");
                list.Add("- Fixed issue where loading large amount of TimeEntries in Reports failed");
                return list.ToArray();
            }
        }
    }
}