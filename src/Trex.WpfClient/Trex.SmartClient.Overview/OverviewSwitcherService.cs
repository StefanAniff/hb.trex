using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Overview.Interfaces;

namespace Trex.SmartClient.Overview
{
    public class OverviewSwitcherService : IOverviewSwitcherService
    {
        // Disabled for H&B
        //private SubMenuInfo SubMenuDailyOverview;
        private SubMenuInfo SubMenuWeekly;

        private bool IsInDayView { get; set; }

        List<SubMenuInfo> history = new List<SubMenuInfo>(); 

        public OverviewSwitcherService()
        {
            OverviewCommands.GoToDaySubMenu.RegisterCommand(new DelegateCommand<object>(GoToDaySubMenuExecute));
            OverviewCommands.GoToPreviousView.RegisterCommand(new DelegateCommand<object>(GoToPreviousViewExecute));
            ApplicationCommands.ChangeSubmenuCommand.RegisterCommand(new DelegateCommand<SubMenuInfo>(ChangeSubmenuExecute));
        }

        private void GoToPreviousViewExecute(object obj)
        {
            var subMenuInfo = history.ElementAt(history.Count - 2);
            if (IsInDayView)
            {
                ApplicationCommands.ChangeSubmenuCommand.Execute(subMenuInfo);
            }
            else
            {
                //ApplicationCommands.ChangeSubmenuCommand.Execute(SubMenuDailyOverview);
            }
        }

        private void ChangeSubmenuExecute(SubMenuInfo obj)
        {
            history.Add(obj);
            IsInDayView = false;

            // Disabled for H&B
            //if (obj.Guid == SubMenuDailyOverview.Guid)
            //{
            //    IsInDayView = true;
            //}
            //else
            //{
            //    IsInDayView = false;
            //}
        }

        private void GoToDaySubMenuExecute(object obj)
        {
            // Disabled for H&B
            //SubMenuDailyOverview.AddArgument(obj);
            //ApplicationCommands.ChangeSubmenuCommand.Execute(SubMenuDailyOverview);
        }

        public void AttachDailyOverviewSubmenu(SubMenuInfo subMenuDailyOverview)
        {
            // Disabled for H&B
            //SubMenuDailyOverview = subMenuDailyOverview;
        }

        public void AttachWeeklyOverviewSubmenu(SubMenuInfo subMenuWeekly)
        {
            SubMenuWeekly = subMenuWeekly;
        }
    }
}
