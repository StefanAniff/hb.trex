using System.Collections.Generic;
using System.Linq;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;

namespace Trex.SmartClient.Infrastructure.Implemented.LocalStorage
{
    public class ClientCompanyRepository : ICompanyRepository
    {
        private readonly DataSetWrapper _dataWrapper;

        public ClientCompanyRepository(DataSetWrapper dataSetWrapper)
        {
            _dataWrapper = dataSetWrapper;

        }

        public List<Company> GetAll()
        {
            var companyList = new List<Company>();
            foreach (var customersRow in _dataWrapper.Customers)
            {
                companyList.Add(Company.Create(customersRow.Name, customersRow.Id, customersRow.InheritsTimeEntryTypes, customersRow.Inactive));
            }
            return companyList;
        }

        public Company GetById(int companyId)
        {
            var customersRow = _dataWrapper.Customers.SingleOrDefault(row => row.Id == companyId);
            if (customersRow != null)
            {
                return Company.Create(customersRow.Name, customersRow.Id, customersRow.InheritsTimeEntryTypes, customersRow.Inactive);
            }
            return null;

        }

        public void AddOrUpdate(List<Company> companies)
        {
            if (!companies.Any())
            {
                return;
            }
            foreach (var company in companies)
            {
                AddOrUpdateInternal(company);
            }
            _dataWrapper.Save();
        }

        private void AddOrUpdateInternal(Company company)
        {
            _dataWrapper.SaveCustomer(company);
        }

        public List<Company> GetByNameSearchString(string searchString)
        {
            var rows = _dataWrapper
                        .Customers
                        .Where(x => x.Name.ToLower().Contains(searchString.ToLower()) && !x.Inactive);

            return rows
                    .Select(x => Company.Create(x.Name, x.Id, x.InheritsTimeEntryTypes, x.Inactive))
                    .ToList();
        }

        public IEnumerable<Company> GetAllActive()
        {
            var rows = _dataWrapper
                        .Customers
                        .Where(x => !x.Inactive);

            return rows.Select(x => Company.Create(x.Name, x.Id, x.InheritsTimeEntryTypes, x.Inactive));
        }
    }
}