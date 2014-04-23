using System.Collections.Generic;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_9_2 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Fixed issue where default off-premise is always shown in Save Timeentry dialog.");
                list.Add("- Fixed rare case where cancel button in settings view could crash the client");
                return list.ToArray();
            }
        }
    }
}