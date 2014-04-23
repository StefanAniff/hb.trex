using System;
using System.Collections.Generic;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface ICustomerRepository
    {
        IList<Customer> GetAll(bool includeInactive);
        IList<Customer> GetAllPreloaded();
        Customer GetByID(int customerID);
        Customer GetByGuid(Guid guid);
        Customer Insert(Customer customer);
        IList<Customer> GetCustomersFilteredByUserPreferences(IUserPreferences userPreferences, User user);
        IList<Customer> GetByChangeDate(DateTime startDate);
        void Update(Customer customer);
        void Delete(Customer customer);
    }
}