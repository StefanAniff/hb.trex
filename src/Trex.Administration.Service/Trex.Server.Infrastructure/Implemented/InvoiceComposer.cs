#region

using System;
using System.IO;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Exceptions;
using Trex.ServiceContracts;

#endregion

namespace Trex.Server.Infrastructure.Implemented
{
    public class InvoiceComposer : ComposerBase, IInvoiceCompose
    {
        public InvoiceComposer(IInvoiceBuilder builder, IGatherData gatherData, ISavePDF savePDF)
            : base(builder, gatherData, savePDF)
        {
        }

        public override void TemplateValidation(int invoiceId, IGatherData gatherData, int format)
        {
            try
            {
                var template = gatherData.GetCustomerInvoiceGroupsTemplateData(invoiceId, format);

                var invoiceTemplate = new InvoiceTemplate();
                if (format == 1 || format == 3)
                {
                    if (template.InvoiceTemplateIdMail == null)
                        throw new InvoiceTemplateIdNotSet();
                    invoiceTemplate = gatherData.GetInvoiceTemplate((int) template.InvoiceTemplateIdMail);
                }

                if (format == 2 || format == 3)
                {
                    if (template.InvoiceTemplateIdPrint == null)
                        throw new InvoiceTemplateIdNotSet();

                    invoiceTemplate = gatherData.GetInvoiceTemplate((int)template.InvoiceTemplateIdPrint);
                }


                var invoiceTemplateFound = gatherData.ValidateTemplate(invoiceTemplate);
                if (!invoiceTemplateFound)
                {
                    throw new FileNotFoundException("The invoice template could not be found");
                }
            }
            catch(Aspose.Words.UnsupportedFileFormatException ex)
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