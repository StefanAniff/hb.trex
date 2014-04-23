using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TRex.Services.Test.Util;
using Trex.Common.ServiceStack;
using Trex.Server.Core.Services;
using TrexSL.Web.ServiceStackServices.Forecast;

namespace TRex.Services.Test.Trex.ServiceStackServices.Forecast
{
    [TestFixture]
    public class ForecastStatisticsServiceTest : AutoFixtureTestBase
    {
        [Test]
        public void Get_ExtractsFromForecastRepository()
        {
            // Arrange
            var fixture = InitializeFixture();            
            // UNITTEST
            var forecastRepoMock = fixture.Create<Mock<IForecastRepository>>();
            fixture.Inject(forecastRepoMock.Object);

            var sut = fixture.Create<ForecastStatisticsService>();

            // Act
            var request = new ForecastStatisticsRequest
                {
                    DisplayedMonth = 12,
                    DisplayedYear = 2013,
                    UserId = 10,
                    Now = new DateTime(2014, 1, 15)
                };

            var result = sut.Post(request);
            

            // Assert
            forecastRepoMock.VerifyAll();
        }
    }
}