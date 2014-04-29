using System.Collections.ObjectModel;
using Trex.Common.DataTransferObjects.ProjectAdministration;
using Trex.Common.ServiceStack;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.ServiceStack;
using System.Linq;

namespace TrexSL.Web.ServiceStackServices.ProjectManagement
{
    public class AllCustomersRequestHandler : NhServiceBasePost<AllCustomersRequest>
    {
        private readonly ICustomerRepository _customerRepository;

        public AllCustomersRequestHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        protected override object Send(AllCustomersRequest request)
        {
            return GetValue(request);
        }

        private object GetValue(AllCustomersRequest _)
        {
            var domCustomers = _customerRepository.GetAllActive();

            var result = new AllCustomersResponse
                {
                    Customers = new Collection<BasicEntityDto>(domCustomers.Select(x => new BasicEntityDto { Id = x.CustomerID, Name = x.CustomerName}).ToList())
                };

            return result;
        }
    }
}