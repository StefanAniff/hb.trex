using System.Collections.Generic;
using ServiceStack.ServiceHost;
using Trex.Common.DataTransferObjects.ProjectAdministration;

namespace Trex.Common.ServiceStack
{
    public class AllCustomersRequest : IReturn<AllCustomersResponse>
    {
        
    }

    public class AllCustomersResponse
    {
        public ICollection<BasicEntityDto> Customers { get; set; }
    }
}