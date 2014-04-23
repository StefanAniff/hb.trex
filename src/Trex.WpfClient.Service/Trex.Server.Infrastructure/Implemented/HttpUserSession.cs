using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class HttpUserSession : IUserSession
    {
        readonly IUserRepository _userRepository;
        private readonly IMembershipProvider _membershipProvider;

        public HttpUserSession(IUserRepository userRepository, IMembershipProvider membershipProvider)
        {
            _userRepository = userRepository;
            _membershipProvider = membershipProvider;
        }


        public bool ValidateCredentials(string username, string password)
        {
            return _membershipProvider.ValidateUser(username, password);
        }
    }
}
