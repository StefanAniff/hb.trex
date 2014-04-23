using System;
using System.Text;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.Implemented;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.EmailComposers
{
    // IVA: No longer functional remove/cleanup
    public class NewCustomerEmailComposer:EmailComposerBase
    {
        private readonly TrexCustomer _trexCustomer;

        public NewCustomerEmailComposer(TrexCustomer trexCustomer)
        {
            _trexCustomer = trexCustomer;

            Title = "A new customer has just activated a T.Rex database";
            IsHtml = false;

        }

        public override string ComposeContent()
        {
            var content = string.Empty;

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(content + "Company name: " + _trexCustomer.CompanyName);
            stringBuilder.AppendLine(content + "User: " + _trexCustomer.CreatorFullName);
            stringBuilder.AppendLine(content + "Email: " + _trexCustomer.CreatorEmail);
            stringBuilder.AppendLine(content + "Phone: " + _trexCustomer.CreatorPhone);

            return stringBuilder.ToString();


        }
    }
}
