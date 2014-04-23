using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.BaseClasses;
using Trex.Server.Infrastructure.EmailComposers;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class InvoiceSender : LogableBase, IInvoiceSender
    {
        private readonly IEmailService _emailService;
        private readonly IAppSettings _appSettings;
        private CustomerInvoiceGroup _cig;
        private Invoice _invoice;
        private Customer _customer;
        private readonly ITrexContextProvider _contextProvider;

        public InvoiceSender(IAppSettings appSettings, IEmailService emailService, ITrexContextProvider contextProvider)
        {
            _appSettings = appSettings;
            _emailService = emailService;
            _contextProvider = contextProvider;
        }

        /// <summary>
        /// Finds data and files for the invoice and sends it to assigned people
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <returns>True if succes, false if failure</returns>
        public bool SendInvoiceEmail(int invoiceId)
        {
            using (var context = _contextProvider.TrexEntityContext)
            {
                _cig = (from cig in context.CustomerInvoiceGroups.Include("Customer")
                        join i in context.Invoices on cig.CustomerInvoiceGroupID equals i.CustomerInvoiceGroupId
                        where i.ID == invoiceId
                        select cig).Single();

                _invoice = (from i in context.Invoices
                            where i.ID == invoiceId
                            select i).First();

                _customer = (from c in context.Customers
                             where c.CustomerID == _cig.CustomerID
                             select c).Single();
            }
            var emailComposer = new SendInvoiceEmailComposer(_invoice, _cig, _appSettings);


            emailComposer.Recipients.Add((_customer.Email ?? _cig.Email));

            string[] mailCC;

            if (!string.IsNullOrEmpty(_cig.EmailCC) && !string.IsNullOrWhiteSpace(_cig.EmailCC))
            {
                mailCC = _cig.EmailCC.Split(';');
                foreach (var s in mailCC)
                {
                    emailComposer.CcRecipients.Add(s);
                }
            }
            else if (!string.IsNullOrEmpty(_customer.EmailCC) && !string.IsNullOrWhiteSpace(_cig.EmailCC) && _cig.DefaultCig == true)
            {
                mailCC = _customer.EmailCC.Split(';');
                foreach (var s in mailCC)
                {
                    emailComposer.CcRecipients.Add(s);
                }
            }

            emailComposer.Attachments.Add(GetInvoiceFile(_invoice));

            if(!_invoice.IsCreditNote)
                emailComposer.Attachments.Add(GetSpecFile(_invoice));

            emailComposer.Sender = _appSettings.TrexInvoicetMail;

            return _emailService.SendEmail(emailComposer);
        }

        public bool SendToMail(int invoiceId)
        {
            using (var entity = _contextProvider.TrexEntityContext)
            {
                int sendFormat = (from i in entity.Invoices
                                  join cig in entity.CustomerInvoiceGroups on i.CustomerInvoiceGroupId equals cig.CustomerInvoiceGroupID
                                  join c in entity.Customers on cig.CustomerID equals c.CustomerID
                                  where i.ID == invoiceId
                                  select (cig.SendFormat == 0 ? c.SendFormat : cig.SendFormat)).First();
                if (sendFormat == 1)
                    return true;
                return false;
            }
        }

        private Attachment GetInvoiceFile(Invoice invoice)
        {
            try
            {
                using (var context = _contextProvider.TrexEntityContext)
                {
                    var invoiceFile = context.InvoiceFiles.First(i => i.InvoiceID == invoice.ID && (i.FileType == 1 || i.FileType == 4)).File;

                    MemoryStream invoiceStream = new MemoryStream(invoiceFile);
                    invoiceStream.Position = 0;

                    return new Attachment(invoiceStream, "Invoice_" + invoice.InvoiceID + ".pdf", "application/pdf");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Attachment GetSpecFile(Invoice invoice)
        {
            try
            {
                using (var context = _contextProvider.TrexEntityContext)
                {

                    var invoiceSpec = context.InvoiceFiles.First(i => i.InvoiceID == invoice.ID && i.FileType == 3).File;

                    MemoryStream specStream = new MemoryStream(invoiceSpec);
                    specStream.Position = 0;

                    return new Attachment(specStream, "Specification_" + invoice.InvoiceID + ".pdf", "application/pdf");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
