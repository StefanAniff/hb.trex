using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface ITimeEntryTypeFactory
    {
        TimeEntryType Create(string name, bool isDefault, bool isBillableByDefault, Company company);
    }
}
