using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IProjectFactory
    {
        Project Create(string projectName, Customer customer, User createdBy, bool isEstimatesEnabled);
    }
}