using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.ServiceContracts;
using Trex.ServiceContracts.Model;

namespace Trex.Server.Infrastructure.Implemented
{
    public interface ICustomerService
    {
        List<CustomersInvoiceView> GetCustomerInvoiceViews(DateTime startDate, DateTime endDate);
        Customer GetCustomerById(int customerId, bool includeInactive, bool includeParents, bool includeProjects,
                                        bool includeTasks, bool includeTimeEntries);

        List<Customer> GetAllCustomers(bool includeInactive, bool includeParents, bool includeProjects,
                                              bool includeTasks, bool includeTimeEntries);

        Customer SaveCustomer(Customer customer);

        bool DeleteCustomer(Customer customer);

        List<Customer> EntityCompanyRequest(List<int> customerIds, bool includeParents, bool includeInactive,
                                            bool includeProjects, bool includeTasks, bool includeTimeEntries);

        Customer GetCustomerByCriteria(GetCustomerByIdCriterias criterias);
    }
}
