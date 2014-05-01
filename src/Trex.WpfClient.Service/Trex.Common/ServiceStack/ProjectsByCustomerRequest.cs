using System.Collections.Generic;
using ServiceStack.ServiceHost;
using Trex.Common.DataTransferObjects.ProjectAdministration;

namespace Trex.Common.ServiceStack
{
    public class ProjectTasksByCustomerRequest : IReturn<ProjectTasksByCustomerResponse>
    {
        public int CustomerId { get; set; }
    }

    public class ProjectTasksByCustomerResponse
    {
        public ICollection<ProjectTaskDto> Projects { get; set; }
    }
}