using System.Collections.Generic;
using System.Xml;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IInvoiceManager
    {
        void BookTimeEntries(Invoice invoice, List<int> timeEntryExcludeList);
        void ResetInvoice(Invoice invoice);
        XmlDocument SerializeInvoice(Invoice invoice);
    }
}