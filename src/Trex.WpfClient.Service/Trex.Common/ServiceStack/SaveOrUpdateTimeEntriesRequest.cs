using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;
using Trex.Common.DataTransferObjects;

namespace Trex.Common.ServiceStack
{
    public class SaveOrUpdateTimeEntriesRequest : IReturn<SaveOrUpdateTimeEntriesResponse>
    {
        public int UserId { get; set; }
        public List<TimeEntryDto> TimeEntries { get; set; }

       public SaveOrUpdateTimeEntriesRequest()
       {
           TimeEntries = new List<TimeEntryDto>();
       }
    }


    public class SaveOrUpdateTimeEntriesResponse
    {
        public SaveOrUpdateTimeEntriesResponse()
        {
            TimeEntryStatus = new List<TimeEntryUpdatedStatusDto>();
        }

        public List<TimeEntryUpdatedStatusDto> TimeEntryStatus { get; set; }
    }
}