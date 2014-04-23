using System;
using Trex.Core.Interfaces;
using Trex.Core.Services;

namespace Trex.Infrastructure.Implemented
{
    public class AppSettings : IAppSettings
    {
        private readonly IEstimateSettings _estimateSettings;
        private readonly IRegionNames _regionNames;

        public AppSettings(IRegionNames regionNames, IEstimateSettings estimateSettings)
        {
            _regionNames = regionNames;
            _estimateSettings = estimateSettings;
        }

        #region IAppSettings Members

        public IRegionNames RegionNames
        {
            get { return _regionNames; }
        }

        public IEstimateSettings EstimateSettings
        {
            get { return _estimateSettings; }
        }

        public TimeSpan SessionTimeOut
        {
            get { return TimeSpan.FromMinutes(20); }
        }

        #endregion
    }
}