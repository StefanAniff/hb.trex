using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_7_3 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Grouping on 'Date' now works in reports again.");
                list.Add("- 'Update password' button now works");
                list.Add("- Now possible to create new tasks without having to open the window twice");
                list.Add("- Sync progress is now less intrusive.");
                list.Add("- Save timeentry GUI enhancements");
                return list.ToArray();
            }
        }
    }
}