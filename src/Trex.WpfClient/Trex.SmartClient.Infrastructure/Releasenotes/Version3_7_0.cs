using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_7_0 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Human readable tooltip on timespent in Reports list of entries");
                list.Add("- Double click timeentry/row in reports window to edit it");
                list.Add("- Fixed bug where unassigned tasks/application state were lost on app update");
                list.Add("- Updated graph engine for reports");
                return list.ToArray();
            }
        }
    }
}
