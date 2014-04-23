using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Core.Services
{
    public interface IUserPreferences
    {
        TimeSpan SyncInterval { get; set; }
        int StatisticsNumOfDaysBack { get; set; }
    }
}
