using System;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public interface IInvoiceWorker
    {
        ServerResponse GenerateSpecificationFile(int invoiceId);

        /// <summary>
        /// Generates both the invoice for mail and print
        /// <exception cref="CreditNoteMailMissing"></exception>
        /// <exception cref="CreditNotePrintMissing"></exception>
        /// <exception cref="InvoiceMailMissing"></exception>
        /// <exception cref="InvoicePrintMissing"></exception>
        /// <exception cref="SpecificationPrintMissing"></exception>
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <returns>Returns a ServerReponse whether the code is invoices are generated or not</returns>
        ServerResponse GenerateInvoiceFiles(int invoiceId);

        /// <summary>
        /// Generates a credit note to both mail and print
        /// <exception cref="NoInvoiceLines"></exception>
        /// <exception cref="CreditNoteMailMissing"></exception>
        /// <exception cref="CreditNotePrintMissing"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        ServerResponse GenerateCreditNote(int invoiceId);

        /// <summary>
        /// Finalizes an invoice and optional sends it with an ID to the customer. If any exception accours the invoice will be rolled back to draft (if necessary) and a message will be returned
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <param name="isPreview">True if it is a preview. This will give the invoice an InvoiceID and will send it. <strong>False</strong> will not give InvoiceID or send it</param>
        /// <returns>If any exception is caught they will be logged and a ServerResponse with a message and Succes = false will be returned</returns>
        ServerResponse FinalizeInvoice(int invoiceId, bool isPreview);
    }
}