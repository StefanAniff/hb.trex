using System.Collections.Generic;
using Aspose.Words;
using Trex.ServiceContracts;
using Trex.Server.Infrastructure.Implemented;

namespace Trex.Server.Core.Interfaces
{
    public interface IInvoiceBuilder : IBuilderBase
    {
    }

    public interface ISpecificationBuilder : IBuilderBase
    {
    }

    public interface IBuilderBase
    {
        void InsertInvoiceData(Invoice invoice, DocumentBuilder builder);
        void ValidateMapInput(string a, string b);
        Dictionary<string, string> MapData(Invoice invoice);
        void InsertInvoiceLines(List<InvoiceLine> invoiceLineData, int invoiceId, DocumentBuilder builder);
        bool WriteAtBookMark(string bookMarkName, string value, DocumentBuilder builder);
        bool SetBookMark(string bookMarkName, DocumentBuilder builder);
        DocumentBuilder BuilderSettings(DocumentBuilder builder, double prefferedWidth, LineStyle lineStyle, ParagraphAlignment alignment);
    }

    public interface IComposerBase
    {
        void UseTemplate(int invoiceId, int format);
        void TemplateValidation(int invoiceId, IGatherData gatherData, int format);
        //void CreatePdf(int invoiceID, string name, int format);
    }
}
