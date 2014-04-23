using System.Collections.Generic;
using Aspose.Words;
using Trex.ServiceContracts;

namespace Trex.Server.Core.Interfaces
{
    public interface IGatherData
    {
        bool ValidateTemplate(InvoiceTemplate template);
        Invoice GetInvoiceData(int invoiceId);
        List<InvoiceLine> GetInvoiceLines(int invoiceId);

        DocumentBuilder DocumentBuilder { get; }
        CustomerInvoiceGroup GetInvoiceTemplateId(int invoiceID, int format);
        InvoiceTemplate GetInvoiceTemplate(int templateId);
        CustomerInvoiceGroup GetMetaData(int customerInvoiceGroupId);
        CustomerInvoiceGroup GetCustomerInvoiceGroupsTemplateData(int invoiceId, int invoiceID);
        void ConvertBinaryToDocument(int templateId);
    }
}