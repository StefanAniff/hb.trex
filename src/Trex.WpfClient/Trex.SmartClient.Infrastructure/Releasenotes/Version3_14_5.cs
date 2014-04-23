using System.Collections.Generic;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_14_5 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>
                    {
                        "- New window Workplan overview",
                    };
                return list.ToArray();
            }
        }
    }
}