using System.Configuration;
using Trex.Server.Infrastructure.Implemented;

namespace Trex.Server.Infrastructure.EmailComposers
{
    // IVA: Seems no longer functional remove/cleanup
    public class WelcomeMailComposer : EmailComposerBase
    {
        private readonly string _language;

        public WelcomeMailComposer(string language)
        {
            _language = language;
            Title = _language.ToLower().Equals("da") 
                        ? ConfigurationManager.AppSettings["downloadMailSubject_da"] 
                        : ConfigurationManager.AppSettings["downloadMailSubject_en"];
        }

        public override string ComposeContent()
        {           
            return GetContent(TemplateFileName);
        }

        private string TemplateFileName
        {
            get
            {
                return _language.ToLower().Equals("da")
                           ? "downloadmailtemplate_da.htm"
                           : "downloadMailTemplate_en.htm";
            }
        }
    }
}
