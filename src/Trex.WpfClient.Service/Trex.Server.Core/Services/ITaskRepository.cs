using System;
using System.Collections.Generic;
using Trex.Common.DataTransferObjects;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface ITaskRepository : IRepository<Task>
    {
        Task GetByGuid(Guid guid);
        bool ExistsByGuid(Guid guid);
    
        List<Task> GetByChangeDate(DateTime? startDate);
        List<Task> GetByIds(List<int> taskIds);
    }
}
