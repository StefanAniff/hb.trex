using System.Collections.Generic;
using ServiceStack.ServiceHost;
using Trex.Common.DataTransferObjects;

namespace Trex.Common.ServiceStack
{
    public class ForecastSearchOptionsRequest : IReturn<ForecastSearchOptionsResponse>
    {
        
    }

    public class ForecastSearchOptionsResponse
    {
        public List<ForecastUserDto> Users { get; set; }
    }
}