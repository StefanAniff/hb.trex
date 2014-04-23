using System;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface ITaskFactory
    {
        Task Create(Guid guid, DateTime createDate, DateTime? changeDate, string name, string description, User createdBy,
                           Project project, Tag tag, Task parentTask, double estimatePessimistic,
                           double estimateOptimistic, double estimateRealistic, double estimateCalculated,
                           double timeLeft);
    }
}
