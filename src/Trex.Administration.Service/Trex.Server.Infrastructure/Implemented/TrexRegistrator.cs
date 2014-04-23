using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using StructureMap;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.EmailComposers;
using Trex.ServiceContracts;
using TrexSL.Web.Exceptions;

namespace Trex.Server.Infrastructure.Implemented
{
    public class TrexRegistrator : ITrexRegistrator
    {
        private readonly ITrexBaseContextProvider _trexBaseContextProvider;
        private readonly IMembershipService _membershipService;
        private readonly IEmailService _emailService;
        private readonly IAppSettings _appSettings;


        public TrexRegistrator(ITrexBaseContextProvider trexBaseContextProvider, IMembershipService membershipService, IEmailService emailService,IAppSettings appSettings  )
        {
            _trexBaseContextProvider = trexBaseContextProvider;
            _membershipService = membershipService;
            _emailService = emailService;
            _appSettings = appSettings;
        }

        public UserCreationResponse RegisterNewUser(string applicationName, string userName, string password, string creatorContactName, string email, string language)
        {
         
            
            var result = CreateMember(applicationName, userName, password, creatorContactName, email);

            if (result.Success)
            {

                var customer = SaveUserDetails(applicationName, userName, creatorContactName, email);
                SendActivationMail(customer, password, language);

            }
            return result;
        }

        private TrexCustomer SaveUserDetails(string applicationName, string userName, string creatorContactName, string email)
        {
            var entityContext = _trexBaseContextProvider.TrexBaseEntityContext;
            var customer = entityContext.TrexCustomers.SingleOrDefault(c => c.CustomerId.ToLower() == applicationName.ToLower());

            customer.ActivationId = Guid.NewGuid().ToString();
            customer.CreatorUserName = userName;
            customer.CreatorFullName = creatorContactName;
            customer.CreatorEmail = email;
            customer.IsActivationEmailSent = true;
            entityContext.SaveChanges();
            return customer;
        }

        private UserCreationResponse CreateMember(string applicationName, string userName, string password, string creatorContactName, string email)
        {
            _membershipService.ApplicationName = applicationName;
            _membershipService.CreateRole(_appSettings.AdministratorDefaultRole);

            var newUser = new User
            {
                UserName = userName,
                Name = creatorContactName,
                Email = email,
                Roles = new List<string>() {_appSettings.AdministratorDefaultRole }
            };

            var creationResponse= _membershipService.CreateMember(newUser, password);
           

            return creationResponse;

        }

        private void SendActivationMail(TrexCustomer customer, string password, string language)
        {
            throw new Exception("Online activation is no longer supported");

            //var mailComposer = new TrexActivationEmailComposer(customer, password, language);

            //mailComposer.Recipients.Add(customer.CreatorEmail);
            //mailComposer.Sender = _appSettings.TrexSupportEmail;
            //_emailService.SendEmail(mailComposer);

        }
    }
}
