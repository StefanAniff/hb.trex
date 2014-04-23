using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class CustomerRepository : RepositoryBase, ICustomerRepository
    {
        #region ICustomerRepository Members

        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <returns></returns>
        public IList<Customer> GetAll(bool includeInactive)
        {
            var session = GetSession();

            var allCustomers = from customers in session.Linq<Customer>()
                               orderby customers.Name
                               select customers;

            if (!includeInactive)
            {
                var filteredCustomers = from customers in allCustomers
                                        where customers.Inactive == false
                                        select customers;
                return filteredCustomers.ToList();
            }
            else
            {
                return allCustomers.ToList();
            }
        }

        public IList<Customer> GetAllPreloaded()
        {
            var session = GetSession();
            var allProjects = session.CreateQuery(
                "select cst"
                + " from Customer cst"
                + " join fetch cst.Projects prj"
                ).SetResultTransformer(new DistinctRootEntityResultTransformer())
                .List<Customer>();
            return allProjects.ToList();
        }

        /// <summary>
        /// Gets the by ID.
        /// </summary>
        /// <param name="customerID">The customer ID.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundByIDException"></exception>
        public Customer GetByID(int customerID)
        {
            var session = GetSession();

            return session.Get<Customer>(customerID);
        }

        public Customer GetByGuid(Guid guid)
        {
            var session = GetSession();

            var customer = from customers in session.Linq<Customer>()
                           where customers.Guid == guid
                           select customers;

            return customer.First();
        }

        /// <summary>
        /// Creates the specified customer
        /// </summary>
        public Customer Insert(Customer customer)
        {
            var session = GetSession();

            session.Save(customer);

            return customer;
        }

        /// <summary>
        /// Gets a list of customers, Changed after the given date
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <returns></returns>
        public IList<Customer> GetByChangeDate(DateTime startDate)
        {
            var session = GetSession();
            var customerList = from customers in session.Linq<Customer>()
                               where customers.CreateDate >= startDate || (customers.ChangeDate != null && customers.ChangeDate >= startDate)
                               select customers;

            return customerList.ToList();
        }

        /// <summary>
        /// Saves the specified customer.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <exception cref="EntityUpdateException"></exception>
        public void Update(Customer customer)
        {
            var session = GetSession();
            session.Transaction.Begin();
            try
            {
                customer.ChangeDate = DateTime.Now;

                session.Save(customer);
                session.Flush();
                session.Transaction.Commit();
            }
            catch (HibernateException ex)
            {
                session.Transaction.Rollback();
                throw new EntityUpdateException("Error updating customer", ex);
            }
        }

        public IList<Customer> GetCustomersFilteredByUserPreferences(IUserPreferences userPreferences, User user)
        {
            if (userPreferences.ShowAllTasks)
            {
                return GetAll(false);
            }
            else
            {
                return user.Customers;
            }
        }

        public void Delete(Customer customer)
        {
            var session = GetSession();
            try
            {
                session.Transaction.Begin();
                session.Delete(customer);
                session.Flush();
                session.Transaction.Commit();
            }
            catch (HibernateException ex)
            {
                session.Transaction.Rollback();
                throw new EntityDeleteException("Error deleting Customer", ex);
            }
        }

        #endregion

        public void Delete(int customerId)
        {
            Delete(GetByID(customerId));
        }
    }
}