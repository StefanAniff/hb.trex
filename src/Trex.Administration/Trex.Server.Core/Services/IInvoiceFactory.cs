using System;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IInvoiceFactory
    {
        Invoice Create(Customer customer, DateTime startTime, DateTime endTime, DateTime invoiceDate, User createdBy);
    }
}