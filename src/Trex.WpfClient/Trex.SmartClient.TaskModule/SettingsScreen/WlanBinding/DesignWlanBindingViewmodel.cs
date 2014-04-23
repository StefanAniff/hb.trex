using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using Telerik.Windows.Data;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Infrastructure.Implemented;

namespace Trex.SmartClient.TaskModule.SettingsScreen.WlanBinding
{
    public interface IWlanBindingViewmodel
    {
        DelegateCommand<object> DeleteBinding { get; set; }
        List<TimeEntryType> TimeEntryTypes { get; }
        ObservableItemCollection<UserWLanTimeEntryTypeItemViewmodel> WLansBoundToTimeEntryTypes { get; }
     
    }

    public class DesignWlanBindingViewmodel : IWlanBindingViewmodel
    {
        public DelegateCommand<object> DeleteBinding { get; set; }

        public List<TimeEntryType> TimeEntryTypes
        {
            get
            {
                var list = new List<TimeEntryType>();
                list.Add(new TimeEntryType
                    {
                        Name = "On premise (client)",
                        Id = 1
                    });
                list.Add(new TimeEntryType
                    {
                        Name = "Off premise",
                        Id = 2
                    });

                return list;
            }
        }

        public ObservableItemCollection<UserWLanTimeEntryTypeItemViewmodel> WLansBoundToTimeEntryTypes
        {
            get
            {
                ObservableItemCollection<UserWLanTimeEntryTypeItemViewmodel> list = new ObservableItemCollection<UserWLanTimeEntryTypeItemViewmodel>();

                list.Add(new UserWLanTimeEntryTypeItemViewmodel(null, new UserWLanTimeEntryType(1, "d60 net")));
         
                list.Add(new UserWLanTimeEntryTypeItemViewmodel(null, new UserWLanTimeEntryType(1, "CTH Home")));
           
                list.Add(new UserWLanTimeEntryTypeItemViewmodel(null, new UserWLanTimeEntryType(2, "Foreign")));

                return list;
            }
        }

     
    }
}