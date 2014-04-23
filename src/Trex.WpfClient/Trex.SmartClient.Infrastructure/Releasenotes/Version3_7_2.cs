using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_7_2 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Fixed MaxObjectsInGraph issue that happened for a small group of users");
                return list.ToArray();
            }
        }
    }
}