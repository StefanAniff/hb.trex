using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Core.Services
{
    public interface IWindowStatePersister
    {
        List<TaskControlState> GetOpenTaskControls();
        void SaveOpenTaskControls(List<TaskControlState> controlStates);
        void ClearOpenTasks();
        
    }
}