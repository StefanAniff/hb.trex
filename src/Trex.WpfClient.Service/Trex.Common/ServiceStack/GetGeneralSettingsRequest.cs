using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ServiceStack.ServiceHost;

namespace Trex.Common.ServiceStack
{
    [DataContract]
    public class GetGeneralSettingsRequest : ReadonlyRequest, IReturn<GetGeneralSettingsResponse>
    {
        [DataMember]
        public int UserId { get; set; }
    }

    [DataContract]
    public class GetGeneralSettingsResponse
    {
        [DataMember]
        public DateTime TimeEntryMinStartDate { get; set; }
    }

}