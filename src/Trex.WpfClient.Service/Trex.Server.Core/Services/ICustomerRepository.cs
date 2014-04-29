using System;
using System.Collections.Generic;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface ICustomerRepository : IRepository<Company>
    {
        IEnumerable<Company> GetByChangeDate(DateTime startDate);
        IEnumerable<Company> GetByNameSearchString(string searchString);
        IEnumerable<Company> GetAllActive();
    }
}
