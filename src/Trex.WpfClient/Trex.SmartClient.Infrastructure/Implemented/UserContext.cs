using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class UserContext
    {
        private static readonly UserContext _instance = new UserContext();

        private UserContext()
        {}

        public static UserContext Instance { get { return _instance; } }

        public User User { get; set; }
        
    }
}
