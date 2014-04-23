using System;
using System.Collections.Generic;

namespace Trex.Server.Core.Model
{
    /// <summary>
    /// Customer object for NHibernate mapped table 'Customers'.
    /// </summary>
    public class Customer
    {
        #region Constructors

        public Customer()
        {
            Guid = Guid.NewGuid();
        }

        public Customer(string name, DateTime createDate, DateTime changeDate, User createdBy, string streetAddress, string zipCode, string country, string contactName, string contactPhone,
                        bool inheritsTimeEntryTypes, int paymentTermsNumberOfDays, bool paymentTermsIncludeCurrentMonth, string address2)
            : this()
        {
            Name = name;
            CreateDate = createDate;
            CreatedBy = createdBy;
            ChangeDate = changeDate;
            StreetAddress = streetAddress;
            ZipCode = zipCode;
            Country = country;
            ContactName = contactName;
            ContactPhone = contactPhone;
            InheritsTimeEntryTypes = inheritsTimeEntryTypes;
            PaymentTermNumberOfDays = paymentTermsNumberOfDays;
            PaymentTermIncludeCurrentMonth = paymentTermsIncludeCurrentMonth;
            Address2 = address2;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        public virtual Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>The phone number.</value>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the cell phone number.
        /// </summary>
        /// <value>The cell phone number.</value>
        public virtual string CellPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public virtual string Email { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public virtual User CreatedBy { get; set; }

        public virtual DateTime? ChangeDate { get; set; }

        public virtual User ChangedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is inactive.
        /// </summary>
        /// <value><c>true</c> if [in active]; otherwise, <c>false</c>.</value>
        public virtual bool Inactive { get; set; }

        /// <summary>
        /// Gets or sets the projects.
        /// </summary>
        /// <value>The projects.</value>
        public virtual IList<Project> Projects { get; set; }

        /// <summary>
        /// Gets or sets the street address.
        /// </summary>
        /// <value>The street address.</value>
        public virtual string StreetAddress { get; set; }

        /// <summary>
        /// Gets or sets the street address.
        /// </summary>
        /// <value>The street address.</value>
        public virtual string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        public virtual string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public virtual string City { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        public virtual string Country { get; set; }

        /// <summary>
        /// Gets or sets the name of the contact.
        /// </summary>
        /// <value>The name of the contact.</value>
        public virtual string ContactName { get; set; }

        /// <summary>
        /// Gets or sets the contact phone.
        /// </summary>
        /// <value>The contact phone.</value>
        public virtual string ContactPhone { get; set; }

        public virtual int PaymentTermNumberOfDays { get; set; }

        public virtual bool PaymentTermIncludeCurrentMonth { get; set; }

        public virtual IList<Invoice> Invoices { get; set; }

        public virtual IList<TimeEntryType> TimeEntryTypes { get; set; }

        public virtual bool InheritsTimeEntryTypes { get; set; }

        #endregion
    }
}