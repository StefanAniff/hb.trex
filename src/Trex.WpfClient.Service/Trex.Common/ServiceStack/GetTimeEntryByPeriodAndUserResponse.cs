using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.Common.DataTransferObjects;

namespace Trex.Common.ServiceStack
{
    public class GetTimeEntryByPeriodAndUserResponse
    {
        public GetTimeEntryByPeriodAndUserResponse()
        {
            TimeEntries = new List<TimeEntryDto>();
        }

        public List<TimeEntryDto> TimeEntries { get; set; }
    }
}