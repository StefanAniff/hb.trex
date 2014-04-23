using System.Collections.Generic;
using System.Threading.Tasks;

namespace Trex.SmartClient.Core.Interfaces
{
    public interface IUserWlanSettingsService
    {
        bool BoundToWLan { get; }
        int UserWLanTimeEntryTypeId { get; }
        List<IUserWLanTimeEntryType> WLansBoundToTimeEntryTypes { get; set; }
        Task<IEnumerable<IUserWLanTimeEntryType>> FindAnyUserWLanMatches();
    }
}