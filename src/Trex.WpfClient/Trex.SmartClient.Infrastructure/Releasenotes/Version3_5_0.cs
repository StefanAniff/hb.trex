using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_5_0 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Release notes");
                list.Add("- Added tooltip on history item");
                list.Add("- Selected history item expands height to content");
                list.Add("- Active task panel can now be dragged/moved");
                list.Add("- Resizing of the historyfeed available");
                list.Add("- Customer name column in historyfeed");
                return list.ToArray();
            }
        }

    }
}
