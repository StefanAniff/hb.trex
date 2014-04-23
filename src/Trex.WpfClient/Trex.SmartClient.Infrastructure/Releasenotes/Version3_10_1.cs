using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_10_1 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Bound TimeentryType:  You do not have to be connected to WLAn anymore, just be near it.");
                list.Add("- Fixed issue with reassign an existing timeentry.");
                list.Add("V. 3.10.1.1: Added reset active task button to UI settings");
                list.Add("V. 3.10.1.2: Fixed issue with reports module and unsynced timeentry");
                list.Add("V. 3.10.1.3: Fixed issue where premise type display was empty in save dialog");
                return list.ToArray();
            }
        }
    }
}