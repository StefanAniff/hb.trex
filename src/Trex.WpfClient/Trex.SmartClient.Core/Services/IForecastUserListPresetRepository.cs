using System.Collections.Generic;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Core.Services
{
    public interface IForecastUserListPresetRepository
    {
        IEnumerable<ForecastUserSearchPreset> GetAll();
        void OverWriteAllWith(IEnumerable<ForecastUserSearchPreset> newItems);
    }
}