using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TRex.Services.Test.Util;
using Trex.Common.ServiceStack;
using Trex.Server.Core.Services;
using TrexSL.Web.ServiceStackServices.Forecast;
using TrexSL.Web.ServiceStackServices.Forecast.Helpers;

namespace TRex.Services.Test.Trex.ServiceStackServices.Forecast
{
    [TestFixture]
    public class ForecastSearchByRegistrationServiceTest : AutoFixtureTestBase
    {
        [Test]
        public void Post_CriteriaCollectorReturnsNoElements_ForecastMonthRepositoryIsNotCalled()
        {
            // Arrange
            var forecastMonthRepoMock = FreezeMock<IForecastMonthRepository>();
            var holidayProviderMock = FreezeMock<HolidaysByPeriodProvider>();
            var criteriaCollectorMock = FreezeMock<ForecastMonthSearchCriteriaCollector>();
            criteriaCollectorMock
                .Setup(x => x.Collect(It.IsAny<ForecastSearchByRegistrationRequest>()))
                .Returns(new List<IForecastMonthQueryCriteria>());

            var sut = Fixture.Create<ForecastSearchByRegistrationService>();

            // Act
            sut.Post(new ForecastSearchByRegistrationRequest { ForecastMonth = 1, ForecastYear = 2010 });

            // Assert
            forecastMonthRepoMock
                .Verify(
                    x =>
                    x.GetBySearchCriterias(It.IsAny<IEnumerable<IForecastMonthQueryCriteria>>(), It.IsAny<int>(),
                                           It.IsAny<int>()), Times.Never());

            holidayProviderMock.Verify(x => x.GetHolidays(1, 2010));

        }
    }
}