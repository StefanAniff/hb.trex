using System.Collections.Generic;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_15_1 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>
                    {
                        "- Edit others workplan by role/permission",                    
                    };
                return list.ToArray();
            }
        }
    }
}