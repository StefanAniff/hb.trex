using System.Configuration;
using System.Data.EntityClient;
using System.Linq;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.EmailComposers;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class TrexActivator:ITrexActivator
    {
        private readonly IDatabaseCreator _databaseCreator;
        private readonly IEmailService _emailService;
        private readonly ITrexBaseContextProvider _contextProvider;
        private readonly IAppSettings _appSettings;

        public TrexActivator(IDatabaseCreator databaseCreator, IEmailService emailService,ITrexBaseContextProvider contextProvider,IAppSettings appSettings)
        {
            _databaseCreator = databaseCreator;
            _emailService = emailService;
            _contextProvider = contextProvider;
            _appSettings = appSettings;
        }

        // IVA: Seems to be apart of new-customer-auto-registration. Maybe remove/cleanup
        public void Activate(string activationId,string language)
        {
            
            var entityContext = _contextProvider.TrexBaseEntityContext;
            
            var trexCustomer = entityContext.TrexCustomers.SingleOrDefault(c => c.ActivationId == activationId);

            if (trexCustomer == null)
                throw new ActivationIdNotFoundException("Trex customer with activation id: " + activationId + " not found");

            _databaseCreator.CreateTrex(trexCustomer.CustomerId);

            trexCustomer.ActivationId = null;
            trexCustomer.ConnectionString = _databaseCreator.ConnectionString;
        
            trexCustomer.IsActivated = true;
            entityContext.SaveChanges();



            CreateUser(trexCustomer);

            SendWelcomeMail(language,trexCustomer);
            SendNewCustomerMail(trexCustomer);

        }

        private void SendNewCustomerMail(TrexCustomer trexCustomer)
        {
            var newCustomerComposer = new NewCustomerEmailComposer(trexCustomer);
            newCustomerComposer.Recipients.Add(_appSettings.RegistrationNotificationEmail);
            newCustomerComposer.Sender = _appSettings.TrexSupportEmail;
            _emailService.SendEmail(newCustomerComposer);
        }

        private void CreateUser(TrexCustomer trexCustomer)
        {
            var newUser = new User
            {
                UserName = trexCustomer.CreatorUserName,
                Name = trexCustomer.CreatorFullName,
                Email = trexCustomer.CreatorEmail,
            };

            var connectionStringTemplate = ConfigurationManager.AppSettings["defaultEFConnectionString"];
            var connectionString = string.Format(connectionStringTemplate, trexCustomer.ConnectionString);
            var entityConnectionStringBuilder = new EntityConnectionStringBuilder(connectionString);

            var entityConnection = new EntityConnection(entityConnectionStringBuilder.ToString());

            var entityContext = new TrexEntities(entityConnection);
            entityContext.Users.AddObject(newUser);
            entityContext.SaveChanges();
        }

        private void SendWelcomeMail(string language,TrexCustomer trexCustomer)
        {
            var welcomeMailComposer = new WelcomeMailComposer(language);
            welcomeMailComposer.Recipients.Add(trexCustomer.CreatorEmail);
            welcomeMailComposer.Sender = _appSettings.TrexSupportEmail;
            _emailService.SendEmail(welcomeMailComposer);
        }
    }
}
