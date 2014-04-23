using System.Collections.Generic;
using System.Linq;
using Trex.Core.Interfaces;

namespace Trex.Core.Model
{
    public class User : IEntity
    {
        #region Constructors

        public User()
        {
            Projects = new List<Project>();
            CustomerInfo = new List<UserCustomerInfo>();
            Roles = new List<string>();
            Permissions = new List<string>();
        }

        public User(string userName, string name, string email, double price) : this()
        {
            UserName = userName;
            Name = name;
            Email = email;
            Price = price;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        /// <value>The name of the user.</value>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public virtual string Email { get; set; }

        /// <summary>
        /// Gets or sets the price. The price is the default price used, when no price has been set elsewhere
        /// </summary>
        /// <value>The price.</value>
        public virtual double Price { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="User"/> is inactive.
        /// </summary>
        /// <value><c>true</c> if inactive; otherwise, <c>false</c>.</value>
        public virtual bool Inactive { get; set; }

        /// <summary>
        /// Gets or sets the projects, the user has selected
        /// </summary>
        /// <value>The projects.</value>
        public virtual IList<Project> Projects { get; set; }

        /// <summary>
        /// Gets or sets the customer info.
        /// </summary>
        /// <value>The customer info.</value>
        public virtual IList<UserCustomerInfo> CustomerInfo { get; set; }

        public virtual IList<Customer> Customers
        {
            get
            {
                var projects = from pr in Projects select pr.Customer;
                return projects.Distinct().ToList<Customer>();
            }
        }

        public int NumOfTimeEntries { get; set; }

        public double TotalTime { get; set; }
        public double TotalBillableTime { get; set; }

        public List<string> Permissions { get; set; }
        public List<string> Roles { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public virtual int Id { get; set; }

        public bool IsValidChild(IEntity entity)
        {
            return false;
        }

        public virtual void AddCustomerInfo(UserCustomerInfo customerInfo)
        {
            //Update existing info item
            if (CustomerInfo.Contains<UserCustomerInfo>(customerInfo))
            {
                var foundInfo = CustomerInfo.Single(ci => ci.Equals(customerInfo));
                foundInfo.PricePrHour = customerInfo.PricePrHour;
            }
            else
            {
                CustomerInfo.Add(customerInfo);
            }
        }

        public virtual void RemoveCustomerInfo(UserCustomerInfo customerInfo)
        {
            if (CustomerInfo.Contains<UserCustomerInfo>(customerInfo))
            {
                var foundInfo = CustomerInfo.Single(ci => ci == customerInfo);
                CustomerInfo.Remove(foundInfo);
            }
        }

        public virtual void RemoveProject(Project project)
        {
            if (HasProject(project))
            {
                var foundProject = Projects.First(p => p.Id == project.Id);
                Projects.Remove(foundProject);
            }
        }

        public virtual void AddProject(Project project)
        {
            if (!HasProject(project))
            {
                Projects.Add(project);
            }
        }

        public virtual bool HasProject(Project project)
        {
            return Projects.Contains<Project>(project);
        }

        public virtual double GetPricePrHour(Customer customer)
        {
            var pricePrHour = Price;

            if (CustomerInfo.Count(uc => uc.CustomerId == customer.Id) > 0)
            {
                var info = CustomerInfo.Single(uc => uc.CustomerId == customer.Id);
                pricePrHour = info.PricePrHour;
            }

            return pricePrHour;
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}