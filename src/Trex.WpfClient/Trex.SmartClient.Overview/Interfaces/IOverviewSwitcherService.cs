using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.SmartClient.Core.Implemented;

namespace Trex.SmartClient.Overview.Interfaces
{
    public interface IOverviewSwitcherService
    {
        void AttachDailyOverviewSubmenu(SubMenuInfo subMenuDailyOverview);
        void AttachWeeklyOverviewSubmenu(SubMenuInfo subMenuWeekly);
    }
}
