using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Words;
using Test_InvoiceBuilder;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.BaseClasses;
using Trex.Server.Infrastructure.Exceptions;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class InvoiceWorker : LogableBase, IInvoiceWorker
    {
        private readonly ISpecificationComposer _specificationComposer;
        private readonly IInvoiceCompose _invoiceCompose;
        private readonly ICreditNoteCompose _creditNoteCompose;
        private readonly IInvoiceService _invoiceService;

        public InvoiceWorker(ISpecificationComposer specificationComposer, IInvoiceCompose invoiceCompose, ICreditNoteCompose creditNoteCompose, IInvoiceService invoiceService)
        {
            _specificationComposer = specificationComposer;
            _invoiceCompose = invoiceCompose;
            _creditNoteCompose = creditNoteCompose;
            _invoiceService = invoiceService;
        }

        public ServerResponse GenerateSpecificationFile(int invoiceId)
        {
            try
            {
                if (invoiceId == 0)
                    throw new ArgumentException("No InvoiceID is detected. Please provide one");

                _specificationComposer.UseTemplate(invoiceId, 3);

                return new ServerResponse("Success", true);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                LogError(ex);
                return new ServerResponse("Generating specification failed", false);
            }
        }

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
        public ServerResponse GenerateInvoiceFiles(int invoiceId)
        {
            try
            {
                if (invoiceId == 0)
                    throw new ArgumentException("No InvoiceID is detected. Please provide one");

                _invoiceCompose.UseTemplate(invoiceId, 1);
                _invoiceCompose.UseTemplate(invoiceId, 2);

                return new ServerResponse("Success", true);
            }
            catch (CreditNoteMailMissing)
            {
                throw;
            }
            catch (CreditNotePrintMissing)
            {
                throw;
            }
            catch (InvoiceMailMissing)
            {
                throw;
            }
            catch (InvoicePrintMissing)
            {
                throw;
            }
            catch (SpecificationPrintMissing)
            {
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                return new ServerResponse(ex.Message, false);
            }
        }

        /// <summary>
        /// Generates a credit note to both mail and print
        /// <exception cref="NoInvoiceLines"></exception>
        /// <exception cref="CreditNoteMailMissing"></exception>
        /// <exception cref="CreditNotePrintMissing"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public ServerResponse GenerateCreditNote(int invoiceId)
        {
            try
            {
                if (invoiceId == 0)
                    throw new ArgumentException("No InvoiceID is detected. Please select one");

                _creditNoteCompose.UseTemplate(invoiceId, 4);
                _creditNoteCompose.UseTemplate(invoiceId, 5);

                return new ServerResponse("Success", true);
            }
            catch (NoInvoiceLines ex)
            {
                LogError(ex);
                return new ServerResponse("Specification-pdf was not create due to no invoice line", true);
            }
            catch (CreditNoteMailMissing ex)
            {
                throw ex;
            }
            catch (CreditNotePrintMissing ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                LogError(ex);
                return new ServerResponse(ex.Message, false);
            }
        }

        /// <summary>
        /// Finalizes an invoice and optional sends it with an ID to the customer. If any exception accours the invoice will be rolled back to draft (if necessary) and a message will be returned
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <param name="isPreview">True if it is a preview. This will give the invoice an InvoiceID and will send it. <strong>False</strong> will not give InvoiceID or send it</param>
        /// <returns>If any exception is caught they will be logged and a ServerResponse with a message and Succes = false will be returned</returns>
        public ServerResponse FinalizeInvoice(int invoiceId, bool isPreview)
        {
            try
            {
                var errorList = _invoiceService.ValidateFinalize(invoiceId);

                if (errorList.Count() != 0)
                {
                    var invoiceData = _invoiceService.GetInvoiceById(invoiceId);
                    var customerInvoiceGroupData = _invoiceService.GetCustomerInvoiceGroupByInvoiceId(invoiceId);
                    var customerData = _invoiceService.GetCustomerByInvoiceId(invoiceId);

                    foreach (var e in errorList)
                    {
                        return new ServerResponse(string.Format("\"{0}\" was not set correctly for customer {1}'s group \"{2}\" regarding \"{3}\" ",
                            e,
                            customerData.CustomerName,
                            customerInvoiceGroupData.Label,
                            invoiceData.Regarding),
                            false);
                    }
                }

                if (!isPreview)
                    _invoiceService.CalculateNextInvoiceId(invoiceId, isPreview);

                var creditNote = _invoiceService.ValidateCreditNote(invoiceId);

                if (!creditNote)
                {
                    var responseInvoice = GenerateInvoiceFiles(invoiceId);
                    if (!responseInvoice.Success)
                    {
                        _invoiceService.RollBack(invoiceId);
                        _invoiceService.ResetInvoiceId(invoiceId);
                        return responseInvoice;
                    }
                    if (!isPreview)
                        _invoiceService.CopyTimeEntries(invoiceId);
                }
                else
                {
                    var responseInvoice = GenerateCreditNote(invoiceId);
                    if (!responseInvoice.Success)
                    {
                        _invoiceService.RollBack(invoiceId);
                        _invoiceService.DeleteInvoiceById(invoiceId);
                        return responseInvoice;
                    }
                }

                var responseSpecification = GenerateSpecificationFile(invoiceId);
                if (!responseSpecification.Success)
                {
                    _invoiceService.RollBack(invoiceId);
                    _invoiceService.ResetInvoiceId(invoiceId);
                    throw new SpecificationFinalizeFail();
                }

                return new ServerResponse("Finalize succeded", true);
            }
            catch (InvoiceTemplateIdNotSet ex)
            {
                LogError(ex);
                _invoiceService.RollBack(invoiceId);
                _invoiceService.ResetInvoiceId(invoiceId);
                return new ServerResponse("The invoice template's ID has not been set for this customer's invoices",
                                          false);
            }
            catch (UnsupportedFileFormatException ex)
            {
                LogError(ex);
                return new ServerResponse("Templatefile is corrupted. Please reupload it", false);
            }
            catch (InvoiceFinalizeFail ex)
            {
                LogError(ex);
                return new ServerResponse("The invoice could not be created", false);
            }
            catch (SpecificationFinalizeFail ex)
            {
                LogError(ex);
                return new ServerResponse("The specification could not be created", false);
            }
            catch (InvoiceMailMissing ex)
            {
                LogError(ex);
                _invoiceService.RollBack(invoiceId);
                _invoiceService.ResetInvoiceId(invoiceId);
                return new ServerResponse("The template for invoices send by mail is missing. Please re-upload it",
                                          false);
            }
            catch (InvoicePrintMissing ex)
            {
                LogError(ex);
                _invoiceService.RollBack(invoiceId);
                _invoiceService.ResetInvoiceId(invoiceId);
                return new ServerResponse("The template for invoices send by paper is missing. Please re-upload it",
                                          false);
            }
            catch (SpecificationPrintMissing ex)
            {
                LogError(ex);
                _invoiceService.RollBack(invoiceId);
                _invoiceService.ResetInvoiceId(invoiceId);
                return new ServerResponse(
                    "The template for specification send by paper is missing. Please re-upload it", false);
            }
            catch (CreditNoteMailMissing ex)
            {
                LogError(ex);
                _invoiceService.DeleteInvoiceById(invoiceId);
                return new ServerResponse("The template for credit notes send by mail is missing. Please re-upload it",
                                          false);
            }
            catch (CreditNotePrintMissing ex)
            {
                LogError(ex);
                _invoiceService.DeleteInvoiceById(invoiceId);
                return new ServerResponse(
                    "The template for credit notes send by paper is missing. Please re-upload it", false);
            }
            catch (CreditNoteFinalizeFail ex)
            {
                LogError(ex);
                return new ServerResponse("The credit note could not be created. Please contact the developers", false);
            }
            catch (ArgumentException ex)
            {
                LogError(ex);
                _invoiceService.DeleteInvoiceById(invoiceId);
                return new ServerResponse(ex.Message, false);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return new ServerResponse("Something went wrong doing the finalizing of the draft", false);
            }
        }
    }
}
