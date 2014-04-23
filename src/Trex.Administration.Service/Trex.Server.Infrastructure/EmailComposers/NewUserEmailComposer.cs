using System.Linq;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Implemented;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.EmailComposers
{
    public class NewUserEmailComposer: EmailComposerBase
    {
        private readonly User _newUser;
        private readonly string _password;
        private readonly string _language;
        private readonly string _customerId;
        private readonly IPermissionService _permissionService;
        private readonly IAppSettings _appSettings;

        private const string UserFullNameMarker = "[UserFullName]";
        private const string UserNameMarker = "[UserName]";
        private const string PasswordMarker = "[Password]";
        private const string CustomerIdMarker = "[CustomerId]";        

        public NewUserEmailComposer(User newUser,string password,string language, string customerId,IPermissionService permissionService, IAppSettings appSettings)
        {
            _newUser = newUser;
            _password = password;
            _language = language;
            _customerId = customerId;
            _permissionService = permissionService;
            _appSettings = appSettings;

            if (_language.ToLower().Equals("da"))
            {
                Title = "Velkommen til T.Rex";
            }
            else
            {
                Title = "Welcome to T.Rex";
            }
        }

        public override string ComposeContent()
        {          
            var content = GetContent(TemplateFileName);
            content = content.Replace(UserNameMarker, _newUser.UserName);
            content = content.Replace(UserFullNameMarker, _newUser.Name);
            content = content.Replace(PasswordMarker, _password);
            content = content.Replace(CustomerIdMarker, _customerId);
            content = ReplaceContentFromAppsettings(content, _appSettings);

            return content;
        }        

        private string TemplateFileName
        {                       
            get
            {
                var permissions = _permissionService.GetPermissionsForRoles(_newUser.Roles, (int)ServiceClients.AdministrationClient);

                // If user has permission to log in to the administration, he is supposed to get the admin mail version
                if (permissions.SingleOrDefault(p => p.Permission == Permissions.ApplicationLoginPermission.ToString()) != null)
                {
                    return _language.ToLower().Equals("da") ? "newadminuser_da.htm" : "newadminuser_en.htm";
                }

                // A "standard" user
                return _language.ToLower().Equals("da") ? "newusermail_da.htm" : "newusermail_en.htm";                             
            }            
        }
    }
}
