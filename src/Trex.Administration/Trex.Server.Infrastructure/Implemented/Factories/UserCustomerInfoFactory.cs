using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented.Factories
{
    public class UserCustomerInfoFactory : IUserCustomerInfoFactory
    {
        #region IUserCustomerInfoFactory Members

        public UserCustomerInfo Create(User user, Customer customer, double pricePrHour)
        {
            if (user == null)
            {
                throw new ParameterNullOrEmptyException("User cannot be null");
            }
            if (customer == null)
            {
                throw new ParameterNullOrEmptyException("Customer cannot be null");
            }

            var userCustomerInfo = new UserCustomerInfo(user, customer, pricePrHour);
            return userCustomerInfo;
        }

        #endregion
    }
}