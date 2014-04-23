using System.Collections.Generic;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
   public class Version3_11_0 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Added premise choice to weekoverview. Changing selection will overrule whole day.");
                list.Add("- Updated Client to .Net 4.5");
                list.Add("- Several I/O performance fixes");
                list.Add("- Several other performance optimizations");
                list.Add("- Fixed issue where a new project or customer could cause sync issue (required full resync)");
                list.Add("- In case of PC crash, the client now saves active task elapsed time every 5. minutes.");
                list.Add("- 3.11.1: Small stability improvements");
                return list.ToArray();
            }
        }
    }
}