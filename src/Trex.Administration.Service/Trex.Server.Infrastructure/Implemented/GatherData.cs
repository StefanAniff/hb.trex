using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Aspose.Words;
using StructureMap;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.BaseClasses;
using Trex.Server.Infrastructure.Exceptions;
using Trex.ServiceContracts;
using System.Linq;

namespace Trex.Server.Infrastructure.Implemented
{
    public class GatherData : LogableBase, IGatherData
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ITrexContextProvider _entityContext;
        public DocumentBuilder DocumentBuilder { get; private set; }

        public GatherData(IInvoiceService invoiceService, ITrexContextProvider contextProvider)
        {
            _invoiceService = invoiceService;
            _entityContext = contextProvider;
        }

        public bool ValidateTemplate(InvoiceTemplate template)
        {
            try
            {
                ConvertBinaryToDocument(template.TemplateId);
                return true;
            }
            catch (IOException ex)
            {
                const string message = "Template is properly open. Close it and run the action again";
                LogError(ex);
                LogMessage(message);
                throw new IOException(message);
            }
        }

        public Invoice GetInvoiceData(int invoiceId)
        {
            return _invoiceService.GetInvoiceById(invoiceId);
        }

        public List<InvoiceLine> GetInvoiceLines(int invoiceId)
        {
            return _invoiceService.GetInvoiceLinesByInvoiceId(invoiceId);
        }

        public CustomerInvoiceGroup GetMetaData(int customerInvoiceGroupId)
        {
            return _invoiceService.GetInvoiceMetaData(customerInvoiceGroupId);
        }

        public CustomerInvoiceGroup GetInvoiceTemplateId(int invoiceID, int format)
        {
            return _invoiceService.GetInvoiceTemplateByInvoiceId(invoiceID, format);
        }

        public CustomerInvoiceGroup GetCustomerInvoiceGroupsTemplateData(int invoiceId, int format)
        {
            return _invoiceService.GetCustomerInvoiceGroupsTemplateData(invoiceId, format);
        }

        public InvoiceTemplate GetInvoiceTemplate(int templateId)
        {
            return _invoiceService.GetTemplateById(templateId);
        }

        /// <summary>
        /// Converts the template to a document and sets the DocumentBuilder to this
        /// </summary>
        /// <exception cref="InvoiceFileNotFound"></exception>
        /// <param name="templateId">The InvoiceTemplateFile's ID</param>
        public void ConvertBinaryToDocument(int templateId)
        {
            try
            {
                using (var entity = _entityContext.TrexEntityContext)
                {
                    var templateBytes =
                        entity.InvoiceTemplateFiles.First(itf => itf.InvoiceTemplateId == templateId).File;

                    var stream = new MemoryStream(templateBytes);
                    DocumentBuilder = new DocumentBuilder(new Document(stream));
                }
            }
            catch (InvalidOperationException exception)
            {
                string message = "Template with ID " + templateId + " not found";
                LogMessage(message);
                LogError(exception);
                throw new InvoiceFileNotFound(message, exception);
            }
        }
    }
}