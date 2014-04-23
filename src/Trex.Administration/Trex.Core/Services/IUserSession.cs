using System;
using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.Services
{
    public interface IUserSession
    {
        User CurrentUser { get; }
        IUserSettingsService UserSettingsService { get; }
        string CustomerId { get; }
        bool IsLoggedIn { get; }
        void LoginUser(string username, string password, string customerId, bool persistPassword,Action<LoginResponse> callbackAction );
        void LogOutUser();

        void Initialize(Action<LoginResponse> callbackAction );
    }
}