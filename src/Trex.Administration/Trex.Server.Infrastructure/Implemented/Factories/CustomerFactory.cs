using System;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented.Factories
{
    public class CustomerFactory : ICustomerFactory
    {
        #region ICustomerFactory Members

        public Customer Create(string name, User creator, string streetAddress, string zipCode, string country, string contactName, string contactPhone, bool inheritsTimeEntryTypes, int paymentTermsNumberOfDays,
                               bool paymentTermsIncludeCurrentMonth, string address2)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ParameterNullOrEmptyException("Name cannot be empty");
            }

            if (creator == null)
            {
                throw new ParameterNullOrEmptyException("Creator cannot be null");
            }

            return new Customer(name, DateTime.Now, DateTime.Now, creator, streetAddress, zipCode, country, contactName, contactPhone, inheritsTimeEntryTypes, paymentTermsNumberOfDays,
                                paymentTermsIncludeCurrentMonth, address2);
        }

        #endregion
    }
}