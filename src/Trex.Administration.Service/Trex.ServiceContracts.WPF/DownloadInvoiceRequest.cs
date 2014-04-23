using System.Runtime.Serialization;
using System.ServiceModel;

namespace Trex.ServiceContracts
{
    [MessageContract]
    public class DownloadInvoiceRequest
    {
        [MessageBodyMember(Order = 1)]
        public Invoice Invoice;

        [MessageBodyMember(Order = 2)]
        public InvoiceTemplate InvoiceTemplate;

        [MessageBodyMember(Order = 3)] 
        public string TrexId;
    }
}
