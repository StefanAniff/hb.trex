using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IUserSession
    {
        User CurrentUser { get; }
        IUserPreferences UserPreferences { get; }
        bool ValidateCredentials(string username, string password);
        User GetUser(string username);
    }
}