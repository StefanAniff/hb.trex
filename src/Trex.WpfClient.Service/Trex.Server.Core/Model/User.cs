
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
using Trex.Server.Core.Model.Forecast;

namespace Trex.Server.Core.Model
{
    /// <summary>
    /// User object for NHibernate mapped table 'Users'.
    /// </summary>
    public class User : EntityBase
    {

        #region Constructors

        public User()
        {
            CustomerInfo = new List<UserCustomerInfo>();
            Projects = new List<Project>();
        }

        public User(string userName, string name, string email, double price)
            : this()
        {
            UserName = userName;
            Name = name;
            Email = email;
            Price = price;
        }

        #endregion


        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public virtual int UserID { get; set; }

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

        /// <summary>
        /// Gets or sets the forecast months for user
        /// </summary>
        public virtual IList<ForecastMonth> ForecastMonths { get; set; }

        public virtual void AddCustomerInfo(UserCustomerInfo customerInfo)
        {
            //Update existing info item
            if (CustomerInfo.Contains<UserCustomerInfo>(customerInfo))
            {
                UserCustomerInfo foundInfo = CustomerInfo.Single(ci => ci.Equals(customerInfo));
                foundInfo.PricePrHour = customerInfo.PricePrHour;
            }
            else
            {
                CustomerInfo.Add(customerInfo);
            }
        }

        public virtual int NumOfTimeEntries { get; set; }

        public virtual double TotalTime { get; set; }
        public virtual double TotalBillableTime { get; set; }

        public override int EntityId
        {
            get { return UserID; }
        }
    }
}