using System;
using System.ServiceModel;
using Trex.Server.Core;
using Trex.Server.Core.Services;
using Trex.Server.Core.Unity;
using Trex.Server.Infrastructure.Implemented;
using Trex.Server.Infrastructure.UnitOfWork;
using TrexSL.Web.Intercepts;
using TrexSL.Web.ServiceInterfaces;
using log4net;

namespace TrexSL.Web
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Single)]
    [UnityAndErrorBehavior]
    [CustomBehavior]
    public class AuthenticationService : IAuthenticationService
    {
        private static readonly ILog Log = LogManager.GetLogger("TRex." + typeof (RoleManagementService).Name);

        private readonly IClientRepository _clientRepository;
        private readonly IUserManagementService _userManagementService;
        private readonly IUserRepository _userRepository;
        private readonly IEmailComposer _emailComposer;
        private readonly IMembershipProvider _membershipProvider;

        public AuthenticationService(IClientRepository clientRepository, 
            IUserManagementService userManagementService, IUserRepository userRepository, 
            IEmailComposer emailComposer, IMembershipProvider membershipProvider)
        {
            _clientRepository = clientRepository;
            _userManagementService = userManagementService;
            _userRepository = userRepository;
            _emailComposer = emailComposer;
            _membershipProvider = membershipProvider;
        }


        [UnitOfWork(true)]
        public bool ResetPassword(string userName)
        {
            try
            {
                var modelUser = _userRepository.GetByUserName(userName);
                var newPassword = _userManagementService.ResetPassword(modelUser);
                if (newPassword != null)
                {
                    _emailComposer.SendForgotPasswordEmail(modelUser.Name, newPassword, modelUser.Email);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw new Exception("Error. Try again.");
            }
        }


        [UnitOfWork(true, true)]
        public bool ValidateUser(string userName, string password, string customerId)
        {
            try
            {
                //Membership.ApplicationName = customerId;

                var client = _clientRepository.FindClientByCustomerId(customerId);
                if (!client.IsActivated)
                {
                    return false;
                }

                if (_membershipProvider.ValidateUser(userName, password))
                {
                    TenantConnectionProvider.DynamicString = client.ConnectionString;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw new Exception("Error. Try again.");
            }
        }
    }
}