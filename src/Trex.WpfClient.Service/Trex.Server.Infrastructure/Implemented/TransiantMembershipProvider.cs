using System.Collections.Specialized;
using System.Web.Security;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class TransiantMembershipProvider : IMembershipProvider
    {
        private readonly TRexMembershipProvider _sqlMembershipProvider;
        private readonly SqlRoleProvider _roleProvider;

        public TransiantMembershipProvider()
        {
            _sqlMembershipProvider = new TRexMembershipProvider();
            _roleProvider = new SqlRoleProvider();

        }

        public bool ValidateUser(string userName, string userPassword)
        {
            var isValid = Membership.Provider.ValidateUser(userName, userPassword);
            if (!isValid)
            {
                isValid = _sqlMembershipProvider.ValidateUser2(userName, userPassword);
            }
            return isValid;
        }

        private MembershipUser GetUser(string userName)
        {
            return Membership.Provider.GetUser(userName, true);
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var memberShipUser = GetUser(username);
            if (memberShipUser != null)
            {
                return memberShipUser.ChangePassword(oldPassword, newPassword);

            }
            return false;
        }

        public string[] GetAllRoles()
        {
            return Roles.GetAllRoles();
        }

        public string[] GetUsersInRole(string role)
        {
            return Roles.GetUsersInRole(role);
        }

        public bool RoleExists(string name)
        {
            return Roles.RoleExists(name);
        }

        public void DeleteRole(string roleName)
        {
            Roles.DeleteRole(roleName, true);
        }

        public void CreateRole(string roleName)
        {
            Roles.CreateRole(roleName);
        }

        public string GetApplicationName()
        {
            return Membership.ApplicationName;
        }


        public string ResetPassword(string username)
        {
            var memberShipUser = GetUser(username);
            if (memberShipUser != null)
            {
                return memberShipUser.ResetPassword();
            }
            return null;
        }

        public void AddUserToRoles(string userName, string[] roles)
        {
            Roles.AddUsersToRoles(new[] {userName}, roles);
        }

        public void RemoveUserFromRoles(string userName, string[] currentRoles)
        {
            Roles.RemoveUsersFromRoles(new[] {userName}, currentRoles);
        }

        public string[] GetRolesForUser(string userName)
        {
            return Roles.GetRolesForUser(userName);
        }

    }

    public class TRexMembershipProvider : SqlMembershipProvider
    {
        private readonly MembershipProvider provider = Membership.Provider;

        public TRexMembershipProvider()
        {
            var configuration = new NameValueCollection();

            configuration.Add("name", "SqlProvider");
            configuration.Add("connectionStringName", "memberDB");
            configuration.Add("applicationName", "Jonas");
            configuration.Add("enablePasswordRetrieval", "false");
            configuration.Add("enablePasswordReset", "true");
            configuration.Add("requiresQuestionAndAnswer", "false");
            configuration.Add("requiresUniqueEmail", "true");
            configuration.Add("passwordFormat", "Hashed");
            configuration.Add("maxInvalidPasswordAttempts", "50");
            configuration.Add("passwordAttemptWindow", "10");
            configuration.Add("minRequiredPasswordLength", "6");
            configuration.Add("minRequiredNonalphanumericCharacters", "0");
            Initialize("SqlProvider", configuration);
        }

        public override string ApplicationName
        {
            get { return provider.ApplicationName; }
            set { provider.ApplicationName = value; }
        }

        public bool ValidateUser2(string username, string password)
        {
            var oldIsValid = this.ValidateUser(username, password);
            //var usr = this.GetUser("cth", false);
            var isValid = provider.ValidateUser(username, password);
            return isValid;
        }

        //public override MembershipUser GetUser(string username, bool userIsOnline)
        //{
        //    return provider.GetUser(username, userIsOnline);
        //}

    }
}