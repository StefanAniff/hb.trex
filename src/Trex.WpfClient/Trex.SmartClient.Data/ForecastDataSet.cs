using System;
using System.Collections.Generic;

namespace Trex.SmartClient.Data
{
    [Serializable]
    public class ForecastDataSet
    {
        public ForecastDataSet()
        {
            ForecastUserSearchPresets = new List<ForecastUserSearchPresetData>();
        }

        /// <summary>
        /// List of user-search-presets created from Workplan/Forecast overview
        /// </summary>
        public List<ForecastUserSearchPresetData> ForecastUserSearchPresets { get; set; }
    }

    [Serializable]
    public class ForecastUserSearchPresetData
    {
        public string Name { get; set; }
        public List<int> UserIds { get; set; }
    }
}