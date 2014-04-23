using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_9_1 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Better sorting when grouping Date in reports");
                list.Add("- Added customer name on inactive tasks");
                list.Add("- Weekoverview: User is prompted when changing date and have unsaved changes");
                return list.ToArray();
            }
        }
    }
}