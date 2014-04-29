using System.Collections.Generic;
using System.Threading.Tasks;
using Trex.Common.DataTransferObjects.ProjectAdministration;

namespace Trex.SmartClient.Core.Services
{
    public interface ICustomerService
    {
        Task<ICollection<BasicEntityDto>> GetAllActiveCustomers();
    }
}