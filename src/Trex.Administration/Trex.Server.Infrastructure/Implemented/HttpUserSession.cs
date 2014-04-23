using System.Web.Security;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class HttpUserSession : IUserSession
    {
        private readonly IUserPreferences _userPreferences;
        private readonly IUserRepository _userRepository;

        public HttpUserSession(IUserRepository userRepository, IUserPreferences userPreferences)
        {
            _userRepository = userRepository;
            _userPreferences = userPreferences;
        }

        #region IUserSession Members

        public User CurrentUser
        {
            get { return new User {UserName = "ultra123"}; }
        }

        public IUserPreferences UserPreferences
        {
            get { return _userPreferences; }
        }

        public bool ValidateCredentials(string username, string password)
        {
            return Membership.ValidateUser(username, password);
        }

        public User GetUser(string username)
        {
            return _userRepository.GetByUserName(username);
        }

        #endregion
    }
}