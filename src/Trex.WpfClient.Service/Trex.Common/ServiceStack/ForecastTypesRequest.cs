using System.Collections.Generic;
using ServiceStack.ServiceHost;
using Trex.Common.DataTransferObjects;

namespace Trex.Common.ServiceStack
{
    public class ForecastTypesRequest : IReturn<ForecastTypesResponse>
    {

    }

    public class ForecastTypesResponse
    {
        public List<ForecastTypeDto> ForecastTypeDtos { get; set; }
    }
}