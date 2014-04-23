using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.Server.Core.Interfaces;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class GenerateInvoices : IGenerateInvoices
    {
        public Invoice CombineInvoiceData(IClock clock, int customerId, float VAT, int userId)
        {
            var draft = new Invoice();

            //Customer info
            draft.Regarding = null;

            //Date info
            draft.CreateDate = clock.Now;
            draft.InvoiceDate = clock.Now.Date;
            draft.StartDate = clock.Now.Date;
            draft.EndDate = clock.Now.Date;
            draft.DueDate = clock.Now.Date;

            //Invoice info
            draft.InvoiceID = null;
            draft.Closed = false;
            draft.CreatedBy = userId;
            draft.VAT = VAT;

            return draft;
        }
    }
}
