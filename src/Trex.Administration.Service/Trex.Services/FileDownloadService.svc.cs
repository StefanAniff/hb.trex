#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Activation;
using Aspose.Words;
using StructureMap;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.BaseClasses;
using Trex.Server.Infrastructure.Implemented;
using Trex.Server.Infrastructure.ServiceBehavior;
using Trex.ServiceContracts;

#endregion

namespace TrexSL.Web
{
    [SilverlightFaultBehavior]
    [CustomBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FileDownloadService : LogableBase, IFileDownloadService
    {
        private readonly IInvoiceWorker _invoiceWorker;
        private readonly IInvoiceService _invoiceService;
        private readonly IInvoiceSender _invoiceSender;
        private readonly ITemplateService _templateService;
        private readonly ITrexContextProvider _contextProvider;

        public FileDownloadService()
        {
            _invoiceWorker = ObjectFactory.GetInstance<IInvoiceWorker>();
            _invoiceService = ObjectFactory.GetInstance<IInvoiceService>();
            _invoiceSender = ObjectFactory.GetInstance<IInvoiceSender>();
            _templateService = ObjectFactory.GetInstance<ITemplateService>();
            _contextProvider = ObjectFactory.GetInstance<ITrexContextProvider>();
        }

        public FileDownloadService(IInvoiceWorker invoiceWorker, IInvoiceService invoiceService, IInvoiceSender invoiceSender, 
                                   ITemplateService templateService, ITrexContextProvider contextProvider)
        {
            _invoiceWorker = invoiceWorker;
            _invoiceService = invoiceService;
            _invoiceSender = invoiceSender;
            _templateService = templateService;
            _contextProvider = contextProvider;
        }

        /// <summary>
        /// Retrieves the InvoiceTemplate file by its ID
        /// </summary>
        /// <param name="templateId">The Template's ID</param>
        /// <returns>byte[] representing it in .docx</returns>
        public byte[] DownloadTemplateFile(int templateId)
        {
            try
            {
                return _templateService.DownloadTemplateFile(templateId);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        /// <summary>
        /// Returns a byte[] with the generated pdf-file
        /// </summary>
        /// <param name="invoiceGuid">The invoice's Guid</param>
        /// <param name="format">1 = mail, 2 = print, 3 = specification</param>
        /// <returns>byte[] representing the pdf-file</returns>
        public byte[] DownloadPdfFile(Guid invoiceGuid, int format)
        {
            try
            {
                return _templateService.DownloadPdfFile(invoiceGuid, format);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<ServerResponse> FinalizeInvoices(List<int> invoiceIds, bool isPreview)
        {
            try
            {
                var list = new List<ServerResponse>();

                foreach (var invoiceId in invoiceIds)
                {
                    var respond = new ServerResponse("", false, invoiceId);

                    if (_invoiceService.InvoiceNumberGiven(invoiceId) && _invoiceService.InvoiceFileCreated(invoiceId))
                        respond.Success = true;

                    else
                    {
                        respond = _invoiceWorker.FinalizeInvoice(invoiceId, isPreview);

                        if (!isPreview && respond.Success)
                        {
                            bool toBeSent = _invoiceSender.SendToMail(invoiceId);

                            if (toBeSent)
                            {
                                bool gotSent;
                                try
                                {
                                    gotSent = _invoiceSender.SendInvoiceEmail(invoiceId);
                                }
                                catch (Exception ex)
                                {
                                    LogError(ex);
                                    respond.Response = ex.Message;
                                    gotSent = false;
                                }

                                if (gotSent)
                                {
                                    _invoiceService.UpdateDeliveredDate(invoiceId);
                                    respond.Response = "Invoices sent";
                                }
                                else
                                {
                                    RollBack(invoiceId);
                                    respond.Success = false;
                                }
                            }
                            else
                            {
                                respond.Success = true;
                                respond.Response = "Invoice must be printet";
                                respond.ToPrint = true;
                            }
                        }
                    }
                    list.Add(new ServerResponse(respond.Response, respond.Success, invoiceId));
                }
                return list;
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public ServerResponse DeleteInvoiceFiles(int invoiceId)
        {
            try
            {
                var result = _templateService.DeleteInvoiceFiles(invoiceId);
                var invoicelist = new List<int>();
                invoicelist.Add(invoiceId);
                FinalizeInvoices(invoicelist, true);
                return result;
            }
            catch (Exception e)
            {
                LogError(e);
                return new ServerResponse("failed", false);

            }
        }

        public ServerResponse ValidateTemplate(byte[] data, int templateType)
        {
            var stream = new MemoryStream(data);
            var doc = new Document(stream);

            var result = new ServerResponse("Failed to validate", false);
            switch (templateType)
            {
                case 1:
                    result.Success = ValidateStandardMail(doc);
                    break;
                case 2:
                    result.Success = ValidateStandardPrint(doc);
                    break;
                case 3:
                    result.Success = ValidateSpec(doc);
                    break;
                case 4:
                    result.Success = ValidateCreaditNoteMail(doc);
                    break;
                case 5:
                    result.Success = ValidateCreaditNotePrint(doc);
                    break;
            }

            if (result.Success)
            {
                result.Response = "Validation complete";
                return result;
            }
            return result;

        }

        private bool ValidateCreaditNotePrint(Document doc)
        {
            if (doc.Range.Bookmarks["InvoiceParent"] != null)
                return true;
            else return false;
        }

        private bool ValidateCreaditNoteMail(Document doc)
        {
            if (doc.Range.Bookmarks["InvoiceParent"] != null)
                return true;
            else return false;
        }

        private bool ValidateSpec(Document doc)
        {
            if (doc.Range.Bookmarks["SpecData"] != null)
                return true;
            else return false;
        }

        private bool ValidateStandardPrint(Document doc)
        {
            if (doc.Range.Bookmarks["InvoiceNumberBOLD"] != null)
                return true;
            else return false;
        }

        private bool ValidateStandardMail(Document doc)
        {
            if (doc.Range.Bookmarks["InvoiceNumberBOLD"] != null)
                return true;
            else return false;
        }

        public void RollBack(int id)
        {
            using (var entity = _contextProvider.TrexEntityContext)
            {
                var files = (from IF in entity.InvoiceFiles
                             where IF.InvoiceID == id
                             select IF);

                foreach (var invoiceFile in files)
                {
                    entity.InvoiceFiles.DeleteObject(invoiceFile);
                }
                entity.SaveChanges();

                var cn = (from creditNote in entity.CreditNote
                          where creditNote.InvoiceID == id
                          select creditNote);
                foreach (var creditNote in cn)
                {
                    entity.CreditNote.DeleteObject(creditNote);
                }
                entity.SaveChanges();

                var invoice = entity.Invoices.First(i => i.ID == id);
                invoice.InvoiceID = null;
                entity.Invoices.ApplyChanges(invoice);
                entity.SaveChanges();
            }
        }
    }
}