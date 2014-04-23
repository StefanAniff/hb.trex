using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Trex.Server.Core.Model
{
    /// <summary>
    /// User object for NHibernate mapped table 'Users'.
    /// </summary>
    public class User : IComparable
    {
        #region Constructors

        public User()
        {
            Projects = new List<Project>();
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
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public virtual int Id { get; set; }

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

        public virtual string Password { get; set; }

        public static String SortExpression { get; set; }

        public static SortDirection SortDirection { get; set; }

        public virtual int NumOfTimeEntries { get; set; }

        public virtual double TotalTime { get; set; }
        public virtual double TotalBillableTime { get; set; }

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

        public virtual bool CustomerInfoExists(UserCustomerInfo customerInfo)
        {
            return CustomerInfo.SingleOrDefault(c => c.Equals(customerInfo)) != null;
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

        #endregion

        #region IComparable Methods

        public virtual int CompareTo(object obj)
        {
            if (!(obj is User))
            {
                throw new InvalidCastException("This object is not of type User");
            }

            int relativeValue;
            switch (SortExpression)
            {
                case "Id":
                    relativeValue = Id.CompareTo(((User) obj).Id);
                    break;
                case "UserName":
                    relativeValue = UserName.CompareTo(((User) obj).UserName);
                    break;
                default:
                    goto case "Id";
            }
            if (SortDirection == SortDirection.Ascending)
            {
                relativeValue *= -1;
            }
            return relativeValue;
        }

        #endregion
    }
}