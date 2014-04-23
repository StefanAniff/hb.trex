using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Implemented;

namespace Trex.Server.Infrastructure.EmailComposers
{
    public class ForgotPasswordEmailComposer : EmailComposerBase
    {
        private readonly string _fullName;
        private readonly string _password;
        private readonly string _language;
        private readonly IAppSettings _appSettings;
        private const string UserFullNameMarker = "[UserFullName]";
        private const string PasswordMarker = "[Password]";
        


        public ForgotPasswordEmailComposer(string fullName, string password, string language, IAppSettings appSettings)
        {
            _fullName = fullName;
            _password = password;
            _language = language;
            _appSettings = appSettings;

            Title = "Your T.Rex password has been reset";
            this.IsHtml = true;           
        }

        public override string ComposeContent()
        {            
            var content = GetContent(TemplateFileName);
            content = content.Replace(UserFullNameMarker, _fullName);
            content = content.Replace(PasswordMarker, _password);
            ReplaceContentFromAppsettings(content, _appSettings);
            return content;
        }        

        private string TemplateFileName
        {
            get
            {
                return _language == "da"
                           ? "passwordreset_da.htm"
                           : "passwordreset_en.htm";
            }
        }
    }
}
