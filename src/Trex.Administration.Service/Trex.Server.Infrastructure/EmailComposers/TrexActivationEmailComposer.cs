using System.Configuration;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.Implemented;

namespace Trex.Server.Infrastructure.EmailComposers
{
    // IVA: No longer functional remove/cleanup
    public class TrexActivationEmailComposer : EmailComposerBase
    {
        private readonly TrexCustomer _customer;
        private readonly string _password;
        private readonly string _language;
        private readonly string _mailSubject;
        private string _mailTemplatePath;

        private const string activationLinkMarker = "[ACTIVATIONLINK]";
        private const string customerNameMarker = "[CustomerName]";
        private const string userNameMarker = "[UserName]";
        private const string passwordMarker = "[Password]";
        private const string customerIdMarker = "[CustomerId]";

        public TrexActivationEmailComposer(TrexCustomer customer, string password, string language)
        {
            _customer = customer;
            _password = password;
            _language = language;

            if (_language == "da")
            {
                _mailSubject = ConfigurationManager.AppSettings["activationMailSubject_da"];
                _mailTemplatePath = ConfigurationManager.AppSettings["activationMailTemplate_da"];

            }
            else
            {
                _mailSubject = ConfigurationManager.AppSettings["activationMailSubject_en"];
                _mailTemplatePath = ConfigurationManager.AppSettings["activationMailTemplate_en"];

            }

            Title = _mailSubject;
        }

        public override string ComposeContent()
        {
            return string.Empty;

            //var activationSite = ConfigurationManager.AppSettings["activationSite"];
            //var mailSubject = string.Empty;
            //var resultString = string.Empty;



            //Title = mailSubject;
            //IsHtml = true;

            //var activationLink = string.Concat(activationSite , _customer.ActivationId , "&lng=" , _language);

            //resultString = GetContentFromUrl(_mailTemplatePath);

            //resultString = resultString.Replace(activationLinkMarker, activationLink);

            //resultString = resultString.Replace(customerNameMarker, _customer.CreatorFullName);
            //resultString = resultString.Replace(userNameMarker, _customer.CreatorUserName);
            //resultString = resultString.Replace(passwordMarker, _password);
            //resultString = resultString.Replace(customerIdMarker, _customer.CustomerId);

            //return resultString;
        }        
    }
}
