using System;
using System.Collections.Generic;
using Trex.ServiceContracts;

namespace Trex.Server.Core.Interfaces
{
    public interface ITemplateService
    {
        List<InvoiceTemplate> GetInvoiceTemplates();
        void SetStandardInvoiceMailTemplate(int templateId);
        void SetStandardInvoicePrintTemplate(int templateId);
        void SetStandardSpecificationTemplate(int templateId);
        void SetStandardCreditNotePrintTemplate(int templateId);
        void SetStandardCreditNoteMailTemplate(int templateId);

        void SaveTemplate(InvoiceTemplate template);
        void SaveTemplateFile(int templateId, byte[] fileStream);

        void DeleteTemplate(InvoiceTemplate template);

        byte[] DownloadTemplateFile(int templateId);
        byte[] DownloadPdfFile(Guid invoiceID, int format);
        List<InvoiceTemplate> GetAllInvoiceTemplates();
        ServerResponse DeleteInvoiceFiles(int invoiceId);
    }
}