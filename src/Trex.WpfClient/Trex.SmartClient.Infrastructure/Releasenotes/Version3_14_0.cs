using System.Collections.Generic;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_14_0 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>
                    {
                        "- Workplan module/registration."
                    };
                return list.ToArray();
            }
        }
    }
}