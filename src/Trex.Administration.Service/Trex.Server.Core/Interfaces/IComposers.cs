using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.Server.Infrastructure.Implemented;
using Trex.ServiceContracts;

namespace Trex.Server.Core.Interfaces
{
    public interface ISpecificationComposer
    {
        //void CreatePdf(int invoiceID, string name);
        void TemplateValidation(int invoiceId, IGatherData gatherData, int format);
        void UseTemplate(int invoiceId, int format);
    }

    public interface IInvoiceCompose
    {
        //void CreatePdf(int invoiceID, string name);
        void TemplateValidation(int invoiceId, IGatherData gatherData, int format);
        void UseTemplate(int invoiceId, int format);
    }

    public interface ICreditNoteCompose
    {
        //void CreatePdf(int invoiceID, string name);
        void TemplateValidation(int invoiceId, IGatherData gatherData, int format);
        void UseTemplate(int invoiceId, int format);
    }
}
