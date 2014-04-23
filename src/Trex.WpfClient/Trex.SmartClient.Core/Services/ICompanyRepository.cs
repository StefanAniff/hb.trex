using System.Collections.Generic;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Core.Services
{
    public interface ICompanyRepository
    {
        List<Company> GetAll();
        Company GetById(int companyId);
        void AddOrUpdate(List<Company> companies);
        List<Company> GetByNameSearchString(string searchString);
        IEnumerable<Company> GetAllActive();
    }
}