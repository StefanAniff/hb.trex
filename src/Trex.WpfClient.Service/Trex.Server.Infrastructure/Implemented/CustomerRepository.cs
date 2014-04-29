using System;
using System.Collections.Generic;
using Trex.Server.Core.Model;
using NHibernate;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.UnitOfWork;

namespace Trex.Server.Infrastructure.Implemented
{
    public class CustomerRepository : GenericRepository<Company>, ICustomerRepository
    {
        public CustomerRepository(ISession openSession)
            : base(openSession)
        {
        }

        public CustomerRepository(IActiveSessionManager activeSessionManager)
            : base(activeSessionManager)
        {
        }

        /// <summary>
        /// Gets a list of customers, Changed after the given date
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <returns></returns>
        public IEnumerable<Company> GetByChangeDate(DateTime startDate)
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();
            var customerList = session.QueryOver<Company>()
                                      .Where(customers => (customers.CreateDate >= startDate
                                                           || (customers.ChangeDate != null 
                                                           && customers.ChangeDate >= startDate))
                                                          && customers.Inactive == false);

            return customerList.List();

        }

        public IEnumerable<Company> GetByNameSearchString(string searchString)
        {
            return Session
                .QueryOver<Company>()
                .WhereRestrictionOn(x => x.CustomerName)
                .IsInsensitiveLike(string.Format("%{0}%", searchString))
                .Where(x => !x.Inactive)
                .List();
        }

        public IEnumerable<Company> GetAllActive()
        {
            return Session
                .QueryOver<Company>()
                .Where(x => !x.Inactive)
                .List();
        }

    }
}
