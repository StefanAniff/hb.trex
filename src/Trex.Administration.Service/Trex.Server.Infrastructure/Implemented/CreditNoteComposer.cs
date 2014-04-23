using System;
using System.IO;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Exceptions;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class CreditNoteComposer : ComposerBase, ICreditNoteCompose
    {
        public CreditNoteComposer(IInvoiceBuilder builder, IGatherData gatherData, ISavePDF savePDF)
            : base(builder, gatherData, savePDF)
        {
        }

        public override void TemplateValidation(int invoiceId, IGatherData gatherData, int format)
        {
            try
            {
                var template = gatherData.GetCustomerInvoiceGroupsTemplateData(invoiceId, format);

                var invoiceTemplate = new InvoiceTemplate();
                if (format == 4 || format == 3)
                {
                    if (template.CreditNoteTemplateIdMail == null)
                        throw new CreditNoteTemplateIdNotSet();

                    invoiceTemplate = gatherData.GetInvoiceTemplate((int)template.CreditNoteTemplateIdMail);
                }

                if (format == 5 || format == 3)
                {
                    if (template.CreditNoteTemplateIdPrint == null)
                        throw new CreditNoteTemplateIdNotSet();

                    invoiceTemplate = gatherData.GetInvoiceTemplate((int)template.CreditNoteTemplateIdPrint);
                }
                
                var invoiceTemplateFound = gatherData.ValidateTemplate(invoiceTemplate);
                if (!invoiceTemplateFound)
                {
                    throw new FileNotFoundException("The invoice template could not be found");
                }
            }
            catch (Aspose.Words.UnsupportedFileFormatException ex)
            {
                LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }
    }
}