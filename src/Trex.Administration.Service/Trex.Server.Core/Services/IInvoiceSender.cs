using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public interface IInvoiceSender
    {
        bool SendInvoiceEmail(int invoiceId);
        bool SendToMail(int invoiceId);
    }
}