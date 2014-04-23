using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Core.Services
{
    public interface IUserSession
    {
        User CurrentUser { get; }
        bool IsLoggedIn { get; }
        void Initialize();
        void LoginUser(string username, string password, string customerId, bool persistPassword);
        void LogOutUser();
        IUserPreferences UserPreferences { get; }
        ILoginSettings LoginSettings { get; }
        UserStatistics UserStatistics { get; set; }
        bool MayEditOthersWorksplan { get; }
        void AttachUserNameAndCustomerId(string username, string customerId);
    }
}