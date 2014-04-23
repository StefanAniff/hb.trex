using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_10_0 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Historyfeew now supports copy command. Copies taskname");
                list.Add("- Historyfeew can overwrite default timeentry type");
                list.Add("- Bind wlan to tmeentryType");
                return list.ToArray();
            }
        }
    }
}