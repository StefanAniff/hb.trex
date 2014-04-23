using System.Collections.Generic;
using Moq;
using Ploeh.AutoFixture;
using Trex.SmartClient.Forecast.Shared;

namespace Trex.SmartClient.Test.Forecast.ForecastRegistration
{
    public static class ForecastTestData
    {
        public static ForecastType ProjectHoursOnlyForecastType
        {
            get
            {
                return new ForecastType
                {
                    Id = 1,
                    Name = "Client",
                    SupportsProjectHours = true,
                    SupportsDedicatedHours = false
                };
            }
        }

        public static ForecastType DedicatedAndProjectHoursForcastType
        {
            get
            {
                return new ForecastType
                {
                    Id = 2,
                    Name = "Open",
                    SupportsProjectHours = true,
                    SupportsDedicatedHours = true
                };
            }
        }

        public static ForecastType SimpleForecastType
        {
            get
            {
                return new ForecastType
                {
                    Id = 3,
                    Name = "Leave",
                    SupportsProjectHours = false,
                    SupportsDedicatedHours = false
                };
            }
        }

        public static List<ForecastType> ForecastTypesList
        {
            get
            {
                return new List<ForecastType>
                    {
                        ProjectHoursOnlyForecastType,
                        DedicatedAndProjectHoursForcastType,
                        SimpleForecastType
                    };
            }
        }

        public static Mock<ForecastTypeProvider> MockForecastTypeProvider(IFixture fixture)
        {
            var forecastTypeProviderMock = fixture.Create<Mock<ForecastTypeProvider>>();
            forecastTypeProviderMock.SetupGet(x => x.Default).Returns(ProjectHoursOnlyForecastType);
            forecastTypeProviderMock.SetupGet(x => x.ForecastTypes).Returns(ForecastTypesList);
            return forecastTypeProviderMock;
        }
    }
}