using Trex.ServiceContracts;

namespace Trex.Server.Core.Services
{
    public interface ITrexRegistrator
    {
        UserCreationResponse RegisterNewUser(string applicationName, string userName, string password, string creatorContactName, string email, string language);
    }
}
