using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Trex.ServiceContracts
{[DataContract]
    public class ServerResponse
    {
        public ServerResponse(string responseMessage, bool success, int invoiceId, bool toPrint = false)
        {
            Response = responseMessage;
            Success = success;
            InvoiceId = invoiceId;
            ToPrint = toPrint;
        }

        public ServerResponse(string responseMessage, bool success)
        {
            Response = responseMessage;
            Success = success;
            InvoiceId = 0;
        }

        [DataMember]
        public string Response { get; set; }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public int InvoiceId { get; set; }

        [DataMember]
        public bool ToPrint { get; set; }
    }
}
