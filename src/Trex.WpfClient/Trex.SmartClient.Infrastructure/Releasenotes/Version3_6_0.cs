using System.Collections.Generic;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_6_0 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Rewrote entire webservice for performance");
                list.Add("- \"Continue search on server\" option when searching");
                list.Add("- Bugfix: \"Forgot password\"");
                list.Add("- Bugfix: Inactive tasks not wrapping to a new line");
                return list.ToArray();
            }
        }
    }
}
