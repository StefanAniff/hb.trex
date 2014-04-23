using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.UnitOfWork;

namespace Trex.Server.Infrastructure.Implemented
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(ISession openSession)
            : base(openSession)
        {
        }

        public ClientRepository(IActiveSessionManager activeSessionManager)
            : base(activeSessionManager)
        {
        }

        public Client FindClientByCustomerId(string customerId)
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();
            if (!session.Connection.ConnectionString.Contains("base"))
            {
                int i =0;
            }
            var client = session.QueryOver<Client>()
                .Where(clients => clients.CustomerId == customerId).List();

            if (!client.Any())
                throw new ClientNotFoundException(string.Format("Customer '{0}' not found", customerId));

            return client.Single();
        }

    }
}
