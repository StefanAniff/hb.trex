using System.Collections.Generic;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_14_3 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>
                    {
                        "- More statistics for displayed month in Workplan",
                        "- Settings UI -> Allow you to define definition of realized hours in Workplan"
                    };
                return list.ToArray();
            }
        }
    }
}