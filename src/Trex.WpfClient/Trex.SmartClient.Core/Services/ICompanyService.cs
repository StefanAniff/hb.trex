using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Core.Services
{

    public interface ICompanyService
    {        
        Task<List<Company>> GetUnsynced(DateTime lastSyncDate);
    }
}
