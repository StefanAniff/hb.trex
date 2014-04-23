using System.Collections.Generic;
using Trex.SmartClient.Core.Model;
using System.Linq;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Data;

namespace Trex.SmartClient.Infrastructure.Implemented.LocalStorage.Forecast
{
    public class ForecastUserListPresetRepository : IForecastUserListPresetRepository
    {
        private readonly IForecastDataSetWrapper _forecastDataSetWrapper;

        public ForecastUserListPresetRepository(IForecastDataSetWrapper forecastDataSetWrapper)
        {
            _forecastDataSetWrapper = forecastDataSetWrapper;
        }

        public IEnumerable<ForecastUserSearchPreset> GetAll()
        {
            return _forecastDataSetWrapper
                .ForecastDataSet
                .ForecastUserSearchPresets
                .Select(x => ForecastUserSearchPreset.Create(x.Name, new List<int>(x.UserIds)));
        } 

        public void OverWriteAllWith(IEnumerable<ForecastUserSearchPreset> newItems)
        {
            var dataSet = _forecastDataSetWrapper.ForecastDataSet;
            dataSet.ForecastUserSearchPresets.Clear();
            foreach (var newItem in newItems)
            {
                dataSet.ForecastUserSearchPresets.Add(new ForecastUserSearchPresetData
                    {
                        Name = newItem.Name,
                        UserIds = new List<int>(newItem.UserIds)
                    });
            }
            _forecastDataSetWrapper.Save();
        }
    }
}