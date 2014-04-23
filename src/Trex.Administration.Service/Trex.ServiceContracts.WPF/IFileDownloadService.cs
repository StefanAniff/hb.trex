using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows.Controls;

namespace Trex.ServiceContracts
{
    [ServiceContract(Namespace = "")]
    public interface IFileDownloadService
    {
        //[OperationContract]
        //ServerResponse GenerateInvoices(int invoiceId);

        //[OperationContract]
        //ServerResponse GenerateSpecification(int invoiceId);

        //[OperationContract]
        //ServerResponse FinalizeInvoiceDraft(List<int> invoiceIds);

        [OperationContract]
        byte[] DownloadTemplateFile(int templateId);

        [OperationContract]
        byte[] DownloadPdfFile(Guid invoiceGuid, int format);

        [OperationContract]
        List<ServerResponse> FinalizeInvoices(List<int> invoiceIds, bool isPreview);

        [OperationContract]
        ServerResponse DeleteInvoiceFiles(int invoiceId);

        [OperationContract]
        ServerResponse ValidateTemplate(byte[] data, int TemplateType);
    }
}
