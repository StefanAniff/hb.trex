using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Core.Services
{
    public interface IApplicationStateService
    {
        ApplicationState CurrentState { get;}
        void AddOpenTimeEntry(TimeEntry timeEntry);
        void RemoveOpenTimeEntry(TimeEntry timeEntry);
        void Save();
    }
}