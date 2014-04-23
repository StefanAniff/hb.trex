using System;
using ServiceStack.ServiceHost;

namespace Trex.Common.ServiceStack
{
    public class GetTimeEntryByPeriodAndUserRequest : ReadonlyRequest, IReturn<GetTimeEntryByPeriodAndUserResponse>
    {
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}