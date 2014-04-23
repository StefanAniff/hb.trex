using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IInvoiceRepository
    {
        void Update(Invoice invoice);
        Invoice GetById(int invoiceId);
    }
}