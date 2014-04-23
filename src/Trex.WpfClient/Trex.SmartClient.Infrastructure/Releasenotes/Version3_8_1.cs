using System.Collections.Generic;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_8_1 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Fixed an issue with 'Continue search on server'");
                list.Add("- New wrapping behaviour for Taskname on inactive tasks");
                list.Add("- Labels on x-axis in reports is now more intelligent");
                list.Add("- Reset inactive task layout to a better default look. Only for some users");
                return list.ToArray();
            }
        }
    }
}