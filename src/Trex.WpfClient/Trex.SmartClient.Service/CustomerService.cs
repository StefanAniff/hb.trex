using System.Collections.Generic;
using System.Threading.Tasks;
using Trex.Common.DataTransferObjects.ProjectAdministration;
using Trex.Common.ServiceStack;
using Trex.SmartClient.Core.Services;

namespace Trex.SmartClient.Service
{
    public class CustomerService : ClientServiceBase, ICustomerService
    {
        public async Task<ICollection<BasicEntityDto>> GetAllActiveCustomers()
        {
            var result = await TrySendAsync(new AllCustomersRequest(), () => "An error occured while get customers");
            return result.Customers;
        }
    }
}