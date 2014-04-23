using System;
using System.ComponentModel;
using Trex.SmartClient.Core.Eventargs;

namespace Trex.SmartClient.Core.Services
{
    public interface ISyncService : IDisposable
    {
        bool SyncInProgress { get; }
        bool IsRunning { get; }
        bool ForceResync { get; }
    }
}
