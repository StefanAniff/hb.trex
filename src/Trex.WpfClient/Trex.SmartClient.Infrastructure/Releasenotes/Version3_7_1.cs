using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_7_1 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Statistics sync fix");
                list.Add("- Application state saving issues");
                list.Add("- Improved sync progressbar");
                list.Add("- Saving a timeentry is now always billable unless explicity set otherwise");
                return list.ToArray();
            }
        }
    }
}