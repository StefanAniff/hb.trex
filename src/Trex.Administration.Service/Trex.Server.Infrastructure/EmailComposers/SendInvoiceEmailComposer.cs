using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Implemented;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.EmailComposers
{
    public class SendInvoiceEmailComposer : EmailComposerBase
    {
        private readonly Invoice _invoice;
        private readonly CustomerInvoiceGroup _cig;
        private readonly IAppSettings _appSettings;

        public SendInvoiceEmailComposer(Invoice invoice,CustomerInvoiceGroup cig, IAppSettings appSettings)
        {
            Title = "d60 Invoice: " + invoice.InvoiceID;
            IsHtml = true;
            _cig = cig;
            _appSettings = appSettings;
            _invoice = invoice;
        }

        private const string Attention = "[Attention]";

        public override string ComposeContent()
        {
            var content = GetContent(TemplateFileName);

            if (_cig.Attention == null)
                _cig.Attention = _cig.Customer.ContactName;
            content = content.Replace(Attention, _cig.Attention);
            ReplaceContentFromAppsettings(content, _appSettings);

            return content;
        }

        private string TemplateFileName
        {
            get
            {
                return _invoice.IsCreditNote
                           ? "creditnote.htm"
                           : "invoice.htm";
            }
        }
    }
}
