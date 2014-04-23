using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_4_0 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get 
            {
                var list = new List<string>();
                list.Add("- Bugfix: Missing focus on comment when saving task");
                list.Add("- Click on unassigned task label to replace with a comment");
                return list.ToArray();
            }
        }

    }
}
