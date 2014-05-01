using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Core.Services
{
    public interface IProjectService
    {
        Task<List<Project>> GetUnsynced(DateTime lastSyncDate);
        void GetProjects(int companyId);
    }
}
