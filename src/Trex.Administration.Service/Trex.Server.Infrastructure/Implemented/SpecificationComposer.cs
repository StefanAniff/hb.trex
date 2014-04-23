using System.IO;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Exceptions;

namespace Trex.Server.Infrastructure.Implemented
{
    public class SpecificationComposer : ComposerBase, ISpecificationComposer
    {
        public SpecificationComposer(ISpecificationBuilder builder, IGatherData gatherData, ISavePDF savePDF)
            : base(builder, gatherData, savePDF)
        {
        }

        public override void TemplateValidation(int invoiceId, IGatherData gatherData, int format)
        {
            var template = gatherData.GetCustomerInvoiceGroupsTemplateData(invoiceId, format);
            if (template.SpecificationTemplateIdMail == null)
                throw new InvoiceTemplateIdNotSet();

            var specificationTemplate = gatherData.GetInvoiceTemplate((int)template.SpecificationTemplateIdMail);

            var specificationTemplateFound = gatherData.ValidateTemplate(specificationTemplate); //Get InvoiceTemplateById
            if (!specificationTemplateFound)
            {
                throw new FileNotFoundException("The specification template could not be found");
            }
        }
    }
}