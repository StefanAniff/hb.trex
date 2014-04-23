using Trex.Server.Core.Interfaces;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public interface IGenerateInvoices
    {
        Invoice CombineInvoiceData(IClock clock, int customerId, float VAT, int userId);
    }
}