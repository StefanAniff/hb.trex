using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_3_5 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Searching tasks does not freeze window anymore");
                list.Add("- Bugfix: Tasks starting next day (00:00) is included in report search");
                list.Add("- Right-click inactive task gives option to clear all saved tasks");
                return list.ToArray();
            }
        }
    }
}
