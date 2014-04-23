using System;
using Trex.Core.Interfaces;

namespace Trex.Core.Services
{
    public interface IAppSettings
    {
        IRegionNames RegionNames { get; }
        TimeSpan SessionTimeOut { get; }
        IEstimateSettings EstimateSettings { get; }
    }
}