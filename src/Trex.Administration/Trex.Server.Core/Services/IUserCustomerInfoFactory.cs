using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IUserCustomerInfoFactory
    {
        UserCustomerInfo Create(User user, Customer customer, double Price);
    }
}