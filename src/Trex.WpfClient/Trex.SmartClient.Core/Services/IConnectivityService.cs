using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Trex.SmartClient.Core.Services
{
    public interface IConnectivityService : IDisposable
    {
        bool IsOnline { get; }
        bool IsUnstable { get; }
        Task<string> GetWLanIdentification();
        Task<IEnumerable<string>> GetAvailableNetworkList();
    }
}