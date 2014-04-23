using System.Collections.Generic;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_15_0 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>
                {
                    "- Invoiced timeentries are now readonly",
                    "- Time/invoiced periods can be locked",
                    "- Taskbar contains start/pause buttons",
                    "- Taskbar icon now shows start/pause state"
                };
                return list.ToArray();
            }
        }
    }
}