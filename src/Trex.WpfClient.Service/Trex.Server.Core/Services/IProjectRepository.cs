using System;
using System.Collections.Generic;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IProjectRepository : IRepository<Project>
    {
        IEnumerable<Project> GetByChangeDate(DateTime startDate);
        IEnumerable<Project> GetByCustomerId(int customerId);
    }
}
