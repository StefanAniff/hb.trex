using System;
using System.Collections.Generic;
using System.Linq;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.BaseClasses;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class CustomerServices : LogableBase, ICustomerService
    {
        private readonly TrexEntities _entityContext;
        public CustomerServices(ITrexContextProvider contextProvider)
        {
            _entityContext = contextProvider.TrexEntityContext;
        }

        public List<CustomersInvoiceView> GetCustomerInvoiceViews(DateTime startDate, DateTime endDate)
        {            
            return _entityContext.CustomersInvoiceView(startDate.Date, endDate.Date + new TimeSpan(0, 23, 59, 59)).ToList();
        }
        
        public Customer GetCustomerById(int customerId, bool includeInactive, bool includeParents, bool includeProjects, bool includeTasks, bool includeTimeEntries)
        {
            var customers = _entityContext.Customers.Include("CustomerInvoiceGroups");

            IQueryable<Customer> query = null;
            if (includeProjects && !includeTasks && !includeTimeEntries)
            {
                query = customers.Include("Projects");
            }

            if (includeTasks && !includeTimeEntries)
            {
                query = customers.Include("Projects.Tasks");
            }

            if (includeTimeEntries)
            {
                query = customers.Include("Projects.Tasks.TimeEntries");
            }

            if (query == null)
                query = customers;

            if (!includeInactive)
            {
                query = query.Where(c => !c.Inactive);
            }

            var result = query.FirstOrDefault(c => c.CustomerID == customerId);

            if (!includeInactive)
                RemoveInactive(result);

            return result;
        }

        public Customer GetCustomerByCriteria(GetCustomerByIdCriterias criterias)
        {
            var query = _entityContext.Customers;
            var result = query
                .Where(c => c.CustomerID == criterias.CustomerId && (!c.Inactive || criterias.IncludeInactive))
                .Select(c => new
                {
                    Customer = c,
                    Projects = c.Projects.Where(x => !x.Inactive || criterias.IncludeInactive),

                    // Tasks wich are created or has timeentries added in the given period 
                    Tasks = c.Projects.Select(x => x.Tasks.Where(t => (!t.Inactive || criterias.IncludeInactive)
                                                && ((t.CreateDate >= criterias.TaskFrom && t.CreateDate <= criterias.TaskTo) 
                                                     || t.TimeEntries.Any(te => te.CreateDate >= criterias.TaskFrom && te.CreateDate <= criterias.TaskTo)))),

                    CustomerInvoiceGroups = c.CustomerInvoiceGroups,
                    TimeEntries = c.Projects.Select(p => p.Tasks.Select(t => t.TimeEntries.Where(te => te.CreateDate >= criterias.TimeEntryFrom && te.CreateDate <= criterias.TimeEntryTo)))
                })
                .AsEnumerable()
                .Select(x => x.Customer)
                .SingleOrDefault();

            return result;
        }

        public List<Customer> GetAllCustomers(bool includeInactive, bool includeParents, bool includeProjects, bool includeTasks, bool includeTimeEntries)
        {
            var customers = _entityContext.Customers;
            IQueryable<Customer> query = null;
            if (includeProjects)
                query = customers.Include("Projects");

            if (includeTasks)
                query = customers.Include("Projects.Tasks");

            if (includeTimeEntries)
                query = customers.Include("Projects.Tasks.TimeEntries");

            if (query == null)
                query = customers;

            if (!includeInactive)
                query = query.Where(c => c.Inactive == false);

            var result = query.ToList();
            foreach (var customer in result)
            {
                RemoveInactive(customer);
            }
            return result;
        }

        public List<Customer> EntityCompanyRequest(List<int> customerIds, bool includeParents, bool includeInactive, bool includeProjects, bool includeTasks, bool includeTimeEntries)
        {
            var companyList = new List<Customer>();

            foreach (var customerId in customerIds)
            {
                var customer = GetCustomerById(customerId, includeInactive, includeParents, includeProjects, includeTasks, includeTimeEntries);
                companyList.Add(customer);
            }
            return companyList.OrderBy(c => c.CustomerName).ToList();
        }

        public Customer SaveCustomer(Customer customer)
        {
            customer.ChangeTracker.ChangeTrackingEnabled = false;
            customer.CustomerInvoiceGroups = null;
            customer.Projects = null;
            customer.ChangeTracker.ChangeTrackingEnabled = true;
            customer.ChangeDate = DateTime.Now;
            _entityContext.Customers.ApplyChanges(customer);
            _entityContext.SaveChanges();
            return customer;
        }

        public bool DeleteCustomer(Customer customer)
        {
            _entityContext.Customers.Attach(customer);
            if (customer != null && customer.Projects.Count == 0)
            {
                _entityContext.Customers.DeleteObject(customer);
                _entityContext.SaveChanges();
                return true;
            }

            if (customer != null && customer.Projects.Count > 0)
            {
                customer.Inactive = true;
                _entityContext.Customers.ApplyChanges(customer);
                _entityContext.SaveChanges();
                return true;
            }
            return false;

        }

        private void RemoveInactive(Customer customer)
        {
            try
            {
                for (int i = customer.Projects.Count() - 1; i >= 0; i--)
                {
                    if (customer.Projects[i].Inactive)
                        customer.Projects.RemoveAt(i);
                    else
                    {
                        for (int y = customer.Projects[i].Tasks.Count() - 1; y >= 0; y--)
                        {
                            if (customer.Projects[i].Tasks[y].Inactive)
                            {
                                customer.Projects[i].Tasks.RemoveAt(y);
                            }
                        }
                        customer.Projects[i].MarkAsUnchanged();
                        customer.Projects[i].AcceptChanges();
                    }
                }

                customer.MarkAsUnchanged();
                customer.AcceptChanges();
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }
    }
}
