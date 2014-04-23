using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Infrastructure.Implemented
{
    public class UserContext
    {
        private static readonly UserContext _instance = new UserContext();

        private UserContext() { }

        public static UserContext Instance
        {
            get { return _instance; }
        }

        public User User { get; set; }
    }
}